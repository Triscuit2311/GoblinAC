using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Shared;

namespace Goblin.Server.Crypto
{
    public class Provider : BaseScript
    {
        private static Dictionary<string, PlayerKeyset> _playerKeysets;
        public static string GlobalClientKey;
        
        public Provider()
        {
            _playerKeysets = new Dictionary<string, PlayerKeyset>();
            GlobalClientKey = KeyUtils.GetKey(64);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["RunKeyTests"] += new Action(TestKeys);
        }

        private async void TestKeys()
        {
            int num = 1_000_000_000;
            Debug.WriteLine($"Testing {num} combinations of Global/Client Keys...");
            string b = "LuaExamples::Server::SomeExtraSafeEvent::Goblin::Client::EventProxy";
            for (int i = 0; i <= num; i++)
            {
                if (i % 100000 == 0)
                {
                    Debug.WriteLine($"Tests Complete: {i}");
                    await Delay(500);
                }

                GlobalClientKey = KeyUtils.GetKey(128);
                var ClientKey = KeyUtils.GetKey(128);

                var hashed = SharedUtils.ComposeHash(GlobalClientKey, ClientKey, b);
                var unhashed = SharedUtils.ComposeHash(GlobalClientKey, ClientKey, hashed);
                if (b != unhashed)
                    throw new Exception("BAD KEYS");
            }
        }
        
        public static PlayerKeyset ClientKeyLookup(string FiveMID)
        {
            return _playerKeysets.FirstOrDefault(pair => pair.Key == FiveMID).Value;
        }

        // [Tick]
        // private async Task UpdateKeyCycle() { }
        // TODO: Create a reliable way to discard old client keys and issue new ones, without a lapse.
        // TODO:... Events need to still come through correctly

        [Tick]
        private async Task EnsureAllClientsHaveKeys()
        {
            await Delay(1000);
            
            foreach (var player in Players)
            {
                if (player.Character is null) continue;
                if (_playerKeysets.ContainsKey(player.Identifiers["fivem"])) continue;

                Debug.WriteLine($"[In Session] Dispatching Keys to [{player.Name}]");
                IssueGlobal(player);
                await IssueClientSpecificKeys(player);
            }
        }

        private void IssueGlobal(Player player)
        {
            TriggerClientEvent(player,
                "ReceiveGlobalKey", GlobalClientKey);
        }

        private async Task IssueClientSpecificKeys(Player player)
        {
            
            var playerKeyset = new PlayerKeyset();

            var flag = false;
            while (!flag)
            {
                playerKeyset.ClientKey = KeyUtils.GetKey(64);
                flag = AreKeysValidForAllAscii(GlobalClientKey, playerKeyset.ClientKey);
                await Delay(1000);
            }


            playerKeyset.NumericalKeys = KeyUtils.GetNumericalKeys(4);

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

        private bool AreKeysValidForAllAscii(string globalKey, string clientKey)
        {
            return !(from str in Charset.AsciiStrings 
                let hashed = SharedUtils.ComposeHash(globalKey, clientKey, str)
                let unhashed = SharedUtils.ComposeHash(globalKey, clientKey, hashed)
                where unhashed != str select str).Any();
        }

    }
}