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
        await FollowupAsync(OtherStuff.GetShutdownResponse());
        await Helpers.LogCommand("Shutting down", Context);
        Environment.Exit(ExitCode);
    }

    [RequireOwner]
    [SlashCommand("register", "Registers commands.")]
    public async Task Register()
    {
        await InteractionService.RegisterCommandsToGuildAsync(851204839605927946);
        await InteractionService.RegisterCommandsToGuildAsync(965325307001339945);
        await InteractionService.RegisterCommandsToGuildAsync(878660332506202122);
        await InteractionService.RegisterCommandsToGuildAsync(959183870085955615);
        await FollowupAsync("All commands registered.");
    }

    [RequireOwner]
    [SlashCommand("getinvite", "Gets invite links. It's a private bot, it won't work for anyone.")]
    public async Task GetInvite()
    {

    }

    /*[RequireOwner]
    [SlashCommand("say", "Makes the bot say something.")]
    public async Task Say(string data, ISocketMessageChannel? channel = null, string? messageId = null, bool ping = true)
    {
        if (channel == null) channel = Context.Channel;

        if (messageId != null)
        {
            var message = (IUserMessage)await channel.GetMessageAsync(ulong.Parse(messageId));
            await message.ReplyAsync(data, allowedMentions: new AllowedMentions { MentionRepliedUser = ping });
            await FollowupAsync($"Said \"{data}\" in <#{channel.Id}> in a reply to someone.", ephemeral: true);
        }
        else
        {
            await channel.SendMessageAsync(data);
            await FollowupAsync($"Said \"{data}\" in <#{channel.Id}>", ephemeral: true);
        }
    }*/

    [RequireOwner]
    [SlashCommand("status", "Set the status of the bot")]
    public async Task Status(ActivityType Type, string Name)
    {
        await Client.SetGameAsync(Name, null, Type);
        await FollowupAsync($"You set my status to type: {Type}, name: {Name}");
    }

    [RequireOwner]
    [SlashCommand("module", "Enable/Disable a module.")]
    public async Task DisableModule(string module, bool value)
    {
        try
        {
            Settings.SetSettingValue(module, value);
            await FollowupAsync($"Set {module} to {value}", ephemeral: true);
        }
        catch(Exception e)
        {
            await FollowupAsync("I think you put in an invalid module.", ephemeral: true);
        }


    }
}
