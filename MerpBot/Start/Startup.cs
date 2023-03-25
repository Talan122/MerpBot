using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MerpBot.Services;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using MerpBot.Interactions.Commands;
using Discord.Rest;
using Discord.Webhook;

namespace MerpBot.Start;
public class StartUp
{
    public IConfigurationRoot Configuration { get; }
    public LogSeverity Severity { get; set; }

    public StartUp(string[] args)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddDotNetEnv(".env")
            .AddYamlFile("Globals.yml");

        Configuration = builder.Build();

        if (Enum.TryParse(Configuration["LogLevel"], out LogSeverity severity))
            Severity = severity;
        else Logger.CrashWhenNoSeverityIsFound();
    }

    public static async Task StartAsync(string[] args)
    {
        StartUp startup = new StartUp(args);

        await startup.RunAsync();
    }

    public async Task RunAsync()
    {
        ServiceCollection services = new ServiceCollection();
        ConfigureServices(services);

        ServiceProvider provider = services.BuildServiceProvider();

        Helpers.Logger = provider.GetRequiredService<Logger>();

        await provider.GetRequiredService<InteractionHandler>().InitAsync();
        await provider.GetRequiredService<StartupService>().StartAsync();
        await Task.Delay(-1);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Severity,
                MessageCacheSize = 0,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All
            }))
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), new InteractionServiceConfig
            {
                LogLevel = Severity,
                DefaultRunMode = RunMode.Async,
                UseCompiledLambda = true
            }))
            .AddSingleton<InteractionHandler>()
            .AddSingleton<Logger>()
            .AddSingleton<StartupService>()
            .AddSingleton(x => new Download(x.GetRequiredService<Logger>()))
            .AddSingleton<HttpClient>()
            .AddSingleton(Configuration);
    }
}
