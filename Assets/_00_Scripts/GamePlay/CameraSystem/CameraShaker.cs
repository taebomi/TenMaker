using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility;
using Unity.Cinemachine;
using UnityEngine;

namespace TenMaker.Gameplay.CameraSystem
{
    public class CameraShaker
    {
        private const float EASE_OUT_DURATION = 0.25f;

        public bool IsShaking { get; private set; }

        private readonly CinemachineBasicMultiChannelPerlin _perlin;

        private readonly List<CameraShakeData> _oriDataList = new();
        private readonly List<CameraShakeData> _curDataList = new();

        private CancellationTokenSource _shakingCts;

        public CameraShaker(CinemachineBasicMultiChannelPerlin perlin)
        {
            _perlin = perlin;
            _perlin.enabled = false;
        }

        public void Shake(CameraShakeData data, CancellationToken externalToken)
        {
            _oriDataList.Add(data);
            _curDataList.Add(data);

            if (IsShaking) return;
            IsShaking = true;
            _perlin.enabled = true;

            _shakingCts?.Dispose();
            _shakingCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            ShakeAsync(_shakingCts.Token).Forget();
        }

        public void Stop()
        {
            if (IsShaking is false) return;
            IsShaking = false;
            _shakingCts.Cancel();

            _perlin.enabled = false;
            _perlin.AmplitudeGain = 0f;
            _oriDataList.Clear();
            _curDataList.Clear();
        }


        private async UniTaskVoid ShakeAsync(CancellationToken ct)
        {
            while (_oriDataList.Count > 0 && ct.IsCancellationRequested is false)
            {
                var maxIntensity = 0f;
                for (var idx = _oriDataList.Count - 1; idx >= 0; idx--)
                {
                    var oriData = _oriDataList[idx];
                    var curData = _curDataList[idx];

                    var easeDuration = oriData.duration < EASE_OUT_DURATION ? oriData.duration : EASE_OUT_DURATION;
                    if (curData.duration >= easeDuration)
                    {
                        curData.intensity = oriData.intensity;
                    }
                    else
                    {
                        var easing = TBMUtility.InOutSine(curData.duration, easeDuration);
                        curData.intensity = oriData.intensity * easing;
                    }

                    curData.duration -= Time.deltaTime;
                    if (curData.duration <= 0f)
                    {
                        _oriDataList.RemoveAt(idx);
                        _curDataList.RemoveAt(idx);
                        continue;
                    }

                    _curDataList[idx] = curData;
                    maxIntensity = Mathf.Max(maxIntensity, curData.intensity);
                }

                _perlin.AmplitudeGain = maxIntensity;
                await UniTask.Yield(ct);
            }

            IsShaking = false;
            _perlin.enabled = false;
            _perlin.AmplitudeGain = 0f;
        }
    }
}