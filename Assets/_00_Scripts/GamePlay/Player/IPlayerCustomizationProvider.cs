namespace TenMaker.Gameplay.Player
{
    public interface IPlayerCustomizationProvider
    {
        CellNumber GetCellNumber();
        ICustomCellBackgroundProvider GetCellBackgroundProvider();
    }
}