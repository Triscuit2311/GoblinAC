using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Goblin.Client.Detectors;

public class BasicCheats : BaseScript
{

    public BasicCheats()
    {
        EventHandlers["Goblin::Client::Detectors::BasicCheats::Check"] += new Action(RunCheckCycle);
    }

    private void RunCheckCycle()
    {
        var pid = API.PlayerPedId();
        //CheckInvisible(pid);
        CheckSpectatorMode();
        CheckNoClip(pid);
        CheckBasicGodmode(pid);
    }

    private void CheckSpectatorMode()
    {
        if (API.NetworkIsInSpectatorMode())
        {
            SendDetectionReport("Spectator Mode");
        }
    }

    private void CheckInvisible(int pid)
    {
        if (!API.IsEntityVisible(pid)) return;
        SendDetectionReport("Invisible");
        API.SetEntityVisible(pid,true,false);
    }

    private void CheckBasicGodmode(int pid)
    {
        if (API.GetPlayerInvincible(pid) || API.GetPlayerInvincible_2(pid) || !API.GetEntityCanBeDamaged(pid))
        {
            SendDetectionReport("Classic Godmode");
        }
    }

    private void CheckNoClip(int pid)
    {
        if (!API.IsPedInAnyVehicle(pid,true) &&
            (API.GetEntityCollisionDisabled(pid) || API.GetEntityCollisonDisabled(pid)))
        {
            SendDetectionReport("No Clip");
        }
    }

    static void SendDetectionReport(string msg)
    {
        TriggerEvent("Goblin::Client::EventProxy",
            "Goblin::Server::Reporting::AddOrModifyReportString",
            "SOURCE_ID",
            "Detections",
            msg
        );
    }
}