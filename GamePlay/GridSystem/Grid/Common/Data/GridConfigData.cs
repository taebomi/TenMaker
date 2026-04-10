using System;
using Unity.Netcode;

namespace TenMaker.Gameplay.GridSystem
{
    [Serializable]
    public struct GridConfigData : INetworkSerializable
    {
        public int rowCount;
        public int colCount;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref rowCount);
            serializer.SerializeValue(ref colCount);
        }
    }
}