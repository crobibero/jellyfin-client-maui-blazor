using System.Threading.Tasks;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// The <see cref="IAuthenticationService"/>.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticate against the host using the provided credentials.
    /// </summary>
    /// <param name="host">The server host.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>The authentication response.</returns>
    Task<(bool Status, string? ErrorMessage)> AuthenticateAsync(string host, string username, string? password);

    /// <summary>
    /// Log out of current session.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task LogoutAsync();

    /// <summary>
    /// Test whether client is currently authenticated.
    /// </summary>
    /// <returns>Authentication status.</returns>
    Task<bool> IsAuthenticatedAsync();
}
