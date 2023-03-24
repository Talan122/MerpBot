using MerpBot.Start;

namespace MerpBot;
public class Program
{
    public static string Version = "2.0.0";

    public static async Task Main(string[] args)
    {
        Console.WriteLine(Version);
        await StartUp.StartAsync(args);
    }
}