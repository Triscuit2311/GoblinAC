
AddEventHandler("FakeScript::Server::GiveMoney", function(...)
    for i,v in ipairs({...}) do
        print(v)
    end
end)