using CitizenFX.Core;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Goblin.Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
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