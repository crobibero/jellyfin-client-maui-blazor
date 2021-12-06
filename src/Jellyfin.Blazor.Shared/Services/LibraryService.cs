using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Sdk;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// The library service.
/// </summary>
public class LibraryService : ILibraryService
{
    private readonly IItemsClient _itemsClient;
    private readonly IStateService _stateService;
    private readonly ITvShowsClient _tvShowsClient;
    private readonly IUserLibraryClient _userLibraryClient;
    private readonly IUserViewsClient _userViewsClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryService"/> class.
    /// </summary>
    /// <param name="itemsClient">Instance of the <see cref="IItemsClient"/> interface.</param>
    /// <param name="stateService">Instance of the <see cref="IStateService"/> interface.</param>
    /// <param name="tvShowsClient">Instance of the <see cref="ITvShowsClient"/> interface.</param>
    /// <param name="userLibraryClient">Instance of the <see cref="IUserLibraryClient"/> interface.</param>
    /// <param name="userViewsClient">Instance of the <see cref="IUserViewsClient"/> interface.</param>
    public LibraryService(
        IItemsClient itemsClient,
        IStateService stateService,
        ITvShowsClient tvShowsClient,
        IUserLibraryClient userLibraryClient,
        IUserViewsClient userViewsClient)
    {
        _itemsClient = itemsClient;
        _stateService = stateService;
        _tvShowsClient = tvShowsClient;
        _userLibraryClient = userLibraryClient;
        _userViewsClient = userViewsClient;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<BaseItemDto>> GetLibrariesAsync()
    {
        var userId = _stateService.GetUserId();
        var views = await _userViewsClient.GetUserViewsAsync(userId)
            .ConfigureAwait(false);
        return views is null ? Array.Empty<BaseItemDto>() : views.Items;
    }

    /// <inheritdoc />
    public async Task<BaseItemDto?> GetLibraryAsync(Guid id)
    {
        var userId = _stateService.GetUserId();
        var result = await _itemsClient.GetItemsAsync(
                userId,
                ids: new[] { id })
            .ConfigureAwait(false);
        return result.Items.Count == 0 ? null : result.Items[0];
    }

    /// <inheritdoc />
    public async Task<BaseItemDto?> GetItemAsync(Guid id)
    {
        var userId = _stateService.GetUserId();
        return await _userLibraryClient.GetItemAsync(userId, id)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BaseItemDtoQueryResult> GetLibraryItemsAsync(
        BaseItemDto library,
        int limit,
        int startIndex)
    {
        var userId = _stateService.GetUserId();
        return await _itemsClient.GetItemsAsync(
                userId,
                recursive: true,
                sortOrder: new[] { SortOrder.Ascending },
                parentId: library.Id,
                includeItemTypes: new[] { GetViewType(library.CollectionType) },
                sortBy: new[] { "SortName" },
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Banner, ImageType.Thumb },
                limit: limit,
                startIndex: startIndex)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<BaseItemDto>> GetNextUpAsync(IEnumerable<Guid> libraryIds)
    {
        var userId = _stateService.GetUserId();
        var items = new List<BaseItemDto>();
        foreach (var library in libraryIds)
        {
            var result = await _tvShowsClient.GetNextUpAsync(
                    userId,
                    limit: 24,
                    fields: new[] { ItemFields.PrimaryImageAspectRatio },
                    imageTypeLimit: 1,
                    enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb },
                    parentId: library)
                .ConfigureAwait(false);
            items.AddRange(result.Items);
        }

        return items;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<BaseItemDto>> GetContinueWatchingAsync()
    {
        var userId = _stateService.GetUserId();
        var result = await _itemsClient.GetResumeItemsAsync(
                userId,
                limit: 24,
                fields: new[] { ItemFields.PrimaryImageAspectRatio },
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb },
                enableTotalRecordCount: false,
                mediaTypes: new[] { "Video" })
            .ConfigureAwait(false);

        return result.Items;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<BaseItemDto>> GetRecentlyAddedAsync(Guid libraryId, CancellationToken cancellationToken = default)
    {
        var userId = _stateService.GetUserId();
        return await _userLibraryClient.GetLatestMediaAsync(
                userId,
                limit: 24,
                fields: new[] { ItemFields.PrimaryImageAspectRatio },
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb },
                parentId: libraryId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BaseItemDtoQueryResult> GetSeasonsAsync(Guid seriesId)
    {
        var userId = _stateService.GetUserId();
        return await _tvShowsClient.GetSeasonsAsync(
                seriesId,
                userId,
                new[] { ItemFields.PrimaryImageAspectRatio },
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb })
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BaseItemDtoQueryResult> GetEpisodesAsync(Guid seriesId, Guid seasonId)
    {
        var userId = _stateService.GetUserId();
        return await _tvShowsClient.GetEpisodesAsync(
                seriesId,
                userId,
                new[] { ItemFields.PrimaryImageAspectRatio },
                seasonId: seasonId,
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb })
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BaseItemDtoQueryResult> GetNextUpAsync(Guid seriesId)
    {
        var userId = _stateService.GetUserId();
        return await _tvShowsClient.GetNextUpAsync(
                userId,
                parentId: seriesId,
                fields: new[] { ItemFields.PrimaryImageAspectRatio },
                imageTypeLimit: 1,
                enableImageTypes: new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Thumb })
            .ConfigureAwait(false);
    }

    private static BaseItemKind GetViewType(string collectionType)
    {
        return collectionType switch
        {
            "tvshows" => BaseItemKind.Series,
            "movies" => BaseItemKind.Movie,
            "books" => BaseItemKind.Book,
            "music" => BaseItemKind.Audio,
            _ => BaseItemKind.Folder
        };
    }
}
