using System;
using CitizenFX.Core;
using Goblin.Shared;

namespace GoblinAC.Server.Modules.EventProxy;

public class EventInterpreter : BaseScript
{
    public EventInterpreter()
    {
        EventHandlers["Goblin::Server::EventInterpreter::SendEvent"] +=
            new Action<Player, string, object[]>(ReceiveEvent);
    }

    private void ReceiveEvent([FromSource] Player source,
        string eventName, params object[] args)
    {
      
        if (eventName is null || args is null)
        {
            Debug.WriteLine($"Event Manually Triggered by: [{source.Name}]");
            return;
        }

        var gc = KeyManager.GlobalKey;
        var cc = KeyManager.ClientKeyLookup(source.Identifiers["fivem"]).ClientKey;
        var nc = KeyManager.ClientKeyLookup(source.Identifiers["fivem"]).NumericalKeys;

        eventName = Utilities.EncodeStr(
            gc,
            cc,
            eventName);

        //var sb = new StringBuilder("(Server) Triggering Event: (");
        //sb.Append(eventName);

        for (var i = 0; i < args.Length; i++)
        {
            //sb.Append(", ");
            switch (args[i])
            {
                case string _:
                    args[i] = Utilities.EncodeStr(gc, cc, args[i].ToString());
                    if ((string)args[i] == "SOURCE_ID") args[i] = source.Identifiers["fivem"];
                    break;
                case short _:
                case int _:
                    args[i] = Utilities.DecodeInt(nc, int.Parse(args[i].ToString()));
                    break;
                case float _:
                case double _:
                    args[i] = Utilities.DecodeDouble(nc, double.Parse(args[i].ToString()));
                    break;
            }

            //sb.Append(args[i]);
        }

        //sb.Append(")");
        //Debug.WriteLine(sb.ToString());

        TriggerEvent(eventName, args);
    }
}