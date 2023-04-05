using Discord;
using Discord.WebSocket;
using Discord.Webhook;
using Discord.Interactions;
using MerpBot.Services;
using System.Text;

namespace MerpBot.Interactions.Commands;

[Group("admin", "Admin commands.")]
public class Admin : InteractionModuleBase<SocketInteractionContext>
{
    public Logger Logger { get; set; }
    public DiscordSocketClient Client { get; set; }

    [SlashCommand("shutdown", "Shutdown the bot.")]
    [RequireOwner]
    public async Task Shutdown(int ExitCode = 0)
    {
        await RespondAsync("Shutting down.");
        Environment.Exit(ExitCode);
    }

    [SlashCommand("register", "Registers every command to every server the bot is in.")]
    [RequireOwner]
    public async Task Register()
    {
        await DeferAsync(ephemeral: true);

        SocketGuild[] guilds = Client.Guilds.ToArray();

        foreach (var guild in guilds)
        {
            await InteractionHandler.Interactions.RegisterCommandsToGuildAsync(guild.Id);
            await FollowupAsync($"Pushed commands to {guild.Name}", ephemeral: true);

            await Task.Delay(2000);
        }
    }

    [SlashCommand("setstatus", "Sets the status of the bot.")]
    [RequireOwner]
    public async Task SetStatus(ActivityTypeForBot activity, string name)
    {
        await Client.SetGameAsync(name, type: Enum.Parse<ActivityType>(activity.ToString()));
        await RespondAsync($"Set status to \"{activity} {name}\"", ephemeral: true);
    }

    [SlashCommand("invitelink", "Get the invite link of the bot.")]
    [RequireOwner]
    public async Task InviteLink()
    {
        await RespondAsync($"https://discord.com/api/oauth2/authorize?client_id=886710931906773053&permissions=8&scope=applications.commands%20bot", ephemeral: true);
    }
}

// This is the same as Discord.ActivityType but without CustomStatus since bots cant even use it.
public enum ActivityTypeForBot
{
    Playing,
    Streaming,
    Listening,
    Watching,
    //CustomStatus,
    Competing = 5
}