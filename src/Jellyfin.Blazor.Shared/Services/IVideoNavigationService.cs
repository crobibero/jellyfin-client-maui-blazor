using System;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// Video navigation service.
/// </summary>
public interface IVideoNavigationService
{
    /// <summary>
    /// Navigate to the item for playback.
    /// </summary>
    /// <param name="itemId">The item id.</param>
    void Navigate(Guid itemId);
}
