using Unity.Netcode;

namespace TenMaker.Gameplay
{
    public struct SelectionVisualizerCustomizationData : INetworkSerializable
    {
        public int temp;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref temp);
        }
    }
}