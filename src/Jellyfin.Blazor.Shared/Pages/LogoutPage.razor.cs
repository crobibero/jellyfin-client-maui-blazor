using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The logout page.
/// </summary>
public partial class LogoutPage
{
    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.LogoutAsync().ConfigureAwait(false);
        NavigationService.NavigateHome();
    }
}
