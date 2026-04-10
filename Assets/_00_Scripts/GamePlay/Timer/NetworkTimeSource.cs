using Unity.Netcode;

namespace TenMaker.Gameplay.Timer
{
    public class NetworkTimeSource : ITimeSource
    {
        public double GetTime()
        {
            return NetworkManager.Singleton.ServerTime.Time;
        }
    }
}