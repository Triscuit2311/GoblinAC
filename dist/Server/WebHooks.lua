print("Started Discord webhooks")

AddEventHandler("Goblin::Extensions::SendDiscordWebhook", function(webhookUrl, avatar, title, message, footer, color)
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