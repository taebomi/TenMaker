using TenMaker.Gameplay;
using TenMaker.Gameplay.Score.Multiplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public struct ClearEventDTO : INetworkSerializable
    {
        public ulong ClientId;
        public Region ClearedRegion;
        public Vector2Int[] ClearedCellCoordinates;

        public int Combo;

        public ScoreDTO ScoreData;
        

        public ClearEventDTO(ulong clientId, RegionClearResult clearResult, int combo, ScoreDTO scoreData)
        {
            ClientId = clientId;
            ClearedRegion = clearResult.ValidMinimalRegion;
            ClearedCellCoordinates = clearResult.ClearedCellCoordinates.ToArray();
            Combo = combo;
            ScoreData = scoreData;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref ClearedRegion);
            serializer.SerializeValue(ref ClearedCellCoordinates);
            serializer.SerializeValue(ref Combo);
            serializer.SerializeValue(ref ScoreData);
        }
    }
}