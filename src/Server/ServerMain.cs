using CitizenFX.Core;

namespace GoblinAC.Server;

public class ServerMain : BaseScript
{
    public ServerMain()
    {
        Debug.WriteLine("GoblinAC.Server is Running");
    }

    [Command("gobsvr")]
    public void HelloServer()
    {
        Debug.WriteLine("ServerCmd Executed");
    }
}