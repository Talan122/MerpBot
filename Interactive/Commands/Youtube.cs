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
using MerpBot.Interactive.Modals;
using MerpBot.Interactive.Options;
using Newtonsoft.Json;
using MerpBot.Destiny;
using MerpBot.Destiny.ResponseTypes.User;
using System.Net.Http;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.Runtime.CompilerServices;
using YoutubeExplode.Videos;
using System.Collections.Generic;

namespace MerpBot.Interactive.Commands
{
    [Group("youtube", "Youtube commands.")]
    public class Youtube : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("download", "Downloads a Youtube video. Maxes at 2 minutes for file size.")]
        public async Task Download(string link, bool spoiler = false)
        {

            if(!link.Contains("https://www.youtube.com/") && !link.Contains("https://youtu.be/") && !link.Contains("https://youtube.com/"))
            {
                await FollowupAsync("Not a valid Youtube link. Must be in the format of either `https://www.youtube.com/` or `https://youtu.be/`");
                return;
            }

            try
            {
                YoutubeHelp youtube = new(link);

                if (!await youtube.CheckSizeAsync())
                {
                    await FollowupAsync("File was larger than 8mb and can't send.");
                    return;
                }

                Stream YTStream = await youtube.GetStreamAsync();

                if(!spoiler) await FollowupWithFileAsync(YTStream, $"{await youtube.GetFormattedForFileTitleAsync()}.mp4");
                else await FollowupWithFileAsync(YTStream, $"SPOILER_{await youtube.GetFormattedForFileTitleAsync()}.mp4");
            }
            catch(Exception e)
            {
                await FollowupAsync("There was an error processing this request. Perhaps the link you gave wasn't a valid Youtube link (it 404s)?");
            }


        }
        
        private class YoutubeHelp
        {
            public string Link { get; set; }
            public StreamManifest Manifest { get; set; }
            public IVideoStreamInfo VideoStreamInfo { get; set; }

            public YoutubeHelp(string link)
            { 
                Link = LinkToValid(link);
            }

            /// <summary>
            /// Takes a potentially invalid link and makes it valid. Won't do anything if it's valid.
            /// </summary>
            /// <param name="link"></param>
            /// <returns>A link in the form of https://www.youtube.com/watch?v=(id)</returns>
            private static string LinkToValid(string link)
            {
                // https://www.youtube.com/watch?v=VWgy-mExOOo

                if (link.Contains("https://www.youtube.com/")) return link;

                string result = "";

                string[] split = link.Split();

                if (split.Last().Contains(" ")) throw new ArgumentException();
                result = split.Last();

                return result;
            }

            public async Task<StreamManifest> GetManifestAsync()
            {
                Manifest ??= await YTClient.client.Videos.Streams.GetManifestAsync(Link);
                return Manifest;
            }

            public async Task<IVideoStreamInfo> GetInfoAsync()
            {
                if (VideoStreamInfo != null) return VideoStreamInfo;

                IEnumerable<MuxedStreamInfo> muxedStreams = (await GetManifestAsync()).GetMuxedStreams();

                FileSize fileSize = muxedStreams.GetWithHighestVideoQuality().Size;

                if (fileSize.MegaBytes < 8) 
                {
                    VideoStreamInfo = muxedStreams.GetWithHighestVideoQuality();
                    return VideoStreamInfo;
                }

                double size = 0;

                foreach(var stream in muxedStreams.ToArray())
                {
                    if(stream.Size.MegaBytes < 8)
                    {
                        if(stream.Size.MegaBytes > size)
                        {
                            Console.WriteLine("t");
                            VideoStreamInfo = stream;
                            size = stream.Size.MegaBytes;
                        }
                    }
                }

                return VideoStreamInfo ?? muxedStreams.GetWithHighestVideoQuality(); // If none can be found, it just defaults to the largest one.
            }

            public async Task<Stream> GetStreamAsync()
            {
                IVideoStreamInfo streamInfo = await GetInfoAsync();

                return await YTClient.client.Videos.Streams.GetAsync(streamInfo);
            }

            public async Task<Video> GetMetadataAsync()
            {
                return await YTClient.client.Videos.GetAsync(Link);
            }

            public async Task<string> GetFormattedForFileTitleAsync()
            {
                string fileName = (await GetMetadataAsync()).Title;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }

                return fileName;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>Returns true if the file size is less than 8MB</returns>
            public async Task<bool> CheckSizeAsync()
            {
                if ((await GetInfoAsync()).Size.MegaBytes >= 8) return false;
                return true;
            }
        }
    }
    internal static class YTClient
    {
        public static YoutubeClient client = new YoutubeClient();
    }
}
