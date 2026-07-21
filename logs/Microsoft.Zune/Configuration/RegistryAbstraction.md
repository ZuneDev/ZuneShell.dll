# Configuration registry abstraction — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — IRegistryProvider abstraction + Win32 pass-through (stage 3, one-off)

**Task:** user explicitly asked to temporarily step into stage 3 for this
session: introduce an abstraction around the `Microsoft.Win32` registry APIs,
route every `Microsoft.Zune.Configuration.*` class through it instead of
touching the registry directly, then add a Windows-only implementation that
passes through to the real registry.

**Findings before writing any code:**

- Despite every `*Configuration.cs` file (44 files) importing `Microsoft.Win32`,
  none of them call `RegistryKey`/`Registry.*` directly. The only actual
  registry surface in use is the `RegistryHive` enum, threaded through public
  constructors (`CConfigurationManagedBase(RegistryHive hive, string basePath,
  string instance)` and every subclass ctor) purely to pick a hive. `hive` was
  never stored or used — `CConfigurationManagedBase`'s `Get*Property`/
  `Set*Property` methods were all pure stage-1 no-op stubs (`return
  defaultValue;` / empty setter). So there was nothing "direct" to abstract
  away yet; the real work is implementing real logic for the first time,
  behind a new seam.
- `MicrosoftZuneLibrary/SafeBitmap.cs`'s `Microsoft.Win32` reference is
  `Microsoft.Win32.SafeHandles`, unrelated to the registry — left untouched.
- No subclass overrides any `Get*Property`/`Set*Property` — confirmed via
  `grep -rl override` over the folder returning nothing. So changing only the
  base class's implementation is sufficient; no subclass needed touching.
- Confirmed on this Linux dev box that `dotnet build ZuneDBApi/ZuneDBApi.csproj`
  already succeeded pre-change with 0 errors even though nothing here runs on
  Windows: `RegistryHive` (and, verified separately, `RegistryKey`/
  `Registry`/`RegistryValueKind`) are present in the plain `net8.0` reference
  assemblies on every OS — they only carry `[SupportedOSPlatform("windows")]`
  (source of the pre-existing CA1416 warnings on `ClientConfiguration.cs` /
  `MachineConfiguration.cs`), not a compile-time Windows requirement. This
  means the Windows-only implementation file below type-checks even on this
  Linux SDK; only ever *calling* its members outside Windows throws
  `PlatformNotSupportedException` at runtime, which is why gating it out of
  non-Windows builds still matters.

**Design (`Microsoft.Zune/Configuration/`):**

- `IRegistryProvider` — new interface: one `Get*Value`/`Set*Value` pair per
  property type `CConfigurationManagedBase` supports (bool, int, long, double,
  DateTime, string, string list, byte[]), plus `IDisposable`. This is the
  abstraction the task asked for; it's intentionally shaped around exactly
  what the base class needs rather than a generic registry-key wrapper, since
  nothing else in this project touches the registry.
- `RegistryProviderFactory.Create(RegistryHive, string subKeyPath)` — picks
  the implementation. Gated with `#if WINDOWS` (matching the existing
  precedent in `MicrosoftZuneInterop/QueryPropertyBag.cs`'s
  `FormatMessageW`/`LocalFree` gating) rather than a runtime
  `OperatingSystem.IsWindows()` check, for consistency with that established
  pattern in this codebase.
- `Win32RegistryProvider` (`#if WINDOWS`-gated file) — the real
  implementation, backed by `Microsoft.Win32.RegistryKey`. `double` and
  `DateTime` have no native registry type; both are round-tripped through a
  `QWord` (`BitConverter.DoubleToInt64Bits`/`Int64BitsToDouble` and
  `DateTime.ToBinary`/`FromBinary` respectively) rather than a
  culture-sensitive string, to stay within native registry value kinds.
  Compiled standalone in isolation (separate scratch csproj, `net8.0` TFM with
  `<DefineConstants>WINDOWS</DefineConstants>` forced, no other project
  references) to confirm it type-checks even though this dev box can't
  actually target `net8.0-windows` — 0 errors/warnings.
- `InMemoryRegistryProvider` — fallback for anything that isn't compiled with
  `WINDOWS` defined. Dictionary-backed, process-lifetime only (matches the
  previous no-op stub's *externally observable* behavior — nothing persists
  across runs — but unlike the stub, values now actually round-trip within a
  single process, which every property already assumed via its own
  hardcoded default parameter). Documented with a `// TODO` for a real
  cross-platform persistent backing (e.g. a config file) once non-Windows
  platform support is actually prioritized, per *Dealing with unknowns*.
- `CConfigurationManagedBase` now creates one `IRegistryProvider` per instance
  in its constructor via the factory, and every `Get*Property`/`Set*Property`
  delegates to it under the existing (previously unused) `m_lock` field.
  `Dispose(bool)` now disposes the provider. No public signature changed.

**Unknown (documented as `// TODO` in `RegistryProviderFactory.cs`, not
guessed further):** the real root registry path the original native
`ZuneDBApi.dll` used (something like `Software\Microsoft\Zune\...`) is not
yet recovered — would need a Ghidra pass over the native binary's string
constants passed to the Win32 registry APIs, which is a stage-1 decompilation
task, not this session's stage-3 abstraction work. Per *Dealing with unknowns
and uncertainty* step 4, used the fewest-predicates assumption
(`Software\Microsoft\Zune` — the standard `HKCU/HKLM\Software\Microsoft\
<Product>` convention) and documented it inline rather than leaving it
unstated.

**Pre-existing bug found and fixed in passing:** `Directory.Build.props`'s
`Choose` block only ever defined the `WINDOWS` constant for TFMs starting
with `net6.0-windows` or `net4*`. `net8.0-windows` — the actual Windows TFM
this solution adds today (`Directory.Build.props` line 22, only when
`$([MSBuild]::IsOsPlatform('Windows'))`) — matched neither branch, so
`WINDOWS` was silently never defined for it. This is a pre-existing gap (also
silently affecting `MicrosoftZuneInterop/QueryPropertyBag.cs`'s existing `#if
WINDOWS` block), not something introduced this session, but it would have
made the new `Win32RegistryProvider` (and the real `FormatMessageW` path in
`QueryPropertyBag`) permanently dead code on the one TFM meant to use them.
Added a third `When` branch defining `WINDOWS` for
`TargetFramework.StartsWith('net8.0-windows')`, mirroring the existing
`net4*` branch. Not verified against an actual Windows build (this box is
Linux) — flagging here in case a Windows checkout surfaces something this
reasoning missed.

**Known behavior change worth flagging, not silently swallowed:**
`ClientConfiguration`/`MachineConfiguration` eagerly construct every
`*Configuration` object as static field initializers. On a real Windows build
this now means each one immediately opens (or creates) its registry subkey —
including `MachineConfiguration`'s four `RegistryHive.LocalMachine` instances
— the first time either static class is touched. Previously this was
inert (stub base class). If the process lacks write access to
`HKLM\Software\Microsoft\Zune\...` (e.g. non-elevated), constructing
`MachineConfiguration` will now throw (surfaced as
`TypeInitializationException` from the static field initializer) instead of
silently no-op'ing. Not changed/mitigated here since it is literally the "real
working logic" stage-2/3 behavior the task asked for, and matches how the
real Windows registry behaves — but it's a genuine behavior change from the
stage-1 stub, so recording it rather than letting it be a surprise later.

**Verified:** `dotnet build ZuneDBApi/ZuneDBApi.csproj` (net8.0, non-Windows
constants): 0 errors, only pre-existing warnings in unrelated files.
`Win32RegistryProvider.cs` verified separately to type-check under
`WINDOWS` (see above) since it can't be exercised via the real
`net8.0-windows` TFM on this box.
