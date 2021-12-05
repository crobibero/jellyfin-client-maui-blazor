using System;
using System.Collections.Generic;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Services;

/// <inheritdoc />
public class NavigationService : INavigationService
{
    private const string RootUrl = "/";
    private const string LogoutUrl = "/logout";

    private readonly NavigationManager _navigationManager;
    private readonly Stack<string> _navigationHistory;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="navigationManager">Instance of the <see cref="NavigationManager"/>.</param>
    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationHistory = new Stack<string>();
    }

    /// <inheritdoc />
    public event EventHandler? OnNavigationChange;

    /// <inheritdoc />
    public string NavigateHome()
    {
        _navigationManager.NavigateTo(RootUrl);
        _navigationHistory.Clear();
        _navigationHistory.Push(RootUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return RootUrl;
    }

    /// <inheritdoc />
    public string NavigateToLibrary(Guid libraryId)
    {
        var destinationUrl = $"/library/{libraryId}";
        _navigationHistory.Push(destinationUrl);
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

        _navigationHistory.Push(destinationUrl);
        _navigationManager.NavigateTo(destinationUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return destinationUrl;
    }

    /// <inheritdoc />
    public string NavigateToLogout()
    {
        _navigationHistory.Clear();
        _navigationManager.NavigateTo(LogoutUrl);
        OnNavigationChange?.Invoke(this, EventArgs.Empty);
        return LogoutUrl;
    }

    /// <inheritdoc />
    public string NavigateBack()
    {
        if (CanGoBack())
        {
            // Pop current page.
            _navigationHistory.Pop();

            // Peek to get previous page.
            var destinationUrl = _navigationHistory.Peek();
            _navigationManager.NavigateTo(destinationUrl);
            OnNavigationChange?.Invoke(this, EventArgs.Empty);
            return destinationUrl;
        }

        throw new MethodAccessException("History is empty");
    }

    /// <inheritdoc />
    public bool CanGoBack() => _navigationHistory.Count > 1;
}
