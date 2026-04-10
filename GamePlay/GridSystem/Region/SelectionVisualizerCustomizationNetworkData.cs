using Unity.Netcode;
using UnityEngine.Serialization;

namespace TenMaker.Gameplay
{
    public struct SelectionVisualizerCustomizationNetworkData : INetworkSerializable
    {
        public ulong clientId;
        public SelectionVisualizerCustomizationData customizationData;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref customizationData);
        }
    }
}