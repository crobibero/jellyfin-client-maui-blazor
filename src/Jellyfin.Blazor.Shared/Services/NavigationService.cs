using System;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Jellyfin.Blazor.Shared.Services;

/// <inheritdoc cref="Jellyfin.Blazor.Shared.Services.INavigationService" />
public class NavigationService : INavigationService, IDisposable
{
    private const string RootUrl = "/";
    private const string LogoutUrl = "/logout";

    private readonly NavigationManager _navigationManager;
    private readonly INavigationStateService _navigationStateService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="navigationManager">Instance of the <see cref="NavigationManager"/>.</param>
    /// <param name="navigationStateService">Instance of the <see cref="INavigationStateService"/> interface.</param>
    public NavigationService(
        NavigationManager navigationManager,
        INavigationStateService navigationStateService)
    {
        _navigationManager = navigationManager;
        _navigationStateService = navigationStateService;
        _navigationManager.LocationChanged += NavigationManagerOnLocationChanged;
    }

    /// <inheritdoc />
    public event EventHandler? OnNavigationChange;

    /// <inheritdoc />
    public string NavigateHome()
    {
        _navigationManager.NavigateTo(RootUrl);
        _navigationStateService.Root(RootUrl);
        return RootUrl;
    }

    /// <inheritdoc />
    public string NavigateToLibrary(Guid libraryId)
    {
        var destinationUrl = $"/library/{libraryId}";
        _navigationStateService.Push(destinationUrl);
        _navigationManager.NavigateTo(destinationUrl);
        return destinationUrl;
    }

    /// <inheritdoc />
    public string NavigateToItem(BaseItemDto baseItemDto)
    {
        var destinationUrl = baseItemDto.Type switch
        {
            BaseItemKind.Movie => $"/movie/{baseItemDto.Id}",
            BaseItemKind.Episode => $"/series/{baseItemDto.SeriesId}/season/{baseItemDto.SeasonId}/episode/{baseItemDto.Id}",
            BaseItemKind.Season => $"/series/{baseItemDto.SeriesId}/season/{baseItemDto.Id}",
            BaseItemKind.Series => $"/series/{baseItemDto.Id}",
            _ => $"/item/{baseItemDto.Id}"
        };

        _navigationStateService.Push(destinationUrl);
        _navigationManager.NavigateTo(destinationUrl);
        return destinationUrl;
    }

    /// <inheritdoc />
    public string NavigateToLogout()
    {
        _navigationStateService.Clear();
        _navigationManager.NavigateTo(LogoutUrl);
        return LogoutUrl;
    }

    /// <inheritdoc />
    public string NavigateBack()
    {
        var destinationUrl = _navigationStateService.GoBack();
        _navigationManager.NavigateTo(destinationUrl);
        return destinationUrl;
    }

    /// <inheritdoc />
    public bool CanGoBack() => _navigationStateService.CanGoBack();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose all resources.
    /// </summary>
    /// <param name="disposing">Whether to dispose.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _navigationManager.LocationChanged -= NavigationManagerOnLocationChanged;
        }
    }

    private void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
    }
}
