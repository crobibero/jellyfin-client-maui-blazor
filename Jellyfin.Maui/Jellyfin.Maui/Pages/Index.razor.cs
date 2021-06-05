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
    /// The dashboard page.
    /// </summary>
    public partial class Index : ComponentBase
    {
        private IReadOnlyList<BaseItemDto>? _continueWatching;
        private IReadOnlyList<BaseItemDto>? _nextUp;
        private (BaseItemDto, IReadOnlyList<BaseItemDto>)[]? _libraries;

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
                    async completed =>
                    {
                        if (completed.IsCompleted)
                        {
                            _continueWatching = await completed;
                            await InvokeAsync(() =>
                            {
                                StateHasChanged();
                            });
                            
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
            var libraryTasks = new Task<IReadOnlyList<BaseItemDto>>[libraries.Count];
            var libraryIds = new Guid[libraries.Count];
            for (var i = 0; i < libraries.Count; i++)
            {
                libraryTasks[i] = LibraryService.GetRecentlyAdded(libraries[i].Id);
                libraryIds[i] = libraries[i].Id;
            }

            var nextUpTask = LibraryService.GetNextUp(libraryIds)
                .ContinueWith(nextUp => _nextUp = nextUp.Result, TaskScheduler.Default);
            await Task.WhenAll(libraryTasks)
                .ConfigureAwait(false);

            _libraries = new (BaseItemDto, IReadOnlyList<BaseItemDto>)[libraries.Count];
            for (var i = 0; i < libraryTasks.Length; i++)
            {
                var libraryItems = await libraryTasks[i].ConfigureAwait(false);
                _libraries[i] = (libraries[i], libraryItems);
            }

            await nextUpTask.ConfigureAwait(false);
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}