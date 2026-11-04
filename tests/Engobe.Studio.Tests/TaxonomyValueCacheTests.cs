namespace Engobe.Studio.Tests;

using Services;

public sealed class TaxonomyValueCacheTests
{
    [Test]
    public async Task Rebuild_IgnoresAndPreservesLegacyCache()
    {
        var root = CreateTempDir();
        var legacy = Path.Combine(root, ".kiln", "taxonomy-values.json");
        Directory.CreateDirectory(Path.GetDirectoryName(legacy)!);
        const string legacyJson = "{\"tags\":[\"legacy\"]}";
        await File.WriteAllTextAsync(legacy, legacyJson);
        try
        {
            var cache = new TaxonomyValueCache();
            await Assert.That(cache.GetSuggestions(root, "tags")).IsEmpty();
            cache.Rebuild(root, new Dictionary<string, IReadOnlyCollection<string>> { ["tags"] = ["engobe"] });
            await Assert.That(cache.GetSuggestions(root, "tags")).Contains("engobe");
            await Assert.That(cache.GetSuggestions(root, "tags")).DoesNotContain("legacy");
            await Assert.That(await File.ReadAllTextAsync(legacy)).IsEqualTo(legacyJson);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private const int TwoValues = 2;

    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Test]
    public async Task GetSuggestions_NoCacheFile_ReturnsEmpty()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            var values = cache.GetSuggestions(dir, "tags");

            await Assert.That(values.Count).IsEqualTo(0);
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }

    [Test]
    public async Task Rebuild_ThenGetSuggestions_ReturnsValues()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            cache.Rebuild(dir, new Dictionary<string, IReadOnlyCollection<string>>
            {
                ["tags"] = new[] { "dotnet", "engobe" },
                ["categories"] = new[] { "news" },
            });

            var tags = cache.GetSuggestions(dir, "tags");
            var categories = cache.GetSuggestions(dir, "categories");

            await Assert.That(tags.Count).IsEqualTo(TwoValues);
            await Assert.That(tags).Contains("dotnet");
            await Assert.That(tags).Contains("engobe");
            await Assert.That(categories).Contains("news");
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }

    [Test]
    public async Task Rebuild_CreatesDotEngobeDirectory()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            cache.Rebuild(dir, new Dictionary<string, IReadOnlyCollection<string>>
            {
                ["tags"] = new[] { "a" },
            });

            await Assert.That(File.Exists(Path.Combine(dir, ".engobe", "taxonomy-values.json"))).IsTrue();
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }

    [Test]
    public async Task AddValues_MergesWithExisting_NeverRemoves()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            cache.Rebuild(dir, new Dictionary<string, IReadOnlyCollection<string>>
            {
                ["tags"] = new[] { "dotnet" },
            });

            cache.AddValues(dir, "tags", ["engobe", "dotnet"]);

            var values = cache.GetSuggestions(dir, "tags");

            await Assert.That(values.Count).IsEqualTo(TwoValues);
            await Assert.That(values).Contains("dotnet");
            await Assert.That(values).Contains("engobe");
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }

    [Test]
    public async Task AddValues_EmptyList_DoesNotCreateFile()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            cache.AddValues(dir, "tags", []);

            await Assert.That(File.Exists(Path.Combine(dir, ".engobe", "taxonomy-values.json"))).IsFalse();
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }

    [Test]
    public async Task AddValues_NoPriorCache_CreatesNewEntry()
    {
        var dir = CreateTempDir();
        try
        {
            var cache = new TaxonomyValueCache();
            cache.AddValues(dir, "categories", ["news"]);

            var values = cache.GetSuggestions(dir, "categories");

            await Assert.That(values.Count).IsEqualTo(1);
            await Assert.That(values).Contains("news");
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }
    }
}
