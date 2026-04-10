using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TenMaker.UI
{
    public class YoyoAnimationImage : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float delaySec;

        [SerializeField] private Image image;
        
        private int _idx;
        private int _direction = 1;
        private float _elapsed;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void Update()
        {    
            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed < delaySec) return;

            _elapsed = 0f;
            image.sprite = sprites[_idx];
            _idx += _direction;

            if (_idx == 0 || _idx == sprites.Length - 1)
                _direction *= -1;
        }
    }
}