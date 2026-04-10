using System;
using Unity.Netcode;

namespace TenMaker.Gameplay.Multiplay
{
    public struct GridDTO : INetworkSerializable, IEquatable<GridDTO>
    {
        public int RowCount;
        public int ColCount;
        public int[] CellValues;

        public int this[int index]
        {
            get => CellValues[index];
            set => CellValues[index] = value;
        }
        public int this[int row, int col]
        {
            get => CellValues[row * ColCount + col];
            set => CellValues[row * ColCount + col] = value;
        }

        public GridDTO(int rowCount, int colCount)
        {
            RowCount = rowCount;
            ColCount = colCount;
            CellValues = null;
        }

        public GridDTO(int rowCount, int colCount, int[] cellValues)
        {
            RowCount = rowCount;
            ColCount = colCount;
            CellValues = cellValues;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref RowCount);
            serializer.SerializeValue(ref ColCount);
            serializer.SerializeValue(ref CellValues);
        }

        public bool Equals(GridDTO other)
        {
            if (RowCount != other.RowCount || ColCount != other.ColCount) return false;
            if (CellValues == null && other.CellValues == null) return true;
            if (CellValues == null || other.CellValues == null) return false;
            if (CellValues.Length != other.CellValues.Length) return false;

            for (var i = 0; i < CellValues.Length; i++)
            {
                if (CellValues[i] != other.CellValues[i]) return false;
            }

            return true;
        }
    }
}