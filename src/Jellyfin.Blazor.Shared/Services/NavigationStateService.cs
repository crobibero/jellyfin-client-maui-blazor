using System;
using System.Collections.Generic;

namespace Jellyfin.Blazor.Shared.Services;

/// <inheritdoc />
public class NavigationStateService : INavigationStateService
{
    private readonly Stack<string> _navigationHistory;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationStateService"/> class.
    /// </summary>
    public NavigationStateService()
    {
        _navigationHistory = new Stack<string>();
    }

    /// <inheritdoc />
    public void Root(string rootUrl)
    {
        Clear();
        Push(rootUrl);
    }

    /// <inheritdoc />
    public void Push(string url)
    {
        _navigationHistory.Push(url);
    }

    /// <inheritdoc />
    public void Clear()
    {
        _navigationHistory.Clear();
    }

    /// <inheritdoc />
    public bool CanGoBack()
        => _navigationHistory.Count > 1;

    /// <inheritdoc />
    public string GoBack()
    {
        if (CanGoBack())
        {
            // Pop current page.
            _navigationHistory.Pop();

            // Peek to get previous page.
            return _navigationHistory.Peek();
        }

        throw new MethodAccessException("History is empty");
    }
}
