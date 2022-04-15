using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace Goblin.Server.Reporting
{
    public class ReportManager : BaseScript
    {
        private Dictionary<string, PlayerReport> _reports;

        public ReportManager()
        {
            _reports = new Dictionary<string, PlayerReport>();
            EventHandlers["AddOrModifyReportString"] += new Action<Player, string, string>(AddOrModifyReportString);
        }

        private void AddOrModifyReportString(Player player, string reportType, string report )
        {
            var id = player.Identifiers["fivem"];
            if (!_reports.ContainsKey(id))
            {
                _reports.Add(id, new PlayerReport(
                    player.Name,
                    player.Identifiers,
                    new Dictionary<string, string>()));
            }
            _reports[id].ReportStrings[reportType] = $"[{DateTime.Now}] {report}";
        }
        
    }
}