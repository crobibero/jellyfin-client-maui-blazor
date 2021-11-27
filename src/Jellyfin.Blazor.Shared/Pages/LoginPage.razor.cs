using System;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.I18nText;
using Jellyfin.Blazor.Shared.PageModels;
using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The login page.
/// </summary>
public partial class LoginPage
{
    private readonly LoginPageModel _loginPageModel = new ();
    private bool _loading;
    private bool _initializing = true;
    private string? _error;

    /// <summary>
    /// Gets or sets the optional return url.
    /// </summary>
    [Parameter]
    public string? ReturnUrl { get; set; }

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
    private ILogger<LoginPage> Logger { get; init; } = null!;

    [Inject]
    private Toolbelt.Blazor.I18nText.I18nText I18NText { get; init; } = null!;

    private Text Text { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Text = await I18NText.GetTextTableAsync<Text>(this).ConfigureAwait(false);
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
                Navigate();
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
                Navigate();
            }
            else
            {
                _error = errorMessage;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unhandled exception");
            _error = Text.LoginUnknownError;
        }
        finally
        {
            _loading = false;
        }
    }

    private void Navigate()
    {
        // FIXME - this doesn't redirect properly.
        var destinationUrl = "/";
        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            if (ReturnUrl[0] == '/')
            {
                destinationUrl += ReturnUrl[1..];
            }
            else
            {
                destinationUrl += ReturnUrl;
            }
        }

        NavigationManager.NavigateTo(destinationUrl);
    }
}
