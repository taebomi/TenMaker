using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using TenMaker.Gameplay.CameraSystem;
using TenMaker.Utility;
using Unity.Cinemachine;
using UnityEngine;

namespace TenMaker.Core
{
    public class CameraShaker : MonoBehaviour
    {
        private const float EASE_OUT_DURATION = 0.25f;

        [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
        
        public bool IsShaking { get; private set; }

        private List<CameraShakeData> _oriDataList;
        private List<CameraShakeData> _curDataList;
        private IEnumerator _shakeCoroutine;

        private void Awake()
        {
            noise.enabled = false;
            IsShaking = false;

            _oriDataList = new List<CameraShakeData>();
            _curDataList = new List<CameraShakeData>();
        }

        public void Shake(CameraShakeData shakeData)
        {
            _oriDataList.Add(shakeData);
            _curDataList.Add(shakeData);

            if (IsShaking) return;
            IsShaking = true;

            _shakeCoroutine = ShakeCoroutine();
            StartCoroutine(_shakeCoroutine);
        }

        public void Stop()
        {
            if (IsShaking is false) return;
            IsShaking = false;

            StopCoroutine(_shakeCoroutine);
            noise.AmplitudeGain = 0f;
            noise.enabled = false;
        }

        private IEnumerator ShakeCoroutine()
        {
            IsShaking = true;
            noise.enabled = true;

            while (_oriDataList.Count > 0)
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
                        var ease = TBMUtility.InOutSine(curData.duration, easeDuration);
                        curData.intensity = oriData.intensity * ease;
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

                noise.AmplitudeGain = maxIntensity;
                yield return null;
            }

            IsShaking = false;
            noise.AmplitudeGain = 0f;
            noise.enabled = false;
        }
    }
}