using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    // Similar to Super User, but allows mods too.
    public class HighUser : PreconditionAttribute
    {
        // A list of users that can use commands anywhere
        private static readonly ulong[] SuperUsers =
        {
            267333357874970624,
            194108558177075201,
            673941526912696351,
            593118233993805834
        };

        public HighUser()
        {
            // this literally doesnt even do anything what
        }

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if (SuperUsers.Contains(Context.User.Id))
                return Task.FromResult(PreconditionResult.FromSuccess());
            if (((SocketGuildUser)Context.User).Roles.Any(r => r.Id == 937175477657944074))
                return Task.FromResult(PreconditionResult.FromSuccess());
            return Task.FromResult(PreconditionResult.FromError("You are not a mod, you cannot use this command."));
        }
    }
}
