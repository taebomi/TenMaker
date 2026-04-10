using System.Collections.Generic;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public struct RegionClearResult
    {
        public Region ValidMinimalRegion;
        public List<Vector2Int> ClearedCellCoordinates;

        public RegionClearResult(Region validMinimalRegion, List<Vector2Int> clearedCellCoordinates)
        {
            ValidMinimalRegion = validMinimalRegion;
            ClearedCellCoordinates = clearedCellCoordinates;
        }
    }
}