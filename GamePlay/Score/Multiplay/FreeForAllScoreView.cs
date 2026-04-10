using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay.Score.Multiplay
{
    // 구현 하다 말았음
    public class FreeForAllScoreView : MonoBehaviour
    {
        private List<ulong> _clientIdRenderOrder;
        private Dictionary<ulong, PlayerScoreViewData> _scoreDataDict;

        private Dictionary<ulong, PlayerSegment> _playerSegments;

        public void Initialize(PlayersContext playerContext)
        {
            _clientIdRenderOrder = new List<ulong>(playerContext.AllPlayers.Count);
            _scoreDataDict = new Dictionary<ulong, PlayerScoreViewData>(playerContext.AllPlayers.Count);

            // 로컬 플레이어는 항상 마지막에 위치하도록
            foreach (var player in playerContext.AllPlayers.Values)
            {
                if (player != playerContext.LocalPlayer)
                {
                    _clientIdRenderOrder.Add(player.OwnerClientId);
                }

                _scoreDataDict.Add(player.OwnerClientId, new PlayerScoreViewData());
            }

            _clientIdRenderOrder.Add(playerContext.LocalPlayer.OwnerClientId);
        }

        /// <summary>
        /// 점수 갱신
        /// </summary>
        public void UpdateScore(List<PlayerScoreViewData> scores)
        {
            foreach (var scoreData in scores)
            {
                _scoreDataDict[scoreData.ClientId] = scoreData;
            }

            foreach (var clientId in _clientIdRenderOrder)
            {
                _playerSegments[clientId].Score = _scoreDataDict[clientId].Score;
            }
        }

        private async UniTask UpdateScoreBarAsync(CancellationToken ct)
        {
            // Player Segment 확인

            await UniTask.CompletedTask;
        }
    }
}