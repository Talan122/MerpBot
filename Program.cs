using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MerpBot.Services;
using System.Net.Http;

namespace MerpBot
{
    class BasicData
    {
        public static string AppVersion = "1.3.2";
    }

    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Running on version {BasicData.AppVersion}");
            await StartUp.RunAsync(args);
        }
    }

    class StartUp
    {
        public IConfigurationRoot Configuration { get; }

        public StartUp(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("_config.yml");
            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {

            StartUp startup = new StartUp(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();
            provider.GetRequiredService<InteractionHandler>();

            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();
            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 0,
                AlwaysDownloadUsers = true,
                GatewayIntents = Discord.GatewayIntents.AllUnprivileged | Discord.GatewayIntents.GuildPresences | Discord.GatewayIntents.GuildMessages
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = Discord.Commands.RunMode.Async,
                CaseSensitiveCommands = true
            }))
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>() ,new InteractionServiceConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = Discord.Interactions.RunMode.Async,
                UseCompiledLambda = true
            }))
            .AddSingleton<CommandHandler>()
            .AddSingleton<InteractionHandler>()
            .AddSingleton<StartupService>()
            .AddSingleton<VolatileData>()
            .AddSingleton<HttpClient>()
            .AddSingleton(Configuration);
        }
    }
}
