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
    protected override void OnInitialized()
    {
        GetSeriesAsync().SafeFireAndForget();
        GetNextUpAsync().SafeFireAndForget();
        GetSeasonsAsync().SafeFireAndForget();
    }

    private async Task GetSeriesAsync()
    {
        Series = await LibraryService.GetItemAsync(ItemId)
            .ConfigureAwait(false);
        await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }

    private async Task GetNextUpAsync()
    {
        var nextUp = await LibraryService.GetNextUpAsync(ItemId)
            .ConfigureAwait(false);
        if (nextUp.Items.Count > 0)
        {
            NextUp = nextUp.Items[0];
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
        }
    }

    private async Task GetSeasonsAsync()
    {
        var seasons = await LibraryService.GetSeasonsAsync(ItemId)
            .ConfigureAwait(false);
        Seasons = seasons.Items;
        await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }
}
