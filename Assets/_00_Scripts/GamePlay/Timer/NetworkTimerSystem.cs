using System;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Timer
{
    public class NetworkTimerSystem : NetworkBehaviour
    {
        public Timer Timer { get; private set; }
        [SerializeField] private TimerUI timerUI;

        public event Action TimerFinished;
        private NetworkTimeSource _timeSource;

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (Timer != null)
            {
                Timer.Stop();
                Timer.TimerUpdated -= OnTimerUpdated;
                Timer.TimerFinished -= OnTimerFinished;
            }
        }

        public void InitializeClient()
        {
            Timer = new Timer(new NetworkTimeSource());
            Timer.TimerUpdated += OnTimerUpdated;
            Timer.TimerFinished += OnTimerFinished;
        }

        public void StartTimerServer(double totalTime)
        {
            StartTimerClientRpc(totalTime, new NetworkTimeSource().GetTime());
        }

        [Rpc(SendTo.ClientsAndHost, RequireOwnership = true)]
        private void StartTimerClientRpc(double totalTime, double startedTime)
        {
            timerUI.SetTotalTime((float)totalTime);
            Timer.Start(totalTime, startedTime, destroyCancellationToken);
        }

        /// <summary>
        /// 오차로 인해 클라이언트 타이머 종료되지 않았을 경우, 추가 종료 처리 
        /// </summary>
        [Rpc(SendTo.NotServer, RequireOwnership = true)]
        private void FinishTimerClientRpc()
        {
            if (Timer.IsRunning is false) return;

            Timer.Stop();
            OnTimerFinished();
        }

        #region Callbacks

        private void OnTimerUpdated(double remainedTime)
        {
            if (IsClient)
            {
                timerUI.UpdateRemainedTime((float)remainedTime);
            }
        }

        private void OnTimerFinished()
        {
            if (IsServer)
            {
                FinishTimerClientRpc();
            }

            if (IsClient)
            {
                timerUI.UpdateRemainedTime(0f);
                timerUI.ResetVisual();
                TimerFinished?.Invoke();
            }
        }

        #endregion
    }
}