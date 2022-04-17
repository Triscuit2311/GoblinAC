using System;
using CitizenFX.Core;

namespace Goblin.Server.Heartbeat;

internal struct HeartbeatData
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