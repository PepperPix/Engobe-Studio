namespace Engobe.Studio.Services;

using DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Manages a Engobe engine provider for a specific project path.
/// </summary>
public sealed class EngineHost
{
    private readonly Action<IServiceCollection> _registerEngobeServices;

    public EngineHost()
    {
        _registerEngobeServices = services => services.AddEngobe();
    }

    /// <summary>
    /// Creates a service provider with the Engobe engine registered for the given project path.
    /// </summary>
    public ServiceProvider CreateProvider(string projectPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);

        var services = new ServiceCollection();
        _registerEngobeServices(services);
        return services.BuildServiceProvider();
    }
}
