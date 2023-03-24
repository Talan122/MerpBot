namespace MerpBot.Services;
public class Webhooks
{
    /// <summary>
    /// A dictionary of servers.
    /// </summary>
    public Dictionary<string, Server> Servers { get; set; }
}

public class Server
{
    public ulong ChannelID { get; set; }
    public ulong WebhookID { get; set; }
    public ulong ServerID { get; set; }
}