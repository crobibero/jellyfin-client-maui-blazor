using Jellyfin.Maui.Services;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.WebUI.Pages
{
    /// <summary>
    /// The logout page.
    /// </summary>
    public partial class Logout : ComponentBase
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
}
