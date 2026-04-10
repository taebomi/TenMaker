using System;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Player
{
    public struct GridSelectionData : IEquatable<GridSelectionData>
    {
        public Vector2Int StartCoordinate;
        public Vector2Int EndCoordinate;

        public GridSelectionData(int x, int y)
        {
            StartCoordinate = new Vector2Int(x, y);
            EndCoordinate = new Vector2Int(x, y);
        }

        public GridSelectionData(int startX, int startY, int endX, int endY)
        {
            StartCoordinate = new Vector2Int(startX, startY);
            EndCoordinate = new Vector2Int(endX, endY);
        }

        public GridSelectionData(Vector2Int coordinate)
        {
            StartCoordinate = coordinate;
            EndCoordinate = coordinate;
        }

        public GridSelectionData(Vector2Int startCoordinate, Vector2Int endCoordinate)
        {
            StartCoordinate = startCoordinate;
            EndCoordinate = endCoordinate;
        }

        public Region ToRegion()
        {
            int minRow, minCol, maxRow, maxCol;

            if (StartCoordinate.x <= EndCoordinate.x)
            {
                minCol = StartCoordinate.x;
                maxCol = EndCoordinate.x;
            }
            else
            {
                minCol = EndCoordinate.x;
                maxCol = StartCoordinate.x;
            }

            if (StartCoordinate.y <= EndCoordinate.y)
            {
                minRow = StartCoordinate.y;
                maxRow = EndCoordinate.y;
            }
            else
            {
                minRow = EndCoordinate.y;
                maxRow = StartCoordinate.y;
            }

            return new Region(minRow, minCol, maxRow, maxCol);
        }

        public override bool Equals(object obj)
        {
            return obj is GridSelectionData other && Equals(other);
        }

        public bool Equals(GridSelectionData other)
        {
            return StartCoordinate.Equals(other.StartCoordinate) && EndCoordinate.Equals(other.EndCoordinate);
        }

        public static bool operator ==(GridSelectionData left, GridSelectionData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridSelectionData left, GridSelectionData right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartCoordinate, EndCoordinate);
        }

        public override string ToString()
        {
            return $"Start: {StartCoordinate}, End: {EndCoordinate}";
        }

        // public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        // {
        //     if(serializer.IsWriter) Write(serializer);
        //     else Read(serializer);
        // }
        //
        // private void Write<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        // {
        //     var writer = serializer.GetFastBufferWriter();
        //     writer.WriteValueSafe(StartCoordinate);
        //     writer.WriteValueSafe(EndCoordinate);
        // }
        //
        // private void Read<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        // {
        //     var reader = serializer.GetFastBufferReader();
        //     reader.ReadValueSafe(out StartCoordinate);
        //     reader.ReadValueSafe(out EndCoordinate);
        // }
    }
}