// SPDX-License-Identifier: MIT

using DSharpPlus.Entities;

namespace DiscordSharpTemplate.Services;

public interface IAuthorisationService {
    bool HasPermission(string permission, DiscordUser user);
}