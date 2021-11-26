using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// Service Extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds the required services.
    /// </summary>
    /// <param name="serviceCollection">Instance of the <see cref="IServiceCollection" />.</param>
    public static void AddSharedServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions();
        serviceCollection.AddAuthorizationCore();
        serviceCollection.AddScoped<JellyfinAuthStateProvider, JellyfinAuthStateProvider>();
        serviceCollection.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<JellyfinAuthStateProvider>());

        serviceCollection.AddI18nText();

        // Register services
        serviceCollection.AddSingleton<IStateService, StateService>();
        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<ILibraryService, LibraryService>();

        // Register sdk services
        serviceCollection.AddSingleton<SdkClientSettings>();

        static HttpMessageHandler DefaultHttpClientHandlerDelegate(IServiceProvider serviceProvider)
        {
            if (SocketsHttpHandler.IsSupported)
            {
                return new SocketsHttpHandler
                {
                    AutomaticDecompression = DecompressionMethods.All,
                    RequestHeaderEncodingSelector = (_, _) => Encoding.UTF8
                };
            }

            return new HttpClientHandler();
        }

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        // TODO remove unused services.
        serviceCollection
            .AddHttpClient<IActivityLogClient, ActivityLogClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IApiKeyClient, ApiKeyClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IArtistsClient, ArtistsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IAudioClient, AudioClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IBrandingClient, BrandingClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IChannelsClient, ChannelsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ICollectionClient, CollectionClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IConfigurationClient, ConfigurationClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDashboardClient, DashboardClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDevicesClient, DevicesClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDisplayPreferencesClient, DisplayPreferencesClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDlnaClient, DlnaClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDlnaServerClient, DlnaServerClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IDynamicHlsClient, DynamicHlsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IEnvironmentClient, EnvironmentClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IFilterClient, FilterClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IGenresClient, GenresClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IHlsSegmentClient, HlsSegmentClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IImageClient, ImageClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IImageByNameClient, ImageByNameClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IInstantMixClient, InstantMixClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IItemLookupClient, ItemLookupClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IItemRefreshClient, ItemRefreshClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IItemsClient, ItemsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ILibraryClient, LibraryClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IItemUpdateClient, ItemUpdateClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ILibraryStructureClient, LibraryStructureClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ILiveTvClient, LiveTvClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ILocalizationClient, LocalizationClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IMediaInfoClient, MediaInfoClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IMoviesClient, MoviesClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IMusicGenresClient, MusicGenresClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<INotificationsClient, NotificationsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IPackageClient, PackageClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IPersonsClient, PersonsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IPlaylistsClient, PlaylistsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IPlaystateClient, PlaystateClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IPluginsClient, PluginsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IQuickConnectClient, QuickConnectClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IRemoteImageClient, RemoteImageClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IScheduledTasksClient, ScheduledTasksClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISearchClient, SearchClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISessionClient, SessionClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IStartupClient, StartupClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IStudiosClient, StudiosClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISubtitleClient, SubtitleClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISuggestionsClient, SuggestionsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISyncPlayClient, SyncPlayClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ISystemClient, SystemClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ITimeSyncClient, TimeSyncClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ITrailersClient, TrailersClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<ITvShowsClient, TvShowsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IUniversalAudioClient, UniversalAudioClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IUserClient, UserClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IUserLibraryClient, UserLibraryClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IUserViewsClient, UserViewsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IVideoAttachmentsClient, VideoAttachmentsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IVideoHlsClient, VideoHlsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IVideosClient, VideosClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);

        serviceCollection
            .AddHttpClient<IYearsClient, YearsClient>()
            .ConfigurePrimaryHttpMessageHandler(DefaultHttpClientHandlerDelegate)
            .AddPolicyHandler(retryPolicy);
    }
}
