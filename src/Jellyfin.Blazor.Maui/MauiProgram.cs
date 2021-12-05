using System;
using Jellyfin.Blazor.Maui.Services;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Jellyfin.Blazor.Maui;

/// <summary>
/// The main maui program.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates the Maui app.
    /// </summary>
    /// <returns>The created Maui app.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddBlazorWebView();
        builder.Services.AddSharedServices();

        builder.Services.AddScoped<IVideoNavigationService, VideoNavigationService>();
        builder.Services.AddScoped<IStateStorageService, StateStorageService>();

        var app = builder.Build();

        app.Services.GetRequiredService<SdkClientSettings>()
            .InitializeClientSettings(
                "Jellyfin Maui",
                "0.0.1",
                "Maui",
                Guid.NewGuid().ToString("N"));

        app.Services.GetRequiredService<IAdditionalAssemblyService>()
            .SetAssemblies(new[] { typeof(MauiProgram).Assembly });

        return app;
    }
}
