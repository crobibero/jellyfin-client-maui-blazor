using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jellyfin.Maui.Shared
{
    public partial class TopNavMenu : ComponentBase
    {
        private bool _showSidebar;

        /// <summary>
        /// Gets or sets a value indicating whether the sidebar should be shown.
        /// </summary>
        [Parameter]
        public bool ShowSidebar
        {
            get => _showSidebar;
            set
            {
                if (_showSidebar == value)
                {
                    return;
                }

                _showSidebar = value;
                ShowSidebarChanged?.InvokeAsync(value);
            }
        }

        /// <summary>
        /// Gets or sets the event callback when the sidebar shown event changes.
        /// </summary>
        [Parameter]
        public EventCallback<bool>? ShowSidebarChanged { get; set; }

        private void ToggleMobileSidebar()
        {
            ShowSidebar = !ShowSidebar;
        }
    }
}
