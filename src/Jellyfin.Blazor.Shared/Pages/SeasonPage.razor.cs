using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
    /// Gets the item id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid ItemId { get; init; }

    [Inject]
    private ILibraryService LibraryService { get; init; } = null!;

    private BaseItemDto? Season { get; set; }

    private IReadOnlyList<BaseItemDto> Episodes { get; set; } = Array.Empty<BaseItemDto>();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Season = await LibraryService.GetItemAsync(ItemId)
            .ConfigureAwait(false);
    }
}
