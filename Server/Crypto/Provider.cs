using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Shared;

namespace Goblin.Server.Crypto
{
    internal class Provider : BaseScript
    {
        private static Dictionary<string, PlayerKeyManager> _playerKeysets;
        public static string GlobalClientKey;
        private static bool _allowIssueKeys = true;
        
        public Provider()
        {
            _playerKeysets = new Dictionary<string, PlayerKeyManager>();
            GlobalClientKey = KeyUtils.GetKey(64);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["resetKeys"] += new Action(TestResetKeys);
        }

        private async void TestResetKeys()
        {
            Debug.WriteLine("Trying to reset keys.");
            foreach (var player in Players)
            {
                TriggerClientEvent(player,"ClearCryptoKeys");
                _playerKeysets[player.Identifiers["fivem"]].NeedsKeys = true;
                Debug.WriteLine($"Reset keys for {player.Name}.");
            }

            _allowIssueKeys = false;
            await Delay(30 * 1000);
            Debug.WriteLine("Allowing key issue.");
            _allowIssueKeys = true;
        }


        public static PlayerKeyManager ClientKeyLookup(string FiveMID)
        {
            return _playerKeysets.FirstOrDefault(pair => pair.Key == FiveMID).Value;
        }


        [Tick]
        private async Task UpdateKeyCycle()
        {
            
        }


        [Tick]
        private async Task<Task<int>> EnsureAllClientsHaveKeys()
        {
            await Delay(1000);

            if (!_allowIssueKeys)
            {
                Debug.WriteLine("Key issue not allowed");
                return Task.FromResult(0);
            }
            
            foreach (var player in Players)
            {
                if (player.Character is null) continue;
                if (_playerKeysets.ContainsKey(player.Identifiers["fivem"]) 
                    && !_playerKeysets[player.Identifiers["fivem"]].NeedsKeys) continue;

                Debug.WriteLine($"[In Session] Dispatching Keys to [{player.Name}]");
                IssueGlobal(player);
                await IssueClientSpecificKeys(player);
            }

            return Task.FromResult(0);
        }

        private void IssueGlobal(Player player)
        {
            TriggerClientEvent(player,
                "ReceiveGlobalKey", GlobalClientKey);
        }

        private async Task IssueClientSpecificKeys(Player player)
        {
            
            var playerKeyset = new PlayerKeyManager();

            var flag = false;
            while (!flag)
            {
                playerKeyset.ClientKey = KeyUtils.GetKey(64);
                flag = AreKeysValidForAllAscii(GlobalClientKey, playerKeyset.ClientKey);
                await Delay(1000);
            }


            playerKeyset.NumericalKeys = KeyUtils.GetNumericalKeys(4);
            playerKeyset.NeedsKeys = false;

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