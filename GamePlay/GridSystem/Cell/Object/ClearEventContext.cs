using System.Collections.Generic;
using TenMaker.Gameplay.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class ClearEventContext
    {
        public ulong ClientId;
        public List<CellObject> ClearedCells;
        public Region Region;

        public ClearEventContext(ClearEventDTO clearEvent, List<CellObject> clearedCells)
        {
            ClientId = clearEvent.ClientId;
            ClearedCells = clearedCells;
            Region = clearEvent.ClearedRegion;
        }
    }
}