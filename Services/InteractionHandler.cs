using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MerpBot.Services
{
    public class InteractionHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static IConfigurationRoot _config;
        public static InteractionService _interactions;

        public InteractionHandler(IServiceProvider provider, DiscordSocketClient discord, InteractionService interactions)
        {
            _provider = provider;
            _discord = discord;
            _interactions = interactions;

            _discord.InteractionCreated += OnInteractionCreated;
            _interactions.SlashCommandExecuted += OnSlashCommandExecuted;
        }

        public async Task InitializeAsync()
        {
            await _interactions.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), _provider);
        }

        private Task OnSlashCommandExecuted(SlashCommandInfo command, IInteractionContext context, IResult result)
        {
            _ = Task.Run(async () =>
            {
                //exceptions
                if (result.Error == InteractionCommandError.Exception)
                {
                    if (result.Error == InteractionCommandError.Exception)
                    {
                        if (context.Interaction.HasResponded)
                            await context.Channel.SendMessageAsync("There was an internal error, please check the logs");
                        else
                            await context.Interaction.FollowupAsync("There was an internal error, please check the logs");

                        var infoChannel = (IMessageChannel)_discord.GetChannel(GlobalIDs.errorChannel);
                        await infoChannel.SendMessageAsync(
                            $"There was an error in {context.Guild.Name} in <#{context.Channel.Id}>\n" +
                            $"error: {result.ErrorReason}");
                    }
                }
                //general errors
                else if (result.Error != null)
                {
                    if (result.Error == InteractionCommandError.UnmetPrecondition)
                        await context.Interaction.FollowupAsync(result.ErrorReason, ephemeral: true);
                    else
                        await context.Interaction.FollowupAsync(result.ErrorReason, ephemeral: true);
                }
            });
            return Task.CompletedTask;
        }

        private Task OnInteractionCreated(SocketInteraction interaction)
        {
            _ = Task.Run(async () =>
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " (In OnInteractionCreated)");

                await interaction.DeferAsync();

                SocketInteractionContext context = new SocketInteractionContext(_discord, interaction);
                try
                {
                    // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                    await _interactions.ExecuteCommandAsync(context, _provider);
                }
                catch (Exception e)
                {
                    Helpers.ConsoleWithTimeStamp("[Interaction error] " + e.Message);

                    var infoChannel = (IMessageChannel)_discord.GetChannel(GlobalIDs.errorChannel);
                    await infoChannel.SendMessageAsync(
                        $"There was an error in {context.Guild.Name} in <#{context.Channel.Id}>\n" +
                        $"created at: {context.Interaction.CreatedAt.ToString("HH:mm:ss")} \n" +
                        $"error: {e.Message}");

                    // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                    // response, or at least let the user know that something went wrong during the command execution.
                    if (interaction.Type == InteractionType.ApplicationCommand)
                        await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            });
            return Task.CompletedTask;
        }
    }
}
