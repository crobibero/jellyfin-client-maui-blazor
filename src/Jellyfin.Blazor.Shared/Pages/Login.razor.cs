using System;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.PageModels;
using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The login page.
/// </summary>
public partial class Login
{
    private readonly LoginPageModel _loginPageModel = new ();
    private bool _loading;
    private bool _initializing = true;
    private string? _error;

    [Inject]
    private IStateService StateService { get; init; } = null!;

    [Inject]
    private IStateStorageService StateStorageService { get; init; } = null!;

    [Inject]
    private JellyfinAuthStateProvider AuthStateProvider { get; init; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; init; } = null!;

    [Inject]
    private IAuthenticationService AuthenticationService { get; init; } = null!;

    [Inject]
    private ILogger<Login> Logger { get; init; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        var currentToken = StateService.GetState().Token;
        if (string.IsNullOrEmpty(currentToken))
        {
            var storedState = await StateStorageService.GetStoredStateAsync()
                .ConfigureAwait(false);
            if (storedState is not null)
            {
                StateService.SetState(storedState);
                currentToken = storedState.Token;
            }
        }

        if (!string.IsNullOrEmpty(currentToken))
        {
            var isAuthenticated = await AuthenticationService.IsAuthenticatedAsync()
                .ConfigureAwait(false);
            if (isAuthenticated)
            {
                AuthStateProvider.StateChanged();
                NavigationManager.NavigateTo(string.Empty);
            }
            else
            {
                StateService.ClearState();
                await StateStorageService.SetStoredStateAsync(null)
                    .ConfigureAwait(false);
            }
        }

        _initializing = false;
    }

    private async Task HandleLogin()
    {
        _loading = true;
        try
        {
            var (status, errorMessage) = await AuthenticationService.AuthenticateAsync(
                    _loginPageModel.Host,
                    _loginPageModel.Username,
                    _loginPageModel.Password)
                .ConfigureAwait(false);
            if (status)
            {
                NavigationManager.NavigateTo(string.Empty);
            }
            else
            {
                _error = errorMessage;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unhandled exception");
            _error = "An unknown error occurred";
        }
        finally
        {
            _loading = false;
        }
    }
}
