using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Wasm.Services;

/// <inheritdoc />
public class VideoNavigationService : IVideoNavigationService
{
    private readonly NavigationManager _navigationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoNavigationService"/> class.
    /// </summary>
    /// <param name="navigationManager">Instance of the <see cref="NavigationManager"/>.</param>
    public VideoNavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    /// <inheritdoc />
    public void Navigate(Guid itemId)
    {
        _navigationManager.NavigateTo($"/play/{itemId}/video");
    }
}
