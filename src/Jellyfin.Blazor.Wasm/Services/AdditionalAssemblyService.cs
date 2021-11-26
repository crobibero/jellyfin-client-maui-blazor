using System.Reflection;
using Jellyfin.Blazor.Shared.Services;

namespace Jellyfin.Blazor.Wasm.Services;

/// <inheritdoc />
public class AdditionalAssemblyService : IAdditionalAssemblyService
{
    private Assembly[]? _assemblies;

    /// <inheritdoc />
    public void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return _assemblies ?? Array.Empty<Assembly>();
    }
}
