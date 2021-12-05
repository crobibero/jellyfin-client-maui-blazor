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
public partial class SeasonPage
{
    /// <summary>
    /// Gets the series id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid SeriesId { get; init; }

    /// <summary>
    /// Gets the season id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid SeasonId { get; init; }

    [Inject]
    private ILibraryService LibraryService { get; init; } = null!;

    private BaseItemDto? Season { get; set; }

    private IReadOnlyList<BaseItemDto> Episodes { get; set; } = Array.Empty<BaseItemDto>();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        GetSeasonAsync().SafeFireAndForget();
        GetEpisodesAsync().SafeFireAndForget();
    }

    private async Task GetSeasonAsync()
    {
        Season = await LibraryService.GetItemAsync(SeasonId)
            .ConfigureAwait(false);
        await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }

    private async Task GetEpisodesAsync()
    {
        var episodes = await LibraryService.GetEpisodesAsync(SeriesId, SeasonId)
            .ConfigureAwait(false);
        Episodes = episodes.Items;
        await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }
}
