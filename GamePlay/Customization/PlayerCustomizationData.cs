using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using Unity.Netcode;

namespace TenMaker.Gameplay.Customization
{
    public struct PlayerCustomizationData : INetworkSerializable
    {
        public ulong ClientId;
        public PlayerColor Color;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Color);
        }
    }
}