using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using GoblinAC.Server.Modules.Crypto;

// ReSharper disable UnusedMember.Local
// ReSharper disable MemberCanBePrivate.Global

namespace GoblinAC.Server.Modules;

// ReSharper disable once ClassNeverInstantiated.Global
public class KeyManager : BaseScript
{
    private const int ReissueInterval = 30_000;
    private const int KeyCheckInterval = 5_000;
    private const int KeySize = 128;
    private const int NumNumericalKeys = 4;
    public static string GlobalKey;
    private static Dictionary<string, PlayerKeyManager> _playerKeyDictionary;

    public KeyManager()
    {
        GlobalKey = UnicodeProvider.GenerateKey(KeySize);
        _playerKeyDictionary = new Dictionary<string, PlayerKeyManager>();
        EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
    }

    public static PlayerKeyManager ClientKeyLookup(string fiveMid)
    {
        return _playerKeyDictionary.FirstOrDefault(pair => pair.Key == fiveMid).Value;
    }

    [Tick]
    private async Task UpdateKeys()
    {
        foreach (var player in Players)
        {
            if (player.Character is null) continue;

            if (_playerKeyDictionary.ContainsKey(player.Identifiers["fivem"]) &&
                !_playerKeyDictionary[player.Identifiers["fivem"]].NeedsKeys) continue;

            TriggerClientEvent(player, "Goblin::Client::KeyManager::ClearCryptoKeys");

            IssueGlobal(player);
            IssueClientSpecificKeys(player);
        }

        await Delay(KeyCheckInterval);
    }

    [Tick]
    private async Task ReissueReset()
    {
        await Delay(ReissueInterval);
        foreach (var playerKeyManager in _playerKeyDictionary) playerKeyManager.Value.NeedsKeys = true;
    }

    private static void IssueGlobal(Player player)
    {
        TriggerClientEvent(player,
            "Goblin::Client::KeyManager::ReceiveGlobalKey", GlobalKey);
    }

    private static void IssueClientSpecificKeys(Player player)
    {
        var playerKeyManager = new PlayerKeyManager
        {
            ClientKey = UnicodeProvider.GenerateKey(KeySize),
            NumericalKeys = NumericalProvider.GenerateKeys(NumNumericalKeys),
            NeedsKeys = false
        };

        if (!_playerKeyDictionary.ContainsKey(player.Identifiers["fivem"]))
            _playerKeyDictionary.Add(player.Identifiers["fivem"], playerKeyManager);

        _playerKeyDictionary[player.Identifiers["fivem"]] = playerKeyManager;

        TriggerClientEvent(player,
            "Goblin::Client::KeyManager::ReceiveClientKey", ClientKeyLookup(player.Identifiers["fivem"]).ClientKey);
        TriggerClientEvent(player,
            "Goblin::Client::KeyManager::ReceiveNumericalKeys",
            ClientKeyLookup(player.Identifiers["fivem"]).NumericalKeys);
    }

    private static void OnPlayerDropped([FromSource] Player player, string reason)
    {
        _playerKeyDictionary.Remove(player.Identifiers["fivem"]);
    }
}