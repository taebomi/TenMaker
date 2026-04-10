using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Combo;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public class ComboController : NetworkBehaviour
    {
        [TitleGroup("References")]
        [SerializeField] private ComboView view;

        [TitleGroup("Runtime Data")]
        [ShowInInspector, ReadOnly]public Dictionary<ulong, ComboTracker> ServerComboTrackers { get; private set; }

        public void InitializeServer(IEnumerable<ulong> clientIds)
        {
            CreateComboTrackers(clientIds);
        }

        private void CreateComboTrackers(IEnumerable<ulong> clientIds)
        {
            ServerComboTrackers = new Dictionary<ulong, ComboTracker>();
            foreach (var clientId in clientIds)
            {
                if (ServerComboTrackers.TryAdd(clientId, new ComboTracker()) is false)
                {
                    TBMLog.HeaderError($"ComboTracker initialized for client {clientId}");
                }
            }
        }

        public int GetCombo(ulong clientId)
        {
            if (ServerComboTrackers.TryGetValue(clientId, out var comboTracker) is false)
            {
                TBMLog.HeaderLog($"Client Id({clientId}) not exists in ComboTrackers.");
                return 0;
            }

            return comboTracker.CurrentCombo;
        }

        /// <summary>
        /// 콤보 증가
        /// </summary>
        public void IncreaseComboServer(ulong clientId)
        {
            if (ServerComboTrackers.TryGetValue(clientId, out var comboTracker) is false)
            {
                TBMLog.HeaderLog($"Client Id({clientId}) not exists in ComboTrackers.");
                return;
            }

            comboTracker.IncreaseCombo(destroyCancellationToken);
        }

        public void PlayComboEffect(int combo, Vector3 position)
        {
            view.ShowCombo(combo, position);
        }
    }
}