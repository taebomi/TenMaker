using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Multiplay.Player;
using TenMaker.Gameplay.Customization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace TenMaker.Gameplay
{
    public struct NetworkInitializationData : INetworkSerializable
    {
        public NetworkBehaviourReference[] PlayerObjectReferences;
        public PlayerCustomizationData[] PlayerCustomizationData;
        public GridInitializationDTO gridInitializationData;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref PlayerObjectReferences);
            serializer.SerializeValue(ref PlayerCustomizationData);
            serializer.SerializeValue(ref gridInitializationData);
        }
    }
}