using Discord;
using Discord.WebSocket;
using Discord.Webhook;
using Discord.Interactions;
using MerpBot.Services;
using System.Text;
using Newtonsoft.Json;

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

    [SlashCommand("download", "Downloads a video/image.")]
    public async Task Download([Summary(description: "The link to the video.")] string link)
    {
        try
        {
            await DeferAsync();

            var Data = await Downloader.DownloadContent(link);

            if (Data == null)
            {
                await FollowupAsync("You're likely using an unsuported link. Currently, it's only `youtube`");
                return;
            }

            await FollowupWithFileAsync(Data.Stream ?? throw new FileNotFoundException(), $"{Data.Name ?? "dl"}.mp4");
        }
        catch (Exception e) { await HandleDownloadErrors(e); }
    }

    private async Task HandleDownloadErrors(Exception error)
    {
        if (error.Message == "File is larger than 8gb") await FollowupAsync("The file was larger than 8mb and couldn't be uploaded.");
        else
        {
            await FollowupAsync("There was an error running this command. Check the error channel.");
            Logger.Error(error);
            await Helpers.ErrorChannel.SendMessageAsync(embed: Helpers.GenerateErrorEmbed(Context, error.Message));
        }
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
