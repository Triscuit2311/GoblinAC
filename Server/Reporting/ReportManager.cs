using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using Goblin.Server.Utils;

namespace Goblin.Server.Reporting
{
    public class ReportManager : BaseScript
    {
        private readonly Dictionary<string, PlayerReport> _reports;

        public ReportManager()
        {
            _reports = new Dictionary<string, PlayerReport>();
            EventHandlers["Goblin::Server::Reporting::AddOrModifyReportString"] 
                += new Action<string, string, string>(AddOrModifyReportString);
            EventHandlers["Goblin::Server::Reporting::SendDiscordReport"]
                += new Action<string>(SendDiscordReport);
        }
        
        private void AddOrModifyReportString(string id, string reportType, string report )
        {
            var player = PlayerUtils.GetPlayerByFiveMid(id, Players);
            
            if (!_reports.ContainsKey(id))
            {
                _reports.Add(id, new PlayerReport(
                    player.Name,
                    player.Identifiers,
                    new Dictionary<string, List<string>>()));
            }

            if (!_reports[id].ReportStrings.ContainsKey(reportType))
            {
                _reports[id].ReportStrings.Add(reportType, new List<string>());
            }
            
            _reports[id].ReportStrings[reportType].Add( $"[{DateTime.Now}] {report}");
        }

        private void PrintReports()
        {
            Debug.WriteLine($"\nPrinting {_reports.Count} reports:\n");
            foreach (var report in _reports)
            {
                Debug.WriteLine($"{report.Value.PlayerName}:");
                foreach (var id in report.Value.Ids)
                {
                    Debug.WriteLine($"\t\t{id}");
                }

                foreach (var kvp in report.Value.ReportStrings)
                {
                    Debug.WriteLine($"\t{kvp.Key}:");
                    foreach (var s in kvp.Value)
                    {
                        Debug.WriteLine($"\t\t{s}");
                    }
                }
                
            }
        }

#if DEVELOPMENT_BUILD
        [Command("SendDiscordReports")]
        public void SendMyOwnReport([FromSource]Player source)
        {
            Debug.WriteLine(source.Name);
            SendDiscordReport(source.Identifiers["fivem"]);
        }
#endif
        
        private void SendDiscordReport(string id)
        {
            if (!_reports.ContainsKey(id))
            {
                Debug.WriteLine($"Attempt to print report for player [{id}], no reports available.");
                return;
            }
            var msgBody = new StringBuilder();
            msgBody.Append("**Player Name:** ");
            msgBody.Append(_reports[id].PlayerName);
            msgBody.Append('\n');
            msgBody.Append("**FiveM ID:** ");
            msgBody.Append(id);
            msgBody.Append('\n');
            
            if(_reports[id].Ids.Contains("steamid"))
            {
                msgBody.Append("**Steam:** ");
                msgBody.Append(_reports[id].Ids["steamid"]);
                msgBody.Append('\n');
            }
            
            msgBody.Append("**Discord:** [");
            msgBody.Append(_reports[id].Ids["discord"]);
            msgBody.Append("](https://www.discoid.cc/");
            msgBody.Append(_reports[id].Ids["discord"]);
            msgBody.Append(")\n**Reports:**\n\n");
            foreach (var kvp in _reports[id].ReportStrings)
            {
                msgBody.Append("**");
                msgBody.Append(kvp.Key);
                msgBody.Append(": **\n```diff\n");
                foreach (var s in kvp.Value)
                {
                    msgBody.Append("- ");
                    msgBody.Append(s);
                    msgBody.Append('\n');
                }
                msgBody.Append("```\n");

            }
        
            DiscordWebhook.SendEmbed(DiscordWebhook.WebhookType.Violation,
                msgBody.ToString(), $"Player Report: {_reports[id].PlayerName}");
        }
        
    }
}