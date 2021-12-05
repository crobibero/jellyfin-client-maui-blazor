using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jellyfin.Blazor.Shared.Services;

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
