// SPDX-License-Identifier: MIT

using DSharpPlus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate;

public class BotHostedService(ILogger<BotHostedService> logger, DiscordClient discordClient) : IHostedService {
    public async Task StartAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Discord Bot Starting, connecting client.");
        await discordClient.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        logger.LogInformation("Discord Bot Stopping, disconnecting client.");
        await discordClient.DisconnectAsync();
    }
}