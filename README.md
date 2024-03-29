*GoblinAC is still under development and is not in a finished state, it is released for free with no warranty or liability!*
Licenced under the [MIT License](LICENSE)

![GoblinAC](https://socialify.git.ci/Triscuit2311/GoblinAC/image?description=1&font=Source%20Code%20Pro&language=1&owner=1&pattern=Brick%20Wall&stargazers=1&theme=Dark)

# Goblin Anti-Cheat Public

Goblin AC Public is a C# Anti-Cheat for FiveM servers. Goblin AC's primary focus is stopping event trigger exploitation. While most anti-cheats rely on topical methods to stop cheaters from triggering events, Goblin re-structures the way that your scripts communicate across the client-server threshold. Goblin AC provides a platform to proxy all event triggers through a more secure channel. This means your server scripts will only accept events triggered by the server, completely stopping the common `TriggerServerEvent(...)` exploitation.

## **How does it work?**

Goblin AC has two main components: Client and Server. Between the two, a line of communication is created via a single server event trigger. 

The Server issues a global client key (one for the whole server), an individual key (one per connected player), and a set of numerical keys (default 4 per player). Once issued, the keys expire (default 30 seconds) and will be re-issued continuously to each client until that client is disconnected.

The client has a local event handler to proxy calls to the server, when this recieves an event from another script, it encodes the arguments and ushers them though to the server. This means the only server event being triggered fromt he client, is for Goblin AC. As the events are encoded, they are difficult to understand for the would-be cheater. Since the global and client keys are unicode, the encoded events cannot be re-triggered either.

On the Server side, when an event comes in, it is decoded using the last issued keys to that specific client. This means that any attempt to send de-coded events to the server, modify the keys, or the arguments of an event will result in an invalid event call. Trying to trigger an event with any of these conditions will trigger an alert on the server. Upon recieving a valid event, GoblinAC will usher that call to the correct server-sided script.
  
## **Showcase** (Using commonly available cheat software)

What the server gets from Goblin AC vs. What the client sends/sees:
![Event Logger](/img/goblic_ac_eventlogger.png)

Attempting to re-trigger events:
![Re-Triggers](/img/goblin_ac_retrigger.png)

## **So can I just drop it in?**

**No**. Setting up Goblin AC requires a very basic understanding of syntax for your scripts. It is compatible with C# and Lua, anything that triggers events between the server and the client.

### **How can I set it up?**
- Change the events on your server-sided scripts to only accept server-sided triggers. See [example script server.lua](/examples/TestResource/server/server.lua).
- Send your client-sided events to EventProxy() instead of to the server. [example script client.lua](/examples/TestResource/client/client.lua).

### **Couldn't this be bypassed?**
**Yes.** A somewhat experienced individual could write a system to work around the features of GoblinAC. And while there are some features planned that will make this significantly more difficult (i.e. dynamically loading your lua scripts so they aren't searchable); that's not our primary target.
Those inividuals willing and able to work around GoblinAC are far from the average cheat-abuser. GoblinAC is still highly effective against the majority of cheaters using tools to trigger events.

### **Why?**
As FiveM servers implement more security in the form of server-sided checks, hackers are getting more creative. I have been working on custom Anti-Cheat development services for private servers with a much broader spectrum of features, but this specific feature (anti-trigger spam) found it's way into every single one. I figured I would release this to the community for free, learn from it and make your own if you're so inclined!
