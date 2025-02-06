// SPDX-License-Identifier: MIT

using System.Reflection;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate.Configuration;

public static class DSharpPlusExtensions {
    public static IServiceCollection AddDiscord(this IServiceCollection services, BotConfiguration config) {
        return services.AddSingleton<DiscordClient>(serviceProvider => {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            Action<string, object?[]> logInfo = logger != null
                ? (message, args) => logger.LogInformation(message, args)
                : (message, args) => Console.WriteLine(string.Format(message, args));
            if (config == null)
                throw new InvalidOperationException(
                    "Unable to read configuration, ensure user-secrets is configured with the BotToken.");
            var prefixes = config.Discord.Prefixes ?? ["$"];
            logInfo("Prefixes selected: {prefixes}", [string.Join(", ", prefixes)]);
            var client = new DiscordClient(new DSharpPlus.DiscordConfiguration {
                // NOTE: config is dependant upon https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0
                // RECOMMENDATION : 
                //   - for local development add an app-secret in "%APPDATA%\Microsoft\UserSecrets\147c60f6-3bfc-424d-85c8-c886fcb007d8\secrets.json" 
                //     (alternatively use the Visual Studio / Rider context menu for the project - "Manage User Secrets" / "Tools" → ".NET User Secrets")
                //   - for production create an environment variable, however note that this environment variable is NOT encrypted in memory.
                Token = config.Discord.BotToken ??
                        throw new InvalidOperationException("BotToken missing from configuration."),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
            var commands = client.UseSlashCommands(new SlashCommandsConfiguration {
                Services = serviceProvider,
            });
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            return client;
        });
    }
}