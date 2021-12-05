using System;
using Jellyfin.Sdk;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// The navigation service.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigation state changed.
    /// </summary>
    event EventHandler OnNavigationChange;

    /// <summary>
    /// Navigate to the home page.
    /// </summary>
    /// <returns>The destination url.</returns>
    string NavigateHome();

    /// <summary>
    /// Navigate to the library.
    /// </summary>
    /// <param name="libraryId">The library id.</param>
    /// <returns>The destination url.</returns>
    string NavigateToLibrary(Guid libraryId);

    /// <summary>
    /// Navigate to the specified item.
    /// </summary>
    /// <param name="baseItemDto">The base item dto.</param>
    /// <returns>The destination url.</returns>
    string NavigateToItem(BaseItemDto baseItemDto);

    /// <summary>
    /// Navigate to the logout page.
    /// </summary>
    /// <returns>The destination url.</returns>
    string NavigateToLogout();

    /// <summary>
    /// Navigate back.
    /// </summary>
    /// <returns>The destination url.</returns>
    string NavigateBack();

    /// <summary>
    /// Determines if the navigation service can go back.
    /// </summary>
    /// <returns>Whether the navigation service can navigate back.</returns>
    bool CanGoBack();
}
