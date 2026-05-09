---
description: "Use when creating, modifying, or organizing backend (C#/ASP.NET Core), Python (Azure Functions), or frontend (Vue 3/TypeScript) code in this project. Covers folder structure under app/, naming conventions, and basic coding rules."
applyTo: "storage-management/app/**"
---

# Project Structure & Coding Rules

## Folder Layout (under `storage-management/app/`)

| Folder | Purpose |
|--------|---------|
| `api/` | ASP.NET Core REST API |
| `azf/` | Azure Functions (C# and Python) |
| `dal/` | Data Access Layer (Entity Framework) |
| `sharedentities/` | Shared C# domain models across projects |
| `webapp/` | Vue 3 + TypeScript frontend |

Do not create new top-level folders under `app/`. Add new concerns to the existing folders above.

---

## Backend Rules (`api/`, `dal/`, `sharedentities/`)

### Folder Organization
- Controllers → `api/Controllers/`
- Services + interfaces → `api/Services/`
- Request/response DTOs → `api/Models/`
- DB entities shared across projects → `sharedentities/DBEntities/`
- EF Core configuration → `dal/Infra/`

### Naming Conventions
- Controllers: `[Resource]Controller` (e.g., `DocumentManagementController`)
- Services: `[Resource]Service` with interface `I[Resource]Service`
- Request DTOs: `[Resource]RequestModel`
- Response DTOs: `[Resource]ResponseModel`
- DB entities: `[Resource]Entity`

### Coding Rules
- Always define an interface for every service (`I[Name]Service`) and register both via dependency injection in `Program.cs`.
- Use AutoMapper (via `AutoMapperProfile.cs`) to map between entities and DTOs. Do not map manually in controllers.
- Keep controllers thin: delegate all business logic to services.
- Use `appsettings.json` / `appsettings.Development.json` for configuration; never hard-code connection strings or secrets.
- Follow existing OpenTelemetry instrumentation patterns (see `Extensions/OpenTelemetryDistroUserDefinedExtensions.cs`).

---

## Frontend Rules (`webapp/`)

### Folder Organization (under `webapp/src/`)
- Pages/views → `views/`
- Reusable components → `components/`
- Pinia stores → `stores/`
- API call wrappers → `services/`
- TypeScript models/types → `models/`
- Route definitions → `router/`

### Naming Conventions
- Vue components: PascalCase filenames (e.g., `DocumentList.vue`)
- Pinia stores: camelCase with `use` prefix (e.g., `useDocumentStore`)
- Service files: camelCase + `Service` suffix (e.g., `documentService.ts`)
- TypeScript interfaces/types: PascalCase

### Coding Rules
- Use Vue 3 Composition API with `<script setup lang="ts">` only. Do not use Options API.
- All new files must be TypeScript (`.ts` / `.vue` with `lang="ts"`).
- Use Pinia for shared/global state. Avoid component-local state for data fetched from the API.
- Use Axios (or the existing service layer) for all HTTP calls. Do not call `fetch` directly.
- Use PrimeVue components for UI elements. Do not introduce additional UI libraries.

---

## Python Rules (`azf/documentreceiverfunction/`)

### Rules
1. Use snake_case for all file names, function names, and variable names (e.g., `blob_uploader.py`, `upload_document()`).
2. Group Azure Function logic in `function_app.py`; extract reusable helpers into separate files (e.g., `blobuploader.py`, `apiconnectorhelper.py`).
3. Read all configuration (connection strings, URLs, keys) from environment variables. Never hard-code them. Use `local.settings.json` for local development only.
4. Define request/response data shapes as classes or dataclasses in dedicated model files (e.g., `document_reqmodel.py`). Do not use plain `dict` for structured data.
5. Declare all third-party dependencies in `requirements.txt`. Do not import packages that are not listed there.

### Best Practices
1. **Type hints**: Add type annotations to all function signatures (e.g., `def upload(data: DocumentRequestModel) -> bool:`).
2. **Error handling**: Wrap external calls (Blob Storage, Service Bus, HTTP) in `try/except` blocks; log the exception and return a meaningful HTTP status code.
3. **Single responsibility**: Each helper file should do one thing (blob upload, API connection, etc.). Keep `function_app.py` as the entry point only.
4. **Logging**: Use `logging.getLogger(__name__)` at the top of each module. Do not use `print()` for diagnostics.
5. **Secrets safety**: Never log secrets, connection strings, or tokens — even at DEBUG level.
