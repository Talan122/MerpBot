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

namespace MerpBot.Interactive.Commands
{
    
    [Group("destiny", "Destiny things.")]
    public class Destiny : InteractionModuleBase<SocketInteractionContext>
    {
        [RequireOwner]
        [SlashCommand("test", "Testing command.")]
        public async Task Test()
        {

            var apps = await Destiny2.GetDestinyManifest();

            await FollowupAsync(apps.Response.ToString());
        }

        [NotYetImplemented]
        [SlashCommand("user", "Gets a user from the Destiny database.")]
        public async Task Users(string Username, int? arrNum = null)
        {

            try
            {
                var users = await User.SearchByGlobalNamePost(new UserSearchPrefixRequest(Username), 0);
                Console.WriteLine($"{users.Response.searchResults.First().destinyMemberships.First().membershipType} and {users.Response.searchResults.First().destinyMemberships.First().crossSaveOverride}");


                var response = users.Response.searchResults.First().destinyMemberships[0];
                if (response.crossSaveOverride != 0) response = users.Response.searchResults.First().destinyMemberships[users.Response.searchResults.First().destinyMemberships.First().membershipType];
                if (arrNum != null) response = users.Response.searchResults.First().destinyMemberships[(int)arrNum];

                EmbedBuilder embed = new EmbedBuilder()
                    .WithColor(new Color(Color.Magenta))
                    .WithAuthor(new EmbedAuthorBuilder().WithName(response.displayName).WithIconUrl($"https://www.bungie.net{response.iconPath}"))
                    .WithDescription($"Membership ID: {response.membershipId}\nMembership Type: {response.membershipType}")
                    ;

                await FollowupAsync(embed: embed.Build());
            }
            catch (Exception e)
            {
                await FollowupAsync($"A user likely couldn't be found.\n{e.Message}");
            }
        }
    }
}
