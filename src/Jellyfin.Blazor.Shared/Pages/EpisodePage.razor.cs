using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Pages;

/// <summary>
/// The movie page.
/// </summary>
public partial class EpisodePage
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

    /// <summary>
    /// Gets the episode id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid EpisodeId { get; init; }

    [Inject]
    private ILibraryService LibraryService { get; init; } = null!;

    private BaseItemDto? Episode { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Episode = await LibraryService.GetItemAsync(EpisodeId)
            .ConfigureAwait(false);
    }
}
