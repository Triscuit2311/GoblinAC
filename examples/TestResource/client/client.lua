
function Proxy(...)
    TriggerEvent('Goblin::Client::EventProxy', ...)
end

RegisterCommand('luatest', function(source, args)
    Proxy("FakeScript::Server::GiveMoney", 123, 9.29123, "Cash")
    --TriggerServerEvent("FakeScript::Server::GiveMoney", 123, 9.29123, "Cash")
end, false)