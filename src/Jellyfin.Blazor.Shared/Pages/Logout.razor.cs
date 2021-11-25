using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The logout page.
/// </summary>
public partial class Logout
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        AuthenticationService.Logout();
        NavigationManager.NavigateTo("/");
    }
}
