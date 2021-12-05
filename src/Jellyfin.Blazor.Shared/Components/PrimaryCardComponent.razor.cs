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

    /// <summary>
    /// Gets or sets a value indicating whether to display the parent name as the title.
    /// </summary>
    [Parameter]
    public bool ShowParentName { get; set; } = true;

    [Inject]
    private IStateService StateService { get; set; } = null!;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        var host = StateService.GetHost();

        var imageItemId = Item.Id;
        if (Item.Type == BaseItemKind.Episode)
        {
            // Get parent instead of self if it exists.
            imageItemId = Item.SeriesId ?? Item.SeasonId ?? Item.Id;
        }

        _imageUrl = $"{host}/Items/{imageItemId}/Images/{ImageType.Primary}";

        switch (Item.Type)
        {
            case BaseItemKind.Episode:
                _title = Item.SeriesName;
                _subTitle = $"S{Item.ParentIndexNumber} E{Item.IndexNumber} {Item.Name}";
                break;
            case BaseItemKind.Season:
                _title = Item.SeriesName;
                _subTitle = string.IsNullOrEmpty(Item.SeasonName) ? $"Season {Item.IndexNumber:D2}" : Item.SeasonName;
                break;
            default:
                _title = Item.Name;
                _subTitle = Item.ProductionYear?.ToString(CultureInfo.InvariantCulture);
                break;
        }

        return base.OnInitializedAsync();
    }

    private void ViewItem()
    {
        NavigationService.NavigateToItem(Item);
    }
}
