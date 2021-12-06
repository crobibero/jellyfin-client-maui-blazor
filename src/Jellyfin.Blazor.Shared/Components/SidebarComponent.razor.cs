using System;
using System.Threading.Tasks;
using Blazorise;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Components;

/// <summary>
/// The sidebar component.
/// </summary>
public partial class SidebarComponent
{
    private BaseItemDtoQueryResult? _views;
    private bool _visible = true;

    [Inject]
    private IStateService StateService { get; set; } = null!;

    [Inject]
    private IUserViewsClient UserViewsClient { get; set; } = null!;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _views = await UserViewsClient.GetUserViewsAsync(StateService.GetUserId())
            .ConfigureAwait(false);
        await base.OnInitializedAsync()
            .ConfigureAwait(false);
    }

    private void NavigateToDashboard()
    {
        NavigationService.NavigateHome();
    }

    private void NavigateToView(Guid libraryId)
    {
        NavigationService.NavigateToLibrary(libraryId);
    }

    private void NavigateToLogout()
    {
        NavigationService.NavigateToLogout();
    }

    private void ToggleVisibility()
    {
        _visible = !_visible;
        InvokeAsync(() => StateHasChanged());
    }
}
