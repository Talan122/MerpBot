using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Services;
using MerpBot.Interactive;
using MerpBot.Interactive.Preconditions;
using System.IO;
using System.Diagnostics;
using System.Management;

namespace MerpBot.Interactive.Modals
{
    public class Feedback : InteractionModuleBase<SocketInteractionContext>
    {
        public DiscordSocketClient Client { get; set; }

        public class FeedbackModal : IModal
        {
            public string Title => "Feedback";

            [InputLabel("Send feedback. Do not use for moderation.")]
            [ModalTextInput("message", TextInputStyle.Paragraph, minLength: 1)]
            public string Message { get; set; }
        }

        [ModalInteraction("feedback_modal")]
        public async Task FeedbackModalResponse(FeedbackModal modal)
        {
            EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle("Feedback")
                    .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                    .WithColor(new Color(255, 255, 10))
                    .AddField(Context.User.Username, modal.Message)
                    .WithCurrentTimestamp();

            IMessageChannel Channel = (IMessageChannel)Client.GetChannel(931653464738660362);

            await Channel.SendMessageAsync(embed: builder.Build());

            await FollowupAsync("Feedback recieved!", ephemeral: true);
        }
    }
}
