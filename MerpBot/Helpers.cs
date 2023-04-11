using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Bson;
using System.Text;

namespace MerpBot;
public static class Helpers
{
    public static string RootFolder = Path.GetFullPath(".");
    public static ITextChannel ErrorChannel { get; set; }
    public static Logger Logger { get; set; }

    public static readonly Dictionary<LogSeverity, string> SeverityFormat = new()
    {
        { LogSeverity.Info,     SetWidth("[Info]", 11) },
        { LogSeverity.Critical, SetWidth("[Critical]", 11) },
        { LogSeverity.Error,    SetWidth("[Error]", 11) },
        { LogSeverity.Warning,  SetWidth("[Warning]", 11) },
        { LogSeverity.Verbose,  SetWidth("[Verbose]", 11) },
        { LogSeverity.Debug,    SetWidth("[Debug]", 11) }
    };
    private static readonly Dictionary<LogSeverity, ConsoleColor> ColorSeverityFormat = new()
    {
        { LogSeverity.Info, ConsoleColor.White },
        { LogSeverity.Critical, ConsoleColor.DarkRed },
        { LogSeverity.Error, ConsoleColor.Red },
        { LogSeverity.Warning, ConsoleColor.Yellow},
        { LogSeverity.Verbose, ConsoleColor.Gray },
        { LogSeverity.Debug, ConsoleColor.DarkGray }
    };

    public static void ConsoleWithTimestamp(string message)
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + message);
    }
    ///<summary>
    /// Doesn't actually use any async code. It is purely for the Log event in Discord.NET.
    /// Use Helpers.Log() instead, as it is synchronous.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Task LogAsync(Discord.LogMessage message)
    {
        Logger.Log(message.Message, message.Severity, message.Source);

        return Task.CompletedTask;
    }

    public static string CombineStringArray(string[] array)
    {
        string result = "";

        foreach(var str in array) 
        {
            result += str;
        }

        return result;
    }

    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    /// <param name="message">A Discord Log message.</param>
    public static void Log(Discord.LogMessage message)
    {
        Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
        Console.ForegroundColor = ColorSeverityFormat[message.Severity];
        Console.Write(SeverityFormat[message.Severity]);
        Console.ResetColor();

        Console.Write($"{SetWidth(message.Source ?? " ", 9)} ");
        Console.WriteLine(message.Message);
    }
    
    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    /// <param name="message">A log message using the LogMessage class.</param>
    public static void Log(LogMessage message)
    {
        Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
        Console.ForegroundColor = ColorSeverityFormat[message.Severity];
        Console.Write(SeverityFormat[message.Severity]);
        Console.ResetColor();

        Console.Write($"{SetWidth(message.Source ?? " ", 9)} ");
        Console.WriteLine(message.Message);
    }
    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    /// <param name="message">The message in the log.</param>
    /// <param name="source">The source of the log. Defaults to null.</param>
    /// <param name="logSeverity">The severity of the log. Defaults to Info.</param>
    public static void Log(string message, LogSeverity logSeverity = LogSeverity.Info, string? source = null)
    {
        Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
        Console.ForegroundColor = ColorSeverityFormat[logSeverity];
        Console.Write(SeverityFormat[logSeverity]);
        Console.ResetColor();

        Console.Write($"{SetWidth(source ?? " ", 9)} ");
        Console.WriteLine(message);
    }

    public static string SetWidth(string input, int width)
    {
        if (input.Length > width) return input.Substring(0, width);
        return input.PadRight(width);
    }
    
    /// <summary>
    /// Combines a string array and returns one full string.
    /// </summary>
    /// <param name="strings">The array of strings.</param>
    /// <param name="combineWith">What it will combine the strings with.</param>
    /// <returns></returns>
    public static string CombineStringArray(string[] strings, string combineWith = "\n")
    {
        string result = "";

        foreach(var str in strings)
        {
            result += str + combineWith;
        }

        return result;
    }

    public static Embed GenerateErrorEmbed(SocketInteractionContext context, string message)
    {
        EmbedBuilder ErrorMessage = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithAuthor(context.User.Username)
                .WithThumbnailUrl(context.User.GetAvatarUrl())
                .AddField("Server", context.Guild.Name, true)
                .AddField("Channel", $"<#{context.Channel.Id}>", true)
                .AddField("Error", message, false);

        return ErrorMessage.Build();
    }
}

/// <summary>
/// Represents a log message.
/// </summary>
public class LogMessage 
{
    public string Message { get; set; }
    public string? Source { get; set; }
    public LogSeverity Severity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">The message associated with the log. Required.</param>
    /// <param name="source">Is not required, when passed into Log() it will just be blank in the console.</param>
    /// <param name="severity">The severity of the log. Defaults to "Info"</param>
    public LogMessage(string message, string? source = null, LogSeverity severity = LogSeverity.Info)
    {
        Message = message;
        Source = source;
        Severity = severity;

        
    }
}

/// <summary>
/// A class heplful for using Helpers.Log() 
/// 
/// </summary>
// Probably should put it in a different file...
public class Logger
{
    public IConfigurationRoot Config { get; set; }
    private LogSeverity GlobalSeverity { get; set; }
    private StreamWriter StreamWriter { get; set; }  

    public Logger(IConfigurationRoot config)
    {
        Config = config;

        if (Enum.TryParse(Config["LogLevel"], out LogSeverity globalSeverity))
            GlobalSeverity = globalSeverity; 
        else CrashWhenNoSeverityIsFound();

        //StreamWriter = new StreamWriter($"{Helpers.RootFolder}\\logs\\{DateTime.Now.ToString("MM-dd-yyyy HH_mm_ss")}.txt");
        StreamWriter = new StreamWriter(Path.Combine(Helpers.RootFolder, "logs", $"{DateTime.Now.ToString("MM-dd-yyyy HH_mm_ss")}.txt"));

        PostSeverity();
    }

    public void PostSeverity()
    {
        Info($"Starting with LogLevel set to {GlobalSeverity}", "Logger");
    }

    private void WriteToFile(string message, LogSeverity severity, string? source = null)
    {
        string builder = new StringBuilder()
            .Append($"[{DateTime.Now.ToString("HH:mm:ss")}] ")
            .Append($"{Helpers.SeverityFormat[severity]}")
            .Append($"{Helpers.SetWidth(source ?? " ", 9)} ")
            .Append(message).ToString();

        Task.Run(async () =>
        {
            await StreamWriter.WriteLineAsync(builder.ToString());
            await StreamWriter.FlushAsync();
        });
    }

    /// <summary>
    /// Log something using a specified severity.
    /// Suggest not to use this unless necessary.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="severity"></param>
    /// <param name="source"></param>
    public void Log(string message, LogSeverity severity, string? source = null)
    {
        Helpers.Log(new LogMessage(message, source, severity));

        WriteToFile(message, severity, source);
    }

    /// <summary>
    /// Log something as debug.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Debug(string message, string? source = null, bool skipCheck = false)
    {
        if(skipCheck)
        {
            DebugSkip(message, source);
            return;
        }

        if (GlobalSeverity < LogSeverity.Debug) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Debug));

        WriteToFile(message, LogSeverity.Debug, source);
    }

    /// <summary>
    /// Log something as debug. Skips the check for severity.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    private void DebugSkip(string message, string? source = null)
    {
        Helpers.Log(new LogMessage(message, source, LogSeverity.Debug));

        WriteToFile(message, LogSeverity.Debug, source);
    }

    /// <summary>
    /// Log something as info.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Info(string message, string? source = null)
    {
        if (GlobalSeverity < LogSeverity.Info) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Info));

        WriteToFile(message, LogSeverity.Info, source);
    }

    /// <summary>
    /// Log something as critical.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Critical(string message, string? source = null)
    {
        if(GlobalSeverity < LogSeverity.Critical) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Critical));

        WriteToFile(message, LogSeverity.Critical, source);
    }

    public void Critical(Exception exception)
    {
        if (GlobalSeverity < LogSeverity.Critical) return;

        Helpers.Log(new LogMessage(exception.Message + exception.StackTrace, exception.Source, LogSeverity.Critical));

        WriteToFile(exception.Message + exception.StackTrace, LogSeverity.Critical, exception.Source);
    }

    /// <summary>
    /// Log something as warning.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Warning(string message, string? source = null)
    {
        if (GlobalSeverity < LogSeverity.Warning) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Warning));

        WriteToFile(message, LogSeverity.Warning, source);
    }

    public void Warning(Exception exception)
    {
        if (GlobalSeverity < LogSeverity.Warning) return;

        Helpers.Log(new LogMessage(exception.Message + exception.StackTrace, exception.Source, LogSeverity.Critical));

        WriteToFile(exception.Message + exception.StackTrace, LogSeverity.Warning, exception.Source);
    }

    /// <summary>
    /// Log something as verbose.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Verbose(string message, string? source = null)
    {
        if (GlobalSeverity < LogSeverity.Verbose) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Verbose));

        WriteToFile(message, LogSeverity.Verbose, source);
    }

    /// <summary>
    /// Log something as error.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    public void Error(string message, string? source = null)
    {
        if (GlobalSeverity < LogSeverity.Error) return;

        Helpers.Log(new LogMessage(message, source, LogSeverity.Error));

        WriteToFile(message, LogSeverity.Error, source);
    }

    public void Error(Exception exception)
    {
        if (GlobalSeverity < LogSeverity.Error) return;

        Helpers.Log(new LogMessage(exception.Message + exception.StackTrace, exception.Source, LogSeverity.Error));

        WriteToFile(exception.Message + exception.StackTrace, LogSeverity.Error, exception.Source);
    }

    /// <summary>
    /// Log something as critical and crashes/closes the program.
    /// </summary>
    /// <param name="message">Message to log.</param>
    /// <param name="source">Source of message. Defaults to null.</param>
    /// <param name="exitCode">Exit code that it will use.</param>
    public void CriticalWithCrash(string message, string? source = null, int exitCode = 1)
    {
        Helpers.Log(new LogMessage(message, source, LogSeverity.Critical));

        Environment.Exit(exitCode);
    }

    public void CriticalWithCrash(Exception exception, int exitCode = 1)
    {
        Helpers.Log(new LogMessage(exception.Message + exception.StackTrace, exception.Source, LogSeverity.Critical));

        WriteToFile(exception.Message + exception.StackTrace, LogSeverity.Critical, exception.Source);

        Environment.Exit(exitCode);
    }

    public static void CrashWhenNoSeverityIsFound()
    {
        Helpers.Log(new LogMessage("LogLevel in Globals.yml is not set correctly. Correct values are (in ascending order of verbosity) 'Critical', 'Error', 'Info', 'Verbose', or 'Debug'", source: "Logger", severity: LogSeverity.Critical));
        Environment.Exit(1);
    }
}