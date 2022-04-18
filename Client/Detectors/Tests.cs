using System;
using CitizenFX.Core;

namespace Goblin.Client.Detectors
{
    public class Tests : BaseScript
    {

        public Tests()
        {
            EventHandlers.Add("GoblinTest", new Action(RunTest));
            // Exports.Add("GoblinExport", new Action(RunTest));
        }

        [Command("trylua")]
        private void tryLua()
        {
            TriggerEvent("luaexamples:reverse");
        }
        
       // [EventHandler("GoblinTest")]
        private void RunTest()
        {
            Debug.WriteLine("Goblin Test Executed!");
        }
    }
}



// if (NetworkIsInSpectatorMode())
//     Debug.WriteLine("spec");

// if(!IsEntityVisible(pid))
//     Debug.WriteLine("Invisible");


// EventHandlers["onClientResourceStop"] += new Action<string>(OnClientResourceStop);
// EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
// private void OnClientResourceStop(string resourceName)
// {
//     Debug.WriteLine($"Resource stopped: [{resourceName}]");
// }
//         
// private void OnClientResourceStart(string resourceName)
// {
//     Debug.WriteLine($"Resource started: [{resourceName}]");
// }


// if (GetNumResources() != resources)
//     Debug.WriteLine(GetNumResources().ToString());


//Anti God-mode
// if(GetPlayerInvincible(pid) || GetPlayerInvincible_2(pid) || !GetEntityCanBeDamaged(pid))
//     Debug.WriteLine("God mode");


// Anti-SemiGodmode (instant heal)
// if (!restoreHP)
// {
//     hp = GetEntityHealth(pid);
//     SetEntityHealth(pid, hp-1);
//     ApplyDamageToPed(pid, 1, false);
//                 
//     restoreHP = true;
// }
// else
// {
//     if (GetEntityHealth(pid) != hp - 2)
//     {
//         Debug.WriteLine("Semi-god mode");
//     }
//     SetEntityHealth(pid, hp);
//     restoreHP = false;
// }


// No clip
// if (GetEntityCollisionDisabled(pid) || GetEntityCollisonDisabled(pid))
// {
//     Debug.WriteLine("Noclip");
// }



//Trolls 
// EnterCursorMode();
// LeaveCursorMode();


// Resource enum
// for (int i = 0; i < GetNumResources(); i++)
// {
//     var res = GetResourceByFindIndex(i);
//     PrintResources(res, "client_script");
//     PrintResources(res, "client_scripts");
//     PrintResources(res, "ui_page");
// }
//
// private static void PrintResources(string res, string metadataKey)
// {
//     var numMetas = GetNumResourceMetadata(res, metadataKey);
//     Debug.WriteLine($"RES: [{res}]\tnumMetas: [{numMetas}]");
//     for (int j = 0; j < numMetas; j++)
//     {
//         var type = GetResourceMetadata(res, metadataKey, j);
//         var path = LoadResourceFile(res, type);
//         Debug.WriteLine($"TYPE: [{type}]\tFILE: []");
//     }
// }



