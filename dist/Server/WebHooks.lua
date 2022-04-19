print("Started Discord webhooks")

AddEventHandler("Goblin::Extensions::SendDiscordEmbed", function(webhookUrl, avatar, title, message, footer, color)
	local connect = {
		{
		  	["color"] = color,
		  	["title"] = "**".. title .."**\n",
			["description"] = message,
			["footer"] = {
				["text"] = footer,
			},
		  }
	  }
	PerformHttpRequest(webhookUrl, function(err, text, headers) end, 'POST', json.encode({username = "Goblin", embeds = connect, avatar_url = avatar}), { ['Content-Type'] = 'application/json' })
end)

AddEventHandler("Goblin::Extensions::SendDiscordMessage", function(webhookUrl, avatar, message)

	PerformHttpRequest(webhookUrl, function(err, text, headers) end, 'POST', json.encode({username = "Goblin",  avatar_url = avatar, content = message}), { ['Content-Type'] = 'application/json' })
end)
