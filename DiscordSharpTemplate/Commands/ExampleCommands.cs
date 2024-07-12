// SPDX-License-Identifier: MIT

using DiscordSharpTemplate.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FluentResults;

namespace DiscordSharpTemplate.Commands;

public class ExampleCommands(IExampleService exampleService, IAuthorisationService authorisationService)
    : BaseCommandModule {
    [Command("hello")]
    internal async Task Board(CommandContext ctx) {
        await ctx.RespondAsync("hello");
    }

    [Command("start")]
    internal async Task Start(CommandContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.RespondAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var response = await exampleService.Start();
        await RespondToCommand(ctx, response);
    }

    [Command("stop")]
    internal async Task Stop(CommandContext ctx) {
        if (!authorisationService.HasPermission(WellKnownRoles.Administrators, ctx.User)) {
            await ctx.RespondAsync("Sorry, you are not authorised to run this command.");
            return;
        }

        var response = await exampleService.Stop();
        await RespondToCommand(ctx, response);
    }

    private static async Task RespondToCommand(CommandContext ctx, Result<string> response) {
        if (response.IsSuccess) await ctx.RespondAsync(response.Value);
        foreach (var error in response.Errors) await ctx.RespondAsync(error.Message);
    }
}