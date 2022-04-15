using System.Collections.Generic;
using CitizenFX.Core;

namespace Goblin.Server.Reporting
{
    public class PlayerReport
    {
        public string PlayerName;
        public IdentifierCollection Ids;
        public Dictionary<string, List<string>> ReportStrings;

        public PlayerReport(string playerName, IdentifierCollection ids, Dictionary<string, List<string>> reportStrings)
        {
            PlayerName = playerName;
            Ids = ids;
            ReportStrings = reportStrings;
        }
    }
}