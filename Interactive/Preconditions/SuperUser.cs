using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    // Only allows super users to use a command.
    public class SuperUser : PreconditionAttribute
    {
        // A list of users that can use commands anywhere
        public static readonly ulong[] SuperUsers =
        {
            267333357874970624,
            194108558177075201,
            673941526912696351,
            593118233993805834
        };

        public SuperUser()
        {
            // this literally doesnt even do anything what
        }

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if(SuperUsers.Contains(Context.User.Id))
                return Task.FromResult(PreconditionResult.FromSuccess());
            return Task.FromResult(PreconditionResult.FromError("You are not a super user, you cannot run this command."));
        }
    }
}
