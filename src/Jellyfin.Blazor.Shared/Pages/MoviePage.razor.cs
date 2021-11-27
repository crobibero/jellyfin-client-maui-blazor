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
public partial class MoviePage
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid ItemId { get; init; }

    [Inject]
    private ILibraryService LibraryService { get; init; } = null!;

    private BaseItemDto? Movie { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Movie = await LibraryService.GetItemAsync(ItemId)
            .ConfigureAwait(false);
    }
}
