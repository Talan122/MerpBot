using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions;
public class Disabled : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
    {
            return Task.FromResult(PreconditionResult.FromError("This command is disabled."));
    }
}
