namespace Engobe.Studio.Tests;

using System.Security.Cryptography;
using Engobe.Studio.Services;

public sealed class ManualUpgradeTests
{
    [Test]
    public async Task BackedUpProject_ManualStateTransfer_PreservesContentAndSupportsBuildAndReopen()
    {
        var root = Path.Combine(Path.GetTempPath(), $"engobe-studio-upgrade-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        try
        {
            var host = new EngineHost();
            var projectService = new ProjectService(host);
            var project = projectService.CreateSite(root, "site");
            var legacy = Directory.CreateDirectory(Path.Combine(project, ".kiln")).FullName;
            const string deploymentJson = "{\"variant\":\"filesystem\",\"filesystemPath\":\"publish\",\"filesystemMode\":\"zip\"}";
            await File.WriteAllTextAsync(Path.Combine(legacy, "deploy.json"), deploymentJson);
            await File.WriteAllTextAsync(Path.Combine(legacy, "taxonomy-values.json"), "{\"tags\":[\"legacy\"]}");
            var backup = Path.Combine(root, "backup");
            foreach (var file in Directory.EnumerateFiles(project, "*", SearchOption.AllDirectories))
            {
                var destination = Path.Combine(backup, Path.GetRelativePath(project, file));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.Copy(file, destination);
            }
            var before = ContentHashes(project);
            var store = new DeploymentConfigStore();
            await Assert.That(store.Load(project).Variant).IsEqualTo(DeploymentVariant.None);
            Directory.CreateDirectory(Path.Combine(project, ".engobe"));
            File.Copy(Path.Combine(legacy, "deploy.json"), Path.Combine(project, ".engobe", "deploy.json"));

            projectService.Open(project);
            await Assert.That(ContentHashes(project)).IsEquivalentTo(before);
            await Assert.That(store.Load(project).FilesystemMode).IsEqualTo(FilesystemMode.Zip);
            await Assert.That(store.Load(project).FilesystemPath).IsEqualTo("publish");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(legacy, "deploy.json"))).IsEqualTo(deploymentJson);

            var page = Path.Combine(project, "content", "pages", "about.md");
            var content = new ContentService();
            var document = content.Load(page);
            content.Save(page, document.FrontMatter, document.Body + "\nManual upgrade verified.\n");
            var result = await new BuildService(host).BuildAsync(project, release: false);
            await Assert.That(result.Success).IsTrue();
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(project, "_site", "about", "index.html")))
                .Contains("Manual upgrade verified.");
            projectService.Open(project);
            await Assert.That(content.Load(page).Body).Contains("Manual upgrade verified.");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(backup, "site.yaml")))
                .IsEqualTo(await File.ReadAllTextAsync(Path.Combine(project, "site.yaml")));
            await Assert.That(ContentHashes(backup)).IsEquivalentTo(before);
            await Assert.That(store.Load(project).Variant).IsEqualTo(DeploymentVariant.Filesystem);

            Directory.Delete(project, recursive: true);
            CopyDirectory(backup, project);
            await Assert.That(ContentHashes(project)).IsEquivalentTo(before);
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(project, ".kiln", "deploy.json")))
                .IsEqualTo(deploymentJson);
            await Assert.That(File.Exists(Path.Combine(project, ".engobe", "deploy.json"))).IsFalse();
            await Assert.That(store.Load(project).Variant).IsEqualTo(DeploymentVariant.None);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task RecentProjects_UseSeparateGlobalDirectoryUntilManuallyCopied()
    {
        var root = Path.Combine(Path.GetTempPath(), $"engobe-studio-recent-{Guid.NewGuid():N}");
        try
        {
            var oldDirectory = Path.Combine(root, "KilnStudio");
            var newDirectory = Path.Combine(root, "EngobeStudio");
            var oldStore = new RecentProjectsStore(oldDirectory);
            oldStore.Add(Path.Combine(root, "site"), "Site");
            var original = await File.ReadAllTextAsync(Path.Combine(oldDirectory, "recent.json"));
            var newStore = new RecentProjectsStore(newDirectory);
            await Assert.That(newStore.GetAll()).IsEmpty();
            Directory.CreateDirectory(newDirectory);
            File.Copy(Path.Combine(oldDirectory, "recent.json"), Path.Combine(newDirectory, "recent.json"));
            await Assert.That(newStore.GetAll().Single().Name).IsEqualTo("Site");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(oldDirectory, "recent.json"))).IsEqualTo(original);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static string[] ContentHashes(string project) =>
        Directory.EnumerateFiles(project, "*", SearchOption.AllDirectories)
            .Where(path => Path.GetRelativePath(project, path) is var relative
                && (relative == "site.yaml" || relative.StartsWith("content" + Path.DirectorySeparatorChar, StringComparison.Ordinal)))
            .Select(path => $"{Path.GetRelativePath(project, path)}:{Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)))}")
            .Order(StringComparer.Ordinal)
            .ToArray();

    private static void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target);
        }
    }
}
