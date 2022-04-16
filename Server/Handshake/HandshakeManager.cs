using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Server.Utils;
using static CitizenFX.Core.Native.API;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Server.Handshake
{
    public class HandshakeManager : BaseScript
    {
        private readonly Random _numberGen;
        private Dictionary<string, HeartbeatData> _outstandingHeartbeats;


        private bool _initialized = false;

        private const int HeartbeatDispatchInterval = 5000;
        private const int HeartbeatCheckInterval = 10000;
        private const double LatencyThreshold = 2.0f;
        private const double ReportThreshold = 10.0f;
        
        

        private struct HeartbeatData
        {
            private int Hash { get; set; }
            public DateTime DispatchedTime { get; }
            public string PlayerName { get; }
            public IdentifierCollection Ids { get; }

            public HeartbeatData(IdentifierCollection ids, string playerName, DateTime dispatchedTime, int hash)
            {
                Ids = ids;
                PlayerName = playerName;
                DispatchedTime = dispatchedTime;
                Hash = hash;
            }
        }

        public HandshakeManager()
        {
            
            _numberGen = new Random();
            _outstandingHeartbeats = new Dictionary<string, HeartbeatData>();

            EventHandlers["HeartbeatCb"] += new Action<Player, string>(HeartbeatCb);
        }

        [Tick]
        public async Task<Task<int>> HeartbeatMonitor()
        {
            if (_outstandingHeartbeats.Count <= 0)
            {
                await Delay(HeartbeatCheckInterval);
                return Task.FromResult(0);
            }

            var heartbeatsToRemove = new List<string>();

            Debug.WriteLine("\nOutstanding Hashes:");

            foreach (var kvp in _outstandingHeartbeats)
            {
                var elapsedTime = (DateTime.Now - kvp.Value.DispatchedTime).TotalSeconds;

                if (elapsedTime < LatencyThreshold)
                {
                    continue;
                }

                if (PlayerUtils.PlayerDisconnected(kvp.Value.Ids["fivem"], Players))
                {
                    heartbeatsToRemove.Add(kvp.Key);
                    continue;
                }

                Debug.WriteLine("[" + kvp.Key + "] overdue by: [" +
                                (DateTime.Now - kvp.Value.DispatchedTime) + "]");

                if (!(elapsedTime >= ReportThreshold)) continue;

                TriggerEvent("AddOrModifyReportString",
                    kvp.Value.Ids["fivem"],
                    "MissedHeartbeat",
                    kvp.Key
                );
                
                heartbeatsToRemove.Add(kvp.Key);
            }

            foreach (var key in heartbeatsToRemove)
            {
                _outstandingHeartbeats.Remove(key);
            }


            await Delay(HeartbeatCheckInterval);
            return Task.FromResult(0);
        }

        [Tick]
        public async Task<Task<int>> HeartbeatDispatcher()
        {

            if (!_initialized)
            {
                await Delay(5000);
                _initialized = true;
                return Task.FromResult(0);
            }

            var hash = _numberGen.Next(1000000, 2000000);

            foreach (var player in Players)
            {
                if (player == null || player.Character == null)
                {
                    await Delay(10000);
                    continue;
                }

                _outstandingHeartbeats.Add(hash + ":" + player.Name,
                    new HeartbeatData(
                        player.Identifiers,
                        player.Name,
                        DateTime.Now,
                        hash));
            }

            TriggerClientEvent("RequestClientHeartbeat", hash);

            await Delay(HeartbeatDispatchInterval);
            return Task.FromResult(0);
        }

        private void HeartbeatCb([FromSource] Player source, string hash)
        {
            if (!_outstandingHeartbeats.Remove(hash + ":" + source.Name))
            {
                Debug.WriteLine("Bad heartbeat from [" + source.Name + "] : [" + hash + "]");

                
                TriggerEvent("AddOrModifyReportString",
                        source.Identifiers["fivem"],
                        "BadHeartbeat",
                        "[" + hash + "]"
                );

                return;
            }

           // Debug.WriteLine("Heartbeat from [" + source.Name + "] : [" + hash + "]");
        }
        
    }
}