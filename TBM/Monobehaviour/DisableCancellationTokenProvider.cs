using System;
using System.Threading;
using UnityEngine;

namespace TenMaker.Utility
{
    public static partial class TBMUtility
    {
        public static DisableCancellationTokenProvider GetDisableCancellationTokenProvider(
            this MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour.TryGetComponent(out DisableCancellationTokenProvider component) is false)
            {
                component = monoBehaviour.gameObject.AddComponent<DisableCancellationTokenProvider>();
            }

            return component;
        }
    }

    public class DisableCancellationTokenProvider : MonoBehaviour
    {
        private CancellationTokenSource _cts;

        /// <summary>
        /// Start 이후, SetActive(true) 이후에만 사용 가능 
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                if (_cts is null)
                {
                    _cts = new CancellationTokenSource();
                    return _cts.Token;
                }

                if (_cts.IsCancellationRequested)
                {
                    _cts.Dispose();
                    _cts = new CancellationTokenSource();
                    return _cts.Token;
                }

                return _cts.Token;
            }
        }

        private void OnDisable()
        {
            _cts?.Cancel();
        }

        private void OnDestroy()
        {
            _cts?.Dispose();
            _cts = null;
        }
    }
}