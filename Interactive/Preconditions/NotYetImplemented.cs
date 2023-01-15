using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    public class NotYetImplemented : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if(!(Context.User.Id == 267333357874970624)) return Task.FromResult(PreconditionResult.FromError($"It appears as if this command is not implemented. Try again later."));

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
