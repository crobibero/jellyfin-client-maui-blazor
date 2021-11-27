using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared.Components;

/// <summary>
/// The primary card component.
/// </summary>
public partial class PrimaryCardComponent
{
    private string? _imageUrl;
    private string? _title;
    private string? _subTitle;

    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    [Parameter]
    [Required]
    public BaseItemDto Item { get; set; } = null!;

    [Inject]
    private IStateService StateService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        var host = StateService.GetHost();
        // Get parent instead of self if it exists.
        var imageItemId = Item.SeriesId ?? Item.Id;

        _imageUrl = $"{host}/Items/{imageItemId}/Images/{ImageType.Primary}?quality=90&maxWidth=960";

        switch (Item.Type)
        {
            case BaseItemKind.Episode:
                _title = Item.SeriesName;
                _subTitle = $"S{Item.ParentIndexNumber} E{Item.IndexNumber} {Item.Name}";
                break;
            case BaseItemKind.Season:
                _title = Item.SeriesName;
                _subTitle = Item.SeasonName;
                break;
            default:
                _title = Item.Name;
                _subTitle = Item.ProductionYear?.ToString(CultureInfo.InvariantCulture);
                break;
        }

        InvokeAsync(() => StateHasChanged());
        return base.OnInitializedAsync();
    }

    private void ViewItem()
    {
        switch (Item.Type)
        {
            case BaseItemKind.Movie:
                NavigationManager.NavigateTo($"/movie/{Item.Id}");
                break;
            case BaseItemKind.Episode:
                NavigationManager.NavigateTo($"/episode/{Item.Id}");
                break;
            case BaseItemKind.Season:
                NavigationManager.NavigateTo($"/season/{Item.Id}");
                break;
            case BaseItemKind.Series:
                NavigationManager.NavigateTo($"/series/{Item.Id}");
                break;
            default:
                NavigationManager.NavigateTo($"/item/{Item.Id}");
                break;
        }
    }
}
