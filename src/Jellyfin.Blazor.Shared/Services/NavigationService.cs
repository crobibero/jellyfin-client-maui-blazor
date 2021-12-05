using System;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Services;

/// <inheritdoc />
public class NavigationService : INavigationService
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
    }

    /// <inheritdoc />
    public event EventHandler? OnNavigationChange;

    /// <inheritdoc />
    public string NavigateHome()
    {
        _navigationManager.NavigateTo(RootUrl);
        _navigationStateService.Root(RootUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return RootUrl;
    }

    /// <inheritdoc />
    public string NavigateToLibrary(Guid libraryId)
    {
        var destinationUrl = $"/library/{libraryId}";
        _navigationStateService.Push(destinationUrl);
        _navigationManager.NavigateTo(destinationUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
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
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return destinationUrl;
    }

    /// <inheritdoc />
    public string NavigateToLogout()
    {
        _navigationStateService.Clear();
        _navigationManager.NavigateTo(LogoutUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return LogoutUrl;
    }

    /// <inheritdoc />
    public string NavigateBack()
    {
        var destinationUrl = _navigationStateService.GoBack();
        _navigationManager.NavigateTo(destinationUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return destinationUrl;
    }

    /// <inheritdoc />
    public bool CanGoBack() => _navigationStateService.CanGoBack();
}
