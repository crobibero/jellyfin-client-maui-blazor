using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Models;
using Jellyfin.Blazor.Shared.Services;

namespace Jellyfin.Blazor.Maui.Services;

/// <inheritdoc />
public class StateStorageService : IStateStorageService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StateStorageService"/> class.
    /// </summary>
    public StateStorageService()
    {
    }

    /// <inheritdoc />
    public Task<StateModel?> GetStoredStateAsync()
    {
        return Task.FromResult<StateModel?>(null);
    }

    /// <inheritdoc />
    public Task SetStoredStateAsync(StateModel? stateModel)
    {
        return Task.CompletedTask;
    }
}
