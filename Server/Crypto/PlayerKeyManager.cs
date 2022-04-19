namespace Goblin.Server.Crypto;

public class PlayerKeyManager
{
    public string ClientKey;
    public int[] NumericalKeys;
    public bool NeedsKeys;

    public PlayerKeyManager(string clientKey, int[] numericalKeys)
    {
        this.NumericalKeys = numericalKeys;
        this.ClientKey = clientKey;
        NeedsKeys = true;
    }

    public PlayerKeyManager()
    {
        this.NumericalKeys = new int[]{};
        this.ClientKey = "";
        NeedsKeys = true;
    }
}