// SPDX-License-Identifier: MIT

using DSharpPlus.Entities;

namespace DiscordSharpTemplate.Services;

public class SimpleAuthorisationService : IAuthorisationService {
    private readonly Dictionary<string, ulong[]> _permissionTable = new() {
        { WellKnownRoles.Administrators, [388760469235695618, 265550347433410560] }
    };

    public bool HasPermission(string permission, DiscordUser user) {
        return _permissionTable.TryGetValue(permission, out var value) && value.Contains(user.Id);
    }
}