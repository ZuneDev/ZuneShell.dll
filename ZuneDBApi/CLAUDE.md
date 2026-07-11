# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this project is

`ZuneDBApi` is one project inside the larger `ZuneShell.dll` solution (root: `..`), which is a clean-room
reimplementation of the original Microsoft Zune desktop software. The dependency chain is:

```
ZuneDBApi (this project) → ZuneImpl → ZuneShell → ZuneHost / ZuneHost.Wpf (entry points)
```

ZuneDBApi is the lowest layer: it re-creates the public API surface of the original closed-source
`Microsoft.Zune.*` / `MicrosoftZuneInterop` / `MicrosoftZuneLibrary` assemblies that shipped with the real
Zune software, so the rest of the stack (`ZuneImpl`, `ZuneShell`) can compile and run against a familiar API
without bundling Microsoft's original binaries or decompiled source.

Namespace-to-folder mapping is strict and should be preserved when adding files:
- `Microsoft.Zune.Configuration` → `Microsoft.Zune/Configuration/`
- `Microsoft.Zune.Service` → `Microsoft.Zune/Service/`
- `Microsoft.Zune.ErrorMapperApi` → `Microsoft.Zune/ErrorMapperApi/`
- `Microsoft.Zune.Util` → `Microsoft.Zune/Util/`
- `MicrosoftZuneInterop` → `MicrosoftZuneInterop/`
- `MicrosoftZuneLibrary` → `MicrosoftZuneLibrary/`
- `ZuneUI` (HRESULT, attributes) → `ZuneUI/`

## Critical constraint: no decompiled code

A prior commit (`d0fd93a`, "Added raw ZuneDBApi decomp") added a raw decompilation of the original Microsoft
assembly and was immediately reverted (`749a29a`) — verbatim decompiled Microsoft code must not be checked in.
Instead, classes here are **clean-room stubs**: same public type/method/property signatures as the original
(so dependent code compiles and links correctly), but with placeholder or minimal real bodies, not copied
Microsoft implementation logic. The `mcp__ilspy__*` tools (already allowlisted in `.claude/settings.local.json`)
are for inspecting the *shape* of the original assembly (type/member lists, signatures) to match the API
surface — not for pulling decompiled method bodies into source files.

Two stub styles coexist in this codebase, both legitimate:
- **Pure no-op stub** — method sets `out` params to defaults and returns `HRESULT._S_OK` (e.g.
  `Microsoft.Zune/Service/AccountManagement.cs`).
- **Real working logic** — e.g. `Microsoft.Zune/Configuration/*Configuration.cs` classes are genuinely
  backed by `RegistryHive`/`CConfigurationManagedBase` with real default values, and `ZuneUI/HRESULT.cs` is a
  fully real HRESULT struct with the actual Zune error codes.

When a stub needs to represent an unmanaged COM pointer the original held (e.g. `IPlaylist*`, `IQueryPropertyBag*`),
do **not** invent raw C-style pointer types or C++-style destructor syntax (`~Foo()` as an ordinary method, or
`!Foo()`) — neither compiles in C#. Use `IntPtr`/`SafeHandle`-style fields and a normal `IDisposable` pattern
(`protected virtual void Dispose(bool disposing)` + `~Foo()` as an actual finalizer) instead, matching the
pattern already used in `Microsoft.Zune/Configuration/CConfigurationManagedBase.cs`.

## Build

```bash
# From repo root or this directory — build just this project:
dotnet build ZuneDBApi/ZuneDBApi.csproj

# Build the whole solution (Windows-only platforms exist in ZuneShell.sln; on Linux only net8.0 TFMs build):
dotnet build ZuneShell.sln
```

`Directory.Build.props` (repo root) sets `TargetFrameworks` to `net8.0` everywhere, adding `net472` and
`net8.0-windows` automatically when building on Windows. There is no test project for ZuneDBApi or the
solution as a whole.

Note: building currently fails on files containing invalid C++-style syntax (see constraint above) —
running a build will surface any remaining instances; treat new ones the same way (fix to valid C#, do not
silently work around them).

## Working in this project

- `Nullable` and `ImplicitUsings` are enabled; `AllowUnsafeBlocks` is true (file-scoped namespaces are fine).
- This project only references `UIX.csproj` (from the `MicrosoftIris`/`ZuneUIXTools` submodule) — keep it free
  of dependencies on `ZuneImpl`/`ZuneShell` to avoid circular references, since those layers depend on it.
- `Microsoft.Win32.RegistryHive`/`Registry*` APIs used by the `Configuration` classes are Windows-only; this
  is expected since the original Zune client was Windows-only, but keep cross-platform buildability in mind
  for the `net8.0` (non-`-windows`) TFM — avoid adding new Windows-only APIs outside what's already used.
