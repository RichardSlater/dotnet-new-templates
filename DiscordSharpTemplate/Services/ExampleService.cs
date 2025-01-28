// SPDX-License-Identifier: MIT

using System.Diagnostics;
using System.Text.Json.Nodes;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace DiscordSharpTemplate.Services;

public class ExampleService(IHttpClientFactory httpClientFactory, ILogger<IExampleService> logger) : IExampleService {
    private const string ApiUrl = "https://www.randomnumberapi.com/api/v1.0/random?min=100&max=1000&count=1";
    private Stopwatch? _stopwatch;

    public async Task<Result<string>> Start() {
        logger.LogInformation("Start() called");
        if (_stopwatch != null) return Result.Fail("Already started!");
        _stopwatch = Stopwatch.StartNew();
        var randomNumber = await GetRandomNumber();
        if (randomNumber.IsFailed) return Result.Fail(randomNumber.Errors);

        return $"Started, your random number is {randomNumber.Value}";
    }

    public async Task<Result<string>> Stop() {
        if (_stopwatch == null) return Result.Fail("Not started!");
        _stopwatch.Stop();
        var randomNumber = await GetRandomNumber();
        var elapsed = _stopwatch.Elapsed;
        if (randomNumber.IsFailed) return Result.Fail(randomNumber.Errors);

        return $"Finished in {elapsed}, your random number is {randomNumber}";
    }

    private async Task<Result<int>> GetRandomNumber() {
        var client = httpClientFactory.CreateClient(nameof(ExampleService));
        var response = await client.GetAsync(ApiUrl);
        if (!response.IsSuccessStatusCode) return Result.Fail($"API responded: {response.StatusCode}");

        var content = await response.Content.ReadAsStreamAsync();
        var json = await JsonNode.ParseAsync(content);
        if (json == null) return Result.Fail("Unable to parse result from API Call");

        return json.AsArray()[0]!.GetValue<int>();
    }
}