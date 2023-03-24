using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MerpBot.Interactions.Commands;
public class Download
{
    public Dictionary<string, Func<Task<Data>>> Downloaders = new Dictionary<string, Func<Task<Data>>>();

    public YoutubeClient YoutubeClient { get; set; }

    private string Url { get; set; } = string.Empty;

    public Download()
    {
        YoutubeClient = new YoutubeClient();

        Downloaders["youtube"] = YoutubeDownload;
    }

    private async Task<Data> YoutubeDownload()
    {

        if(Url == "") throw new NullReferenceException(nameof(Url));

        var About = await YoutubeClient.Videos.GetAsync(Url);

        Data videoData = new Data()
            .WithName(About.Title)
            .WithDescription(About.Description)
            .WithTime(About.Duration ?? new TimeSpan());

        var Manifest = await YoutubeClient.Videos.Streams.GetManifestAsync(Url);

        var StreamInfo = Manifest.GetMuxedStreams().GetWithHighestVideoQuality();

        if (StreamInfo.Size > new FileSize(8000000 /* 8 megabyes */)) throw new Exception("File is larger than 8gb");

        videoData.WithStream(await YoutubeClient.Videos.Streams.GetAsync(StreamInfo));

        return videoData;
            
    }

    /// <summary>
    /// Downloads content from any source.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>Returns null if that source is not yet supported, otherwise it returns the data needed.</returns>
    public async Task<Data?> DownloadContent(string url)
    {
        Url = url;

        if(url.Contains("youtube") || url.Contains("youtu.be")) return await Downloaders["youtube"]();

        return null;
    }
}


public class Data
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan? Time { get; set; }
    public Stream? Stream { get; set; }

    public Data(Stream? stream, string? name, string? description, TimeSpan? time)
    {
        Stream = stream;
        Name = name;
        Description = description;
        Time = time;
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
}
