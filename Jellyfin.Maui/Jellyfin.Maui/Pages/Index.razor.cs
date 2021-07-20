using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Maui.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Maui.Pages
{
    /// <summary>
    ///     The dashboard page.
    /// </summary>
    public partial class Index : ComponentBase
    {
        private readonly object _libraryLock = new();
        private IReadOnlyList<BaseItemDto>? _continueWatching;
        private (BaseItemDto, IReadOnlyList<BaseItemDto>?)[]? _libraries;
        private IReadOnlyList<BaseItemDto>? _nextUp;

        [Inject]
        private ILibraryService LibraryService { get; set; } = null!;

        [Inject]
        private ILogger<Index> Logger { get; set; } = null!;

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            var initializeDashboardTask = InitializeDashboard();
            await Task.WhenAll(
                    initializeDashboardTask,
                    base.OnInitializedAsync())
                .ConfigureAwait(false);

            Logger.LogDebug("Init complete");
        }

        private async Task InitializeDashboard()
        {
            var continueWatchingTask = LibraryService.GetContinueWatching()
                .ContinueWith(
                    completed =>
                    {
                        if (completed.IsCompleted)
                        {
                            _continueWatching = completed.Result;
                            StateHasChanged();
                        }
                    }, TaskScheduler.Default);
            var librariesTask = LibraryService.GetLibraries()
                .ContinueWith(completed => InitializeLibraries(completed.Result), TaskScheduler.Default);
            await Task.WhenAll(
                    continueWatchingTask,
                    librariesTask)
                .ConfigureAwait(false);
        }

        private async Task InitializeLibraries(IReadOnlyList<BaseItemDto> libraries)
        {
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

            StateHasChanged();
            var libraryTask = Parallel.ForEachAsync(libraries, async (library, cancellationToken) =>
            {
                var index = Array.IndexOf(libraryIds, library.Id);
                var items = await LibraryService.GetRecentlyAdded(library.Id, cancellationToken)
                    .ConfigureAwait(false);
                lock (_libraryLock)
                {
                    _libraries[index] = (library, items);
                    StateHasChanged();
                }
            });

            var nextUpTask = LibraryService.GetNextUp(libraryIds)
                .ContinueWith(
                    nextUp =>
                    {
                        if (nextUp.IsCompleted)
                        {
                            _nextUp = nextUp.Result;
                            StateHasChanged();
                        }
                    }, TaskScheduler.Default);

            await nextUpTask.ConfigureAwait(false);
            await libraryTask.ConfigureAwait(false);
        }
    }
}