using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace MerpBot.Services;
public class StartupService
{
    public static IServiceProvider Provider;
    private readonly DiscordSocketClient Discord;
    private readonly IConfiguration Config;
    private readonly Logger Logger;
    public static Dictionary<string, DiscordWebhookClient> Webhooks;

    public StartupService(IServiceProvider provider, DiscordSocketClient discord, IConfigurationRoot config, Logger logger)
    {
        Provider = provider;
        Discord = discord;
        Config = config;
        Logger = logger;
        Webhooks = new();

        WebhookService.Client = discord;
        WebhookService.Logger = logger;

        Discord.Ready += OnReady;
        Discord.Log += Helpers.LogAsync;
    }

    public async Task StartAsync()
    {
        // Connect bot.
        string? token = Config["DiscordToken"];
        if(token is null) Logger.CriticalWithCrash("DiscordToken couldn't be found in the .env file.");

        await Discord.LoginAsync(TokenType.Bot, token);
        await Discord.StartAsync();
    }

    private async Task OnReady()
    {

        WebhookService.GetDiscordWebhookClients(ref Webhooks);

        Logger.Info($"Connected as {Discord.CurrentUser.Username}#{Discord.CurrentUser.Discriminator}", "Startup");

        ActivityType Activity = ActivityType.Playing;

        if(!Enum.TryParse(Config["StartupStatus:StatusType"], out Activity)) 
            Logger.Warning("Activity type is not set correctly in Globals.yml. Defaulting to Playing.", "Startup"); // No need to crash.

        await Discord.SetGameAsync(Config["StartupStatus:StatusMessage"], type: Activity);
        Logger.Verbose($"Set activity to type {Activity}, type {Config["StartupStatus:StatusMessage"]}", "Startup");

        if(!ulong.TryParse(Config["Channels:ErrorChannel"], out ulong ChannelID)) 
            Logger.CriticalWithCrash("Error channel could not be found. Make sure it's set to a valid channel in Globals.yml.", "Startup");

        Helpers.ErrorChannel = (ITextChannel)Discord.GetChannel(ChannelID);

        if (!ulong.TryParse(Config["DebugGuild"], out ulong DebugGuild))
            Logger.CriticalWithCrash("Debug guild could not be found. Make sure it's set to a valid guild in Globals.yml.", "Startup");

        await InteractionHandler.Interactions.RegisterCommandsToGuildAsync(DebugGuild);
    }
}
