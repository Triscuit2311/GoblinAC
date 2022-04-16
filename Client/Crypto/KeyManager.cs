using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Goblin.Client.Crypto
{
    public class KeyManager : BaseScript
    {
        internal static string globalKey = "";
        internal static string clientKey = "";
        internal static List<int> numericalKeys;
        private bool globalKeyReceived = false;
        private bool clientKeyReceived = false;
        private bool numericalKeysReceived = false;
        
        public KeyManager()
        {
            EventHandlers["ReceiveClientKey"] += new Action<string>(key =>
            {
                clientKey = key;
                clientKeyReceived = true;
                Debug.WriteLine("Client Key Received");
            });
            
            EventHandlers["ReceiveGlobalKey"] += new Action<string>(key => { 
                globalKey = key;
                globalKeyReceived = true;
                Debug.WriteLine("Global Key Received");
            });
            
            EventHandlers["ReceiveNumericalKeys"] += new Action<List<object>>(keys =>
            {
                numericalKeys = new List<int>();
                foreach (var obj in keys)
                {
                    numericalKeys.Add(int.Parse(obj.ToString()));
                }
                numericalKeysReceived = true;
                Debug.WriteLine("Numerical Keys Received");
            });
        }

        public bool ClientHasAllKeys()
        {
            return globalKeyReceived && clientKeyReceived && numericalKeysReceived;
        }

    }
}