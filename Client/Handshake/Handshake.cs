using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Client.Handshake
{
    public class Handshake : BaseScript
    {
        private bool _debugUseHeartBeat = true;

        [Command("togglehb")]
        public void ToggleHeartbeat()
        {
            _debugUseHeartBeat = !_debugUseHeartBeat;
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

            if (!_debugUseHeartBeat) return;
            
            TriggerServerEvent("HeartbeatCB", 
                (seed ^ (LocalPlayer.Character.NetworkId * 1000000)).ToString());
        }

    }
}