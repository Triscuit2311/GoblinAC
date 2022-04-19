#if DEVELOPMENT_BUILD
using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Shared;

namespace Goblin.Client.Development;

public class DevCommands : BaseScript
{
    [Command("debugprintreports")]
    public void PrintReports()
    {
        TriggerServerEvent("PrintReports");
    }
        
    [Command("testkeys")]
    public void RunKeys()
    {
        TriggerServerEvent("RunKeyTests");
    }
    
    [Command("tryev")]
    public async Task TryEvent()
    {
        TriggerEvent("Goblin::Client::EventProxy", "funevent", "sssss");
        Random genny = new Random();
        for (var i = 0; i < 10; i++)
        {
            TriggerEvent("Goblin::Client::EventProxy", 
                SharedUtils.RandomAscii(genny.Next(5,30)), 
                "A",
                "B",
                genny.Next(-100,100), 
                genny.NextDouble());
            await Delay(100);
        }
    }
    
    [Command("trylua")]
    private void tryLua()
    {
        TriggerEvent("luaexamples:reverse");
    }
}
#endif