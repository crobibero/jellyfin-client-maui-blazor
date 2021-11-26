using Blazored.LocalStorage;
using Jellyfin.Blazor.Shared;
using Jellyfin.Blazor.Shared.Services;
using Jellyfin.Blazor.Wasm.Services;
using Jellyfin.Sdk;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSharedServices();
builder.Services.AddScoped<IVideoNavigationService, VideoNavigationService>();
builder.Services.AddSingleton<IAdditionalAssemblyService, AdditionalAssemblyService>();
builder.Services.AddScoped<IStateStorageService, StateStorageService>();
builder.Services.AddBlazoredLocalStorage();

var host = builder.Build();

host.Services.GetRequiredService<SdkClientSettings>()
    .InitializeClientSettings(
        "Jellyfin Blazor",
        "0.0.1",
        "Wasm",
        Guid.NewGuid().ToString("N"));

host.Services.GetRequiredService<IAdditionalAssemblyService>()
    .SetAssemblies(new[] { typeof(Program).Assembly });

await host.RunAsync().ConfigureAwait(false);
