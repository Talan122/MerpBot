using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    // Ignores the owner/mods. Takes in an array.
    public class ValidChannels : PreconditionAttribute
    {
        // A list of users that can use commands anywhere
        private static readonly ulong[] SuperUsers =
        {
            267333357874970624,
            194108558177075201,
            673941526912696351,
            593118233993805834
        };

        private ulong[] _channelId;
        private string _channelIdFormatted;

        public ValidChannels(ulong[] channelId) 
        {
            _channelId = channelId;

            foreach(var channelid in channelId)
            {
                _channelIdFormatted += $"<#{channelid}> ";
            }

        }

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if (Context.Channel is SocketGuildChannel gChannel)
            {
                if (_channelId.Contains(gChannel.Id))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else if (SuperUsers.Contains(Context.User.Id))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else if (((SocketGuildUser)Context.User).Roles.Any(r => r.Id == 937175477657944074))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else
                    return Task.FromResult(PreconditionResult.FromError($"You're in the wrong channel. Valid channels include: {_channelIdFormatted}"));

            }
            else
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to execute this command."));
        }
    }
}
