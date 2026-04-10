using TenMaker.Core.Save;

namespace TenMaker.Core.Data
{
    public interface ISettingsDataRepository : IRepository<SettingsData>
    {
        GeneralSettingsData GeneralSettingsData { get; }
        AudioSettingsData AudioSettingsData { get; }
    }
}