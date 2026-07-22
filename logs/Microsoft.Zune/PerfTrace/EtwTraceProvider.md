# PerfTrace ETW provider — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Switched EtwTraceProvider to System.Diagnostics.Tracing.EventSource (stage 2 reimplementation)

**Task:** user asked to act on the findings from the entry below and actually
switch `EtwTraceProvider`'s internals from the raw `advapi32` P/Invokes to
`EventSource`, following the project's usual procedure (verify against real
APIs before committing to a design, keep the consumed surface stable, build,
log).

**Verification before writing any code:** rather than trust half-remembered
`EventSource` API shapes, compiled a series of throwaway probes
(`/tmp/.../scratchpad/eventsource-probe`) against the actual `net8.0`
`EventSource` in this SDK (`dotnet 8.0.28`) and inspected it via reflection.
Findings that changed the design from my first assumption:

- `EventSource` **does** have `(Guid, string)` / `(Guid, string,
  EventSourceSettings, string[])` constructors, but reflection
  (`ConstructorInfo.IsAssembly`) shows both are `internal` to
  `System.Private.CoreLib` — not callable from a subclass via `base(...)`.
  The only constructors a subclass can actually call are the `string
  name[, EventSourceSettings[, string[] traits]]` and parameterless/`bool`/
  `EventSourceSettings`-only overloads. So there is **no supported way to
  set an explicit provider Guid at runtime** — `EventSource` always derives
  the Guid deterministically from the provider *name* (or a compile-time
  `[EventSource(Guid = "...")]` attribute, which can't take a value computed
  from a constructor parameter). Documented as a `// TODO` in
  `EtwEventSource`'s ctor rather than silently assumed.
- `EventSource.Level` / `EventSource.MatchAnyKeyword` — the protected
  properties I expected subclasses to read the "currently enabled
  level/keywords" from — **do not exist** on modern (.NET 8)
  `EventSource`; only the private backing fields `m_level`/
  `m_matchAnyKeyword` do (confirmed via `GetMembers` over
  `Public|NonPublic|Instance`). Likewise `EventCommandEventArgs`'s
  `level`/`matchAnyKeyword` fields are private, not exposed to
  `OnEventCommand` overrides. The only supported way to query enablement is
  the boolean `IsEnabled()` / `IsEnabled(EventLevel, EventKeywords)` methods.
  This made the original plan (mirror `Level`/`Flags` as gettable
  byte/uint properties, populated from an `OnEventCommand` override just
  like the original `ControllerChangeCallback`) impossible to implement
  honestly, so it was dropped — see the design section below.
- `EventSource.Write<T>(string?, EventSourceOptions, T)` (self-describing,
  non-`ref`, no struct constraint) does exist and compiles cleanly against a
  plain struct with public get-only properties. This is what carries the
  original's `data0..data8` payload now.
- `EventLevel`'s underlying values (`LogAlways=0, Critical=1, Error=2,
  Warning=3, Informational=4, Verbose=5`) line up exactly with
  `PerfTrace.Level`'s (`Fatal=1..Verbose=5`), so `(EventLevel)level` /
  `(byte)eventLevel` round-trip with no translation table needed.

**Design (`ZuneShell/Microsoft/Zune/PerfTrace/EtwTraceProvider.cs`):**

- Constructor signature, `TraceEvent` overload family (0 through 8 explicit
  `data` args, all delegating to the 9-arg/`uint`-returning master method),
  and the `IsEnabled` bool property are unchanged — `PerfTrace.cs`'s call
  sites didn't need to change.
- `internal byte Level` / `internal uint Flags` properties are **removed** —
  per the verification above, there is no way to source them from a real
  `EventSource`. In their place: `internal bool IsLevelEnabled(byte level,
  uint flags)`, a new method (can't reuse the name `IsEnabled` — C# doesn't
  allow a property and method to share a name) that forwards straight to
  `EventSource.IsEnabled(EventLevel, EventKeywords)`. This is actually a
  more correct implementation of what `PerfTrace.IsEnabled`'s hand-rolled
  formula (`level <= provider.Level && (flag & provider.Flags) != 0`,
  including its `0/0 means enable everything` special case) was already
  trying to approximate — `EventSource`'s own `IsEnabled` implements that
  exact "any-match keyword, level-or-more-severe" semantics natively.
- `PerfTrace.cs`'s three `IsEnabled` overloads keep their exact signatures;
  only the one real implementation's body changed to `provider.IsEnabled &&
  provider.IsLevelEnabled((byte)level, (uint)flag)`. `internal`,
  only-called-within-this-file, so no external-compat concern per CLAUDE.md's
  signature-preservation rule.
- All native-interop plumbing that existed purely to talk to raw `advapi32`
  (`ControllerChangeCallback`, `Register`, `ProcessOneObject`/`EncodeObject`'s
  byte-packing switch, the `MofField`/`BaseEvent`/`EtwProc`/
  `TraceGuidRegistration`/`CSTRACE_GUID_REGISTRATION`/`RequestCodes` nested
  types, the finalizer's `UnregisterTraceGuids` call, `_version` which was
  already dead in the original decompilation) is deleted outright rather than
  kept dark — grepped the whole repo first and confirmed nothing outside this
  one file ever referenced any of it. `unsafe`/`stackalloc`/pointer code is
  gone from this file entirely.
- New private nested types: `EtwTracePayload` (9-string-field self-describing
  struct, one per original MOF data slot) and `EtwEventSource : EventSource`
  (owns the `IsLevelEnabled` passthrough and the `OnEventCommand`-free
  enablement check). `FormatField` replaces `EncodeObject`: same enum→
  underlying-type conversion as the original (so an enum arg still traces as
  its numeric value, not its symbolic name), but formats to a
  `CultureInfo.InvariantCulture` string instead of packing raw bytes, since
  the payload is now string fields, not a MOF byte buffer.
- No finalizer anymore: the original's finalizer directly released an
  unmanaged handle (`UnregisterTraceGuids(_registrationHandle)`), which is
  the correct finalizer pattern. `EtwTraceProvider` no longer owns any
  unmanaged handle itself — `_eventSource` is a managed `IDisposable` that
  already finalizes itself. Adding a finalizer here that calls
  `_eventSource.Dispose()` would touch another finalizable object from a
  finalizer, a well-known anti-pattern (finalization order across unrelated
  objects isn't guaranteed), so it was deliberately not carried forward.

**Known, flagged behavior change (not silently swallowed):** the actual ETW
provider Guid a native Windows tracing controller would see is no longer
`PerfTrace.ZUNE_ETW_CONTROL_GUID` — `EventSource` derives its own Guid from
the provider name (`$"Zune-{controlGuid:N}"` here), and there is no supported
public API to override that. Anyone re-enabling tracing by referencing the
old literal control Guid (e.g. via `logman`/`wevtutil` on a real Windows box)
will need the new derived name instead. This is an inherent consequence of
moving off raw MOF ETW registration onto `EventSource`, not a workaround —
documented both here and as a `// TODO` next to `EtwEventSource`'s
constructor per CLAUDE.md's *Dealing with unknowns and uncertainty*.

**Verified:** `dotnet build ZuneShell/ZuneShell.csproj` (net8.0): 0 errors.
Only pre-existing warning categories present (`CA1416` on the
`RegistryHive.LocalMachine` call already flagged the same way elsewhere in
this codebase since the earlier registry-abstraction work, `CS0168`/`CS0067`/
etc. in unrelated files). Grepped the whole repo afterward for every removed
symbol name (`RequestCodes`, `MofField`, `TraceGuidRegistration`,
`CSTRACE_GUID_REGISTRATION`, `EtwProc`, `ControllerChangeCallback`,
`RegisterTraceGuids`, `GetTraceEnableFlags`, `GetTraceEnableLevel`) to confirm
nothing outside this file/log referenced them.

---

## 2026-07-21 — advapi32 → cross-platform/Linux equivalent (question, no code changed)

**Task:** user asked what the cross-platform or Linux equivalent to
`advapi32` is, pointing at `ZuneShell/Microsoft/Zune/PerfTrace/EtwTraceProvider.cs`
for the APIs actually in use. Research/answer only — user is implementing
the change themselves, this entry just records the findings for reference.

**APIs found in `EtwTraceProvider.cs` (all `[DllImport("advapi32", ...)]`):**

- `RegisterTraceGuidsW` (entry point explicitly pinned; the `RegisterTraceGuids`
  managed name is an alias)
- `UnregisterTraceGuids`
- `GetTraceEnableFlags`
- `GetTraceEnableLevel`
- `TraceEvent` (the raw MOF/`EVENT_TRACE_HEADER`-based classic ETW call, not
  to be confused with `System.Diagnostics.Tracing.EventSource`'s method of
  the same name)

These are all classic, MOF-based Event Tracing for Windows (ETW) provider
APIs — the pre-manifest-based ETW model (superseded on Windows itself by
`EventRegister`/`EventWrite` in `advapi32` and, in managed code, by
`System.Diagnostics.Tracing.EventSource`). `advapi32.dll` is not a
single-purpose library — it also hosts the registry, SCM, and security/ACL
APIs — so there is no single 1:1 native DLL replacement on Linux; the answer
is scoped to the ETW/tracing subsystem specifically, since that's the only
part of `advapi32` this type touches.

**Findings / options identified:**

- `System.Diagnostics.Tracing.EventSource` (BCL, fully managed, already
  cross-platform, no P/Invoke required) is .NET's own successor to raw ETW.
  On Windows it still writes to ETW; on Linux/macOS it writes via
  **EventPipe** (same mechanism `dotnet-trace` consumes), and CoreCLR can
  bridge `EventSource` output to **LTTng** user-space tracepoints on Linux.
  This satisfies the project's "make as few platform assumptions as
  possible" guidance better than `#if WINDOWS`-gating the raw P/Invokes,
  since the OS split is already handled inside the runtime rather than by
  this codebase.
- **LTTng** (`liblttng-ust` / `lttng-tools`) is the closest *native* Linux
  analog to the classic ETW provider/GUID/level model this type implements
  (runtime-enabled providers, session control, verbosity levels), if a
  lower-level-than-`EventSource` native path were ever needed.

**Not yet decided/implemented:** user is writing the actual change
themselves; this entry is reference-only. No code touched, no assumption
requiring a `// TODO` was introduced.
