using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.Authorization;
using Jellyfin.Maui.Services;
using Serilog;
using System.Net.Http;
using System;
using System.Net;
using System.Text;
using Microsoft.Maui.Controls.Hosting;

namespace Jellyfin.Maui
{
	public class Startup : IStartup
	{
		public void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder
				.UseFormsCompatibility()
				.RegisterBlazorMauiWebView(typeof(Startup).Assembly)
				.UseMicrosoftExtensionsServiceProviderFactory()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				})
				.ConfigureServices(services =>
				{
                    static HttpMessageHandler DefaultHttpClientHandlerDelegate(IServiceProvider serviceProvider)
                    {
                        return new SocketsHttpHandler
                        {
                            AutomaticDecompression = DecompressionMethods.All,
                            RequestHeaderEncodingSelector = (_, _) => Encoding.UTF8
                        };
                    }

                    services.AddBlazorWebView();
                    services.AddLogging(builder =>
                    {
                        builder.AddSerilog(dispose: true);
                    });

					services.AddOptions();
					services.AddAuthorizationCore();
					services.AddScoped<JellyfinAuthStateProvider, JellyfinAuthStateProvider>();
					services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<JellyfinAuthStateProvider>());

                    // Register 3rd party services
                    // services.AddCssEvents();

                    // Register services
                    services.AddSingleton<IStateService, StateService>();
                    services.AddScoped<IAuthenticationService, AuthenticationService>();
                    services.AddScoped<ILibraryService, LibraryService>();

                    // Register sdk services
                    services.AddSingleton<SdkClientSettings>();

                    services.AddHttpClient<IActivityLogClient, ActivityLogClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IApiKeyClient, ApiKeyClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IArtistsClient, ArtistsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IAudioClient, AudioClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IBrandingClient, BrandingClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IChannelsClient, ChannelsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ICollectionClient, CollectionClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IConfigurationClient, ConfigurationClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDashboardClient, DashboardClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDevicesClient, DevicesClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDisplayPreferencesClient, DisplayPreferencesClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDlnaClient, DlnaClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDlnaServerClient, DlnaServerClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IDynamicHlsClient, DynamicHlsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IEnvironmentClient, EnvironmentClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IFilterClient, FilterClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IGenresClient, GenresClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IHlsSegmentClient, HlsSegmentClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IImageClient, ImageClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IImageByNameClient, ImageByNameClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IInstantMixClient, InstantMixClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IItemLookupClient, ItemLookupClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IItemRefreshClient, ItemRefreshClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IItemsClient, ItemsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ILibraryClient, LibraryClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IItemUpdateClient, ItemUpdateClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ILibraryStructureClient, LibraryStructureClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ILiveTvClient, LiveTvClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ILocalizationClient, LocalizationClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IMediaInfoClient, MediaInfoClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IMoviesClient, MoviesClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IMusicGenresClient, MusicGenresClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<INotificationsClient, NotificationsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IPackageClient, PackageClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IPersonsClient, PersonsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IPlaylistsClient, PlaylistsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IPlaystateClient, PlaystateClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IPluginsClient, PluginsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IQuickConnectClient, QuickConnectClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IRemoteImageClient, RemoteImageClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IScheduledTasksClient, ScheduledTasksClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISearchClient, SearchClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISessionClient, SessionClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IStartupClient, StartupClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IStudiosClient, StudiosClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISubtitleClient, SubtitleClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISuggestionsClient, SuggestionsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISyncPlayClient, SyncPlayClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ISystemClient, SystemClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ITimeSyncClient, TimeSyncClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ITrailersClient, TrailersClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<ITvShowsClient, TvShowsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IUniversalAudioClient, UniversalAudioClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IUserClient, UserClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IUserLibraryClient, UserLibraryClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IUserViewsClient, UserViewsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IVideoAttachmentsClient, VideoAttachmentsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IVideoHlsClient, VideoHlsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IVideosClient, VideosClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                    services.AddHttpClient<IYearsClient, YearsClient>()
                        .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate);
                });

		}
	}
}