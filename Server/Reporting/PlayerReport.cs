using System.Collections.Generic;
using CitizenFX.Core;

namespace Goblin.Server.Reporting
{
    public class PlayerReport
    {
        private string PlayerName;
        private IdentifierCollection Ids;
        public Dictionary<string, string> ReportStrings;

        public PlayerReport(string playerName, IdentifierCollection ids, Dictionary<string, string> reportStrings)
        {
            PlayerName = playerName;
            Ids = ids;
            ReportStrings = reportStrings;
        }
    }
}