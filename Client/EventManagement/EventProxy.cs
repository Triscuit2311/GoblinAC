using System;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Client.Crypto;
using Goblin.Shared;

namespace Goblin.Client.EventManagement
{
    public class EventProxy : BaseScript
    {
        public EventProxy()
        {
            EventHandlers.Add("Goblin::Client::EventProxy", new Action<object[]>(ProxyEvent));
        }

        
        [Command("tryev")]
        public async Task TryEvent()
        {
            TriggerEvent("Goblin::Client::EventProxy", "funevent", "sssss");
            Random genny = new Random();
            for (var i = 0; i < 10; i++)
            {
                TriggerEvent("Goblin::Client::EventProxy", 
                    SharedUtils.RandomAscii(genny.Next(5,30)), 
                    "A",
                    "B",
                    genny.Next(-100,100), 
                    genny.NextDouble());
                await Delay(100);
            }
        }
        
        private void ProxyEvent(params object[] args)
        {
            var globalKey = KeyManager.GlobalKey;
            var clientKey = KeyManager.ClientKey;
            var numKeys = KeyManager.NumericalKeys;
            
            StringBuilder sb = new StringBuilder("Triggering Server Event: (Goblin::Server::EventManagement::Event");

            for (var i = 0; i < args.Length; i++)
            {
                sb.Append(", ");
                switch (args[i])
                {
                    case string _:
                        args[i] =  SharedUtils.ComposeHash(globalKey, clientKey, args[i].ToString());
                        break;
                    case short _:
                    case int _:
                        args[i] = SharedUtils.ConcealInt(numKeys, int.Parse(args[i].ToString()));
                        break;
                    case float _:
                    case double _:
                        args[i] = SharedUtils.ConcealDouble(numKeys, double.Parse(args[i].ToString()));
                        break;
                }
                sb.Append(args[i].ToString());
            }
            
            sb.Append(")");
            Debug.WriteLine(sb.ToString());

            TriggerServerEvent("Goblin::Server::EventManagement::Event", args);
            
        }
    }
}