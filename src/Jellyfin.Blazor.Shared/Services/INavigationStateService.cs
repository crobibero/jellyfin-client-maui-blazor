namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// Navigation state service.
/// </summary>
public interface INavigationStateService
{
    /// <summary>
    /// Clear history and store root url.
    /// </summary>
    /// <param name="rootUrl">The root url.</param>
    void Root(string rootUrl);

    /// <summary>
    /// Push url onto history.
    /// </summary>
    /// <param name="url">The url to store.</param>
    void Push(string url);

    /// <summary>
    /// Clear current history.
    /// </summary>
    void Clear();

    /// <summary>
    /// Determines if there is a previous state.
    /// </summary>
    /// <returns>Whether there is a page to go back to.</returns>
    bool CanGoBack();

    /// <summary>
    /// Get the previous url.
    /// </summary>
    /// <returns>The previous url.</returns>
    string GoBack();
}
