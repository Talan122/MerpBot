using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace MerpBot.Interactive.Preconditions
{
    public class RequireGuild : PreconditionAttribute
    {
        private ulong _guildId;

        public RequireGuild(ulong guildId) => _guildId = guildId;

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext Context, ICommandInfo Command, IServiceProvider Services)
        {
            if (Context.Guild is SocketGuild Guild)
            {
                if (Guild.Id == _guildId)
                    return Task.FromResult(PreconditionResult.FromSuccess());

                return Task.FromResult(PreconditionResult.FromError($"This command likely wasn't made for this guild. Try elsewhere."));

            }
            else
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to execute this command."));
        }
    }
}