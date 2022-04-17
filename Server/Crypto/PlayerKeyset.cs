namespace Goblin.Server.Crypto;

public struct PlayerKeyset
{
    public string ClientKey;
    public int[] NumericalKeys;

    public PlayerKeyset(string clientKey, int[] numericalKeys)
    {
        this.NumericalKeys = numericalKeys;
        this.ClientKey = clientKey;
    }
}