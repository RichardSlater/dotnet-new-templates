using DSharpPlus.CommandsNext;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate.Helpers;

public static class DiscordHelper {
    public static async Task RespondToCommand<TResult, TLogger>(this CommandContext ctx, Result<TResult> result,
        ILogger<TLogger> logger) {
        if (result.IsSuccess) {
            var output = result.Value?.ToString() ?? "Empty Result";
            logger.LogInformation("Command result indicates success, responding to user with '{output}'", output);
            await ctx.RespondAsync(output);
        }

        foreach (var error in result.Errors) {
            logger.LogError("Command result indicates failure, responding to user with: {error}", error.Message);
            await ctx.RespondAsync(error.Message);
        }
    }
}