using System.Collections.Generic;
using System.Reflection;

namespace Jellyfin.Blazor.Shared.Services;

/// <summary>
/// Service to provide additional assemblies to the blazor router.
/// </summary>
public interface IAdditionalAssemblyService
{
    /// <summary>
    /// Sets the list of additional assemblies.
    /// </summary>
    /// <param name="assemblies">The list of additional assemblies.</param>
    void SetAssemblies(IEnumerable<Assembly> assemblies);

    /// <summary>
    /// Gets the additional assemblies.
    /// </summary>
    /// <returns>The collection of assemblies.</returns>
    IReadOnlyCollection<Assembly> GetAssemblies();
}
