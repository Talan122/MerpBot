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

namespace MerpBot.Interactive.Commands;
public class Normal : InteractionModuleBase<SocketInteractionContext>
{
    public VolatileData VolatileData { get; set; }
    public DiscordSocketClient Client { get; set; }
    public HttpClient HttpClient { get; set; }

    [SlashCommand("test", "Test command")]
    public async Task Test(string? test = null)
    {
        await FollowupAsync(test ?? "naw fam");
    }

    [SlashCommand("ping", "Pong!")]
    public async Task Ping()
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff "));
        await FollowupAsync($"Pong! ({Client.Latency} ms)");
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
            await FollowupAsync("Ratio must be in format X:Y", ephemeral: true); 
            return; 
        }

        try
        {
            int.Parse(needs[0]);
            int.Parse(needs[1]);
        }
        catch(Exception e)
        {
            await FollowupAsync($"You made an error inputting the ratio. Did you input numbers or letters?", ephemeral: true);
            return;
        }

        if(options == RatioOptions.IsOutput)
        {
            double result = (number * int.Parse(needs[0])) / int.Parse(needs[1]);
            await FollowupAsync($"For an output of {number} with ratio {ratio}, you will need to input {Math.Ceiling(result)} (ceiling) ");
        }
        else
        {
            double result = (number / int.Parse(needs[0])) * int.Parse(needs[1]);
            await FollowupAsync($"From an input of {number} with ratio {ratio}, you will get {Math.Floor(result)} (floor)");
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

        await FollowupAsync($"{result} ° {to}");
    }

    /*[SlashCommand("cursetest", "Curse test")]
    public async Task CurseTest()
    {

        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add("x-api-key", "$2a$10$qsaMDW6UFMp.IGAk7rvlE.PydrFiCbDYI5GETMvY9nlFxNsJAcLzi");

        string Serialized = await HttpClient.GetStringAsync("https://api.curseforge.com/v1/mods/search?gameId=432");

        JsonObjects.Curseforge.Rootobject? Response = JsonConvert.DeserializeObject<JsonObjects.Curseforge.Rootobject>(Serialized);

        if (Response != null) await FollowupAsync(Response.data.First().authors.First().name);

    }*/


    [RequireOwner] 
    [SlashCommand("say", "Makes the bot say something.")]
    public async Task Say(string data, ISocketMessageChannel? channel = null, string? messageId = null, bool ping = true)
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " (In /say)");
        if (channel == null) channel = Context.Channel;

        if (messageId != null)
        {
            var message = (IUserMessage)await channel.GetMessageAsync(ulong.Parse(messageId));
            await message.ReplyAsync(data, allowedMentions: new AllowedMentions { MentionRepliedUser = ping });
            await FollowupAsync($"Said \"{data}\" in <#{channel.Id}> in a reply to someone.", ephemeral: true);
        }
        else
        {
            await channel.SendMessageAsync(data);
            await FollowupAsync($"Said \"{data}\" in <#{channel.Id}>", ephemeral: true);
        }

    }

    [SlashCommand("httppet", "http pets. Default is cat.")]
    public async Task HttpPet(int code, HttpPets pet = HttpPets.Cat)
    {
        HttpClient.DefaultRequestHeaders.Clear();

        string PetType = pet.ToString().ToLower();

        if(Enum.IsDefined((HttpStatusCode)code))
        {
            await FollowupAsync($"http://http.{PetType}/404.jpg\n(The int you gave 404'd)");
            return;
        }

        await FollowupAsync($"http://http.{PetType}/{code}.jpg");
    }

    /* Commented out because who the hell needs this?
    [SlashCommand("avatar", "Gets the avatar of whoever you put in.")]
    [RequireChannel]
    public async Task Avatar(SocketUser user, AvatarSizeOptions size = AvatarSizeOptions.S256)
    {
        var avatar = user.GetAvatarUrl(size: (ushort)size);
        await FollowupAsync(avatar);
    }*/
}
