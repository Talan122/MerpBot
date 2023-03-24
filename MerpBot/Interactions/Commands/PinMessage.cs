using Discord;
using Discord.Interactions;
using Discord.Webhook;
using MerpBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactions.Commands;

[Group("pin", "Pin message commands.")]
public class PinMessage : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("createwebhook", "Creates a webhook.")]
    [RequireUserPermission(ChannelPermission.ManageWebhooks)]
    public async Task CreateWebhook([ChannelTypes(ChannelType.Text)][Summary(description:"Channel to put webhook in.")] IChannel channel, string name = "Captain Hook")
    {
        if (StartupService.Webhooks.ContainsKey(Context.Guild.Id.ToString()))
        {
            await RespondAsync("You already have a pin channel. You can delete it if you like with `/pin delete`", ephemeral: true);
            return;
        }

        await RespondAsync("Not yet implemented.", ephemeral: true);
    }

    [SlashCommand("delete", "Deletes the previous webhook.")]
    [RequireUserPermission(ChannelPermission.ManageWebhooks)]
    public async Task Delete()
    {
        string GuildID = Context.Guild.Id.ToString();

        if (!StartupService.Webhooks.ContainsKey(GuildID))
        {
            await RespondAsync("You don't even have a pin channel. What?", ephemeral: true);
            return;
        }

        await DeferAsync(ephemeral: true);

        DiscordWebhookClient client = StartupService.Webhooks[GuildID];
        var clientDetails = client;

        await client.DeleteWebhookAsync();
        StartupService.Webhooks.Remove(GuildID);

        await FollowupAsync($"Successfully deleted webhook from <#{clientDetails.ChannelId}>, as well as any data pertaining to it internally. You can make a new one if you'd like.", ephemeral: true);
    }
}
