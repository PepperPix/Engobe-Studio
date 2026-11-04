## MODIFIED Requirements

### Requirement: Studio uses Engobe Core in-process
Studio MUST execute Engobe functionality through typed Core services from `Engobe.Core` and `Engobe.Abstractions` rather than by starting the CLI process.

#### Scenario: A Studio feature uses the engine
- **WHEN** Studio performs an engine operation
- **THEN** it accesses the reused Engobe Core services within the Studio process.
