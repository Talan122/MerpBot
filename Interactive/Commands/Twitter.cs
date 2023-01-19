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
using MerpBot.Interactive.Options;

namespace MerpBot.Interactive.Commands;

[Group("twitter", "Twitter commands")]
public class Twitter : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("download", "Download a Twitter video/image.")]
    public async Task Download(string link)
    {

        if(!link.Contains("https://twitter.com/"))
        {
            await RespondAsync("Not a valid twitter link.", ephemeral: true);
            return;
        }

        await DeferAsync();

        string[] buffer = link.Split("/");
        string id = TwitterHelp.UntilChar(buffer.Last(), '?');

        if(!TwitterHelp.TryParseInt(id))
        {
            await FollowupAsync("Couldn't parse the twitter ID as an int. Make sure there's no spaces or extra characters.");
            return;
        }

        // copied and pasted from stack overflow lmao
        string? data = "";
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.RedirectStandardOutput = true;
        startInfo.FileName = "twitter-media-downloader.exe";
        startInfo.Arguments = $"-t {id} --url -B";
        process.StartInfo = startInfo;
        await Task.Run(() => process.Start());
        await process.WaitForExitAsync();
            
        while (!process.StandardOutput.EndOfStream)
        {
            data = process.StandardOutput.ReadLine();
        }

        process.Close();

        await FollowupAsync(data);
    }
}

public class TwitterHelp
{
    /// <summary>
    /// Takes a string and a character and returns everything up to a specified character.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string UntilChar(string s, char c)
    {
        string output = "";
        char[] chars = s.ToCharArray();
        foreach(char ch in chars)
        {
            if (ch == c) break;
            output += ch;
        }
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns>True if it is an int, false if not.</returns>
    public static bool TryParseInt(string i)
    {
        bool output = true;

        try
        {
            Int64 att = Int64.Parse(i);
        }
        catch(Exception e)
        {
            output = false;
            Console.WriteLine(e);
        }
        return output;
    }
}
