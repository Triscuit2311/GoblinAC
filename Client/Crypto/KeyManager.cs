using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Shared;

namespace Goblin.Client.Crypto
{
    public class KeyManager : BaseScript
    {
        internal static string GlobalKey = "";
        internal static string ClientKey = "";
        internal static List<int> NumericalKeys;
        private bool _globalKeyReceived = false;
        private bool _clientKeyReceived = false;
        private bool _numericalKeysReceived = false;
        
        public KeyManager()
        {
            EventHandlers["ReceiveClientKey"] += new Action<string>(key =>
            {
                ClientKey = key;
                _clientKeyReceived = true;
                Debug.WriteLine("Client Key Received");
            });
            
            EventHandlers["ReceiveGlobalKey"] += new Action<string>(key => { 
                GlobalKey = key;
                _globalKeyReceived = true;
                Debug.WriteLine("Global Key Received");
            });
            
            EventHandlers["ReceiveNumericalKeys"] += new Action<List<object>>(keys =>
            {
                NumericalKeys = new List<int>();
                foreach (var obj in keys)
                {
                    NumericalKeys.Add(int.Parse(obj.ToString()));
                }
                _numericalKeysReceived = true;
                Debug.WriteLine("Numerical Keys Received");
            });

        }

        public bool ClientHasAllKeys()
        {
            return _globalKeyReceived && _clientKeyReceived && _numericalKeysReceived;
        }
        
        

    }
}