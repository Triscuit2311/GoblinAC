using System.Threading.Tasks;
using CitizenFX.Core;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Debug.WriteLine("Hi from Goblin.Client!");
        }

        [Command("debugprintreports")]
        public void PrintReports()
        {
            TriggerServerEvent("PrintReports");
        }
        
        [Tick]
        public Task OnTick()
        {
            return Task.FromResult(0);
        }
    }
}

