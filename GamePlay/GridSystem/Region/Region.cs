using System;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public struct Region : IEquatable<Region>, INetworkSerializable
    {
        public int minRow;
        public int minCol;
        public int maxRow;
        public int maxCol;
        public int Area => (maxCol - minCol + 1) * (maxRow - minRow + 1);

        public Region ShrinkFromBottom => new(minRow + 1, minCol, maxRow, maxCol);
        public Region ShrinkFromTop => new(minRow, minCol, maxRow - 1, maxCol);
        public Region ShrinkFromLeft => new(minRow, minCol + 1, maxRow, maxCol);
        public Region ShrinkFromRight => new(minRow, minCol, maxRow, maxCol - 1);

        public bool CanShrinkRow => minRow < maxRow;
        public bool CanShrinkCol => minCol < maxCol;


        public Region(int row, int col)
        {
            minRow = row;
            minCol = col;
            maxRow = row;
            maxCol = col;
        }

        public Region(Vector2Int coordinate)
        {
            minRow = coordinate.y;
            minCol = coordinate.x;
            maxRow = coordinate.y;
            maxCol = coordinate.x;
        }

        public Region(int minRow, int minCol, int maxRow, int maxCol)
        {
            this.minRow = minRow;
            this.minCol = minCol;
            this.maxRow = maxRow;
            this.maxCol = maxCol;
        }

        public bool Equals(Region other)
        {
            return minRow == other.minRow && minCol == other.minCol &&
                   maxRow == other.maxRow && maxCol == other.maxCol;
        }

        public override string ToString()
        {
            return $"Min({minRow},{minCol}), Max({maxRow},{maxCol})";
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsWriter)
            {
                NetworkWrite(serializer);
            }
            else if (serializer.IsReader)
            {
                NetworkRead(serializer);
            }
        }

        private void NetworkWrite<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(minRow);
            writer.WriteValueSafe(minCol);
            writer.WriteValueSafe(maxRow);
            writer.WriteValueSafe(maxCol);
        }

        private void NetworkRead<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out minRow);
            reader.ReadValueSafe(out minCol);
            reader.ReadValueSafe(out maxRow);
            reader.ReadValueSafe(out maxCol);
        }
    }
}