## Why

The engine is adopting the Engobe identity and the canonical website `https://engobe.studio`. Studio must present the same identity and use the renamed engine without leaving mixed package, application, or project-state contracts.

## What Changes

- **BREAKING**: Rename the application to Engobe Studio, its command to `engobe-studio`, and its tool package to `PepperPix.Engobe.Studio`.
- **BREAKING**: Rename source/test projects, namespaces, assemblies, resources, and public API references; consume `Engobe.Core` and `Engobe.Abstractions`.
- **BREAKING**: Store project-local Studio state under `.engobe` instead of `.kiln`, with documented manual migration and no legacy fallback.
- Rebrand visible UI, application metadata, asset names, generated deployment commands, documentation, and release paths.
- Keep Studio on its own version line; do not force a Studio 2.0 release or reset release history.

## Capabilities

### New Capabilities
- `kiln-studio/product-identity`: Observable application identity, project-state, packaging, and migration behavior.

### Modified Capabilities
- `kiln-studio/architecture`: Use the renamed engine packages in-process.

## Impact

- Target: `PepperPix/Kiln-Studio`, to become `PepperPix/Engobe-Studio`; implementation on a feature branch from current `beta`, merge into `beta`.
- Affects all source/test projects, Avalonia resources, package metadata, release configuration, UI snapshots, deployment generation, and documentation.
- Requires an agreed Engobe engine package version that is available to CI, or an explicit local feed during isolated integration testing.
- Reconcile the open `studio-macos-bundle-identity` and `studio-deployment-destination` plans; do not implement those features incidentally.
- Non-goals: new installer channels, signing/notarization, automatic data migration, a visual redesign, or unrelated deployment features.
