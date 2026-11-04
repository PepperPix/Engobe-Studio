# Tasks

## 1. Prepare dependencies and boundaries

- [x] 1.1 Start a feature branch from current `beta` after product-local planning is available there; reconcile bundle/deployment plans and verify no unrelated feature work enters the diff.
- [x] 1.2 Select exact available Engobe Core/Abstractions versions, or temporarily use an isolated local feed for integration; verify restore without committed machine-specific paths.

## 2. Rename application and engine integration

- [x] 2.1 Rename solution, projects, namespaces, assemblies, Avalonia resource URIs, source/test references and in-process registration; verify build plus architecture tests against `AddEngobe()`.
- [x] 2.2 Rename package to `PepperPix.Engobe.Studio`, command to `engobe-studio`, package metadata/icons, release project paths and current documentation; inspect the packed tool and assert there is no installed legacy alias.
- [x] 2.3 Update window titles, welcome/about/error text and branded assets; run focused UI tests, inspect identity-related snapshot changes and verify resources load in an actual application startup.

## 3. Migrate state and generated instructions

- [x] 3.1 Change project-local deployment, taxonomy and thumbnail state to `.engobe`, inventory application-global settings/recent-project locations, and test new storage without legacy reads/writes.
- [x] 3.2 Update all existing generated deployment tool references and commands, plus current spec/context wording; test generated variants without implementing the separate deployment feature.
- [x] 3.3 Document and exercise manual tool replacement, backed-up deployment-setting transfer, global-setting treatment, derived-cache regeneration and rollback; verify existing site content is unchanged.
- [x] 3.4 Reconcile the open bundle plan's future name and identifier with Engobe Studio, and update the overlapping deployment plan's literals; verify no unapproved packaging feature or competing identity remains in active plans.

## 4. Release readiness

- [x] 4.1 Regenerate package locks and verify the independent Studio version progression and release project path; inspect packed dependencies and perform a non-publishing release calculation.
- [x] 4.2 Verify the renamed repository's OIDC/publish configuration and release gate; keep releases disabled while the Studio package is unregistered.
- [ ] 4.3 After a successful beta release, verify NuGet package registration and ownership, install the exact published version in an isolated tool directory, and update public installation instructions with that version.

## 5. Integration validation and handoff

- [x] 5.1 Run `rtk dotnet build` with zero warnings/errors, `dotnet test` without rtk using existing TUnit settings, and `rtk run jb inspectcode EngobeStudio.slnx -e=WARNING -f=Text` with zero findings.
- [ ] 5.2 Start the actual renamed app, open a manually migrated project, edit/save, preview, build and reopen it; verify preserved deployment settings and render readiness. Report any existing macOS startup blocker separately rather than claiming a successful smoke test.
- [x] 5.3 Run strict OpenSpec validation, inspect all remaining tracked Kiln references and the staged diff, inspect `rtk git show --stat HEAD` after committing, and hand off for independent review without blanket snapshot regeneration.

## Verification notes

- Build: zero warnings/errors. Tests: 299 passed, zero failed, two existing platform-sensitive snapshot skips. InspectCode: zero findings. No snapshot baselines were regenerated.
- The isolated installed tool starts without logged errors. UI tests assert the Engobe Studio title, welcome text and resolved icon. A complete interactive desktop workflow remains pending under 5.2.
- Manual-upgrade tests exercise backed-up deployment-setting transfer, unchanged content/configuration, edit/save/build/reopen, backup restoration and separate recent-project storage. A local isolated tool install verified the Engobe and pinned Kiln commands can coexist and that removing Engobe leaves the legacy rollback tool available. Taxonomy and thumbnail tests verify regenerated `.engobe` caches without changing legacy caches. Interactive desktop migration remains pending under 5.2.
- Publisher-free semantic-release dry runs used isolated clones with the actual remote release notes. Committed `beta` recognizes `v1.0.0-beta.3` and proposes no release; committed `main` calculates its first stable `1.0.0`. Uncommitted rebrand changes are not included in these calculations.
- NuGet lists stable `2.0.0` packages for `Engobe.Core` and `Engobe.Abstractions`; `PepperPix.Engobe.Studio` still returns HTTP 404. The release workflow grants `id-token: write` and uses `NuGet/login@v1`; `NUGET_USER` is configured and `RELEASE_ENABLED` remains `false`. The user confirmed that the Studio NuGet policy has been updated for the renamed repository and package. Package registration, ownership, and an installable public version are tracked in task 4.3 after the first successful beta release.
- The user reported a successful initial Studio smoke test and postponed the full interactive workflow. Task 3.3 is now covered by migration and rollback tests; task 5.2 remains pending and the earlier report is not evidence of completing it.
- A new macOS source launch rendered the welcome screen only after its off-screen window was moved onscreen. The native new-site folder picker opened, but subsequent keyboard navigation left the app content black while the process remained alive. No site was created or modified; task 5.2 remains pending.
- Strict validation passes for this change and the two overlapping bundle/deployment changes. Commit `1d09687` records the rebrand. An independent review found one medium issue in the README's unpublished install command; the command was removed before the commit. No NuGet publication or release was made; `RELEASE_ENABLED` remains `false`.
