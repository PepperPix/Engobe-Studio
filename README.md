# Engobe Studio

A cross-platform desktop CMS for static websites — powered by [Engobe](https://github.com/PepperPix/Engobe).

## Status

**Early development** — Product behavior and package availability are evolving;
Studio follows its own version line, independent of the Engobe engine.

## What is Engobe Studio?

Engobe Studio is a desktop application for creating and managing static websites. It provides a visual
interface for content editing, theme configuration, and one-click deployment — powered by the Engobe
static site generator engine.

### Planned Features

- Visual content editor (Markdown with live preview)
- Project and site management
- Theme selection and customization
- Media library
- Build and deployment from the UI
- Cross-platform: macOS and Windows

## Solution Layout

```
src/
  Engobe.Studio/             — Avalonia App (UI, DI host, Views)
  Engobe.Studio.ViewModels/  — ViewModels (UI-free, CommunityToolkit.Mvvm)
  Engobe.Studio.Services/    — Studio services (EngineHost, ProjectSession)
tests/
  Engobe.Studio.Tests/       — TUnit tests
```

### Conventions

Every `.cs` file contains exactly one top-level type. Nested types are used only as a deliberate
encapsulation aid.

## Installation

`PepperPix.Engobe.Studio` has not been published yet. Do not install or update it
from NuGet until a public Studio version is announced. For local integration,
use the isolated local-feed procedure in [UPGRADING.md](UPGRADING.md#replace-the-tool).
Studio and the engine have independent versions.

Native installers and OS integration (Dock, Start menu, `.app` bundle) are
outside this rebrand.

## Building

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (10.0.301 or later)
- [Node.js](https://nodejs.org/) (for commit tooling)

### Building

```bash
dotnet build
```

### Running tests

We use TUnit. Run the full suite with:

```bash
dotnet test
```

Run a single test project, class, or test with:

```bash
dotnet test tests/Projekt.Tests
dotnet test --treenode-filter "/*/*/ClassName/*"
dotnet test --treenode-filter "/*/*/ClassName/TestName"
```

Do not use `dotnet test --filter "FullyQualifiedName~..."` here — that is VSTest syntax and does
not work with TUnit.

### Running the app

```bash
dotnet run --project src/Engobe.Studio
```

### Commit tooling

```bash
npm install
```

Husky installs a `commit-msg` hook enforcing [Conventional Commits](https://www.conventionalcommits.org/).

## Related Projects

- **[Engobe](https://github.com/PepperPix/Engobe)** — The static site generator engine and CLI tool

## License

[AGPL-3.0](LICENSE)

Copyright 2026 Marcel Kummerow
