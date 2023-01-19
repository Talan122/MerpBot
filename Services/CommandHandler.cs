using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace MerpBot.Services;
public class CommandHandler
{
    public static IServiceProvider _provider;
    public static DiscordSocketClient _discord;
    public static CommandService _commands;
    public static IConfiguration _config;

    public CommandHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
    {
        _provider = provider;
        _discord = discord;
        _commands = commands;
        _config = config;

        Helpers.SetClient(discord);

        _discord.Ready += Onready;
        _discord.Log += Log;
        _discord.MessageReceived += OnMessageReceived;
        //_commands.CommandExecuted += OnCommandExecuted;
    }

    private async Task Onready()
    {

        Helpers.ConsoleWithTimeStamp($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
        await _discord.SetGameAsync("merp merp merp merp merp merp merp merp merp", type: ActivityType.Listening);
        // InteractionHandler._interactions.RegisterCommandsToGuildAsync(851204839605927946);
        // Only uncomment if you're having issues with /register!
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg.ToString());
        return Task.CompletedTask;
    }

    private async Task OnCommandExecuted(Optional<CommandInfo> info, ICommandContext context, IResult result)
    {
        //exceptions
        if(result.Error == CommandError.Exception)
        {
            await context.Channel.SendMessageAsync("There was an internal error, please check the logs");

            var infoChannel = (IMessageChannel)_discord.GetChannel(GlobalIDs.errorChannel);
            await infoChannel.SendMessageAsync(
                $"There was an error in {context.Guild.Name} in <#{context.Channel.Id}>\n" +
                $"jump link: {context.Message.GetJumpUrl()} \n" +
                $"error: {result.ErrorReason}");
        }
        //general errors
        else if(result.Error != null)
        {
            if (result.ErrorReason == "Unknown command.")
                _ = BotPinged((SocketCommandContext)context); //it wasn't a command
            else if (result.Error == CommandError.UnmetPrecondition)
                await context.Channel.SendMessageAsync("You may not use this command");
            else
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }

    private async Task OnMessageReceived(SocketMessage arg)
    {
        if (arg is SocketUserMessage message) //safe cast message to prevent errors for example pin added notifications
        {
            if (message.Author.IsBot) return;

            SocketCommandContext context = new SocketCommandContext(_discord, message);

            if (context.Channel.GetChannelType() == ChannelType.DM)
            {
                await DMMessageHandling(message, context);
            }
            else //all other channels (in this case guild text channels really)
            {
                try
                {
                    //command handling
                    if (message.Content.Contains($"<@{_discord.CurrentUser.Id}>") || message.Content.Contains($"<@{_discord.CurrentUser.Id}>"))
                    {
                        await BotPinged(context);
                    }

                    try
                    {
                        await AtSomeone(message);
                    }
                    catch (Exception e) { };
                }
                catch (Exception e)
                {
                    await context.Channel.SendMessageAsync("I wasn't able to process whatever you typed");

                    var infoChannel = (IMessageChannel)_discord.GetChannel(GlobalIDs.errorChannel);
                    await infoChannel.SendMessageAsync(
                        $"There was an error in {context.Guild.Name} in <#{context.Channel.Id}>\n" +
                        $"jump link: {context.Message.GetJumpUrl()} \n" +
                        $"error: {e}");
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }

    private async Task DMMessageHandling(SocketUserMessage message, SocketCommandContext context)
    {
        IMessageChannel DMChannel = (IMessageChannel)_discord.GetChannel(1065719004762746892);

        await DMChannel.SendMessageAsync($"From {message.Author},\n{message}");
    }

    private async Task AtSomeone(SocketUserMessage message)
    {
        if (!Settings.GetSettingValue("AtSomeone")) return;

        if (message.MentionedRoles.First().Name.ToLower() == "someone" || message.Content.Contains("@someone"))
        {
            IUser[] users = (await AsyncEnumerableExtensions.FlattenAsync<IUser>(message.Channel.GetUsersAsync())).ToArray<IUser>();
            Random random = new Random();

            IUser randomUser = users[random.Next(users.Length)];

            while (randomUser.IsBot)
            {
                randomUser = users[random.Next(users.Length)];
            }

            await message.Channel.SendMessageAsync($"<@{randomUser.Id}>");
        }
    }

    private async Task BotPinged(SocketCommandContext context)
    {
        await context.Channel.SendMessageAsync("no.");
    }

    private string GetPingReply(SocketCommandContext context)
    {
        var Rand = new Random();
        int Num = Rand.Next(GlobalIDs.pingReplies.Length);
            

        string unParsed = GlobalIDs.pingReplies[Num];

        string parsed = unParsed.Replace("<user>", context.User.Username)
            .Replace("<userid>", context.User.Id.ToString());

        return parsed;
    }
}
