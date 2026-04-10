using System.Collections.Generic;
using TenMaker.Gameplay.Multiplay;

namespace TenMaker.Gameplay.Player.Multiplay
{
    public class PlayersContext
    {
        public NetworkPlayerObject LocalPlayer { get; }
        public IReadOnlyDictionary<ulong, NetworkPlayerObject> RemotePlayers { get; }
        public IReadOnlyDictionary<ulong, NetworkPlayerObject> AllPlayers { get; }
        public IEnumerable<ulong> AllClientIds => AllPlayers.Keys;

        public PlayersContext(NetworkPlayerObject localPlayer,
            Dictionary<ulong, NetworkPlayerObject> remotePlayers)
        {
            LocalPlayer = localPlayer;
            RemotePlayers = remotePlayers;
            AllPlayers = new Dictionary<ulong, NetworkPlayerObject>(remotePlayers)
                { { localPlayer.OwnerClientId, localPlayer } };
        }
    }
}