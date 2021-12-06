using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Models;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// The state storage service.
/// </summary>
public interface IStateStorageService
{
    /// <summary>
    /// Gets the stored state.
    /// </summary>
    /// <returns>The stored state.</returns>
    ValueTask<StateModel?> GetStoredStateAsync();

    /// <summary>
    /// Sets the stored state.
    /// </summary>
    /// <param name="stateModel">The state model.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    ValueTask SetStoredStateAsync(StateModel? stateModel);
}
