using System;
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
public partial class PosterComponent
{
    private string? _imageUrl;

    /// <summary>
    /// Gets or sets the item id.
    /// </summary>
    [Parameter]
    [Required]
    public Guid ItemId { get; set; }

    [Inject]
    private IStateService StateService { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        var host = StateService.GetHost();
        _imageUrl = $"{host}/Items/{ItemId}/Images/{ImageType.Primary}";
        base.OnInitialized();
    }
}
