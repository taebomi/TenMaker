using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;

namespace TenMaker.Utility
{
    public static partial class TBMUtility
    {
        #region Network Behaviours

        public static async UniTask WaitUntilAllSpawned
            (this IEnumerable<NetworkBehaviour> networkBehaviours, CancellationToken ct)
        {
            await UniTask.WaitUntil(networkBehaviours.AllSpawned, cancellationToken: ct);
        }

        public static bool AllSpawned(this IEnumerable<NetworkBehaviour> networkBehaviours)
        {
            return networkBehaviours.All(networkBehaviour => networkBehaviour.IsSpawned);
        }

        public static NetworkBehaviourReference[] GetNetworkBehaviourReferences
            (this IEnumerable<NetworkBehaviour> networkBehaviours)
        {
            return networkBehaviours.Select(networkBehaviour => (NetworkBehaviourReference)networkBehaviour).ToArray();
        }

        #endregion

        #region Network Objects

        public static async UniTask WaitUntilAllSpawned
            (this IEnumerable<NetworkObject> networkObjects, CancellationToken ct)
        {
            await UniTask.WaitUntil(networkObjects.AllSpawned, cancellationToken: ct);
        }

        public static bool AllSpawned(this IEnumerable<NetworkObject> networkObjects)
        {
            return networkObjects.All(networkObject => networkObject.IsSpawned);
        }

        #endregion
    }
}