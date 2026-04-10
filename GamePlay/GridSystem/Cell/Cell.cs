using TenMaker.Gameplay.Multiplay;

namespace TenMaker.Gameplay.GridSystem.Multiplay
{
    public class Cell
    {
        public int Value;
        public bool Cleared;
        public ulong ClearedClientId;

        public Cell()
        {
            Value = 0;
            Cleared = false;
        }

        public Cell(int value)
        {
            Value = value;
            Cleared = false;
        }

        public Cell(CellInitializationDTO initializationData)
        {
            Value = initializationData.Value;
            Cleared = false;
        }

        public Cell(NetworkCellData networkCellData)
        {
            Value = networkCellData.Value;
            Cleared = networkCellData.Cleared;
        }

        public void Apply(NetworkCellData data)
        {
            Value = data.Value;
            Cleared = data.Cleared;
        }

        public void Clear(ulong clientId)
        {
            Cleared = true;
            ClearedClientId = clientId;
        }
    }
}