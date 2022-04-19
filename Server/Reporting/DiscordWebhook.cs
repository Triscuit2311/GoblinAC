using CitizenFX.Core;

namespace Goblin.Server.Reporting;

public class DiscordWebhook : BaseScript
{
    private const string WebhookUrl =
        "https://discord.com/api/webhooks/966042083100082226/WXNtjEHWANeLec56dHOdQ7XOFLyeZTNLswlyNFyqsbM9pzc90K-pukG-kkbfC6AW4Zd3";

    private const string AvatarUrl = "https://avatars.githubusercontent.com/u/70150617?v=4";

    public enum WebhookType
    {
        Info,
        Error,
        Violation,
        Other
    }

    private enum WebhookColor
    {
        Green = 6415476,
        Purple = 7419530,
        Red = 15158332,
        Yellow = 16776960
    }

    [Command("DoSendHook")]
    public static void DoHook()
    {
        // SendEmbed(WebhookType.Info, "They're in amish.", "INFO: Big Doinks Located!", "rip.");
        // SendEmbed(WebhookType.Error, "Sum ting wong");
        // SendEmbed(WebhookType.Violation,"Some idiot triggered \'DumEventName5000\' with NULL parameters. ", "Violation: Triggering Event");
        // SendEmbed(WebhookType.Other, "It keeps you safe.", ":japanese_goblin: BUY GOBLIN :japanese_goblin:", "Goblin makes your dick bigger.");
        
        SendMessage("HEY");
    }

    internal static void SendEmbed(WebhookType hookType, string msg, string customTitle = "", string extraInfo = "")
    {
        var color = hookType switch
        {
            WebhookType.Info => WebhookColor.Green,
            WebhookType.Error => WebhookColor.Red,
            WebhookType.Violation => WebhookColor.Yellow,
            _ => WebhookColor.Purple
        };

        var title = customTitle.Length > 0 ? customTitle : hookType.ToString();

        TriggerEvent("Goblin::Extensions::SendDiscordEmbed",
            WebhookUrl,
            AvatarUrl,
            title,
            msg,
            extraInfo,
            color
        );
    }
    
    internal static void SendMessage(string msg)
    {
        TriggerEvent("Goblin::Extensions::SendDiscordMessage",
            WebhookUrl,
            AvatarUrl,
            msg
        );
    }
    

    
    
    
}