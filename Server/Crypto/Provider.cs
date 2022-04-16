using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Goblin.Server.Crypto
{
    public class Provider : BaseScript
    {
        public struct PlayerKeyset
        {
            public string ClientKey;
            public int[] NumericalKeys;

            public PlayerKeyset(string clientKey, int[] numericalKeys)
            {
                this.NumericalKeys = numericalKeys;
                this.ClientKey = clientKey;
            }
        }
        private static Dictionary<string, PlayerKeyset> _playerKeysets;
        public static string GlobalClientKey;

        public static PlayerKeyset ClientKeyLookup(string FiveMID)
        {
            return _playerKeysets.FirstOrDefault(pair => pair.Key == FiveMID).Value;
        }


        // private static string Getb64(int len)
        // {
        //     var crypt = new RNGCryptoServiceProvider();
        //     var buf = new byte[len]; 
        //     crypt.GetBytes(buf);
        //     return Convert.ToBase64String(buf);
        // }
        
        private static string Getb64(int len)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append(Unicode.UnicodeChars[r.Next(0,10000)]);
            }
            return sb.ToString();
        }


        private int[] GetNumericalKeys(int num)
        {
            Random gen = new Random();
            var keys = new List<int>();
            for (int i = 0; i < num; i++)
            {
                keys.Add(gen.Next(-10000,10000));
            }
            return keys.ToArray();
        }

        public Provider()
        {
            _playerKeysets = new Dictionary<string, PlayerKeyset>();
            GlobalClientKey = Getb64(64);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }

        [Tick]
        private async Task EnsureAllClientsHaveKeys()
        {
            await Delay(5000);
            
            foreach (var player in Players)
            {
                if (player.Character is null) continue;
                if (_playerKeysets.ContainsKey(player.Identifiers["fivem"])) continue;

                Debug.WriteLine($"[In Session] Dispatching Keys to [{player.Name}]");
                IssueGlobal(player);
                IssueClientSpecificKeys(player);
            }
        }

        private void IssueGlobal(Player player)
        {
            TriggerClientEvent(player,
                "ReceiveGlobalKey", GlobalClientKey);
        }

        private void IssueClientSpecificKeys(Player player)
        {
            PlayerKeyset playerKeyset = new PlayerKeyset(Getb64(64), GetNumericalKeys(4));

            if (!_playerKeysets.ContainsKey(player.Identifiers["fivem"]))
            {
                _playerKeysets.Add(player.Identifiers["fivem"], playerKeyset);
            }

            _playerKeysets[player.Identifiers["fivem"]] = playerKeyset;

            TriggerClientEvent(player,
                "ReceiveClientKey", ClientKeyLookup(player.Identifiers["fivem"]).ClientKey);
            TriggerClientEvent(player,
                "ReceiveNumericalKeys", ClientKeyLookup(player.Identifiers["fivem"]).NumericalKeys);
        }

        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            _playerKeysets.Remove(player.Identifiers["fivem"]);
        }
        
    }
}