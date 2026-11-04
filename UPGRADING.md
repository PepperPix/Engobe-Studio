# Upgrade from Kiln Studio to Engobe Studio

Engobe Studio uses the Engobe engine in-process. Its package is
`PepperPix.Engobe.Studio` and its command is `engobe-studio`. There is no
`kiln-studio` alias and no automatic migration of legacy state.

## Back up before upgrading

1. Close Studio and stop any preview server.
2. Back up the entire site, including hidden files, plugins, themes, and
   `.kiln/deploy.json`. Keep the backup outside the working site.
3. Record the installed Kiln Studio tool version and retain its package for
   rollback. Do not uninstall it until the replacement has been verified.
4. Back up the application-global recent-project list described below.

## Replace the tool

Studio and the engine have independent versions. This source revision targets
Studio `1.0.0-beta.3` with `Engobe.Core` and `Engobe.Abstractions` exactly
`2.0.0`. The engine packages are available from NuGet; the Studio package is
not yet published. Do not infer a published Studio version from this source
revision or from the engine version.

For local integration, pack the Studio tool and install it into an isolated
tool directory. This does not publish the package or modify global tools:

```sh
dotnet pack src/Engobe.Studio/Engobe.Studio.csproj --configuration Release \
  --output <local-feed>
dotnet tool install PepperPix.Engobe.Studio \
  --version 1.0.0-beta.3 \
  --tool-path <isolated-tool-directory> \
  --add-source <local-feed>
```

After an approved Studio version is publicly available, install that exact
version. The legacy tool has a separate package ID and command; keep it
installed until the replacement has been verified:

```sh
dotnet tool install --global PepperPix.Engobe.Studio --version <verified-studio-version>
engobe-studio
```

The last publicly available legacy version is `PepperPix.Kiln.Studio`
`1.0.0-beta.3`, with command `kiln-studio`.

This change does not add a macOS application bundle, installer, or OS
integration. Do not infer a published package version from the engine version.

## Transfer project settings manually

Create `.engobe` in the working site and copy `.kiln/deploy.json` to
`.engobe/deploy.json`. Preserve the JSON content, deployment variant, file
system destination, and publishing mode. If `.engobe/deploy.json` already
exists, compare the files and choose the intended settings before replacing
anything. Engobe Studio does not read or overwrite `.kiln`.

Do not copy `.kiln/taxonomy-values.json` or `.kiln/studio-thumbnails`.
Taxonomy suggestions are rebuilt when the project opens; thumbnails are
regenerated as assets are displayed. New caches use `.engobe`. Thumbnails
without a project use the temporary directory `engobe-studio-thumbnails`;
the old temporary cache can be left for rollback.

Do not rewrite `site.yaml`, front matter, content paths, theme syntax, or public
URLs merely to change the application name. The engine's separate manual
upgrade guide covers CLI manifests, environment variables, plugin
reinstallation, and plugin locks.

## Transfer application-global state

The application stores `recent.json` beneath the directory returned by
`.NET Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)`.
The old subdirectory is `KilnStudio`; the new one is `EngobeStudio`. No legacy
directory is automatically read or copied.

Either start with an empty recent-project list or, while both applications
are closed, copy the backed-up `KilnStudio/recent.json` to
`EngobeStudio/recent.json` beneath that application-data directory. Check the
saved project paths before reopening them. No other application-global
settings file is used by this revision.

## Verify and roll back

1. Open the working copy in Engobe Studio.
2. Verify the deployment variant and destination before publishing.
3. Edit and save a test page, start preview, and build the site. Compare
   rendered URLs and content with the backup, allowing only intentional
   branding changes.
4. Close and reopen the project and verify saved content and settings.

If verification fails, stop Engobe Studio and its preview server, restore the
complete project backup, and return to the previously pinned
`PepperPix.Kiln.Studio` version. If that tool is still installed, start
`kiln-studio`; otherwise reinstall the recorded version:

```sh
dotnet tool uninstall --global PepperPix.Engobe.Studio
dotnet tool install --global PepperPix.Kiln.Studio --version <recorded-version>
```

Keep the legacy global recent-project file intact. Do not combine new lock
entries or generated caches with the restored legacy installation.
