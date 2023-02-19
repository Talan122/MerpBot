using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using MerpBot.Services;

namespace MerpBot.Modules
{
    public class Testing : ModuleBase
    {
        public DiscordSocketClient discord { get; set; }
        public HttpClient HttpClient { get; set; }

        [Command("test")]
        [RequireOwner]
        public async Task Test()
        {
            await Context.Channel.SendMessageAsync("Message Received");
        }

        [Command("testembed")]
        [RequireOwner]
        public async Task Testembed()
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                .WithDescription("This is an embed")
                .WithColor(new Color(0, 255, 255))
                .AddField("User ID", Context.User.Id, true)
                .WithCurrentTimestamp();

            Embed embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }

        [Command("testtask")]
        [RequireOwner]
        public async Task Testtask()
        {
            await Context.Channel.SendMessageAsync("doing random stuff... ");
            await Task.Delay(2500);
            await Context.Channel.SendMessageAsync("task completed. ");
        }
    }
}
