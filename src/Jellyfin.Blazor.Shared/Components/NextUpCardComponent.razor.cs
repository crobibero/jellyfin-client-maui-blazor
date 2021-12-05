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
public partial class NextUpCardComponent
{
    private string? _imageUrl;
    private string? _title;

    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    [Parameter]
    [Required]
    public BaseItemDto Item { get; set; } = null!;

    [Inject]
    private IStateService StateService { get; set; } = null!;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        var host = StateService.GetHost();

        _imageUrl = $"{host}/Items/{Item.Id}/Images/{ImageType.Primary}";
        _title = $"S{Item.ParentIndexNumber}E{Item.IndexNumber} - {Item.Name}";

        return base.OnInitializedAsync();
    }

    private void ViewItem()
    {
        NavigationService.NavigateToItem(Item);
    }
}
