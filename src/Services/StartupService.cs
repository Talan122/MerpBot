using System;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Discord.Commands;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Reflection;

namespace MerpBot.Services;
public class StartupService
{
    public static IServiceProvider _provider;
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfiguration _config;

    public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
    {
        _provider = provider;
        _discord = discord;
        _commands = commands;
        _config = config;
    }

    public async Task StartAsync()
    {
        //initialize bot connection
        string token = _config["tokens:discord"];
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Discord token not found in config file");
            return;
        }
        await _discord.LoginAsync(TokenType.Bot, token);
        await _discord.StartAsync();
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
    }
}
