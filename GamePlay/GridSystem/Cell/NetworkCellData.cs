using System;
using Unity.Netcode;

namespace TenMaker.Gameplay
{
    public struct NetworkCellData : INetworkSerializable, IEquatable<NetworkCellData>
    {
        public ulong ClientId;
        public int Value;
        public bool Cleared;

        /// <summary>
        /// 최초 초기화 시 사용
        /// </summary>
        /// <param name="value"></param>
        public NetworkCellData(int value)
        {
            ClientId = ulong.MaxValue;
            Value = value;
            Cleared =false;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Value);
            serializer.SerializeValue(ref Cleared);
        }
        
        public bool Equals(NetworkCellData other)
        {
            return ClientId == other.ClientId && Value == other.Value && Cleared == other.Cleared;
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkCellData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Cleared);
        }
    }
}