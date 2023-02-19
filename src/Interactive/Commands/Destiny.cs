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
using Newtonsoft.Json;
using MerpBot.Destiny;
using MerpBot.Destiny.ResponseTypes.User;

namespace MerpBot.Interactive.Commands;
    
[Group("destiny", "Destiny things.")]
public class Destiny : InteractionModuleBase<SocketInteractionContext>
{
    [RequireOwner]
    [SlashCommand("test", "Testing command.")]
    public async Task Test()
    {
        var apps = await Destiny2.GetDestinyManifest();

        await RespondAsync(apps.Response.ToString());
    }
}
