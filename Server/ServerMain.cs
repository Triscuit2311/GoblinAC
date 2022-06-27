using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Server.Utils;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
            EventHandlers["CoolFunc1"] += new Action<string>(CoolFunction1);
        }

        [Command("TryPass")]
        private void TryPass([FromSource] Player source)
        {
            TriggerEvent("CoolFunc1", source.Identifiers["fivem"]);
        }
        
        private void CoolFunction1(string id)
        {
            Player source = PlayerUtils.GetPlayerByFiveMid(id, Players);
            Debug.WriteLine(source.Name);
        }

#if DEVELOPMENT_BUILD
        [Command("SelfReport")]
        private void SelfReport([FromSource] Player source)
        {
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Resource Modification","Tried to stop Goblin-AC Client");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Resource Modification","Tried to start a resource called [js92hjcv73bspa1]");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Triggering Events","Triggered Some::Really::Really::Long::Event::Name with Null Parameters.");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Triggering Events","Triggered GiveMoney");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Triggering Events","Triggered GiveSkills");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Triggering Events","Triggered GiveLoveHeNeverHadAsAChild");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Triggering Events","Triggered GiveCasinoChips");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Other Cheats","No Clipping for ~ 25 seconds");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Other Cheats","Invisibility detected for over 1 minute");
            TriggerEvent("Goblin::Server::Reporting::AddOrModifyReportString", source.Identifiers["fivem"], 
                "Other Cheats","Does not take damage (forced by server) - Potential \'God mode\'");
        }
#endif


    }
}