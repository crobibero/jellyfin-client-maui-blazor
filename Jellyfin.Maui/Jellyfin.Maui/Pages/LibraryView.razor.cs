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
    /// The library view.
    /// </summary>
    public partial class LibraryView : ComponentBase
    {
        private BaseItemDto? _library;

        private IReadOnlyList<BaseItemDto> _items = Array.Empty<BaseItemDto>();

        private bool _loading = true;
        private int _pageIndex = 0;
        private int _pageSize = 100;

        /// <summary>
        /// Gets or sets the library id.
        /// </summary>
        [Parameter]
        public Guid LibraryId { get; set; }

        [Inject]
        private ILibraryService LibraryService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private ILogger<LibraryView> Logger { get; set; } = null!;

        /// <inheritdoc />
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                _loading = true;
                _library = null;
                _library = await LibraryService.GetLibrary(LibraryId)
                    .ConfigureAwait(false);
                if (_library is null)
                {
                    // Library is null, redirect
                    NavigationManager.NavigateTo(string.Empty);
                }

                await base.OnParametersSetAsync()
                    .ConfigureAwait(false);

                await GetItems()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _library = null;
                Logger.LogError(ex, "Error getting library or items");
            }
            finally
            {
                if (_library is null)
                {
                    // Library is null, redirect to dashboard.
                    NavigationManager.NavigateTo(string.Empty);
                }
                else
                {
                    await InvokeAsync(() =>
                    {
                        StateHasChanged();
                    })
                        .ConfigureAwait(false);
                }
            }
        }

        private async Task GetItems()
        {
            if (_library is null)
            {
                return;
            }

            var queryResult = await LibraryService.GetLibraryItems(
                    _library,
                    _pageSize,
                    _pageIndex * _pageSize)
                .ConfigureAwait(false);
            _items = queryResult.Items;
            _loading = false;
        }
    }
}