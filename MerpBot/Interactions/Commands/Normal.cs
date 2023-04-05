using Discord;
using Discord.WebSocket;
using Discord.Webhook;
using Discord.Interactions;
using MerpBot.Services;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MerpBot.Interactions.Autocomplete;
using MerpBot.Interactions.Enums;
using System.Security.Cryptography.X509Certificates;
using MerpBot.Interactions.Interfaces;

namespace MerpBot.Interactions.Commands;
public class Normal : InteractionModuleBase<SocketInteractionContext>
{
    public DiscordSocketClient Client { get; set; }
    public Download Downloader { get; set; }
    public Logger Logger { get; set; }
    public HttpClient HttpClient { get; set; }

    [SlashCommand("test", "Test Command")]
    public async Task Test()
    {
        await RespondAsync(Math.PI.ToString());
    }

    [SlashCommand("snowflake", "Convert a snowflake to a readable time.")]
    public async Task Snowflake(ISnowflake snowflake)
    {
        await RespondAsync($"<t:{snowflake.ToUnixTimestampSeconds()}>");
    }

    [SlashCommand("getplayers", "Gets players on a steam game.")]
    public async Task GetPlayers([Summary(description: "Steam app id.")] int appid)
    {
        await DeferAsync();
        HttpClient.DefaultRequestHeaders.Clear();
        string json = await HttpClient.GetStringAsync($"http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v0001/?appid={appid}");

        Players? players = JsonConvert.DeserializeObject<Players>(json);

        if(players is null)
        {
            await FollowupAsync("Not a valid game likely. Try again.");
            return;
        }

        await FollowupAsync($"Current online players: {players.response.player_count}");
    }

    [SlashCommand("httpcat", "Http cat.")]
    public async Task HttpCat([Autocomplete(typeof(HttpCodes))] int code)
    {
        string str = "";

        if (!Enum.IsDefined(typeof(HttpStatusCodeExtention), code)) 
        { 
            code = 404; 
            str = "This wasn't a valid http code."; 
        }

        await RespondAsync($"{str}\nhttps://http.cat/{code}.jpg");
    }

    [SlashCommand("download", "Downloads a video/image.")]
    public async Task Download([Summary(description: "The link to the video.")] string link)
    {
        try
        {
            if(link.Contains(' '))
            {
                await RespondAsync("A link cant contain a space.", ephemeral: true);
            }

            string[] root = link.Split('/');

            if (root[0] != "https:" || root[1] != "") await RespondAsync("This link is invalid.", ephemeral: true);

            await DeferAsync();

            string rootDL = root[2];

            FixRoot(ref rootDL);

            if(!Downloader.Downloaders.ContainsKey(rootDL))
            {

                await FollowupAsync($"You're likely using an unsuported link. Currently, it's only `{Helpers.CombineStringArray(Downloader.Downloaders.Keys.ToArray())}`");
                return;
            }

            var Data = await Downloader.Downloaders[rootDL](link);

            await FollowupWithFileAsync(Data.Stream ?? throw new FileNotFoundException(), $"{Data.Name ?? "dl"}.{Data.FileExtention}");
        }
        catch (Exception e) { await HandleDownloadErrors(e); }
    }

    private async Task HandleDownloadErrors(Exception error)
    {
        if (error.Message == "File is larger than 8mb") await FollowupAsync("The file was larger than 8mb and couldn't be uploaded.");
        else
        {
            await FollowupAsync("There was an error running this command. Check the error channel.");
            Logger.Error(error);
            await Helpers.ErrorChannel.SendMessageAsync(embed: Helpers.GenerateErrorEmbed(Context, error.Message));
        }
    }

    /// <summary>
    /// There are certain subdomains of some sites that the downloaders DO support but are not properly registered in Download.Downloaders.
    /// This is more or less a bandaid fix for bad code lol
    /// </summary>
    /// <param name="root"></param>
    private void FixRoot(ref string root)
    {
        root = Regex.Replace(root, @"www\.|\.com|\.net", "");

        if (root == "youtu.be") root = "youtube";
    }
}

public class Players
{
    public Response response { get; set; }

    public class Response
    {
        public int player_count { get; set; }
        public int result { get; set; }
    }
}
