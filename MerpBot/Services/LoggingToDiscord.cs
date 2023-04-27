using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace MerpBot.Services;

public class LoggingToDiscord
{
    private DiscordSocketClient Client { get; set; }
    private IConfigurationRoot Config { get; set; }
    private DiscordWebhookClient WebhookClient { get; set; }

    public LoggingToDiscord(DiscordSocketClient client, IConfigurationRoot config)
    {
        Client = client;
        Config = config;

        WebhookClient = new(Config["LoggingWebhook"]);
    }

    private List<string> Messages = new List<string>();

    public void AddLogMessage(string message)
    {
        Messages.Add(message);
    }

    public void Start()
    {
        Timer timer = new Timer(async (a) =>
        {
            if (!Messages.Any()) return;

            await WebhookClient.SendMessageAsync($"```txt\n{Helpers.CombineStringArray(Messages, combineWith: "\n")}```", avatarUrl: Client.CurrentUser.GetAvatarUrl(), username: "MerpBot Logging");

            Messages.RemoveRange(0, Messages.Count);
        });

        timer.Change(0, 2000);
    }
}