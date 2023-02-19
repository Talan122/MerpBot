using Discord;
using Discord.Interactions;
using Discord.Interactions.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Services;
using MerpBot.Interactive.Preconditions;
using System.Reflection;

namespace MerpBot.Interactive.Commands;

[Group("admin", "Admin commands")]
public class Admin : InteractionModuleBase<SocketInteractionContext>
{
    public DiscordSocketClient Client { get; set; }
    public InteractionService InteractionService { get; set; }

    [RequireOwner]
    [SlashCommand("shutdown", "Shuts down the bot")]
    public async Task Shutdown(int ExitCode = 0)
    {
        await RespondAsync(OtherStuff.GetShutdownResponse());
        Environment.Exit(ExitCode);
    }

    [RequireOwner]
    [SlashCommand("register", "Registers commands.")]
    public async Task Register()
    {
        SocketGuild[] guilds = Client.Guilds.ToArray();

        foreach (var guild in guilds)
            await InteractionService.RegisterCommandsToGuildAsync(guild.Id);

        await RespondAsync("All commands registered.");
    }

    [RequireOwner]
    [SlashCommand("getinvite", "Gets invite links. It's a private bot, it won't work for anyone.")]
    public async Task GetInvite()
    {
        await RespondAsync($"Invite: https://discord.com/oauth2/authorize?client_id=886710931906773053&scope=bot&permissions=8\nOAuth thingy: https://discord.com/api/oauth2/authorize?client_id=886710931906773053&scope=applications.commands");
    }

    [RequireOwner]
    [SlashCommand("status", "Set the status of the bot")]
    public async Task Status(ActivityType Type, string Name)
    {
        await Client.SetGameAsync(Name, null, Type);
        await RespondAsync($"You set my status to type: {Type}, name: {Name}");
    }

    [RequireOwner]
    [SlashCommand("module", "Enable/Disable a module.")]
    public async Task DisableModule(string module, bool value)
    {
        try
        {
            Settings.SetSettingValue(module, value);
            await RespondAsync($"Set {module} to {value}", ephemeral: true);
        }
        catch(Exception e)
        {
            await RespondAsync("I think you put in an invalid module.", ephemeral: true);
        }


    }

    [RequireOwner]
    [SlashCommand("say", "Makes the bot say something.")]
    public async Task Say(string data, ISocketMessageChannel? channel = null, string? messageId = null, bool ping = true)
    {
        //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " (In /say)");
        if (channel == null) channel = Context.Channel;

        if (messageId != null)
        {
            var message = (IUserMessage)await channel.GetMessageAsync(ulong.Parse(messageId));
            await message.ReplyAsync(data, allowedMentions: new AllowedMentions { MentionRepliedUser = ping });
            await RespondAsync($"Said \"{data}\" in <#{channel.Id}> in a reply to someone.", ephemeral: true);
        }
        else
        {
            await channel.SendMessageAsync(data);
            await RespondAsync($"Said \"{data}\" in <#{channel.Id}>", ephemeral: true);
        }

    }

    [RequireOwner]
    [SlashCommand("dm", "DMs a member.")]
    public async Task DM(IUser user, string data)
    {
        await user.SendMessageAsync(data);
        await RespondAsync($"Sent \"{data}\" to <@!{user.Id}>", ephemeral: true);
    }
}
