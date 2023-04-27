using Discord;
using Discord.Interactions;
using System;
using System.Text;
using System.Threading.Tasks;
using Discord.Webhook;
using Microsoft.Extensions.Configuration;
using MerpBot.Services;

namespace MerpBot.Interactions.Message;
public class PinMessage : InteractionModuleBase<SocketInteractionContext>
{
    public IConfigurationRoot Configuration { get; set; }
    public Logger Logger { get; set; }

    [MessageCommand("Pin Message")]
    public async Task PinMsg(IMessage message) // name had to be different for the class lol (im stupid and didnt think ahead)
    {

        try
        {
            IGuildChannel guildChannel = (IGuildChannel)message.Channel;

            string content = "";

            if(message.Attachments.Any())
            {
                foreach(Attachment attachment in message.Attachments)
                    content += $"{attachment.Url}\n";
            }

            if (message.Embeds.Any())
            {
                foreach (var embed in message.Embeds)
                    content += $"{embed.Url}\n";
            }

            if (message.Content != string.Empty) content += $"\n\n{message.Content}";

            ComponentBuilder buttons = new ComponentBuilder()
                .WithButton(
                    label: "Jump", 
                    style: ButtonStyle.Link, 
                    url: $"https://discord.com/channels/{guildChannel.GuildId}/{guildChannel.Id}/{message.Id}"
                );

            // yeah this is a one-liner, deal with it.

            await StartupService.Webhooks[Context.Guild.Id.ToString()]
                .SendMessageAsync(
                text: content,
                username: message.Author.Username,
                avatarUrl: message.Author.GetAvatarUrl(),
                components: buttons.Build()
                );

            await RespondAsync("Pinned this message to the pin channel.", ephemeral: true);
        }
        catch(Exception e)
        {
            if (e is KeyNotFoundException) await NoWebhookFoundAsync();
            else Logger.Error(e);
        }
        
        await RespondAsync("Not implemented.", ephemeral: true);
    }

    /// <summary>
    /// Used to respond if a webhook is not found.
    /// </summary>
    /// <returns></returns>
    private async Task NoWebhookFoundAsync()
    {
        await RespondAsync("No webhook was found in your guild for this command. Run `/pin createwebhook` to make one.", ephemeral: true);
    }
}
