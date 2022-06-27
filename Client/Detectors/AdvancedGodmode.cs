using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Goblin.Client.Detectors;

public class AdvancedGodmode : BaseScript
{
    private bool CheckForDamage = false;
    private int lastPlayerHp = 0;
    private bool CheckCycleComplete = true;

    public AdvancedGodmode()
    {
        EventHandlers["Goblin::Client::Detectors::AdvancedGodMode::Check"] += new Action(() =>
        {
            CheckCycleComplete = false;
        });
    }

    [Tick]
    async Task<Task<int>> OnTick()
    {
        if (CheckCycleComplete)
        {
            await Delay(1000);
            return Task.FromResult<int>(0);
        }


        var pid = API.PlayerPedId();
        await Delay(500);
        if (!CheckForDamage)
        {
            lastPlayerHp = API.GetEntityHealth(pid);
            API.SetEntityHealth(pid, lastPlayerHp - 50);
            API.ApplyDamageToPed(pid, 1, false);

            CheckForDamage = true;
        }
        else
        {
            if (API.GetEntityHealth(pid) != lastPlayerHp - 51)
            {
                TriggerEvent("Goblin::Client::EventProxy",
                    "Goblin::Server::Reporting::AddOrModifyReportString",
                    "SOURCE_ID",
                    "Detections",
                    "Advanced Godmode"
                );
            }

            API.SetEntityHealth(pid, lastPlayerHp);
            CheckForDamage = false;
            CheckCycleComplete = true;
        }

        return Task.FromResult(0);
    }
}