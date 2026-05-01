# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

This repo has **two independent solutions**, each with its own pinned Uno.Sdk version:

- `src/MZikmund.Toolkit.sln` — the published library (`MZikmund.Toolkit.WinUI`). Uno.Sdk pinned in `src/global.json`.
- `samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample.sln` — gallery sample app. Uno.Sdk pinned in `samples/MZikmund.Toolkit.Sample/global.json`.

**Both library and samples target Uno 6.x.** Keep the two `global.json` files aligned on the same Uno.Sdk version; treat the samples version as authoritative when bumping.

Each tree has its own `Directory.Packages.props` — they do not share package versions.

## New features must ship with a sample

Every new feature added to `src/MZikmund.Toolkit.WinUI` must come with a corresponding page or example in `samples/MZikmund.Toolkit.Sample`. The sample app is the gallery users browse to discover what the toolkit does — a feature without a sample is invisible. PRs that add a service/extension/infrastructure piece without sample coverage are incomplete.

## Build & verify

```powershell
# Library
dotnet workload restore src/MZikmund.Toolkit.sln
dotnet build src/MZikmund.Toolkit.sln

# Sample app
dotnet workload restore samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample.sln
dotnet build samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample.sln
```

Run `/verify-build` to build both solutions in one shot.

There is no test project yet; `dotnet test` is a no-op.

## Gotchas Claude tends to get wrong

- **Central Package Management is on.** Add or update versions in `src/Directory.Packages.props` or `samples/MZikmund.Toolkit.Sample/Directory.Packages.props` — never put `Version="..."` on a `<PackageReference>` in a csproj.
- **GitVersion owns the package version.** CI computes the package version from git history; the `<Version>0.0.1</Version>` in `src/MZikmund.Toolkit.WinUI/MZikmund.Toolkit.WinUI.csproj` is a placeholder. Do not bump it manually to "release" a version.
- **Uno Single Project.** `UnoSingleProject=true` means one csproj produces all platform heads (Android/iOS/macCatalyst/Windows/WASM/Desktop). Use `#if __ANDROID__`, `#if HAS_UNO`, etc. for platform-specific code rather than splitting projects. The `TargetFrameworks` list in the library csproj is the source of truth for supported platforms.

## Conventions

- **Commits & PRs:** Conventional Commits (`feat:`, `fix:`, `chore:`, `docs:`, `refactor:`, `test:`). GitVersion uses the history to compute the next package version.
- **Code style:** governed by `.editorconfig` — 4-space indent, file-scoped namespaces, `I`-prefixed interfaces, `System.*` usings first, CRLF line endings. Nullable is enabled repo-wide.

## CI

`.github/workflows/dotnet.yml` builds on `windows-latest` against .NET 10 SDK, packs the library, and pushes to NuGet.org from `main` (uses `NUGETAPIKEY` secret). PRs build but don't publish.
