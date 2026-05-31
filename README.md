# TenMaker
> 써드파티 에셋 라이센스로 인해 전체 프로젝트가 아닌 C# 스크립트만 포함

> 사과게임 류 (합이 10인 영역 제거) | Unity | 1인 개발 | PC · 모바일 | 미출시

---

## 기술 스택

- **Unity, C#**
- **Netcode for GameObjects** — 서버 권위 멀티플레이
- **Unity Gaming Services (UGS)** — Relay(NAT 우회 연결), 게스트 로그인, 랭킹
- **Unity Mediation** — 광고 미디에이션

---

## 핵심 구현

| 파일 | 역할 |
|------|------|
| [RegionSelectionHandler.cs](GamePlay/RegionClear/RegionSelectionHandler.cs) | 서버 검증 + 결과 브로드캐스트 (Rpc) |
| [PlayerInputController.cs](GamePlay/Player/Multiplay/PlayerInputController.cs) | Owner 전용 드래그 입력 → 서버 요청 |
| [GridController.cs](GamePlay/GridSystem/Grid/Multiplay/GridController.cs) | 그리드 네트워크 동기화 |
| [ValidRegionChecker.cs](GamePlay/GridSystem/Region/ValidRegionChecker.cs) | 2D Prefix Sum 검증 + 최소 유효 영역 트림 |
| [RegionSumCalculator.cs](GamePlay/GridSystem/Region/RegionSumCalculator.cs) | 2D Prefix Sum 계산 (영역 합 O(1)) |
| [MultiplayManager.cs](Services/UGS/Multiplay/MultiplayManager.cs) | UGS Relay 연결 관리 |
| [TMAudioManager.cs](Core/Audio/TMAudioManager.cs) | 서비스 로케이터 (구현체/인터페이스/접근자 분리) |

### Netcode for GameObjects 기반 서버 권위 멀티플레이

Photon·Fishnet 등 외부 솔루션과 비교한 끝에, **"누가 무엇을 결정할 권한을 갖는가"** 를 명확히 하기 위해 서버 권위 구조를 선택했습니다. 클라이언트는 드래그 영역을 *요청*만 하고, 합이 10인지에 대한 최종 판정과 셀 제거는 서버가 수행합니다.

```csharp
// PlayerInputController.cs — 드래그 종료 시 로컬 반영 + 서버로 요청 (Owner 전용)
private void HandleCellDragCanceled(GridSelectionData data)
{
    var selectedRegion = data.ToRegion();
    _selectionHandler.FinishLocalDrag();
    FinishDragServerRpc(selectedRegion);
}

[Rpc(SendTo.Server, RequireOwnership = true)]
private void FinishDragServerRpc(Region selectedRegion, RpcParams rpcParams = default)
{
    var senderId = rpcParams.Receive.SenderClientId;
    _selectionHandler.FinishSelectionServer(senderId, selectedRegion);
}
```

```csharp
// RegionSelectionHandler.cs — 검증·점수는 서버에서, 결과만 전 클라이언트로 브로드캐스트
public void FinishSelectionServer(ulong clientId, Region selectedRegion)
{
    var regionClearResult = gridController.ClearRegionServer(clientId, selectedRegion); // 서버 검증 + 제거
    if (regionClearResult == null) return; // 무효한 선택 → 무시 (치팅 방지)

    comboController.IncreaseComboServer(clientId);
    scoreSystem.UpdateScoreServer();

    var clearEvent = new ClearEventDTO(
        clientId, regionClearResult.Value, comboController.GetCombo(clientId), scoreSystem.GetScoreDTO());
    HandleRegionClearedClientRpc(clearEvent); // 전 클라이언트 동기화
}
```

판정을 서버에 집중시켜 클라이언트가 조작된 영역을 보내도 검증에서 걸러집니다. `GridController`는 실제로 **`ServerGrid`(+ `ValidRegionChecker`)와 `LocalGrid`를 분리**해 보유하며, 검증은 서버 그리드에서만 수행합니다. 추후 DGS(전용 서버)로 전환할 때 서버 검증 코드를 그대로 이전할 수 있도록 한 설계입니다.

### 2D Prefix Sum 기반 영역 합 계산

서버는 매 선택마다 영역 합을 검증해야 합니다. 매번 영역 내 셀을 전부 순회하면 영역 크기에 비례한 비용이 들지만, 2D Prefix Sum 테이블을 한 번 구축해두면 **임의 영역의 합을 O(1)로** 구할 수 있습니다.

```csharp
// ValidRegionChecker.cs — Prefix Sum 테이블 구축 (그리드 변경 시 호출)
public void Compute()
{
    for (var row = 0; row < _grid.RowCount; row++)
        for (var col = 0; col < _grid.ColCount; col++)
        {
            var cell = _grid[row, col];
            var value = cell.Cleared ? 0 : cell.Value;
            _prefixSumGrid[row + 1, col + 1] =
                value + _prefixSumGrid[row, col + 1] + _prefixSumGrid[row + 1, col] - _prefixSumGrid[row, col];
        }
}

// 임의 영역의 합을 O(1)로 계산
public int GetRegionSum(Region region)
{
    return _prefixSumGrid[region.maxRow + 1, region.maxCol + 1]
         - _prefixSumGrid[region.minRow, region.maxCol + 1]
         - _prefixSumGrid[region.maxRow + 1, region.minCol]
         + _prefixSumGrid[region.minRow, region.minCol];
}

public bool IsValidRegion(Region region) => GetRegionSum(region) == GameRule.TARGET_SUM;
```

여기에 더해, 검증된 영역을 네 변에서 한 줄씩 줄여가며 **최소 유효 영역으로 트림**합니다. 동일한 선택이라도 정규화된 영역으로 다루어, 이후 처리의 일관성과 표현력을 함께 끌어올렸습니다.

```csharp
// ValidRegionChecker.cs — 합이 유지되는 한 네 변에서 영역 축소
public Region TrimMinimalValidRegion(Region region)
{
    while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromBottom)) region = region.ShrinkFromBottom;
    while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromTop))    region = region.ShrinkFromTop;
    while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromLeft))   region = region.ShrinkFromLeft;
    while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromRight))  region = region.ShrinkFromRight;
    return region;
}
```

### 서비스 로케이터 패턴

전역에서 필요한 매니저는 **구현체 / 인터페이스 / static 접근자** 세 부분으로 분리했습니다. 예를 들어 오디오는 `AudioManager`(구현체) · `IAudioManager`(인터페이스) · `TMAudioManager`(static 접근자)로 나뉩니다. 호출부는 `TMAudioManager.Instance.PlaySfx(...)` 처럼 인터페이스로만 접근해, 구현체 교체나 테스트 대역 주입이 자유롭습니다.

```csharp
// TMAudioManager.cs — 인터페이스만 노출하는 static 접근자
public static class TMAudioManager
{
    public static IAudioManager Instance { get; private set; }

    public static void Initialize(IAudioManager manager) // 부팅 시 구현체 등록
    {
        if (Instance != null) { TBMLog.HeaderError("already initialized"); return; }
        Instance = manager;
    }

    public static void Deinitialize(IAudioManager manager)
    {
        if (manager != Instance) return;
        Instance = null;
    }
}
```

전 시스템에 이벤트 채널을 깔았던 이전 프로젝트(마왕 수박 v1)의 경험을 거쳐, 이번에는 전역 접근이 꼭 필요한 *매니저 객체에 한정*해 이 구조를 적용했습니다.

---

## 핵심 코드 구조

```
08_TenMaker/
├── GamePlay/
│   ├── GridSystem/
│   │   ├── Grid/Multiplay/        # 책임 분리된 그리드 ★
│   │   │   └── GridController.cs  # 네트워크 동기화
│   │   └── Region/                # 영역 검증 ★
│   │       ├── RegionSumCalculator.cs   # 2D Prefix Sum
│   │       └── ValidRegionChecker.cs    # 검증 + Trim
│   ├── Player/Multiplay/
│   │   ├── PlayerInputController.cs     # Owner 전용 입력 → 서버 요청
│   │   ├── PlayerController.cs
│   │   └── PlayerNetworkSpawner.cs
│   └── RegionClear/
│       └── RegionSelectionHandler.cs    # 서버 검증 + 동기화 ★
├── Services/UGS/
│   ├── Multiplay/                       # Relay 연결
│   │   ├── MultiplayManager.cs
│   │   └── TMMultiplayService.cs
│   ├── Authentication/AuthenticationManager.cs # 게스트 로그인
│   └── Leaderboards/LeaderboardsManager.cs     # 랭킹
├── Core/
│   ├── Audio/                     # 서비스 로케이터 예시 ★
│   │   ├── AudioManager.cs        # 구현체
│   │   ├── IAudioManager.cs       # 인터페이스
│   │   └── TMAudioManager.cs      # static 접근자
│   └── Region/                    # 싱글플레이 레거시 (Before 비교용)
└── ...
```
