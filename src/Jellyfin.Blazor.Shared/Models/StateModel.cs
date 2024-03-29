﻿using Jellyfin.Sdk;

namespace Jellyfin.Blazor.Shared.Models;

/// <summary>
/// The state model.
/// </summary>
public class StateModel
{
    /// <summary>
    /// Gets or sets the current token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the current host.
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// Gets or sets the current user.
    /// </summary>
    // TODO [JsonIgnore]
    public UserDto? UserDto { get; set; }
}
