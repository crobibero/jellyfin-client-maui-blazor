using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Jellyfin.Maui.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Maui.Components
{
    /// <summary>
    /// The primary card component.
    /// </summary>
    public partial class PrimaryCard : ComponentBase
    {
        private string? _imageUrl;
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

            if (Item.ParentIndexNumber is not null && Item.IndexNumber is not null)
            {
                _subTitle = $"S{Item.ParentIndexNumber} E{Item.IndexNumber}";
            }

            return base.OnInitializedAsync();
        }
    }
}