
using Discord;
using Discord.Interactions;
using Discord.Webhook;
using Discord.WebSocket;
using MerpBot.Services;

namespace MerpBot.Interactions.Commands;

[Group("pin", "Pin message commands.")]
public class PinMessage : InteractionModuleBase<SocketInteractionContext>
{
    public DiscordSocketClient Client { get; set; }

    [SlashCommand("createwebhook", "Creates a webhook.")]
    [RequireUserPermission(ChannelPermission.ManageWebhooks)]
    public async Task CreateWebhook([ChannelTypes(ChannelType.Text)][Summary(description:"Channel to put webhook in.")] ITextChannel channel, string name = "Captain Hook")
    {
        string GuildID = Context.Guild.Id.ToString();

        if (StartupService.Webhooks.ContainsKey(GuildID))
        {
            await RespondAsync("You already have a pin channel. You can delete it if you like with `/pin delete`", ephemeral: true);
            return;
        }

        await DeferAsync(true);

        DiscordWebhookClient newClient = new DiscordWebhookClient(await channel.CreateWebhookAsync(name));
        StartupService.Webhooks[GuildID] = newClient;
        await WebhookService.WriteWebhooksToFileAsync(StartupService.Webhooks);

        await FollowupAsync($"Successfully made a pin channel in <#{newClient.Webhook.ChannelId}>. You can use it by right clicking on a message and hitting \"PinMessage\" under apps.");
    }

    [SlashCommand("delete", "Deletes the previous webhook.")]
    [RequireUserPermission(ChannelPermission.ManageWebhooks)]
    public async Task Delete()
    {
        string GuildID = Context.Guild.Id.ToString();

        if (!StartupService.Webhooks.ContainsKey(GuildID))
        {
            await RespondAsync($"You don't even have a pin channel, and you can't delete something that doesn't exist.\n*If you're receiving this in spite of having one, message <@{(await Client.GetOwnerAsync()).Id}>*", ephemeral: true);
            return;
        }

        await DeferAsync(ephemeral: true);

        DiscordWebhookClient client = StartupService.Webhooks[GuildID];
        var clientDetails = client.Webhook;

        await client.DeleteWebhookAsync();
        StartupService.Webhooks.Remove(GuildID);
        await WebhookService.WriteWebhooksToFileAsync(StartupService.Webhooks);

        await FollowupAsync($"Successfully deleted webhook from <#{clientDetails.ChannelId}>, as well as any data pertaining to it internally. You can make a new one if you'd like.", ephemeral: true);
    }
}
