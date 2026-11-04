# Design

## Context

See [proposal.md](proposal.md). Studio currently publishes `PepperPix.Kiln.Studio` with command `kiln-studio`; its inspected version is `1.0.0-beta.2`. Services reference `Kiln.Core` and `Kiln.Abstractions`. `EngineHost` registers the engine in-process. Project state includes `.kiln/deploy.json`, taxonomy values and thumbnail caches.

The open macOS bundle plan proposes `Kiln Studio.app` and `com.pepperpix.kilnstudio`, but that plan is not evidence of an existing shipped bundle. Release project paths live in `package.json`. The deployment change is also open and may alter generated workflow surfaces.

## Goals / Non-Goals

**Goals:** Consistent application identity and engine integration without changing content-editing behavior, data ownership, or release independence.

**Non-Goals:** No new packaging channel, migration wizard, signing, deployment feature, or broad UI redesign.

## Decisions

1. **Keep existing packaging structure.** Rename the tool to `engobe-studio`, package to `PepperPix.Engobe.Studio`, namespaces/assemblies to `Engobe.Studio.*`, and solution to `EngobeStudio.slnx`. Retain PepperPix as publisher and keep the independent Studio version line. An incompatible API/data change must still receive the appropriate semantic-release classification.
2. **Use the engine in-process.** Reference exact available Engobe package versions and `AddEngobe()`. Do not introduce CLI subprocesses to simplify migration. Use a temporary local feed for pre-publication integration, never commit developer-specific feed paths.
3. **Use `.engobe` without fallback.** Manually migrate deployment settings after backing up projects; regenerate derived taxonomy/thumbnail caches as appropriate. Inventory any global preferences and recent-project paths before changing them. Document their explicit transfer or deliberate cache reset without claiming an automatic migration.
4. **Rename assets without inventing a design project.** Update branded icon filenames, visible wordmarks/text, window titles, Avalonia URIs and package icons. Preserve useful unbranded artwork. If existing artwork contains a kiln-specific symbol, obtain approval for a replacement rather than inventing a new logo.
5. **Reconcile existing plans before implementation.** Update planned bundle identity to `Engobe Studio.app` and `com.pepperpix.engobestudio` within the bundle change when that work is taken up. Do not add bundle code here if it does not yet exist. Similarly align deployment-command literals without absorbing the deployment feature. Keep existing spec paths and change IDs stable.

## Risks / Trade-offs

- Renamed assemblies break Avalonia resources or compiled XAML -> inspect all assembly/resource references and execute actual startup plus UI tests.
- Missing deployment settings look like a fresh project -> migration instructions must preserve `.kiln/deploy.json` as `.engobe/deploy.json`; no silent automatic transfer is promised.
- Engine packages are not yet available to CI -> wait for an approved Engobe prerelease before merging package-dependent changes; isolated local packing is not public availability.
- Snapshot changes hide UI regressions -> inspect only identity-related changes; no blanket baseline acceptance.

## Migration Plan

Prepare on a feature branch from current `beta`, reconcile overlapping plans, integrate renamed engine packages, validate and independently review. Publish on Studio's own release schedule after dependencies exist. Provide manual tool replacement and project-state instructions. Roll back with the prior pinned Studio tool and project backup; preserve old packages and tags.
