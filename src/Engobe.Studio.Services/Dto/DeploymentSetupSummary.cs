namespace Engobe.Studio.Services.Dto;

public sealed record DeploymentSetupSummary(DeployTarget Target, IReadOnlyList<string> CreatedFiles);