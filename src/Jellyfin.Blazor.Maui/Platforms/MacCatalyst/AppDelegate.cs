#pragma warning disable CA1711

using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Jellyfin.Blazor.Maui;

/// <summary>
/// The MacCatalyst App Delegate.
/// </summary>
[Register("AppDelegate")]

public class AppDelegate : MauiUIApplicationDelegate
{
    /// <inheritdoc />
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
