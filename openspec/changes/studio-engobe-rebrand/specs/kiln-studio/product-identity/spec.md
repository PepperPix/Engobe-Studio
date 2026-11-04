## Purpose

Define the Engobe Studio identity presented to users, package consumers, and existing project owners during the rebrand.

## ADDED Requirements

### Requirement: Engobe Studio application identity
Studio SHALL identify itself as Engobe Studio in its window title, current UI text, package metadata, and documentation. The tool package SHALL be `PepperPix.Engobe.Studio`, with command `engobe-studio`, and current product links SHALL use `https://engobe.studio`.

#### Scenario: Install and open the application
- **WHEN** the renamed tool is installed and started on a supported platform
- **THEN** the application opens as Engobe Studio with resolving icons and resources
- **AND** the package does not install a `kiln-studio` alias.

### Requirement: Studio uses Engobe project state
Studio SHALL persist project-local deployment settings and derived state under `.engobe`, not `.kiln`. It SHALL NOT automatically read, modify, or migrate legacy state. Site content and `site.yaml` SHALL remain the project source of truth.

#### Scenario: Save deployment settings
- **WHEN** project deployment settings are saved
- **THEN** the settings are written to `.engobe/deploy.json`
- **AND** no legacy deployment file is overwritten.

#### Scenario: Open a manually migrated project
- **WHEN** a user transfers deployment settings according to the migration guide
- **THEN** Studio reloads the settings and content from the new locations
- **AND** derived caches can be regenerated without content loss.

### Requirement: Generated deployment instructions use Engobe
Existing Studio features that generate deployment workflows or CLI instructions SHALL emit Engobe tool/package references and commands while preserving their existing deployment semantics.

#### Scenario: Generate existing deployment instructions
- **WHEN** an existing supported deployment variant generates configuration
- **THEN** generated commands invoke `engobe` rather than `kiln`
- **AND** no new deployment variant is introduced by the rebrand.

### Requirement: Studio migration is documented and independently versioned
Studio SHALL retain its own release progression and publish manual instructions for tool replacement, project settings, any application-global settings, and rollback. The rebrand SHALL NOT force its version number to equal the engine version.

#### Scenario: Upgrade Studio
- **WHEN** a user follows the documented migration from a backed-up Kiln Studio installation
- **THEN** the renamed application uses its declared Engobe engine dependency
- **AND** the guide identifies preserved settings, regenerated caches, and the previous-version rollback path.
