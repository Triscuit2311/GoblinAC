
-- Example of a proxy function
function Proxy(...)
    TriggerEvent('Goblin::Client::EventProxy', ...)
end


RegisterCommand('luatest', function(source, args)
    
    -- Old server trigger
    --TriggerServerEvent("FakeScript::Server::GiveMoney", 123, 9.29123, "Cash")
    -- Since we do not have a client event handler on the server side
    -- TrigerServerEvent(...) will not do anything.

    -- Proxy the through Goblin event instead
    Proxy("FakeScript::Server::GiveMoney", 123, 9.29123, "Cash")

    
end, false)
