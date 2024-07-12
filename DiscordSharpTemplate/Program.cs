// SPDX-License-Identifier: MIT

using System.Reflection;
using DiscordSharpTemplate;
using DiscordSharpTemplate.Configuration;
using DiscordSharpTemplate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using ConfigurationRoot = DiscordSharpTemplate.Configuration.ConfigurationRoot;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", optional: true)
    .AddUserSecrets<ConfigurationRoot>()
    .AddCommandLine(Environment.GetCommandLineArgs())
    .AddEnvironmentVariables()
    .Build();

ConfigurationRoot configurationRoot = new();
configuration.GetSection(nameof(ConfigurationRoot)).Bind(configurationRoot);

var assemblyName = Assembly.GetExecutingAssembly().GetName();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.Configure<ConfigurationRoot>(configuration.GetSection(nameof(ConfigurationRoot)));
        services.AddHttpClient();
        services.AddHostedService<BotHostedService>();
        services.AddScoped<IExampleService, ExampleService>();
        services.AddScoped<IAuthorisationService, SimpleAuthorisationService>();
        services.AddSingleton(TimeProvider.System);
        services.AddDiscord(configurationRoot);
    })
    .ConfigureLogging(l => l.AddOpenTelemetry(logging => {
        logging.SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(assemblyName.Name!)
                .AddAttributes([ new KeyValuePair<string, object>("service.version", assemblyName.Version!.ToString() ) ])
                .AddEnvironmentVariableDetector()
            );
        
        logging.AddConsoleExporter();
    }))
    .Build();

await host.RunAsync();