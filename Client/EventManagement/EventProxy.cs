using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Client.Crypto;
using Goblin.Shared;

namespace Goblin.Client.EventManagement
{
    public class EventProxy : BaseScript
    {
        private List<object[]> EventQueue;
        public EventProxy()
        {
            EventQueue = new List<object[]>();
            EventHandlers.Add("Goblin::Client::EventProxy", new Action<object[]>(ProxyEvent));
        }
        
        
        private void ProxyEvent(params object[] args)
        {
            if (!KeyManager.AllKeys)
            {
                EventQueue.Add(args);
                Debug.WriteLine("Added event to Queue.");
                return;
            }
            
            var globalKey = KeyManager.GlobalKey;
            var clientKey = KeyManager.ClientKey;
            var numKeys = KeyManager.NumericalKeys;

            for (var i = 0; i < args.Length; i++)
            {
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
            }
            TriggerServerEvent("Goblin::Server::EventManagement::Event", args);
        }

        
        [Tick]
        private async Task<Task<int>> OnTick()
        {
            if (EventQueue.Count > 0 && KeyManager.AllKeys)
            {
                foreach (var argSet in EventQueue)
                {
                    Debug.WriteLine("Sending Event from queue.");
                    ProxyEvent(argSet);
                }
                EventQueue.Clear();
            }
            
            await Delay(1000);
            return Task.FromResult(0);
        }
        
    }
}