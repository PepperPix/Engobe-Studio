using Engobe.Services;
using Engobe.Studio.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Engobe.Studio.Tests;

public class EngineHostTests
{
    [Test]
    public async Task CreateProvider_ResolvesEngobeCoreServices()
    {
        var host = new EngineHost();
        using var provider = host.CreateProvider("/tmp/test-project");

        await Assert.That(provider.GetRequiredService<ISiteConfigLoader>()).IsNotNull();
        await Assert.That(provider.GetRequiredService<IContentReader>()).IsNotNull();
        await Assert.That(provider.GetRequiredService<ISiteBuilder>()).IsNotNull();
        await Assert.That(provider.GetRequiredService<IScaffolder>()).IsNotNull();
        await Assert.That(typeof(ISiteConfigLoader).Assembly.GetName().Name).IsEqualTo("Engobe.Core");
        await Assert.That(typeof(EngineHost).Assembly.GetReferencedAssemblies()
            .Any(reference => reference.Name?.StartsWith("Kiln", StringComparison.Ordinal) == true)).IsFalse();
    }
}