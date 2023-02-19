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
using MerpBot.Interactive.JsonObjects;
using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using HttpStatusCode = MerpBot.Interactive.Options.HttpStatusCode;
using System.CodeDom;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.Steam;
using System.Runtime.InteropServices;

namespace MerpBot.Interactive.Commands;
[Group("normal", "'Normal' commands.")]
public class Normal : InteractionModuleBase<SocketInteractionContext>
{
    public VolatileData VolatileData { get; set; }
    public DiscordSocketClient Client { get; set; }
    public HttpClient HttpClient { get; set; }

    [SlashCommand("test", "Test command")]
    public async Task Test()
    {
        var handler = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new SteamHandler(new WindowsRegistry())
            : new SteamHandler(registry: null);

        // method 1: iterate over the game-error result
        foreach (var (game, error) in handler.FindAllGames())
        {
            if (game is not null)
            {
                Console.WriteLine($"Found {game}");
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }

        await RespondAsync("Check your console.");
    }

    [SlashCommand("ping", "Pong!")]
    public async Task Ping()
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff "));
        await RespondAsync($"Pong! ({Client.Latency} ms)");
    }

    [SlashCommand("ratiohelp", "Get the exact needed depending on the given ratios.")]
    public async Task RatioHelp(
        string ratio,
        int number,
        RatioOptions options = RatioOptions.IsOutput
        )
    {
        string[] needs = ratio.Split(':');
        int[] numbers = new int[2];

        if (needs[0] == ratio) 
        { 
            await RespondAsync("Ratio must be in format X:Y", ephemeral: true); 
            return; 
        }

        try
        {
            int.Parse(needs[0]);
            int.Parse(needs[1]);
        }
        catch(Exception e)
        {
            await RespondAsync($"You made an error inputting the ratio. Did you input numbers or letters?", ephemeral: true);
            return;
        }

        if(options == RatioOptions.IsOutput)
        {
            double result = (number * int.Parse(needs[0])) / int.Parse(needs[1]);
            await RespondAsync($"For an output of {number} with ratio {ratio}, you will need to input {Math.Ceiling(result)} (ceiling) ");
        }
        else
        {
            double result = (number / int.Parse(needs[0])) * int.Parse(needs[1]);
            await RespondAsync($"From an input of {number} with ratio {ratio}, you will get {Math.Floor(result)} (floor)");
        }
    }


    [SlashCommand("tempconvert", "Converts temperatures.")]
    [NotYetImplemented]
    public async Task TempConvert(TempOptions from, int temperature, TempOptions to)
    {
        double result = 0;

        if (from == to)
            result = temperature;

        if (from == TempOptions.Fahrenheit && to == TempOptions.Celcius) 
            result = TemperatureConverter.FahrenheitToCelcius(temperature);

        if (from == TempOptions.Celcius && to == TempOptions.Fahrenheit)
            result = TemperatureConverter.CelciusToFahrenheit(temperature);

        if (from == TempOptions.Celcius && to == TempOptions.Kelvin)
            result = TemperatureConverter.CelciusToKelvin(temperature);

        if (from ==TempOptions.Kelvin  && to == TempOptions.Celcius)
            result = TemperatureConverter.KelvinToCelcius(temperature);

        if (from == TempOptions.Fahrenheit && to == TempOptions.Kelvin)
            result = TemperatureConverter.FahrenheitToKelvin(temperature);

        if (from == TempOptions.Kelvin && to == TempOptions.Fahrenheit)
            result = TemperatureConverter.KelvinToFahrenheit(temperature);

        await RespondAsync($"{result} ° {to}");
    }

    [SlashCommand("httppet", "http pets. Default is cat.")]
    public async Task HttpPet(int code, HttpPets pet = HttpPets.Cat)
    {
        await DeferAsync();

        HttpClient.DefaultRequestHeaders.Clear();

        string PetType = pet.ToString().ToLower();

        if(Enum.IsDefined((HttpStatusCode)code))
        {
            await FollowupAsync($"http://http.{PetType}/404.jpg\n(The int you gave 404'd)");
            return;
        }

        await FollowupAsync($"http://http.{PetType}/{code}.jpg");
    }
    
    [SlashCommand("avatar", "Gets the avatar of whoever you put in.")]
    public async Task Avatar(SocketUser user, AvatarSizeOptions size = AvatarSizeOptions.S256)
    {
        var avatar = user.GetAvatarUrl(size: (ushort)size);
        await RespondAsync(avatar);
    }

    [SlashCommand("owofy", "I wegwet nyothing")]
    public async Task OwOfy(string input)
    {
        // I had to use this method eventually lol
        await RespondAsync(OtherStuff.OwOfy(input));
    }
}
