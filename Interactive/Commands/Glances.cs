using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using System;
using System.Linq;
using System.Threading.Tasks;
using MerpBot.Services;
using MerpBot.Interactive.Preconditions;
using System.IO;
using System.Diagnostics;
using MerpBot.Interactive.Modals;
using MerpBot.Interactive.Options;
using Newtonsoft.Json;
using MerpBot.Destiny;
using MerpBot.Destiny.ResponseTypes.User;
using System.Net.Http;

namespace MerpBot.Interactive.Commands;

[Group("glances", "Glances commands.")]
public class Glances : InteractionModuleBase<SocketInteractionContext>
{
    private readonly string BaseLink = "http://localhost:61208/api/3/";
    public HttpClient HttpClient { get; set; }

    [NotYetImplemented]
    [SlashCommand("test", "Test command for glances")]
    public async Task Test()
    {

        HttpClient.DefaultRequestHeaders.Clear();

        HttpResponseMessage report = await HttpClient.GetAsync(BaseLink + "status");

        //Console.WriteLine(report);

        await FollowupAsync($"`http://localhost:61208/api/3/status` responded with StatusCode: {((int)report.StatusCode)}, ReasonPhrase: {report.ReasonPhrase}");
    }
    [SlashCommand("uptime", "Uptime")]
    [NotYetImplemented]
    public async Task Uptime()
    {

        HttpClient.DefaultRequestHeaders.Clear();

        string report = await HttpClient.GetStringAsync(BaseLink + "uptime");

        //Console.WriteLine(report);

        await FollowupAsync($"Uptime: {report.Replace("\"", "")}");
    }
}
