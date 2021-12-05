using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
///     The dashboard page.
/// </summary>
public partial class IndexPage
{
    private readonly object _libraryLock = new();
    private IReadOnlyList<BaseItemDto>? _continueWatching;
    private (BaseItemDto, IReadOnlyList<BaseItemDto>?)[]? _libraries;
    private IReadOnlyList<BaseItemDto>? _nextUp;

    [Inject]
    private ILibraryService LibraryService { get; set; } = null!;

    [Inject]
    private ILogger<IndexPage> Logger { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        InitializeContinueWatchingAsync().SafeFireAndForget();
        InitializeLibrariesAsync().SafeFireAndForget();
        base.OnInitialized();
    }

    private async Task InitializeContinueWatchingAsync()
    {
        var continueWatching = await LibraryService.GetContinueWatchingAsync()
            .ConfigureAwait(false);
        _continueWatching = continueWatching;
        await InvokeAsync(() => StateHasChanged())
            .ConfigureAwait(false);
    }

    private async Task InitializeLibrariesAsync()
    {
        var libraries = await LibraryService.GetLibrariesAsync().ConfigureAwait(false);

        var libraryIds = new Guid[libraries.Count];
        lock (_libraryLock)
        {
            _libraries = new (BaseItemDto, IReadOnlyList<BaseItemDto>?)[libraries.Count];
            for (var i = 0; i < libraries.Count; i++)
            {
                libraryIds[i] = libraries[i].Id;
                _libraries[i] = (libraries[i], null);
            }
        }

        await InvokeAsync(() => StateHasChanged())
            .ConfigureAwait(false);

        Parallel.ForEachAsync(libraries, async (library, cancellationToken) =>
            {
                var index = Array.IndexOf(libraryIds, library.Id);
                var items = await LibraryService.GetRecentlyAddedAsync(library.Id, cancellationToken)
                    .ConfigureAwait(false);
                lock (_libraryLock)
                {
                    _libraries[index] = (library, items);
                }

                await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
            })
            .SafeFireAndForget();

        var nextUp = await LibraryService.GetNextUpAsync(libraryIds)
            .ConfigureAwait(false);
        _nextUp = nextUp;
        await InvokeAsync(() => StateHasChanged())
            .ConfigureAwait(false);
    }
}
