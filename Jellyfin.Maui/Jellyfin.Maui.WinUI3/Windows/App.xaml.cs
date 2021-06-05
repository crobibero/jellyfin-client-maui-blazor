using Jellyfin.Sdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.UI.Xaml;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Text;
using Windows.ApplicationModel;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Jellyfin.Maui.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MiddleApp
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            var storageFolder = ApplicationData.Current.LocalFolder;

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] [{ThreadId}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
                .WriteTo.Async(x => x.File(
                    Path.Join(storageFolder.Path, "log", "jellyfin.winui..log"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{ThreadId}] {SourceContext}: {Message}{NewLine}{Exception}",
                    encoding: Encoding.UTF8))
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .CreateLogger();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            Microsoft.Maui.Essentials.Platform.OnLaunched(args);

            this.Services.GetRequiredService<SdkClientSettings>()
                .InitializeClientSettings(
                "Jellyfin Maui WinUI",
                "0.0.1",
                "Desktop",
                Guid.NewGuid().ToString("N"));

        }
    }
    public class MiddleApp : MauiWinUIApplication<Startup>
    {
    }
}
