using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace MerpBot.Services;
public class InteractionHandler
{
    public IServiceProvider Provider;
    public DiscordSocketClient Discord;
    public IConfigurationRoot Config;
    public static InteractionService Interactions;
    public Logger Logger;

    public InteractionHandler(IServiceProvider provider, DiscordSocketClient discord, InteractionService interactions, IConfigurationRoot config, Logger logger)
    {
        Provider = provider;
        Discord = discord;
        Interactions = interactions;
        Config = config;
        Logger = logger;

        Discord.InteractionCreated += OnInteractionCreated;
        Interactions.SlashCommandExecuted += OnSlashCommandExecuted;
    }

    public async Task InitAsync()
    {
        Logger.Debug("Initializing slash commands.");
        await Interactions.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), Provider);
    }

    private async Task OnInteractionCreated(SocketInteraction interaction)
    {
        SocketInteractionContext context = new SocketInteractionContext(Discord, interaction);

        try
        {
            await Interactions.ExecuteCommandAsync(context, Provider);
        }
        catch (Exception e)
        {
            Logger.Error($"Interaction Error: {e}");
            var InfoChannel = (IMessageChannel)Discord.GetChannel(ulong.Parse(Config["Channels:ErrorChannel"] 
                ?? throw new Exception("No erorr channel was found.")));

            await InfoChannel.SendMessageAsync(embed: GenerateErrorEmbed(context, e.Message));

            if(interaction.Type == InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }

    private async Task OnSlashCommandExecuted(SlashCommandInfo command, IInteractionContext context, IResult result)
    {
        if (result.Error is null) return;

        if(result.Error == InteractionCommandError.Exception)
        {
            if (context.Interaction.HasResponded) await context.Channel.SendMessageAsync("There was an internal error, please check the logs");
            else await context.Interaction.FollowupAsync("There was an internal error, please check the logs.");

            var InfoChannel = (IMessageChannel)Discord.GetChannel(ulong.Parse(Config["Channels:ErrorChannel"]
                ?? throw new Exception("No erorr channel was found.")));

            await InfoChannel.SendMessageAsync(embed: GenerateErrorEmbed((SocketInteractionContext)context, result.ErrorReason));

            return;
        }

        if (!context.Interaction.HasResponded) await context.Interaction.RespondAsync(result.ErrorReason, ephemeral: true);
        else await context.Interaction.FollowupAsync(result.ErrorReason, ephemeral: true);
    }

    private static Embed GenerateErrorEmbed(SocketInteractionContext context, string message)
    {
        EmbedBuilder ErrorMessage = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithAuthor(context.User.Username)
                .WithThumbnailUrl(context.User.GetAvatarUrl())
                .AddField("Server", context.Guild.Name, true)
                .AddField("Channel", $"<#{context.Channel.Id}>", true)
                .AddField("Error", message, false);

        return ErrorMessage.Build();
    }
}