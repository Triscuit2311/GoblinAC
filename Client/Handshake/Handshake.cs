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
            TriggerServerEvent("HeartbeatCb", "BADB0I");
        }
        
        public Handshake()
        {
            EventHandlers["RequestClientHeartbeat"] += new Action<string>(DispatchHeartbeat);
            
        }
        
        private void DispatchHeartbeat(string hash)
        {
            if (!_debugUseHeartBeat) return;
            TriggerServerEvent("HeartbeatCb", hash);
        }

    }
}