﻿using System;
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
        private static Dictionary<string, PlayerKeyset> _playerKeysets;
        public static string GlobalClientKey;
        
        public Provider()
        {
            _playerKeysets = new Dictionary<string, PlayerKeyset>();
            GlobalClientKey = KeyUtils.GetUnicodeString(64);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
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
            var playerKeyset = new PlayerKeyset(KeyUtils.GetUnicodeString(64), KeyUtils.GetNumericalKeys(4));

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