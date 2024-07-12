// SPDX-License-Identifier: MIT

using FluentResults;

namespace DiscordSharpTemplate.Services;

public interface IExampleService {
    public Task<Result<string>> Start();
    public Task<Result<string>> Stop();
}