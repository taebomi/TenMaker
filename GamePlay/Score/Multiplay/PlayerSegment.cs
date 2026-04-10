using System;
using TenMaker.Gameplay;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.Gameplay.Score.Multiplay
{
    public class PlayerSegment : MonoBehaviour
    {
        public PlayerSegment LeftSegment { get; private set; }
        public PlayerSegment RightSegment { get; private set; }
        public ulong ClientId { get; private set; }
        public int Score { get; set; }

        public PlayerScoreViewData Data { get; private set; }

        private Image _sr;
        private RectTransform _rt;

        [SerializeField] private PlayerColorSO colorSO;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _sr = GetComponent<Image>();
        }

        public void Initialize(PlayerColor color)
        {
            _sr.color = colorSO.Colors[color];
        }

        public void SetData(PlayerScoreViewData data)
        {
            Score = data.Score;
        }

        public void SetSize(float minX, float maxX)
        {
            _rt.anchorMin = new Vector2(minX, 0f);
            _rt.anchorMax = new Vector2(maxX, 1f);
            _rt.offsetMin = Vector2.zero;
            _rt.offsetMax = Vector2.zero;
        }
    }
}