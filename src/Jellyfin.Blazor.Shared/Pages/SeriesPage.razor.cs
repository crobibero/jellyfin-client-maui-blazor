using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The movie page.
/// </summary>
public partial class SeriesPage
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid ItemId { get; init; }

    [Inject]
    private ILibraryService LibraryService { get; init; } = null!;

    private BaseItemDto? Series { get; set; }

    private BaseItemDto? NextUp { get; set; }

    private IReadOnlyList<BaseItemDto> Seasons { get; set; } = Array.Empty<BaseItemDto>();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Series = await LibraryService.GetItemAsync(ItemId)
            .ConfigureAwait(false);

        LibraryService.GetNextUpAsync(ItemId)
            .ContinueWith(
                task =>
                {
                    if (task.IsCompletedSuccessfully
                        && task.Result.TotalRecordCount == 1)
                    {
                        NextUp = task.Result.Items[0];
                        InvokeAsync(() => StateHasChanged());
                    }
                }, TaskScheduler.Default)
            .SafeFireAndForget();

        LibraryService.GetSeasonsAsync(ItemId)
            .ContinueWith(
                task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        Seasons = task.Result.Items;
                        InvokeAsync(() => StateHasChanged());
                    }
                }, TaskScheduler.Default)
            .SafeFireAndForget();
    }
}
