# Event Platform

This repository is being re-homed from the archived `Base/` solution into the workspace root.

## Current Status

- `Base/` is the source snapshot during migration.
- `Reference/` contains learning and reference material only.
- New live project folders will be created at the repository root.

## Scaffold Direction

Target root layout:

- `API/`
- `Application/`
- `Domain/`
- `Infrastructure/`
- `Persistence/`
- `client-app/`
- `EventPlatform.sln`

## Migration Notes

1. Keep `Base/` and `Reference/` ignored during the initial move.
2. Copy and validate the live application at the root before removing archived folders.
3. Preserve current API-to-client hosting behavior until the root build is stable.
