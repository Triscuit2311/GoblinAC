using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Goblin.Server.Utils;

namespace Goblin.Server.Reporting
{
    public class ReportManager : BaseScript
    {
        private Dictionary<string, PlayerReport> _reports;

        public ReportManager()
        {
            
            _reports = new Dictionary<string, PlayerReport>();
            EventHandlers["AddOrModifyReportString"] += new Action<string, string, string>(AddOrModifyReportString);

            EventHandlers["PrintReports"] += new Action(PrintReports);
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

        public void PrintReports()
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
        
        
    }
}