using System.Threading.Tasks;
using CitizenFX.Core;

namespace Goblin.Server.Detections;

public class ClientMonitor : BaseScript
{
    private int ClientCheckInterval = 10000;
    
    
    
    [Tick]
    async Task ClientCheckDispatch()
    {
        await Delay(ClientCheckInterval);
            
        foreach (var p in Players)
        {
            TriggerClientEvent(p,"Goblin::Client::Detectors::AdvancedGodMode::Check");
            TriggerClientEvent(p,"Goblin::Client::Detectors::BasicCheats::Check");
        }
    }
    
    
    // TODO Heartbeat
    
    // TODO 
    
    
    
}