using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Goblin.Client.Detectors;

public class ResourceMonitor : BaseScript
{
    private int _lastNumResources = 0;
    public ResourceMonitor()
    {
        EventHandlers["onClientResourceStop"] += new Action<string>(OnClientResourceStop);
    }

    [Tick]
    async Task OnTick()
    {
        _lastNumResources = API.GetNumResources();

        await Delay(5000);
        
        if (_lastNumResources != API.GetNumResources())
        {
            SendDetectionReport($"Resource number changed from [{_lastNumResources}] to [{API.GetNumResources()}].");
        }
    }
    
    private void OnClientResourceStop(string resourceName)
    {
        // if (resourceName == "chat")
        // {
        //     API.CancelEvent();
        //     SendDetectionReport($"Resource [{resourceName}] attempted stop by client.");
        // }

        SendDetectionReport($"Resource [{resourceName}] stopped by client.");
    }
    
    
    void SendDetectionReport(string msg)
    {
        TriggerEvent("Goblin::Client::EventProxy",
            "Goblin::Server::Reporting::AddOrModifyReportString",
            "SOURCE_ID",
            "Resource Detections",
            msg
        );
    }

}