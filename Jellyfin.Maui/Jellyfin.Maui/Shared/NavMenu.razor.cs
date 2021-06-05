using System;
using System.Threading.Tasks;
using Jellyfin.Maui.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Maui.Shared
{
    public partial class NavMenu : ComponentBase/*, IAsyncDisposable*/
    {
        private const string LinkCssClass = "hover:bg-indigo-600 group flex items-center px-2 py-2 text-sm font-medium rounded-md cursor-pointer";
        private const string IconCssClass = "mr-3 flex-shrink-0 h-6 w-6";

        private const string MenuOverlayShowCss = "transition-opacity ease-linear duration-300 opacity-100";
        private const string MenuOverlayHideCss = "transition-opacity ease-linear duration-300 opacity-0";
        private const string MenuShowCss = "transition ease-in-out duration-300 transform translate-x-0";
        private const string MenuHideCss = "transition ease-in-out duration-300 transform -translate-x-full";
        private const string MenuCloseShowCss = "ease-in-out duration-300 opacity-100";
        private const string MenuCloseHideCss = "ease-in-out duration-300 opacity-0";

        private string _currentRoute = string.Empty;
        private BaseItemDtoQueryResult? _views;
        private bool _showSidebar;
        private bool _sidebarHidden;

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

                _sidebarHidden = false;
                _showSidebar = value;
                ShowSidebarChanged?.InvokeAsync(value);
            }
        }

        /// <summary>
        /// Gets or sets the event callback when the sidebar shown event changes.
        /// </summary>
        [Parameter]
        public EventCallback<bool>? ShowSidebarChanged { get; set; }

        [Inject]
        private IUserViewsClient UserViewsClient { get; set; } = null!;

        [Inject]
        private IStateService StateService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            _views = await UserViewsClient.GetUserViewsAsync(StateService.GetUserId())
                .ConfigureAwait(false);
            await base.OnInitializedAsync()
                .ConfigureAwait(false);
        }

        private void NavigateToDashboard()
        {
            _currentRoute = string.Empty;
            NavigationManager.NavigateTo(_currentRoute);
        }

        private void NavigateToView(Guid libraryId)
        {
            _currentRoute = $"/view/{libraryId}";
            NavigationManager.NavigateTo(_currentRoute);
        }

        private void NavigateToLogout()
        {
            NavigationManager.NavigateTo("/logout");
        }

        private void ToggleMobileSidebar()
        {
            ShowSidebar = !ShowSidebar;
        }

        /*
        private void OnSidebarTransitionEnd(TransitionEventArgs e)
        {
            _sidebarHidden = !ShowSidebar;
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        /// <returns>The <see cref="ValueTask"/>.</returns>
        public async ValueTask DisposeAsync()
        {
            await TransitionEventsService.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
        */
    }
}
