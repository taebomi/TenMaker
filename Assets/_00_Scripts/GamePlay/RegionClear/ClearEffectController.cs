using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay;
using TenMaker.Gameplay.RegionClear;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public class ClearEffectController : MonoBehaviour
    {
        public ClearEffector LocalEffector { get; private set; }
        public Dictionary<ulong, ClearEffector> ClearEffectors { get; private set; } = new();

        public void Register(NetworkPlayerObject playerObject)
        {
            var provider = playerObject.GetComponentInChildren<IClearEffectProvider>();
            if (provider == null)
            {
                throw new Exception($"{playerObject} does not implement IClearEffectProvider.");
            }

            var isOwner = playerObject.IsOwner;
            if (isOwner)
            {
                if (LocalEffector != null) TBMLog.HeaderWarning($"{playerObject} already registered.");
                LocalEffector = provider.GetClearEffector();
            }

            var succeed = ClearEffectors.TryAdd(playerObject.OwnerClientId, provider.GetClearEffector());
            if (succeed is false)
            {
                Debug.LogWarning($"Client {playerObject.OwnerClientId} already registered.");
                return;
            }
        }


        public void PlayLocalEffect(IEnumerable<CellObject> clearedCellObjects)
        {
            LocalEffector.PlayEffect(clearedCellObjects);
        }

        public void PlayRemoteEffect(ulong clientId, IEnumerable<CellObject> clearedCells)
        {
            if (ClearEffectors.TryGetValue(clientId, out var remoteEffector) is false)
            {
                Debug.LogWarning($"Client {clientId} not found.");
                return;
            }

            remoteEffector.PlayEffect(clearedCells);
        }
    }
}