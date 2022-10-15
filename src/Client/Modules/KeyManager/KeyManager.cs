using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GoblinAC.Client.Modules;

public class KeyManager : BaseScript
{
    internal static string GlobalKey = "";
    internal static string ClientKey = "";
    internal static List<int> NumericalKeys;
    private bool _clientKeyReceived;
    private bool _globalKeyReceived;
    private bool _numericalKeysReceived;

    public KeyManager()
    {
        GlobalKey = "";
        ClientKey = "";
        NumericalKeys = new List<int>();

        EventHandlers["Goblin::Client::KeyManager::ReceiveClientKey"] += new Action<string>(key =>
        {
            ClientKey = key;
            _clientKeyReceived = true;
            
            // Debug.WriteLine("Goblin::Client::KeyManager::ReceiveClientKey");
            // Debug.WriteLine(key);
        });

        EventHandlers["Goblin::Client::KeyManager::ReceiveGlobalKey"] += new Action<string>(key =>
        {
            GlobalKey = key;
            _globalKeyReceived = true;
            
            // Debug.WriteLine("Goblin::Client::KeyManager::ReceiveGlobalKey");
            // Debug.WriteLine(key);
        });

        EventHandlers["Goblin::Client::KeyManager::ReceiveNumericalKeys"] += new Action<List<object>>(keys =>
        {
            NumericalKeys = new List<int>();
            foreach (var obj in keys) NumericalKeys.Add(int.Parse(obj.ToString()));
            _numericalKeysReceived = true;
            
            // Debug.WriteLine("Goblin::Client::KeyManager::ReceiveNumericalKeys");
            // foreach (var k in NumericalKeys) Debug.WriteLine(k.ToString());
        });

        EventHandlers["Goblin::Client::KeyManager::ClearCryptoKeys"] += new Action(() =>
        {
            _clientKeyReceived = false;
            _globalKeyReceived = false;
            _numericalKeysReceived = false;
            AllKeys = false;
            GlobalKey = "";
            ClientKey = "";
            NumericalKeys.Clear();
            
            //Debug.WriteLine("Goblin::Client::KeyManager::ClearCryptoKeys");
        });
    }

    internal static bool AllKeys { get; private set; }

    [Tick]
    private async Task<Task<int>> OnTick()
    {
        if (_globalKeyReceived && _clientKeyReceived && _numericalKeysReceived)
        {
            AllKeys = true;
            await Delay(5000);
            return Task.FromResult(0);
        }

        AllKeys = false;
        await Delay(1000);
        return Task.FromResult(0);
    }
}