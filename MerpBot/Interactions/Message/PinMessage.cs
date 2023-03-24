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

    [MessageCommand("PinMessage")]
    public async Task PinMsg(IMessage message) // name had to be different for the class lol (im stupid and didnt think ahead)
    {
        try
        {
            // yeah this is a one-liner, deal with it.

            await StartupService.Webhooks[Context.Guild.Id.ToString()]
                .SendMessageAsync(
                text: message.Content,
                username: message.Author.Username,
                avatarUrl: message.Author.GetAvatarUrl()
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
