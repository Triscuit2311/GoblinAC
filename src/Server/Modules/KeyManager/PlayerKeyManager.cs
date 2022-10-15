namespace GoblinAC.Server.Modules;

// ReSharper disable UnusedMember.Local
public class PlayerKeyManager
{
    public string ClientKey;
    public bool NeedsKeys;
    public int[] NumericalKeys;

    public PlayerKeyManager(string clientKey, int[] numericalKeys)
    {
        NumericalKeys = numericalKeys;
        ClientKey = clientKey;
        NeedsKeys = true;
    }

    public PlayerKeyManager()
    {
        NumericalKeys = new int[] { };
        ClientKey = "";
        NeedsKeys = true;
    }
}