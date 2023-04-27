using AngleSharp;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace MerpBot.Interactions.Commands;
public class Download
{
    public Dictionary<string, Func<string, Task<Data>>> Downloaders = new Dictionary<string, Func<string, Task<Data>>>();

    public YoutubeClient YoutubeClient { get; set; }
    public HttpClient RedditHttpClient { get; set; }
    public Logger Logger { get; set; }
    //public IConfigurationRoot Config { get; set; }
    public FileSize MaxFileSize { get; set; }

    public Download(Logger logger, IConfigurationRoot config)
    {
        Logger = logger;
        YoutubeClient = new YoutubeClient();
        RedditHttpClient = new HttpClient();
        //Config = config;

        MaxFileSize = new FileSize(int.Parse(config["MaxFileSize"]));

        Downloaders["youtube"] = YoutubeDownload;
        Downloaders["reddit"] = RedditDownload;
    }

    private async Task<Data> YoutubeDownload(string url)
    {
        var About = await YoutubeClient.Videos.GetAsync(url);

        Data videoData = new Data()
            .WithName(About.Title)
            .WithDescription(About.Description)
            .WithTime(About.Duration ?? new TimeSpan())
            .WithFileExtension("mp4");

        var Manifest = await YoutubeClient.Videos.Streams.GetManifestAsync(url);

        var StreamInfo = Manifest.GetMuxedStreams()
            .Where(x => x.Size < MaxFileSize)
            .Where(x => x.Container != new Container("3gpp"));

        if (!StreamInfo.Any()) throw new Exception($"File is larger than {MaxFileSize}");

        Logger.Debug(StreamInfo.GetWithHighestBitrate().Size.ToString(), skipCheck: true);

        var highest = StreamInfo.GetWithHighestBitrate();

        videoData.WithStream(await YoutubeClient.Videos.Streams.GetAsync(highest));
        videoData.WithSize(highest.Size);

        return videoData; 
    }

    public async Task<Data> RedditDownload(string url)
    {
        RedditHttpClient.DefaultRequestHeaders.Clear();

        string download = await RedditHttpClient.GetStringAsync($"https://rapidsave.com/info?url={url}");
        var doc = new HtmlDocument();

        doc.LoadHtml(download);

        var dataValue = doc.DocumentNode.SelectNodes("//a[@class='downloadbutton']")["a"];

        string cdnLink = dataValue.Attributes["href"].Value;

        MemoryStream outStream = new MemoryStream();

        await (await RedditHttpClient.GetStreamAsync(cdnLink)).CopyToAsync(outStream);

        if (outStream.Length > MaxFileSize.Bytes) throw new Exception($"File is larger than {MaxFileSize}");

        if (cdnLink.StartsWith("https://sd.rapidsave.com")) return new()
        {
            FileExtention = ".mp4",
            Stream = outStream,
            Name = url.Split("/").Last(),
            Size = new(outStream.Length),
        };
        else return new()
        {
            FileExtention = cdnLink.Remove(0, cdnLink.Length - 3),
            Name = url.Split("/").Last(),
            Stream = outStream,
            Size = new(outStream.Length)
        };
    }
}


public struct Data
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan? Time { get; set; }
    public Stream? Stream { get; set; }
    public string? FileExtention { get; set; }
    public FileSize? Size { get; set; }

    public Data(Stream? stream = null, string? name = null, string? description = null, TimeSpan? time = null, FileSize? size = null)
    {
        Stream = stream;
        Name = name;
        Description = description;
        Time = time;
        Size = size;
    }
    public Data() { }

    public Data WithStream(Stream stream)
    {
        Stream = stream;

        return this;
    }

    public Data WithTime(TimeSpan time)
    {
        Time = time;
        return this;
    }

    public Data WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public Data WithName(string name)
    {
        Name = name;
        return this;
    }

    public Data WithFileExtension(string extension)
    {
        FileExtention = extension;
        return this;
    }

    public Data WithSize(FileSize size)
    {
        Size = size;
        return this;
    }
}