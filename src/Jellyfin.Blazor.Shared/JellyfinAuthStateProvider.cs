using System.Security.Claims;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jellyfin.Blazor.Shared;

/// <summary>
/// The jellyfin auth state provider.
/// </summary>
public class JellyfinAuthStateProvider : AuthenticationStateProvider
{
    private readonly IStateService _stateService;
    private readonly IUserClient _userClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="JellyfinAuthStateProvider"/> class.
    /// </summary>
    /// <param name="userClient">Instance of the <see cref="IUserClient"/> interface.</param>
    /// <param name="stateService">Instance of the <see cref="IStateService"/> interface.</param>
    public JellyfinAuthStateProvider(
        IUserClient userClient,
        IStateService stateService)
    {
        _userClient = userClient;
        _stateService = stateService;
    }

    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = _stateService.GetState();
        if (string.IsNullOrWhiteSpace(state.Host) || string.IsNullOrWhiteSpace(state.Token))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        try
        {
            var currentUser = await _userClient.GetCurrentUserAsync()
                .ConfigureAwait(false);
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, currentUser.Name),
                    new Claim(ClaimTypes.Sid, currentUser.Id.ToString())
                },
                "jellyfin");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch (UserException)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }
    }

    /// <summary>
    /// Notifies the authentication provider that the state has changed.
    /// </summary>
    public void StateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
