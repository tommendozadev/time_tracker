# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project status

`time_tracker` is an ASP.NET Core MVC app targeting **.NET 10** (`net10.0`). It started from the `dotnet new mvc` template and uses PostgreSQL via EF Core (Npgsql). So far there is a `User` model with signup/login/logout (`AccountController`) and a placeholder `ActivitiesController`; there is no time-tracking domain code yet, and no test project.

## Commands

Run from the repository root (the folder containing `time_tracker.csproj`):

```bash
dotnet build                      # build
dotnet run                        # run with the "http" profile → http://localhost:5243
dotnet run --launch-profile https # https://localhost:7286 and http://localhost:5243
dotnet watch                      # run with hot reload
dotnet ef migrations add <Name>   # after changing a model
dotnet ef database update         # apply migrations to the configured database
```

The connection string is `ConnectionStrings:DefaultConnection` (currently in `appsettings.Development.json` for local dev; use user-secrets or the `ConnectionStrings__DefaultConnection` env var elsewhere).

There is no test project yet. If one is added (e.g. `dotnet new xunit`), run a single test with `dotnet test --filter "FullyQualifiedName~TestName"`.

## Architecture

- **Single project, conventional MVC.** `Program.cs` uses minimal hosting: `AddControllersWithViews()`, then the default route `{controller=Home}/{action=Index}/{id?}`. New controllers in `Controllers/` named `XController` are routed automatically; their views go in `Views/X/<Action>.cshtml`.
- **Static assets** are served via `MapStaticAssets()` / `.WithStaticAssets()` (the .NET 9+ fingerprinted static-asset pipeline), not `UseStaticFiles()`. Client libraries (Bootstrap 5, jQuery, jQuery Validation) are vendored under `wwwroot/lib/` — there is no npm/libman setup.
- **Views:** `Views/Shared/_Layout.cshtml` is the shared layout (with scoped CSS in `_Layout.cshtml.css`); `_ViewImports.cshtml` imports the `time_tracker` and `time_tracker.Models` namespaces and the MVC tag helpers. Include `_ValidationScriptsPartial` in views that need client-side form validation.
- **Conventions:** file-scoped namespaces (`namespace time_tracker.Controllers;`), nullable reference types and implicit usings enabled.
- **Environment:** the developer exception page is used in Development; otherwise `/Home/Error` plus HSTS. Both launch profiles set `ASPNETCORE_ENVIRONMENT=Development`.
- **Database:** `Data/AppDbContext.cs` is the EF Core context, registered in `Program.cs`. Tables and columns are snake_case (`users`, `first_name`, …), set explicitly with `[Table]`/`[Column]` attributes on each model — keep doing this for new entities. `DateTime` maps to `timestamptz`, so values must be UTC (`DateTime.UtcNow`) or Npgsql throws. Migrations live in `Migrations/`.
- **Passwords** are hashed with ASP.NET Core Identity's `IPasswordHasher<User>` (registered in `Program.cs`, verify with `VerifyHashedPassword`); the full Identity framework is not used.
- **Auth** is plain cookie authentication (`AddAuthentication().AddCookie()` in `Program.cs`, login path `/Account/Login`). `AccountController.SignInAsync` issues claims `NameIdentifier` = user id, `Name` = first name, `Email`. Protect controllers with `[Authorize]`; get the current user's id via `User.FindFirstValue(ClaimTypes.NameIdentifier)`. Emails are stored trimmed and lowercased, with a unique index.
- **Form models** (e.g. `SignupViewModel`) are separate from entities and carry the DataAnnotations validation; controllers map them to entities.
