using Unity.Netcode;

namespace TenMaker.Gameplay.Multiplay
{
    public struct CellInitializationDTO : INetworkSerializable
    {
        public int Value;
        
        public CellInitializationDTO(int value)
        {
            Value = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Value);
        }
    }
}