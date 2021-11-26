using System.Text.Json;
using Blazored.LocalStorage;
using Jellyfin.Blazor.Shared.Models;
using Jellyfin.Blazor.Shared.Services;

namespace Jellyfin.Blazor.Wasm.Services;

/// <inheritdoc />
public class StateStorageService : IStateStorageService
{
    private const string StateKey = "jellyfin.state";
    private readonly ILocalStorageService _localStorageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StateStorageService"/> class.
    /// </summary>
    /// <param name="localStorageService">Instance of the <see cref="ILocalStorageService"/>.</param>
    public StateStorageService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    /// <inheritdoc />
    public async Task<StateModel?> GetStoredStateAsync()
    {
        try
        {
            return await _localStorageService.GetItemAsync<StateModel>(StateKey)
                .ConfigureAwait(false);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task SetStoredStateAsync(StateModel? stateModel)
    {
        if (stateModel is null)
        {
            await _localStorageService.RemoveItemAsync(StateKey)
                .ConfigureAwait(false);
        }
        else
        {
            await _localStorageService.SetItemAsync(StateKey, stateModel)
                .ConfigureAwait(false);
        }
    }
}
