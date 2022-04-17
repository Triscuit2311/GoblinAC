using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Server.Utils;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Server.Heartbeat
{
    public class HandshakeManager : BaseScript
    {
        private readonly Random _numberGen;
        private readonly Dictionary<string, HeartbeatData> _outstandingHeartbeats;


        private bool _initialized = false;

        private const int HeartbeatDispatchInterval = 1000*60*5;
        private const int HeartbeatCheckInterval = 10000;
        private const double LatencyThreshold = 10.0f;
        private const double ReportThreshold = 30.0f;
        
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

                if (!(elapsedTime >= ReportThreshold)) continue;

                TriggerEvent("AddOrModifyReportString", kvp.Value.Ids["fivem"], "MissedHeartbeat", kvp.Key);
                
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
            
            await Delay(HeartbeatDispatchInterval);

            var hash = _numberGen.Next();

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

            return Task.FromResult(0);
        }

        private void HeartbeatCb([FromSource] Player source, string hash)
        {
            if (_outstandingHeartbeats.Remove(hash + ":" + source.Name)) return;
            TriggerEvent("AddOrModifyReportString",
                source.Identifiers["fivem"],
                "BadHeartbeat",
                "[" + hash + "]"
            );
        }
        
    }
}