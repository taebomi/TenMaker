using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Gameplay.Score.Multiplay
{
    public class DuelScoreView : MonoBehaviour, IScoreView
    {
        private const float MIN_RATIO = 0.1f;
        [SerializeField] private PlayerSegment playerSegmentPrefab;

        private Dictionary<ulong, PlayerSegment> _playerSegmentDict;
        private List<PlayerSegment> _orderedSegments;

        private List<float> _segmentBoundaries;
        private List<float> _displayBoundaries;

        // Components
        private RectTransform _rt;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }


        #region Initialization

        public void Initialize(PlayersContext playerContext)
        {
            InitializePlayerSegments(playerContext);
            InitializeBoundaries();
            ShowInitialView();
            UpdateAsync(destroyCancellationToken).Forget();
        }

        /// <summary>
        /// 플레이어 Segment 생성
        /// </summary>
        private void InitializePlayerSegments(PlayersContext playerContext)
        {
            _playerSegmentDict = new Dictionary<ulong, PlayerSegment>(playerContext.AllPlayers.Count);
            _orderedSegments = new List<PlayerSegment>(playerContext.AllPlayers.Count);
            foreach (var playerObject in playerContext.RemotePlayers.Values)
            {
                CreatePlayerSegment(playerObject);
            }

            CreatePlayerSegment(playerContext.LocalPlayer);
        }

        private void CreatePlayerSegment(NetworkPlayerObject playerObject)
        {
            var playerSegment = Instantiate(playerSegmentPrefab, _rt);
            playerSegment.Initialize(playerObject.Color);
            _playerSegmentDict.Add(playerObject.OwnerClientId, playerSegment);
            _orderedSegments.Add(playerSegment);
        }

        /// <summary>
        /// Segment의 Boundary 초기화, 동일 비율로 분할
        /// </summary>
        private void InitializeBoundaries()
        {
            var boundaryCount = _orderedSegments.Count - 1;
            _segmentBoundaries = new List<float>(boundaryCount);
            _displayBoundaries = new List<float>(boundaryCount);

            _displayBoundaries.Add(0f);
            if (boundaryCount >= 1)
            {
                var accumulated = 0f;
                var ratio = 1f / _orderedSegments.Count;
                for (var idx = 0; idx < boundaryCount; idx++)
                {
                    accumulated += ratio;
                    _segmentBoundaries.Add(accumulated);
                    _displayBoundaries.Add(accumulated);
                }
            }

            _displayBoundaries.Add(1f);
        }

        /// <summary>
        /// 초기화한 값 적용
        /// </summary>
        private void ShowInitialView()
        {
            for (var i = 0; i < _orderedSegments.Count; i++)
            {
                var segment = _orderedSegments[i];
                segment.SetSize(_displayBoundaries[i], _displayBoundaries[i + 1]);
            }
        }

        #endregion

        private async UniTask UpdateAsync(CancellationToken ct)
        {
            while (ct.IsCancellationRequested is false)
            {
                for (var i = 0; i < _orderedSegments.Count; i++)
                {
                    var segment = _orderedSegments[i];
                    segment.SetSize(_displayBoundaries[i], _displayBoundaries[i + 1]);
                }

                await UniTask.Yield(ct);
            }
        }

        public void UpdateScore(List<PlayerScoreViewData> scoreDataList)
        {
            UpdateSegmentScore(scoreDataList);
            CalculateBoundaries();
        }

        private void UpdateSegmentScore(List<PlayerScoreViewData> scoreDataList)
        {
            foreach (var scoreData in scoreDataList)
            {
                if (_playerSegmentDict.TryGetValue(scoreData.ClientId, out var playerSegment) is false)
                {
                    TBMLog.HeaderWarning($"PlayerSegment not found for ClientId: {scoreData.ClientId}");
                    continue;
                }

                playerSegment.SetData(scoreData);
            }
        }

        /// <summary>
        /// 플레이어들의 점수 비율을 계산하여 Segment의 Boundary 설정
        /// </summary>
        private void CalculateBoundaries()
        {
            var totalScore = 0;
            foreach (var playerSegment in _orderedSegments)
            {
                totalScore += playerSegment.Score;
            }

            if (totalScore == 0) return;

            var ratio = 1f / totalScore;
            var currentBoundary = 0f;

            for (var index = 0; index < _segmentBoundaries.Count; index++)
            {
                var playerSegment = _orderedSegments[index];
                var segmentRatio = Mathf.Max(playerSegment.Score * ratio, MIN_RATIO);
                currentBoundary += segmentRatio;
                _displayBoundaries[index + 1] = currentBoundary;
            }
        }

        public void PulsePlayer(int index) { }
    }
}