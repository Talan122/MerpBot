using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    // Ignores the owner/mods.
    public class RequireChannel : PreconditionAttribute
    {
        // A list of users that can use commands anywhere
        private static readonly ulong[] SuperUsers =
        {
            267333357874970624, // Stupid idiot known as Talan
            194108558177075201, // Traso
            673941526912696351, // Katari
            593118233993805834
        };

        private ulong _channelId;


        public RequireChannel(ulong channelId) => _channelId = channelId;

        public RequireChannel() => _channelId = 941536375210127420;

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if (Context.Channel is SocketGuildChannel gChannel)
            {
                if (gChannel.Id == _channelId)
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else if (SuperUsers.Contains(Context.User.Id))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else if (((SocketGuildUser)Context.User).Roles.Any(r => r.Id == 937175477657944074))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else
                    return Task.FromResult(PreconditionResult.FromError($"You're in the wrong channel. The only valid channel for this command is <#{_channelId}>"));

            }
            else
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to execute this command."));
        }
    }
}
