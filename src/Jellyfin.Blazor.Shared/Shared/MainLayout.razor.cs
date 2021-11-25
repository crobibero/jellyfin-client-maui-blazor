using System;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Shared;

/// <summary>
/// The main layout.
/// </summary>
public partial class MainLayout
{
    private const string LinkCssClass = "hover:bg-indigo-600 group flex items-center px-2 py-2 text-sm font-medium rounded-md cursor-pointer";
    private const string IconCssClass = "mr-3 flex-shrink-0 h-6 w-6";

    private string _currentRoute = string.Empty;
    private BaseItemDtoQueryResult? _views;
    private bool _showSidebar;

    /// <summary>
    /// Gets or sets a value indicating whether the sidebar should be shown.
    /// </summary>
    [Parameter]
    public bool ShowSidebar
    {
        get => _showSidebar;
        set
        {
            if (_showSidebar == value)
            {
                return;
            }

            _showSidebar = value;
            ShowSidebarChanged?.InvokeAsync(value).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Gets or sets the event callback when the sidebar shown event changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool>? ShowSidebarChanged { get; set; }

    [Inject]
    private IUserViewsClient UserViewsClient { get; set; } = null!;

    [Inject]
    private IStateService StateService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        ShowSidebar = false;
        _views = await UserViewsClient.GetUserViewsAsync(StateService.GetUserId())
            .ConfigureAwait(false);
        await base.OnInitializedAsync()
            .ConfigureAwait(false);
    }

    private void NavigateToDashboard()
    {
        ShowSidebar = false;
        _currentRoute = string.Empty;
        NavigationManager.NavigateTo(_currentRoute);
    }

    private void NavigateToView(Guid libraryId)
    {
        ShowSidebar = false;
        _currentRoute = $"/view/{libraryId}";
        NavigationManager.NavigateTo(_currentRoute);
    }

    private void NavigateToLogout()
    {
        ShowSidebar = false;
        NavigationManager.NavigateTo("/logout");
    }
}
