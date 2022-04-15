using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Goblin.Client.Handshake
{
    public class Handshake : BaseScript
    {
        private bool debugusehb = true;

        [Command("togglehb")]
        public void ToggleHeartbeat()
        {
            debugusehb = !debugusehb;
        }
        
        [Command("sendbadhb")]
        public void BadHeartbeat()
        {
            TriggerServerEvent("HeartbeatCB", "BADB0I");
        }
        
        public Handshake()
        {
            EventHandlers["RequestClientHeartbeat"] += new Action<int>(DispatchHeartbeat);
            
        }
        private void DispatchHeartbeat(int seed)
        {
            while (LocalPlayer == null || LocalPlayer.Character == null)
            {
                Wait(100);
            }

            if (!debugusehb) return;
            
            TriggerServerEvent("HeartbeatCB", 
                (seed ^ (LocalPlayer.Character.NetworkId * 1000000)).ToString());
        }

    }
}