// SPDX-License-Identifier: MIT

using DiscordSharpTemplate;
using DiscordSharpTemplate.Configuration;
using DiscordSharpTemplate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", true)
    .AddUserSecrets<BotConfiguration>()
    .AddCommandLine(Environment.GetCommandLineArgs())
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

BotConfiguration botConfiguration = new();
var botConfigurationSection = configuration.GetSection(nameof(BotConfiguration).Replace("Configuration", string.Empty));
botConfigurationSection.Bind(botConfiguration);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.Configure<BotConfiguration>(botConfigurationSection);
        services.AddHttpClient();
        services.AddHostedService<BotHostedService>();
        services.AddScoped<IExampleService, ExampleService>();
        services.AddScoped<IAuthorisationService, SimpleAuthorisationService>();
        services.AddSingleton(TimeProvider.System);
        services.AddDiscord(botConfiguration);
    })
    .UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(configuration))
    .Build();

await host.RunAsync();