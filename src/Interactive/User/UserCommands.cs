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
using Discord.Webhook;
using System.Security.Cryptography.X509Certificates;

namespace MerpBot.Interactive.User;
public class UserCommands : InteractionModuleBase<SocketInteractionContext>
{
    public DiscordWebhookClient WebhookClient { get; set; }

    [MessageCommand("PinMessage")]
    [RequireGuild(851204839605927946)]
    public async Task PinMessage(IMessage message)
    {
        await RespondAsync("test", ephemeral: true);

        StringBuilder toSend = new StringBuilder()
            .AppendLine(message.ToString())
            .AppendLine()
            .AppendLine(GetEmbedLinks((IUserMessage)message));

        await WebhookClient.SendMessageAsync(toSend.ToString(), username: message.Author.Username, avatarUrl: GetDefaultAvatar(message.Author));
    }

    private string GetDefaultAvatar(IUser user) 
    {
        string Result = "";

        if (user.AvatarId.StartsWith("a_")) Result = user.GetAvatarUrl(format: ImageFormat.Gif);
        else Result = user.GetAvatarUrl(format: ImageFormat.Png);

        return Result;
    } 

    private string GetEmbedLinks(IUserMessage message)
    {
        Console.WriteLine(message.Attachments);

        string EmbedLinks = "";

        foreach(var embed in message.Attachments)
        {
            Console.WriteLine(embed.Url);

            EmbedLinks += " " + embed.Url;
        }

        return EmbedLinks;
    }
}