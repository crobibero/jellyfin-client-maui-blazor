using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Sdk;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// The library service interface.
/// </summary>
public interface ILibraryService
{
    /// <summary>
    /// Gets the list of visible libraries.
    /// </summary>
    /// <returns>The list of libraries.</returns>
    Task<IReadOnlyList<BaseItemDto>> GetLibrariesAsync();

    /// <summary>
    /// Gets the library by id.
    /// </summary>
    /// <param name="id">The library id.</param>
    /// <returns>The library.</returns>
    Task<BaseItemDto?> GetLibraryAsync(Guid id);

    /// <summary>
    /// Gets the item.
    /// </summary>
    /// <param name="id">The item id.</param>
    /// <returns>The item.</returns>
    Task<BaseItemDto?> GetItemAsync(Guid id);

    /// <summary>
    /// Gets the library items.
    /// </summary>
    /// <param name="library">The library dto.</param>
    /// <param name="limit">The count of items to return.</param>
    /// <param name="startIndex">The first item index.</param>
    /// <returns>The library items.</returns>
    Task<BaseItemDtoQueryResult> GetLibraryItemsAsync(BaseItemDto library, int limit, int startIndex);

    /// <summary>
    /// Gets the next up items.
    /// </summary>
    /// <param name="libraryIds">The list of library ids.</param>
    /// <returns>The next up items.</returns>
    Task<IReadOnlyList<BaseItemDto>> GetNextUpAsync(IEnumerable<Guid> libraryIds);

    /// <summary>
    /// Gets the continue watching items.
    /// </summary>
    /// <returns>The continue watching items.</returns>
    Task<IReadOnlyList<BaseItemDto>> GetContinueWatchingAsync();

    /// <summary>
    /// Gets the recently added items.
    /// </summary>
    /// <param name="libraryId">The library id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The recently added library items.</returns>
    Task<IReadOnlyList<BaseItemDto>> GetRecentlyAddedAsync(Guid libraryId, CancellationToken cancellationToken = default);
}
