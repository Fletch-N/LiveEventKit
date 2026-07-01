# Event Platform

This repository is being re-homed from the archived Event Platform in .NET Core 3.2 solution into .NET 11.

## Getting Started

Run the API with watch from the repo root:
```ps
dotnet watch --project .\API\API.csproj
```

Get the correct version of EF CLI:
```ps
dotnet tool install --global dotnet-ef --version 11.0.0-preview.5.26302.115
```

Or update it:
```ps
dotnet tool update --global dotnet-ef --version 11.0.0-preview.5.26302.115
```

Build the projects and commit the DB:
```ps
dotnet restore
dotnet build
dotnet ef database update --project .\Persistence\Persistence.csproj --startup-project .\API\API.csproj
```

## Authentication

Identity API endpoints are mapped under `/api/auth`.

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`

For development, startup seeds an admin user and all roles from `UserRoles`.

- Email: `admin@liveeventkit.local`
- Password: `Admin123!`

To log in with bearer tokens:

```http
POST /api/auth/login?useCookies=false
Content-Type: application/json

{
  "email": "admin@liveeventkit.local",
  "password": "Admin123!"
}
```

To log in with cookies instead, set `useCookies=true`.

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
