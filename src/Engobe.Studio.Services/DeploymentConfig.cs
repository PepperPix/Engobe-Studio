namespace Engobe.Studio.Services;

public sealed record DeploymentConfig(
    DeploymentVariant Variant,
    string? FilesystemPath,
    FilesystemMode FilesystemMode);
