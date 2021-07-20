using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Jellyfin.Maui.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Maui.Components
{
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

        /// <inheritdoc />
        protected override Task OnInitializedAsync()
        {
            var host = StateService.GetHost();
            // Get parent instead of self if it exists.
            var imageItemId = Item.SeriesId ?? Item.Id;

            _imageUrl = $"{host}/Items/{imageItemId}/Images/{ImageType.Primary}?quality=90&maxWidth=960";

            if (string.Equals(Item.Type, "episode", StringComparison.OrdinalIgnoreCase))
            {
                _title = Item.SeriesName;
                _subTitle = $"S{Item.ParentIndexNumber} E{Item.IndexNumber} {Item.Name}";
            }
            else if (string.Equals(Item.Type, "season", StringComparison.OrdinalIgnoreCase))
            {
                _title = Item.SeriesName;
                _subTitle = Item.SeasonName;
            }
            else
            {
                _title = Item.Name;
                _subTitle = Item.ProductionYear?.ToString(CultureInfo.InvariantCulture);
            }

            StateHasChanged();
            return base.OnInitializedAsync();
        }
    }
}