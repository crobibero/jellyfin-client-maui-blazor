using Microsoft.AspNetCore.Components;

namespace Jellyfin.Maui.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to display the sidebar.
        /// </summary>
        /// <remarks>
        /// This is data-bound to the <see cref="NavMenu"/> and <see cref="TopNavMenu"/>.
        /// </remarks>
        private bool ShowSidebar { get; set; }
    }
}
