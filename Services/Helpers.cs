using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using System.IO;
using System.Collections.ObjectModel;

namespace MerpBot.Services;
public static class Helpers //this entire thing should be static probably
{
    private static DiscordSocketClient discord = new DiscordSocketClient();

    private static Random random = new Random();

    public static void SetClient(DiscordSocketClient client) => discord = client;

    public static void ConsoleWithTimeStamp(string message)
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + message);
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string MakeDirectoryFromWorking(string fileLocation) => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!, fileLocation);

    public static async Task<SocketMessageComponent?> AwaitButtonAsync(ulong messageID, ulong? userId, int delayInSeconds)
    {
        SocketMessageComponent? response = null;

        CancellationTokenSource cancler = new CancellationTokenSource();
        Task waiter = Task.Delay(delayInSeconds * 1000, cancler.Token);

        discord.ButtonExecuted += OnButtonPressed;
        try
        { await waiter; }
        catch (TaskCanceledException)
        { }
        discord.ButtonExecuted -= OnButtonPressed;

        return response;

        async Task OnButtonPressed(SocketMessageComponent component)
        {
            if (userId != null)
            {
                if (component.Message.Id != messageID || component.User.Id != userId) return;
            }
            else
            {
                if (component.Message.Id != messageID) return;
            }
            response = component;
            await component.DeferAsync();
            cancler.Cancel();
            await Task.CompletedTask;
                
        }
    }
    public static async Task<List<SocketMessageComponent>> AwaitButtonMultipleAsync(ulong messageID, List<ulong> userId, int delayInSeconds)
    {
        List<SocketMessageComponent> response = new List<SocketMessageComponent>(userId.Count);

        CancellationTokenSource cancler = new CancellationTokenSource();
        Task waiter = Task.Delay(delayInSeconds * 1000, cancler.Token);

        discord.ButtonExecuted += OnButtonPressed;
        try
        { await waiter; }
        catch (TaskCanceledException)
        { }
        discord.ButtonExecuted -= OnButtonPressed;

        return response;

        async Task OnButtonPressed(SocketMessageComponent component)
        {
            if (component.Message.Id != messageID || !userId.Contains(component.User.Id)) return;

            response.Add(component);
            userId.Remove(component.User.Id);
            await component.DeferAsync();

            if (userId.Count == 0)
            {
                cancler.Cancel();
                await Task.CompletedTask;
            }
        }
    }
    public static async Task<SocketMessage?> AwaitMessageAsync(ulong channelID, ulong? userId, int delayInSeconds)
    {
        SocketMessage? response = null;

        CancellationTokenSource cancler = new CancellationTokenSource();
        Task waiter = Task.Delay(delayInSeconds * 1000, cancler.Token);

        discord.MessageReceived += OnMessageReceived;
        try
        { await waiter; }
        catch (TaskCanceledException)
        { }
        discord.MessageReceived -= OnMessageReceived;

        return response;

        async Task OnMessageReceived(SocketMessage message)
        {
            if (userId != null)
            {
                if (message.Channel.Id != channelID || message.Author.Id != userId) return;
            }
            else
            {
                if (message.Channel.Id != channelID) return;
            }
            response = message;
            cancler.Cancel();
            await Task.CompletedTask;
        }
    }
    public static async Task<List<SocketMessage>> AwaitMessageMultipleAsync(ulong channelID, List<ulong> userId, int delayInSeconds)
    {
        List<SocketMessage> response = new List<SocketMessage>(userId.Count);

        CancellationTokenSource cancler = new CancellationTokenSource();
        Task waiter = Task.Delay(delayInSeconds * 1000, cancler.Token);

        discord.MessageReceived += OnMessageReceived;
        try
        { await waiter; }
        catch (TaskCanceledException)
        { }
        discord.MessageReceived -= OnMessageReceived;

        return response;

        async Task OnMessageReceived(SocketMessage message)
        {
            if (message.Channel.Id != channelID || !userId.Contains(message.Author.Id)) return;

            response.Add(message);
            userId.Remove(message.Author.Id);

            if (userId.Count == 0)
            {
                cancler.Cancel();
                await Task.CompletedTask;
            }
        }
    }
    public static ComponentBuilder DisableAllButtons(ComponentBuilder oldBuilder)
    {
        var builder = new ComponentBuilder();

        var rows = oldBuilder.ActionRows;

        for (int i = 0; i < rows.Count; i++)
        {
            foreach (var component in rows[i].Components)
            {
                switch (component)
                {
                    case ButtonComponent button:
                        builder.WithButton(button.ToBuilder()
                            .WithDisabled(true), i);
                        break;
                    case SelectMenuComponent menu:
                        builder.WithSelectMenu(menu.ToBuilder()
                            .WithDisabled(true), i);
                        break;
                }
            }
        }
        return builder;
    }

    /// <summary>
    /// Gets the Spotify status of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static SpotifyGame? GetSpotifyStatus(SocketGuildUser user)
    {

        foreach (var Activity in user.Activities)
        {

            if (Activity is SpotifyGame spotify)
            {
                return spotify;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the first found rich game status of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static RichGame? GetRichGameStatus(SocketGuildUser user)
    {

        foreach (var Activity in user.Activities)
        {

            if (Activity is RichGame game)
            {
                return game;
            }
        }
        return null;
    }

    public static StreamingGame? GetStreamingStatus(SocketGuildUser user)
    {

        foreach (var Activity in user.Activities)
        {
            if (Activity is StreamingGame game)
            {
                return game;
            }
        }
        return null;
    }
    /// <summary>
    /// Gets a game without a rich presence.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static Game? GetNonRichGame(SocketGuildUser user)
    {
        foreach (var Activity in user.Activities)
        {
                
            // Yes I know it's ugly. You don't have to yell at me Traso.
            if (Activity is Game game)
            {
                if(game.GetType() != typeof(SpotifyGame))
                {
                    if(game.GetType() != typeof(StreamingGame))
                    {
                        if(game.GetType() != typeof(RichGame))
                        { 
                            if(game.GetType() != typeof(CustomStatusGame))
                            {
                                return game;
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    public static CustomStatusGame? GetCustomStatus(SocketGuildUser user)
    {

        foreach (var Activity in user.Activities)
        {
            if (Activity is CustomStatusGame game)
            {
                return game;
            }
        }
        return null;
    }
    /// <summary>
    /// For use in debug purposes only.
    /// </summary>
    /// <param name="user"></param>
    public static void DebugStatus(SocketGuildUser user)
    {
        foreach (var Activity in user.Activities)
        {   
            Console.WriteLine($"{Activity.GetType()}: {Activity.Name}");
        }
    }

    /// <summary>
    /// Combines a collection of strings.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string StringCollectionJoin(IReadOnlyCollection<string> input, string joinWith = " ")
    {
        string result = "";

        foreach(var str in input)
        {
            result += str + joinWith;
        }
        return result.Substring(0, result.Length - joinWith.Length);
    }

    public static async Task LogCommand(string title, IInteractionContext Context, ulong channelId = 968611003820548236, ExtraEmbedField[]? extraFields = null, bool noResponse = false)
    {
        if (!Context.Interaction.HasResponded)
        {
            Console.WriteLine("Interaction hasn't been responded to yet.");
            return;
        }

        SocketSlashCommandData data = (SocketSlashCommandData)Context.Interaction.Data;
        string Options = "`";
        foreach(var option in data.Options)
        {
            Options += $"{option.Name}: {option.Value} ";
        }
        Options += "`";

        IUserMessage message = await Context.Interaction.GetOriginalResponseAsync();

        string response = message.Content;

        EmbedBuilder builder = new EmbedBuilder().WithTitle(title)
                .WithThumbnailUrl(Context.User.GetAvatarUrl(size: 256))
                .WithColor(new Color(255, 255, 10))
                .AddField(Context.User.Username, $"Name: {data.Name}\nId: {data.Id}\nOptions: {Options}")
                .AddField("In channel", $"<#{Context.Channel.Id}> / {Context.Channel.Name}");

        if (noResponse == false)
        {
            builder.AddField("Responded with", $"{response}")
                .WithCurrentTimestamp();
        }
        else
        {
            builder.AddField("Responded with", $"*The response was either null or was an embed.")
                .WithCurrentTimestamp();
        }
            

        if(extraFields != null)
        {
            foreach(var field in extraFields)
            {
                builder.AddField(field.Name, field.Value);
            }
        }

        IMessageChannel logChannel = (IMessageChannel)discord.GetChannel(channelId);
        await logChannel.SendMessageAsync(embed: builder.Build());
    }
}

public static class ExtensionMethods
{
    public static string ToReadableString(this TimeSpan span)
    {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? "" : "s") : "",
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? "" : "s") : "",
            span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? "" : "s") : "",
            span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? "" : "s") : "");

        if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

        return formatted;
    }
}

public class ExtraEmbedField
{
    public string Name { get; set; }
    public string Value { get; set; }

    public ExtraEmbedField(string name, string value)
    {
        Name = name; Value = value; 
    }
}

public class Kyvera
{
    public string[] Links { get; set; }
    private string MakeDirectoryFromWorking(string fileLocation) => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!, fileLocation);

    public Kyvera()
    {
        string[] _links = File.ReadAllLines(MakeDirectoryFromWorking("KyveraLinks.txt"));

        Links = _links;
    }

    private void Refresh()
    {
        Links = File.ReadAllLines(MakeDirectoryFromWorking("KyveraLinks.txt"));
    }

    public string GetRand()
    {
        Random rand = new Random();

        int randNum = rand.Next(Links.Length);

        return Links[randNum];
    }

    public void AddString(string input)
    {
        StreamWriter sw = new StreamWriter(MakeDirectoryFromWorking("KyveraLinks.txt"), append: true);

        sw.Write(input + "\n");
        sw.Close();

        Refresh();
    }

    public string GetSpecific(int input)
    {
        return Links[input];
    }

    public void AddMultiple(string[] input)
    {
        StreamWriter sw = new StreamWriter(MakeDirectoryFromWorking("KyveraLinks.txt"), append: true);
        foreach (string inputItem in input)
        {
            sw.Write(inputItem + "\n");
        }

        sw.Close();

        Refresh();
    }
}
