using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Gameplay.Score.Multiplay.Data;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;
using Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using GridSystem_Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;

namespace TenMaker.Gameplay.Score.Multiplay
{
    public class ScoreSystem : NetworkBehaviour
    {
        [TitleGroup("References"), SerializeField]
        private GameObject viewGameObject;

        [TitleGroup("Runtime Data")]
        [ShowInInspector, ReadOnly] public Dictionary<ulong, int> ServerPlayerScores { get; private set; }
        [ShowInInspector, ReadOnly] public Dictionary<ulong, int> ClientPlayerScores { get; private set; }


        [TitleGroup("Runtime Dependencies")]
        [ShowInInspector, ReadOnly] private GridSystem_Multiplay_Grid _serverGrid;
        [ShowInInspector, ReadOnly] private PlayersContext _context;
        [ShowInInspector, ReadOnly] private IScoreView _view;

        private void Awake()
        {
            _view = viewGameObject.GetComponent<IScoreView>();
        }

        public void InitializeServer(IEnumerable<ulong> clientIds, GridSystem_Multiplay_Grid grid)
        {
            ServerPlayerScores = new Dictionary<ulong, int>();
            foreach (var clientId in clientIds)
            {
                if (ServerPlayerScores.TryAdd(clientId, 0) is false)
                {
                    TBMLog.HeaderError($"{clientId} already exists in PlayerScores.");
                    continue;
                }
            }

            _serverGrid = grid;
        }

        public void Initialize(PlayersContext playerContext)
        {
            _context = playerContext;

            ClientPlayerScores = new Dictionary<ulong, int>(playerContext.AllPlayers.Count);
            foreach (var clientId in playerContext.AllClientIds)
            {
                if (ClientPlayerScores.TryAdd(clientId, 0) is false)
                {
                    TBMLog.HeaderError($"{clientId} already exists in PlayerScores.");
                    continue;
                }
            }

            _view.Initialize(playerContext);
        }

        /// <summary>
        /// Score 재계산
        /// </summary>
        public void UpdateScoreServer()
        {
            foreach (var clientId in _context.AllClientIds)
            {
                ServerPlayerScores[clientId] = 0;
            }

            foreach (var cell in _serverGrid.Cells)
            {
                if (!cell.Cleared) continue;

                if (ServerPlayerScores.ContainsKey(cell.ClearedClientId) is false)
                {
                    TBMLog.HeaderError($"{cell.ClearedClientId} not found in PlayerScores.");
                    continue;
                }

                ServerPlayerScores[cell.ClearedClientId]++;
            }
        }

        public ScoreDTO GetScoreDTO()
        {
            return new ScoreDTO(ServerPlayerScores);
        }

        public void UpdateScore(ScoreDTO scoreData)
        {
            var clients = scoreData.ClientIds;
            var scores = scoreData.Scores;
            if (clients.Length != scores.Length)
            {
                TBMLog.HeaderError($"{nameof(ScoreDTO)} data is invalid. ClientIds and Scores length mismatch.");
                return;
            }

            for (var idx = 0; idx < clients.Length; idx++)
            {
                if (ClientPlayerScores.ContainsKey(clients[idx]))
                {
                    ClientPlayerScores[clients[idx]] = scores[idx];
                }
                else
                {
                    TBMLog.HeaderError($"ClientId {clients[idx]} not found in ClientPlayerScores.");
                }
            }

            var scoreDataList = new List<PlayerScoreViewData>(clients.Length);
            for (var idx = 0; idx < clients.Length; idx++)
            {
                scoreDataList.Add(new PlayerScoreViewData(clients[idx], scores[idx]));
            }

            _view.UpdateScore(scoreDataList);
        }
    }
}