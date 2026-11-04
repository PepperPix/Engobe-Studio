namespace Engobe.Studio.Services;

public sealed record MenuDefinition(string Name, IReadOnlyList<MenuItemDefinition> Items);
