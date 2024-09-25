using System;
using System.Globalization;
using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using InteractionFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdeptusDicus;

public class Program
{
    private static IConfiguration? _configuration;
    private static IServiceProvider? _services;

    private static readonly DiscordSocketConfig SocketConfig = new()
    {
        GatewayIntents = GatewayIntents.All,
        AlwaysDownloadUsers = true,
    };

    private static readonly InteractionServiceConfig InteractionServiceConfig = new()
    {
        LocalizationManager = new ResxLocalizationManager("AdeptusDicus.Resources.CommandLocales", Assembly.GetEntryAssembly(),
         new CultureInfo("en-US"), new CultureInfo("fr-FR"))
    };

    public static async Task Main(string[] args)
    {
        _configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "DISCORD_")
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        _services = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton(SocketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), InteractionServiceConfig))
            .AddSingleton<InteractionHandler>()
            .BuildServiceProvider();

        var client = _services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;

        // Here we can initialize the service that will register and execute our commands
        await _services.GetRequiredService<InteractionHandler>()
            .InitializeAsync();

        // Bot token can be provided from the Configuration object we set up earlier
        await client.LoginAsync(TokenType.Bot, _configuration["token"]);
        await client.StartAsync();

        // Never quit the program until manually forced to.
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        return Task.CompletedTask;
    }
}