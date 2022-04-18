using System;
using System.Text;
using CitizenFX.Core;
using Goblin.Shared;

namespace Goblin.Server.Events
{
    public class EventInterpreter : BaseScript
    {
        public EventInterpreter()
        {
            EventHandlers["Goblin::Server::EventManagement::Event"] += new Action<Player, string, object[]>(ReceiveEvent);
            EventHandlers["funevent"] += new Action<object[]>(FunEvent);
        }

        private void FunEvent(params object[] args)
        {
            Debug.WriteLine("FunEvent!!");
            for (var i = 0; i< args.Length; i++)
            {
                Debug.WriteLine($"arg[{i}]: {args[i]}");
            }
        }

        private void ReceiveEvent([FromSource]Player source,
            string eventName, params object[] args)
        {
            if (eventName is null || args is null)
            {
                Debug.WriteLine($"Bad event call from: [{source.Name}]");
                return;

            }

            var gc = Crypto.Provider.GlobalClientKey;
            var cc = Crypto.Provider.ClientKeyLookup(source.Identifiers["fivem"]).ClientKey;
            var nc  = Crypto.Provider.ClientKeyLookup(source.Identifiers["fivem"]).NumericalKeys;
            
            eventName = SharedUtils.ComposeHash(
                    gc,
                    cc,
                    eventName);

            var sb = new StringBuilder("(Server) Triggering Event: (");
            sb.Append(eventName);
            
            for (var i = 0; i < args.Length; i++)
            {
                sb.Append(", ");
                switch (args[i])
                {
                    case string _:
                        args[i] =  SharedUtils.ComposeHash(gc, cc, args[i].ToString());
                        break;
                    case short _:
                    case int _:
                        args[i] = SharedUtils.RevealInt(nc, int.Parse(args[i].ToString()));
                        break;
                    case float _:
                    case double _:
                        args[i] = SharedUtils.RevealDouble(nc, double.Parse(args[i].ToString()));
                        break;
                }
                sb.Append(args[i]);
            }
            
            sb.Append(")");
            Debug.WriteLine(sb.ToString());
            
            TriggerEvent(eventName, args  );
        }
    }
}
