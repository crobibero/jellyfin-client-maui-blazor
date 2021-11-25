using UIKit;

namespace Jellyfin.Blazor.Maui;

/// <summary>
/// The iOS program.
/// </summary>
public class Program
{
    // This is the main entry point of the application.
    private static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
