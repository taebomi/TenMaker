# TenMaker
> 써드파티 에셋 라이선스로 인해 전체 프로젝트가 아닌 C# 스크립트만 포함

> 퍼즐 (사과게임 류) | Unity, C# | 1인 개발 | PC · Android | 미출시 (싱글 · 멀티 타임어택 모드 구현)
> 개발 기간 2024.05 – 2024.09

<img src="https://github.com/user-attachments/assets/aab9ebcb-43c4-4e0d-9b74-955c5aedcf6b" width="440" />

🔗 [멀티플레이 영상](https://youtu.be/l_5srmBzxZI)

---

## 기술 스택

- **Unity, C#**
- **Netcode for GameObjects (NGO)** — 서버 권위 멀티플레이 (입력을 서버에서 검증·동기화)
- **Unity Relay** — 호스트·클라이언트 연결 중계 (NAT 우회)
- **Unity Gaming Services (UGS)** — Authentication(게스트 로그인), Leaderboards(랭킹)
- **UniTask** — UGS 비동기 연동, 취소 토큰 전파·수명 관리

---

## 게임 규칙

보드 위 1~9 숫자 타일에서 **합이 10이 되는 사각 영역**을 선택해 제거하고, 그 영역을 자신의 땅으로 차지합니다. 최종적으로 확보한 타일 수로 점수를 겨룹니다.

---

## 핵심 1 — 서버 권위 멀티플레이 (NGO + Relay)

| 파일 | 역할 |
|------|------|
| [PlayerInputController.cs](GamePlay/Player/Multiplay/PlayerInputController.cs) | Owner 전용 드래그 입력 → 서버에 ServerRpc로 *요청* |
| [GridController.cs](GamePlay/GridSystem/Grid/Multiplay/GridController.cs) | 서버 그리드에서 검증·Trim·반영 + Server/Local Grid 분리 |
| [RegionSelectionHandler.cs](GamePlay/RegionClear/RegionSelectionHandler.cs) | 서버 판정 오케스트레이션 + 결과 ClientRpc 브로드캐스트 |

**동기** — 멀티플레이를 단순 구현에 그치지 않고, **실제 라이브 서비스에서 쓰일 법한 치팅에 안전한 구조**로 만드는 것을 목표로 잡았습니다.

**기술 선택** — Photon·Mirror 같은 외부 솔루션 대신, Unity가 멀티플레이를 자체 생태계(UGS)로 통합해 가는 흐름이라 장기 학습 가치가 크다고 보고 **NGO + Relay**를 택했습니다. 치팅에 취약한 클라이언트 권위 대신, **서버가 검증한 결과만 동기화하는 서버 권위 구조**로 설계했습니다.

**구현** — 흐름을 `입력 → 검증 → 동기화` 한 방향으로 단순화했습니다. 클라이언트는 입력과 연출만 담당하고, 게임 규칙은 서버가 단일 진실 공급원(SSOT)으로 통제합니다.

```csharp
// PlayerInputController.cs — 드래그 종료 시: 로컬 반영 후 서버로 '요청'만 (Owner 전용)
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
// GridController.cs — 서버 그리드에서만 [검증 → Trim → 반영], 무효 선택은 null로 차단
public RegionClearResult? ClearRegionServer(ulong clearedClientId, Region region)
{
    if (ValidRegionChecker.IsValidRegion(region) is false) return null;          // 합=10 검증 (치팅 차단)

    region = ValidRegionChecker.TrimMinimalValidRegion(region);                  // 최소 유효 영역 정규화
    var clearedCellCoordinates = ServerGrid.ClearRegion(clearedClientId, region);
    if (clearedCellCoordinates.Count == 0) return null;

    ValidRegionChecker.Compute();                                               // Prefix Sum 갱신
    return new RegionClearResult(region, clearedCellCoordinates);
}
```

이 흐름을 직접 설계하며, 멀티플레이는 단순한 패킷 교환이 아니라 **누가 결정 권한을 갖는가**의 문제임을 체감했습니다.

**한계와 대비** — 현 구조에는 넷코드의 근본적 한계 두 가지가 있습니다. 둘 다 개선 방향을 확인했고, 그중 **호스트 치팅 문제는 구조적으로 미리 대비**해 두었습니다.

| 한계 | 직면한 문제 | 개선 방향 |
|------|------------|----------|
| **입력 반응 지연** (RPC 왕복) | 입력이 서버 검증을 거친 뒤 반영되어 네트워크 지연만큼 반응이 느려짐 | 클라이언트 예측(Client-side Prediction)으로 완화 가능함을 *확인* |
| **호스트 치팅** (Host-Client) | 호스트가 서버를 겸해 본인 치팅을 원천 차단 불가 | 서버 판정용 **Server Grid**와 표시용 **Local Grid**를 처음부터 분리 → 로직 유지한 채 Dedicated Server 전환 가능하도록 *대비* |

---

## 핵심 2 — 2D Prefix Sum 기반 영역 합 최적화

| 파일 | 역할 |
|------|------|
| [GridGenerator.cs](GamePlay/GridSystem/Grid/Common/GridGenerator.cs) | 시작 보드 생성 + 최소 정답 수 검증 (Prefix Sum) |
| [ValidRegionChecker.cs](GamePlay/GridSystem/Region/ValidRegionChecker.cs) | 런타임 영역 합 O(1) 검증 + 최소 유효 영역 Trim |
| [Region.cs](GamePlay/GridSystem/Region/Region.cs) | `ShrinkFrom*` 4방향 축소 프리미티브 |

**동기** — 임의의 사각 영역 합을 반복해서 구해야 하는 핵심 연산이 두 곳 있었습니다.
- **시작 보드 검증** — 무작위 보드에 풀 수 있는 정답(합이 10인 최소 영역)이 기준치(`30`개) 이상인지 확인, 부족하면 재생성
- **런타임 영역 정규화(Trim)** — 빈 칸을 포함해 넓게 드래그해도 점수 규칙에 맞춰 '최소 유효 영역'으로 보정

**해결** — 단순 4중 반복은 영역 후보 `O(r²c²)`개마다 `O(rc)` 합산이 들어 총 `O(r³c³)`까지 커집니다. 격자를 `O(rc)`로 한 번 전처리해두면 임의 영역 합을 `O(1)`로 조회하는 **2D Prefix Sum(누적합)** 을 도입해, 시작 검증을 `O(r²c²)`로 한 차수 낮췄습니다. (현 보드 17×10은 작지만 확장 대비.) 댕댕이 서바이벌에서 단순 연산의 한계를 Flow Field로 돌파했던 경험을 떠올려 적용했습니다.

```csharp
// ValidRegionChecker.cs — Prefix Sum 전처리 (그리드 변경 시 호출). 이후 임의 영역 합을 O(1)로 조회
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

public int GetRegionSum(Region region) =>
      _prefixSumGrid[region.maxRow + 1, region.maxCol + 1] - _prefixSumGrid[region.minRow, region.maxCol + 1]
    - _prefixSumGrid[region.maxRow + 1, region.minCol]     + _prefixSumGrid[region.minRow, region.minCol];

public bool IsValidRegion(Region region) => GetRegionSum(region) == GameRule.TARGET_SUM;
```

**구현 — 런타임 Trim** — 검증된 영역을 합이 유지되는 한 네 변에서 한 줄씩 줄여 '최소 유효 영역'으로 정규화합니다. `Region`의 `ShrinkFrom*` 프로퍼티로 축소 의도가 코드에 그대로 드러납니다.

```csharp
// ValidRegionChecker.cs — 합이 유지되는 한 네 변에서 한 줄씩 축소 → 최소 유효 영역
public Region TrimMinimalValidRegion(Region region)
{
    while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromBottom)) region = region.ShrinkFromBottom;
    while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromTop))    region = region.ShrinkFromTop;
    while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromLeft))   region = region.ShrinkFromLeft;
    while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromRight))  region = region.ShrinkFromRight;
    return region;
}
```

**결론** — 한 번의 전처리로 시작 검증과 런타임 Trim 두 곳을 동시에 최적화했고, `ShrinkFrom*` 4방향 축소 프로퍼티로 Trim 로직의 가독성까지 끌어올렸습니다.

---

## 핵심 3 — UGS 비동기 연동 · 취소/예외 처리

| 파일 | 역할 |
|------|------|
| [AuthenticationManager.cs](Services/UGS/Authentication/AuthenticationManager.cs) | 게스트 로그인 async, 취소·예외 구분 처리 |
| [LeaderboardsManager.cs](Services/UGS/Leaderboards/LeaderboardsManager.cs) | 랭킹 조회, `UniTask.WhenAll`로 동시 요청 |

**설계** — UGS 네트워크 작업을 `UniTask<Result<T>>`로 감싸 예외를 throw 대신 값으로 반환하고, `CancellationToken`을 전 계층에 전파했습니다. 취소(`OperationCanceledException`)는 도메인 예외와 구분해 처리합니다.

```csharp
// AuthenticationManager.cs — 취소를 도메인 예외와 구분, 결과는 Result로 반환
try
{
    await Service.SignInAnonymouslyAsync().AsUniTask().AttachExternalCancellation(ct);
}
catch (AuthenticationException ex) { return Result<SignInPayload>.Fail(ex.ErrorCode, new SignInPayload(ex.Notifications)); }
catch (OperationCanceledException) { return Result<SignInPayload>.Fail(TMAuthenticationErrorCode.USER_CANCELLED, new SignInPayload()); }
```

---

## 서비스 로케이터 — 전역 매니저 접근 (적용 예시)

| 파일 | 역할 |
|------|------|
| [TMAudioManager.cs](Core/Audio/TMAudioManager.cs) | 인터페이스만 노출하는 static 접근자 |

전역에서 필요한 매니저는 **구현체 / 인터페이스 / static 접근자** 세 부분으로 분리했습니다. 오디오는 `AudioManager`(구현체) · `IAudioManager`(인터페이스) · `TMAudioManager`(static 접근자)로 나뉘고, 호출부는 인터페이스로만 접근해 구현체 교체·테스트 대역 주입이 자유롭습니다.

```csharp
// TMAudioManager.cs — 인터페이스만 노출하는 static 접근자 (부팅 시 구현체 1회 등록)
public static class TMAudioManager
{
    public static IAudioManager Instance { get; private set; }

    public static void Initialize(IAudioManager manager)
    {
        if (Instance != null) { TBMLog.HeaderError($"{typeof(TMAudioManager)} is already initialized."); return; }
        Instance = manager;
    }
    // ...
}
```

```csharp
// 호출부 — 구현체를 모른 채 인터페이스로만 사용 (SettingsUI.cs)
TMAudioManager.Instance.SetBgmVolume(value);
```

전 시스템에 이벤트 채널을 깔았던 이전 프로젝트(마왕 수박)의 경험을 거쳐, 이번에는 전역 접근이 꼭 필요한 *매니저 객체에 한정*해 이 구조를 적용했습니다.

---

## 핵심 코드 구조
<pre>
08_TenMaker/
├── GamePlay/
│   ├── Player/Multiplay/
│   │   └── <a href="GamePlay/Player/Multiplay/PlayerInputController.cs">PlayerInputController.cs</a>   # Owner 입력 → ServerRpc 요청
│   ├── RegionClear/
│   │   └── <a href="GamePlay/RegionClear/RegionSelectionHandler.cs">RegionSelectionHandler.cs</a>  # 서버 판정 + ClientRpc 브로드캐스트
│   └── GridSystem/
│       ├── Grid/Multiplay/<a href="GamePlay/GridSystem/Grid/Multiplay/GridController.cs">GridController.cs</a>   # Server/Local Grid 분리·검증·동기화 ★
│       ├── Grid/Common/<a href="GamePlay/GridSystem/Grid/Common/GridGenerator.cs">GridGenerator.cs</a>      # 보드 생성 + Prefix Sum 검증
│       └── Region/
│           ├── <a href="GamePlay/GridSystem/Region/ValidRegionChecker.cs">ValidRegionChecker.cs</a>           # Prefix Sum 검증 + Trim ★
│           └── <a href="GamePlay/GridSystem/Region/Region.cs">Region.cs</a>                       # ShrinkFrom* 4방향 축소
├── Core/Audio/                         # 서비스 로케이터 적용 예시
│   ├── <a href="Core/Audio/AudioManager.cs">AudioManager.cs</a>                  # 구현체
│   ├── <a href="Core/Audio/IAudioManager.cs">IAudioManager.cs</a>                 # 인터페이스
│   └── <a href="Core/Audio/TMAudioManager.cs">TMAudioManager.cs</a>                # static 접근자
└── Services/UGS/
    ├── Multiplay/<a href="Services/UGS/Multiplay/MultiplayManager.cs">MultiplayManager.cs</a>           # Relay 연결 중계
    ├── Authentication/<a href="Services/UGS/Authentication/AuthenticationManager.cs">AuthenticationManager.cs</a>  # 게스트 로그인 — async·취소/예외 구분
    └── Leaderboards/<a href="Services/UGS/Leaderboards/LeaderboardsManager.cs">LeaderboardsManager.cs</a>       # 랭킹 — UniTask.WhenAll 동시 조회
</pre>
