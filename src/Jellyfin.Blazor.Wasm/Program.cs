using Jellyfin.Blazor.Shared;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSharedServices();

var host = builder.Build();
var clientSettings = host.Services.GetRequiredService<SdkClientSettings>();
clientSettings.InitializeClientSettings(
    "Jellyfin Blazor",
    "0.0.1",
    "Desktop",
    Guid.NewGuid().ToString("N"));

await host.RunAsync().ConfigureAwait(false);
