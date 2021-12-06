using System.Text.Json;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Models;
using Jellyfin.Blazor.Shared.Services;
using Microsoft.Maui.Essentials;

namespace Jellyfin.Blazor.Maui.Services;

/// <inheritdoc />
public class StateStorageService : IStateStorageService
{
    private const string StateKey = "jellyfin.state";

    /// <inheritdoc />
    public async ValueTask<StateModel?> GetStoredStateAsync()
    {
        try
        {
            var storedState = await SecureStorage.GetAsync(StateKey)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(storedState))
            {
                return null;
            }

            return JsonSerializer.Deserialize<StateModel>(storedState);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async ValueTask SetStoredStateAsync(StateModel? stateModel)
    {
        if (stateModel is null)
        {
            SecureStorage.Remove(StateKey);
        }
        else
        {
            var stateString = JsonSerializer.Serialize(stateModel);
            await SecureStorage.SetAsync(StateKey, stateString)
                .ConfigureAwait(false);
        }
    }
}
