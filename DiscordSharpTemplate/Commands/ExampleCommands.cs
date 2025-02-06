// SPDX-License-Identifier: MIT

using DiscordSharpTemplate.Helpers;
using DiscordSharpTemplate.Services;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate.Commands;

public class ExampleCommands(
    IExampleService exampleService,
    IAuthorisationService authorisationService,
    ILogger<ExampleCommands> logger)
    : ApplicationCommandModule {
    [SlashCommand("hello", "Says hello")]
    public async Task Hello(InteractionContext ctx) {
        await ctx.CreateResponseAsync("hello");
    }

    [SlashCommand("start", "Starts the example service")]
    public async Task Start(InteractionContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.CreateResponseAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var result = await exampleService.Start();
        await ctx.RespondToCommand(result, logger);
    }

    [SlashCommand("stop", "Stops the example service")]
    public async Task Stop(InteractionContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.CreateResponseAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var result = await exampleService.Stop();
        await ctx.RespondToCommand(result, logger);
    }
}