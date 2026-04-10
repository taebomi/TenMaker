using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Multiplay;
using Unity.Netcode;

namespace TenMaker.Gameplay
{
    public struct GridInitializationDTO : INetworkSerializable
    {
        public GridConfigData ConfigData;
        public CellInitializationDTO[] CellData;

        public GridInitializationDTO(GridConfigData configData, CellInitializationDTO[] cellData)
        {
            ConfigData = configData;
            CellData = cellData;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ConfigData);
            serializer.SerializeValue(ref CellData);
        }
    }
}