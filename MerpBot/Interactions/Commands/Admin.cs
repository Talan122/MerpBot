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

}
