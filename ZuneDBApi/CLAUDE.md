# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with
code in this repository, specifically ZuneDBApi.

## What this project is

`ZuneDBApi` is one project inside the larger `ZuneShell.dll` solution
(root: `..`), which is a reimplementation of the original Microsoft Zune
desktop software. The dependency chain is:

```
ZuneDBApi (this project) → ZuneImpl → ZuneShell.dll → ZuneHost / ZuneHost.Wpf (entry points)
```

Note that Iris UI files (both `.uix` and `.uib`) can load arbitrary assemblies.
This meaans that `ZuneShell.dll` has a direct dependency on `ZuneDBApi`, even
though according to the C# project system, `ZuneDBApi` is only a transitive
dependency.

ZuneDBApi is the lowest layer with managed code: it re-creates the public API
surface of the original closed-source `Microsoft.Zune.*` /
`MicrosoftZuneInterop` / `MicrosoftZuneLibrary` assemblies that shipped with
the real Zune software, so the rest of the stack (`ZuneImpl`, `ZuneShell`) can
compile and run against a familiar API without bundling Microsoft's original
binaries. It also unlocks modifications and potentially even cross-platform
support. When writing new code, make as few assumptions about the platform
as possible. If platform-specific code is unavoidable without hacks or relying
on fragile behavior, wrap the code with the appropirate `#if PLATFORM` directive
and write a `// TODO` comment to implement the other platforms.

Namespace-to-folder mapping is strict and should be preserved when adding files:
- `Microsoft.Zune.Configuration` → `Microsoft.Zune/Configuration/`
- `Microsoft.Zune.Service` → `Microsoft.Zune/Service/`
- `Microsoft.Zune.ErrorMapperApi` → `Microsoft.Zune/ErrorMapperApi/`
- `Microsoft.Zune.Util` → `Microsoft.Zune/Util/`
- `MicrosoftZuneInterop` → `MicrosoftZuneInterop/`
- `MicrosoftZuneLibrary` → `MicrosoftZuneLibrary/`
- `ZuneUI` (HRESULT, attributes) → `ZuneUI/`

## Project goals

The goal of this project is to revive Zune and make it interoperable with
modern tools and platforms. This requires reverse engineering the original
software and reimplementing it in a way that is maintainable and makes as few
assumptions about the target platform as possible.

## Project stages

This revival effort is realized in distinct phases:

1. **Decompilation:** investigating the original code, understanding how it works
and fits into the rest of the stack. High-quality decompilations, such as those
produced for pure C# code, may be used as a starting point for future stages.
2. **Implementation:** reimplementing the original API surface with code that
is maintainable but has the same features. It is imperative that the new
implementation is entirely backwards-compatible and can function as a drop-in
replacement for the original libraries. The project at this stage is more
manageable but naturally has the similar requirements and limitations to the
original.
3. **Enhancement:** adding new features such as additional platform support
or OS integration. This may require extending existing abstractions or
creating new ones for the sake of modularity.

**Strict adherence to these phases is required**. To ensure stability and
quality, each stage is highly dependent on the one before. When decompiling
an assembly or type, do not attempt to use new abstractions. Simply write
equivalent code, such that the new implementation is a drop-in replacement
for the original. Similarly, the enhancement stage must not introduce any new
platform-specific code without being gated behind proper conditionals and
accessed via suitable abstractions.

ZuneShell.dll and the Microsoft.Iris libraries are mostly in stage 2, going on
stage 3, but ZuneDBApi is currently in stage 1. You first need to decompile
the assembly, then implement it using the rules outlined in the rest of this
document.

## Dealing with unknowns and uncertainty

**Do not make assumptions**. If you have a question about something,
follow this procedure:

1. **Log your question.** Use `LOG.md` in the repo root to log any questions
or confusions. Follow the instructions in *Logging*.
2. **Ask the human.** They've been working on Zune longer than you have. They may
have an immediate answer for you, or at least can narrow down your search.
3. **Search the Wiki.** Some information has already been documented in the
ZuneDev Wiki located in the following repo on GitHub:
http://github.com/ZuneDev/Wiki
4. **Move on.** If you still haven't gotten a concrete, verifiable answer,
you may make whatever assumption is both likely and has the fewest predicates,
**if and only if you document this assumption in the code**, using `// TODO`
comments. You are also required to log this assumption and its corresponding
rationale in `LOG.md`.

### STAGE 1: Decompilation

Reliance on the original libraries is a no-go, as these binaries are only
available for x86 and x86_64 Windows. Certain components of the Zune software
have already been reimplemented (see stage 2 of *Project Goals*), including
`ZuneShell.dll` and `Microsoft.Iris`, but all of the original native
libraries are stuck at the start of stage 1.

#### Managed assemblies

Pure managed assemblies, such as those written in C#, can be inspected using
ILSpy or the `mcp__ilspy__*` tools. Reverse engineering these assemblies is
typically quite easy, as the decompilation is very accurate. Such
decompilations are suitable starting points for stages 2 and 3.

#### Native assemblies

Native or unmanaged assemblies, such as those written in C or C++, can be
inspected using Ghidra or the `ghidra-mcp` tools. Decompilations from these
assemblies should not be used verbatim, as their outputs are usually
low-quality and are not readable, but information can be extracted from
them.

#### Mixed-mode assemblies

Mixed-mode assemblies, such as those written in C++/CLI, contain both managed
and unmanaged code. ILSpy and its corresponding MCP tools may be used to
inspect the public surface of these assemblies, but implementation details
are likely to be missing. A combination of Ghidra and ILSpy may be required
to fully reverse engineer such assemblies. Decompilations of mixed-mode
assemblies should only be used as-is if the specific code does not touch any
native or unmanaged code, either internally or externally.

### STAGE 2: Implementation

All initial implementations must be backwards-compatible with the original
API surfaces. This means reimplemented modules must have the same public
type/method/property signatures as the original (so dependent code compiles
and links correctly). Searching through the existing codebase to justify
changing the signature is **expressly forbidden**, as there are additional
dependencies that will not be obvious. You are required to match the original
API. The actual body of these surfaces can follow one of the
following conventions:

- **Pure no-op stub** — method sets `out` params to defaults and returns a
success result, typically `HRESULT._S_OK`.
- **Real working logic** — logic is implemented and matches the original
behavior.

Whenever possible, real logic is preferred over the former: stubs require
more work in the future by a human, which can make the revival effort take
longer to become useful. Real, working business logic makes future work much
easier because the code is much more self-documenting, requiring less
consultation with outputs from previous stages.

#### COM objects

When a reimplementation needs to represent an unmanaged COM pointer the
original held (e.g. `IPlaylist*`, `IQueryPropertyBag*`), do **not** invent
raw C-style pointer types or C++-style destructor syntax (`~Foo()` as an
ordinary method, or `!Foo()`) — neither compiles in C#.

When a class holds a COM pointer, follow these steps in order:

1. Rewrite the decompiled code using `IntPtr` or `SafeHandle` with proper
`IDisposable` and finalizer logic.
2. Reconstruct the methods on the corresponding COM interface in such a way
that it is compatible with `Marshal.GetObjectForIUnknown`, ideally using
source-generated COM. Note that for some Zune-specific interfaces, you may not
need to locate a GUID or support `CoCreateInstance`, as some objects are
instantiated in the native libraries. Follow the rules in *Dealing with
unknowns and uncertainty*.
3. Update the initial reimplementation to use the new modern COM structure.
Ensure you do not affect the public surface of the class; external code may
depend on the exact shape of the original API.

These steps are the only acceptable "enhancement" for stages 1 and 2.

### STAGE 3: Enhancement

Once parity has been restored with the original software, enhancements can be
made to the codebase. This may involve creating new abstractions for platform-
specific features, or implementing existing interfaces to extend support.

New features may be added, such as extended support for newer codecs, syncing
with other MTP devices like modern smartphones, or integrating new Social
features from ActivityPub and ListenBrainz scrobbling.

## Build

```bash
# From repo root or this directory — build just this project:
dotnet build ZuneDBApi/ZuneDBApi.csproj

# Build the whole solution (Windows-only platforms exist in ZuneShell.sln;
# on Linux only net8.0 TFMs build):
dotnet build ZuneShell.sln
```

No test projects are currently available, neither for the entire project or
just ZuneDBApi.

Note that building will fail on files containing invalid C++-style syntax
(see *COM objects*) — running a build will surface any remaining instances;
treat new ones the same way (fix to valid C#, do not silently work
around them).

## Working in this project

- `Nullable` and `ImplicitUsings` are enabled; `AllowUnsafeBlocks` is true
(file-scoped namespaces are fine).
- This project only references `UIX.csproj` (from the
`MicrosoftIris`/`ZuneUIXTools` submodule) — keep it free of dependencies on
`ZuneImpl`/`ZuneShell` to avoid circular references, since those layers depend
on it.
- `Microsoft.Win32.RegistryHive`/`Registry*` APIs used by the `Configuration`
classes are Windows-only; this is expected since the original Zune client was
Windows-only, but keep cross-platform buildability in mind for the `net8.0`
(non-`-windows`) TFM. When working on stage 3, avoid adding new Windows-only
APIs outside what's already used.

### Logging

You are expected to log your thought process in the `logs` directory in the
repo root. You may create any subdirectories and as many different log files
as you see fit. These logs must contain your thought process for everything.
The human will review this document: provide any and all useful context
for your query. Most importantly, **do not edit previous content**; all log
files are *prepend-only*. This also means your log will be in reverse
chronological order.

If you were unable to figure something out using the procedure in *Dealing with
unknowns and uncertainty*, log your observations and unknowns; **do not log
guesses or hunches**.

If you confidently made a change, explain why the change was warranted and back
it up with references to documentation or other known-good sources. **Do not**
depend on sources that themselves contain uncertainty. This propagates and
compounds errors, making development unnecessarily difficult.
