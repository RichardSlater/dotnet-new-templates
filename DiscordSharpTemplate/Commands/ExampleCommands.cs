// SPDX-License-Identifier: MIT

using DiscordSharpTemplate.Helpers;
using DiscordSharpTemplate.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate.Commands;

public class ExampleCommands(
    IExampleService exampleService,
    IAuthorisationService authorisationService,
    ILogger<ExampleCommands> logger)
    : BaseCommandModule {
    [Command("hello")]
    public async Task Hello(CommandContext ctx) {
        await ctx.RespondAsync("hello");
    }

    [Command("start")]
    public async Task Start(CommandContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.RespondAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var result = await exampleService.Start();
        await ctx.RespondToCommand(result, logger);
    }

    [Command("stop")]
    public async Task Stop(CommandContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.RespondAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var result = await exampleService.Stop();
        await ctx.RespondToCommand(result, logger);
    }
}