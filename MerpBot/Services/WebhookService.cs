using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MerpBot.Services;

public static class WebhookService
{
    public static DiscordSocketClient Client { get; set; }
    public static Logger Logger { get; set; }

    /// <summary>
    /// Gets all the clients from webhooks.json
    /// </summary>
    /// <param name="clients">Ref variable. Suggest using the already-existing dictionary in singletons.</param>
    public static void GetDiscordWebhookClients(ref Dictionary<string, DiscordWebhookClient> clients)
    {


        Dictionary<string, DiscordWebhookClient> _clients = new Dictionary<string, DiscordWebhookClient>();

        Task.Run(async () =>
        {
            Webhooks hooks = await GetWebhooksFromFileAsync();

            foreach (var serverKey in hooks.Servers.Keys)
            {
                Server server = hooks.Servers[serverKey];

                _clients[server.ServerID.ToString()] = await GetWebhookClientAsync(server);
            }
        }).Wait();

        

        clients = _clients;
    }

    private static async Task<Webhooks> GetWebhooksFromFileAsync()
    {
        string jsonFile = Helpers.CombineStringArray(await File.ReadAllLinesAsync(Path.Combine(Helpers.RootFolder, "webhooks.json")), " ");

        Webhooks? result = JsonConvert.DeserializeObject<Webhooks>(jsonFile);

        if (result is null) throw new NullReferenceException();

        return result;
    }

    public static async Task<DiscordWebhookClient> GetWebhookClientAsync(Server server)
    {
        ITextChannel channel = (ITextChannel)await Client.GetChannelAsync(server.ChannelID);
        IWebhook webhook = await channel.GetWebhookAsync(server.WebhookID);
        DiscordWebhookClient client = new DiscordWebhookClient(webhook);

        return client;
    }

    public static async Task WriteWebhooksToFileAsync(Dictionary<string, DiscordWebhookClient> clients)
    {
        Webhooks toWrite = new Webhooks()
        {
            Servers = new()
        };

        foreach(var clientKey in clients.Keys)
        {
            var client = clients[clientKey].Webhook;
            Server server = new Server()
            {
                ChannelID = client.ChannelId,
                ServerID = (ulong)client.GuildId,
                WebhookID = client.Id
            };

            toWrite.Servers[clientKey] = server;
        }

        var json = JsonConvert.SerializeObject(toWrite);
        await File.WriteAllTextAsync(Path.Combine(Helpers.RootFolder, "webhooks.json"), json);
    }
}