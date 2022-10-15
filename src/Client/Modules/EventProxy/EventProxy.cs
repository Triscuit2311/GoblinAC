using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CitizenFX.Core;
using Goblin.Shared;

namespace GoblinAC.Client.Modules;

public class EventProxy : BaseScript
{
    private List<object[]> _eventQueue;

    public EventProxy()
    {
        _eventQueue = new List<object[]>();
        EventHandlers.Add("Goblin::Client::EventProxy", new Action<object[]>(ProxyEvent));
    }

    private void ProxyEvent(params object[] args)
    {
        if (!KeyManager.AllKeys)
        {
            _eventQueue.Add(args);
            return;
        }

        var globalKey = KeyManager.GlobalKey;
        var clientKey = KeyManager.ClientKey;
        var numKeys = KeyManager.NumericalKeys;

        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case short _:
                case int _:
                    args[i] = Utilities.EncodeInt(numKeys, int.Parse(args[i].ToString()));
                    break;
                case float _:
                case double _:
                    args[i] = Utilities.EncodeDouble(numKeys, double.Parse(args[i].ToString()));
                    break;
                case string _:
                default:
                    args[i] = Utilities.EncodeStr(globalKey, clientKey, args[i].ToString());
                    break;
            }

        TriggerServerEvent("Goblin::Server::EventInterpreter::SendEvent", args);
    }
    

    [Tick]
    private async Task<Task<int>> OnTick()
    {
        if (_eventQueue.Count > 0 && KeyManager.AllKeys)
        {
            foreach (var argSet in _eventQueue)
            {
                ProxyEvent(argSet);
            }

            _eventQueue.Clear();
        }

        await Delay(1000);
        return Task.FromResult(0);
    }
}