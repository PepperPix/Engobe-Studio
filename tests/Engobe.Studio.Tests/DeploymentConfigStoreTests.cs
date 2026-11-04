namespace Engobe.Studio.Tests;

using Services;

public class DeploymentConfigStoreTests
{
    [Test]
    public async Task LoadAndSave_DoNotReadOrOverwriteLegacyDeploymentSettings()
    {
        var root = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var legacy = Path.Combine(root, ".kiln", "deploy.json");
        Directory.CreateDirectory(Path.GetDirectoryName(legacy)!);
        const string legacyJson = "{\"variant\":\"filesystem\",\"filesystemPath\":\"backup\",\"filesystemMode\":\"zip\"}";
        await File.WriteAllTextAsync(legacy, legacyJson);
        try
        {
            var store = new DeploymentConfigStore();
            await Assert.That(store.Load(root).Variant).IsEqualTo(DeploymentVariant.None);

            var expected = new DeploymentConfig(DeploymentVariant.GitHubPages, null, FilesystemMode.PlainCopy);
            store.Save(root, expected);

            await Assert.That(store.Load(root)).IsEqualTo(expected);
            await Assert.That(await File.ReadAllTextAsync(legacy)).IsEqualTo(legacyJson);
            File.Copy(legacy, Path.Combine(root, ".engobe", "deploy.json"), overwrite: true);
            var migrated = store.Load(root);
            await Assert.That(migrated.Variant).IsEqualTo(DeploymentVariant.Filesystem);
            await Assert.That(migrated.FilesystemPath).IsEqualTo("backup");
            await Assert.That(migrated.FilesystemMode).IsEqualTo(FilesystemMode.Zip);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task SaveLoad_RoundTrip_PreservesConfig()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        var store = new DeploymentConfigStore();

        try
        {
            var original = new DeploymentConfig(
                DeploymentVariant.Filesystem,
                "/tmp/my-site-output",
                FilesystemMode.Zip);

            store.Save(tempDir, original);

            var loaded = store.Load(tempDir);

            await Assert.That(loaded.Variant).IsEqualTo(DeploymentVariant.Filesystem);
            await Assert.That(loaded.FilesystemPath).IsEqualTo("/tmp/my-site-output");
            await Assert.That(loaded.FilesystemMode).IsEqualTo(FilesystemMode.Zip);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Test]
    public async Task Load_MissingFile_ReturnsNone()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        var store = new DeploymentConfigStore();

        try
        {
            var loaded = store.Load(tempDir);

            await Assert.That(loaded.Variant).IsEqualTo(DeploymentVariant.None);
            await Assert.That(loaded.FilesystemPath).IsNull();
            await Assert.That(loaded.FilesystemMode).IsEqualTo(FilesystemMode.PlainCopy);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Test]
    public async Task Save_CreatesDotEngobeDirectory()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var store = new DeploymentConfigStore();

        try
        {
            var config = new DeploymentConfig(DeploymentVariant.GitHubPages, null, FilesystemMode.PlainCopy);
            store.Save(tempDir, config);

            await Assert.That(File.Exists(Path.Combine(tempDir, ".engobe", "deploy.json"))).IsTrue();
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Test]
    public async Task SaveLoad_RoundTrip_GitHubPagesVariant()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        var store = new DeploymentConfigStore();

        try
        {
            var original = new DeploymentConfig(DeploymentVariant.GitHubPages, null, FilesystemMode.PlainCopy);
            store.Save(tempDir, original);

            var loaded = store.Load(tempDir);

            await Assert.That(loaded.Variant).IsEqualTo(DeploymentVariant.GitHubPages);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }
}
