using System.Collections.Generic;
using Unity.Netcode;

namespace TenMaker.Gameplay.Score.Multiplay.Data
{
    public struct ScoreDTO : INetworkSerializable
    {
        public ulong[] ClientIds;
        public int[] Scores;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientIds);
            serializer.SerializeValue(ref Scores);
        }

        public ScoreDTO(Dictionary<ulong, int> scoreData)
        {
            var count = scoreData.Count;
            ClientIds = new ulong[count];
            Scores = new int[count];

            var index = 0;
            foreach (var kvp in scoreData)
            {
                ClientIds[index] = kvp.Key;
                Scores[index] = kvp.Value;
                index++;
            }
        }
    }
}