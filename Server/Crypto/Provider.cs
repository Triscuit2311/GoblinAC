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
        private static readonly int _keyReissueInterval = 30_000; // ms
        
        public Provider()
        {
            _playerKeysets = new Dictionary<string, PlayerKeyManager>();
            GlobalClientKey = KeyUtils.GetKey(64);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }

        public static PlayerKeyManager ClientKeyLookup(string FiveMID)
        {
            return _playerKeysets.FirstOrDefault(pair => pair.Key == FiveMID).Value;
        }

        
        [Tick]
        private async Task UpdateKeys()
        {
            foreach (var player in Players)
            {
                if (player.Character is null) continue;
                
                if (_playerKeysets.ContainsKey(player.Identifiers["fivem"]) &&
                !_playerKeysets[player.Identifiers["fivem"]].NeedsKeys) continue;
                
                TriggerClientEvent(player,"Goblin::Client::KeyManager::ClearCryptoKeys");
                Debug.WriteLine($"Reset keys for {player.Name}.");
                
                IssueGlobal(player);
                IssueClientSpecificKeys(player);
                
                Debug.WriteLine($"Issued keys for {player.Name}.");
            }

            await Delay(5000);
        }

        [Tick]
        private async Task ReissueReset()
        {
            await Delay(_keyReissueInterval);
            foreach (var playerKeyManager in _playerKeysets)
            {
                playerKeyManager.Value.NeedsKeys = true;
            }
        }
        

        private void IssueGlobal(Player player)
        {
            TriggerClientEvent(player,
                "Goblin::Client::KeyManager::ReceiveGlobalKey", GlobalClientKey);
        }

        private void IssueClientSpecificKeys(Player player)
        {
            var playerKeyset = new PlayerKeyManager();

            playerKeyset.ClientKey = KeyUtils.GetKey(64);
            playerKeyset.NumericalKeys = KeyUtils.GetNumericalKeys(4);
            playerKeyset.NeedsKeys = false;

            if (!_playerKeysets.ContainsKey(player.Identifiers["fivem"]))
            {
                _playerKeysets.Add(player.Identifiers["fivem"], playerKeyset);
            }
            
            _playerKeysets[player.Identifiers["fivem"]] = playerKeyset;
            
            TriggerClientEvent(player,
                "Goblin::Client::KeyManager::ReceiveClientKey", ClientKeyLookup(player.Identifiers["fivem"]).ClientKey);
            TriggerClientEvent(player,
                "Goblin::Client::KeyManager::ReceiveNumericalKeys", ClientKeyLookup(player.Identifiers["fivem"]).NumericalKeys);
        }

        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            _playerKeysets.Remove(player.Identifiers["fivem"]);
        }

        private bool AreKeysValidForAllAscii(string globalKey, string clientKey)
        {
            return !(from str in Charset.AsciiStrings 
                let hashed = SharedUtils.ComposeHash(globalKey, clientKey, str)
                let unHashed = SharedUtils.ComposeHash(globalKey, clientKey, hashed)
                where unHashed != str select str).Any();
        }

    }
}