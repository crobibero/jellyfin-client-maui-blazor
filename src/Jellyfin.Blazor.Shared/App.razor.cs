using Blazorise;
using Jellyfin.Blazor.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Jellyfin.Blazor.Shared;

/// <summary>
/// App.
/// </summary>
public partial class App
{
    private const string Dark = "#4a4a4a";
    private const string White = "#f1f1f1";
    private const string Primary = "#AA5CC3";
    private const string Secondary = "#00A4DC";

    private static readonly Theme _theme =
        new()
        {
            Enabled = true,
            White = White,
            Black = Dark,
            BodyOptions = new ThemeBodyOptions
            {
                TextColor = White,
                BackgroundColor = Dark
            },
            ColorOptions = new ThemeColorOptions
            {
                Dark = Dark,
                Light = White,
                Primary = Primary,
                Secondary = Secondary
            },
            BackgroundOptions = new ThemeBackgroundOptions
            {
                Dark = Dark,
                Light = White,
                Primary = Primary,
                Secondary = Secondary
            },
            TextColorOptions = new ThemeTextColorOptions
            {
                Dark = Dark,
                Light = White,
                Primary = Primary,
                Secondary = Secondary
            },
            SpinKitOptions = new ThemeSpinKitOptions
            {
                Color = White
            }
        };

    [Inject]
    private IAdditionalAssemblyService AdditionalAssemblyService { get; set; } = null!;
}
