
using System.Linq;
using CitizenFX.Core;
namespace Goblin.Server.Utils
{
    public static class PlayerUtils
    {
        public static bool PlayerDisconnected(string id, PlayerList players)
        {
            var playerDisconnected = true;
            foreach (var player in players)
            {
                if (player.Identifiers["fivem"] != id) continue;
                playerDisconnected = false;
            }

            return playerDisconnected;
        }

        public static Player GetPlayerByFiveMid(string id, PlayerList players)
        {
            return players.FirstOrDefault(player => player.Identifiers["fivem"] == id);
        }
    }
}