using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility.Core;

namespace TenMaker.Services.UGS.Multiplay
{
    public interface ITMMultiplayService
    {
        bool IsServer { get; }
        bool IsClient { get; }

        /// <summary>
        /// 호스트 전용. MAX_PLAYERS 모두 연결됐을 때 발생.
        /// </summary>
        event Action OnAllPlayersConnected;

        UniTask<Result<CreateRoomPayload>> CreateRoomAsync(CancellationToken ct);
        UniTask<Result> JoinRoomAsync(string joinCode, CancellationToken ct);
        UniTask LeaveRoomAsync();

        /// <summary>
        /// 호스트는 씬 로드 명령, 클라이언트는 NGO가 자동 처리하므로 no-op.
        /// </summary>
        void StartGame();
    }
}
