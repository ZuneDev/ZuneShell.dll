# IFileAssociationHandler — stage 3 cross-platform implementation

Append-only. Do not edit previous entries.

---

## 2026-07-28 — Cross-platform IFileAssociationHandler (stage 3, user-directed)

**Task:** user explicitly asked to skip stage 1 (native decompilation) for this
surface and move straight to stage 3: implement `IFileAssociationHandler`
(`Microsoft.Zune.Configuration`) with real, working, cross-platform logic.

**State found before changing anything:**

- `IFileAssociationHandler` (3 methods: `CanAssociationBeChanged`,
  `GetFileAssociationInfoList`, `SetFileAssociationInfo`) and
  `FileAssociationInfo` (immutable record-like class with a mutable
  `IsCurrentlyOwned`) were already decompiled and untouched — kept as-is.
- `FileAssociationHandlerFactory.CreateFileAssociationHandler()` was a stub
  throwing `NotImplementedException`.
- `FileAssociationHandlerWrapper` (`internal`, implements both
  `IFileAssociationHandler` and `IDisposable`) is a stage-1 decompiled stub
  whose shape (`FileInfoToStruct(FileAssociationInfo, IntPtr)`,
  `CleanupFileInfoArray(IntPtr, uint)`, finalizer) strongly suggests it
  originally wrapped a native COM/interop object marshaling a
  `FileAssociationInfo`-shaped native struct array. Reimplementing that
  faithfully would require recovering the native struct layout via Ghidra —
  explicitly out of scope since the user asked to skip stage 1 for this
  feature. Left this file untouched; the new implementation below does not
  build on it at all, it's a fresh stage-3 abstraction sitting behind the
  same public `IFileAssociationHandler`/`FileAssociationHandlerFactory`
  surface (which stage 2/3 rules require preserving byte-for-byte).
- Only consumer in the tree is `ZuneUI.Management`
  (`ZuneShell/ZuneUI/Management.cs`), which: reads `_defaultFileTypeExtensions`
  (`.mp3`, `.m4a`, `.mp4`, `.m4b`, `.m4v`, `.mbr`, `.zpl`), calls
  `GetFileAssociationInfoList`/`SetFileAssociationInfo`/`CanAssociationBeChanged`,
  and only special-cases `EMediaTypes.eMediaTypeAudio`/`eMediaTypeVideo` in its
  switch (everything else just lands in the flat "all file types" list).
- `Microsoft.Iris.Data.Registry.IRegistryProvider`/`RegistryProviderFactory`
  (moved into `UIX.csproj` in an earlier session, see
  `RegistryAbstraction.md`) is already reachable from `ZuneDBApi` and is
  product-agnostic — reused directly for the Windows implementation instead of
  touching `Microsoft.Win32` directly.

**Design:**

- `ZuneFileAssociations` (new, internal) — single shared table of
  `(Extension, ProgId, Description, MediaType, MimeType)` entries, consumed by
  every platform handler so they only need to know *how* to read/write one
  association, not *which* extensions exist. Per *Dealing with unknowns and
  uncertainty* step 4 (fewest predicates + documented in code): the extension
  set and `EMediaTypes` classification are grounded in evidence already
  present in this repo, not invented —
  - extensions == `ZuneUI.Management._defaultFileTypeExtensions` exactly (a
    real decompiled array, not a guess);
  - `.mbr` → Video and `.zpl` → Playlist are confirmed by
    `EMediaTypes.eMediaTypeVideoMBR` / `eMediaTypePlaylistZPL`
    (`ZuneDBApi/EMediaTypes.cs`, already decompiled);
  - `.mp3`/`.m4a`/`.m4b` → Audio and `.mp4`/`.m4v` → Video follow their
    unambiguous, standard container formats.
  - `ProgId` (`"ZuneShell.<ext>"`) and `Description` (`"<EXT> File"`) are
    **not** recovered originals — placeholders following the standard
    per-extension-ProgId convention, documented inline via XML doc `<remarks>`
    on `ZuneFileAssociations`, not silently presented as fact.
  - `MimeType` is populated only where a type is unambiguously registered
    with shared-mime-info/IANA (`audio/mpeg`, `audio/mp4`, `video/mp4`,
    `audio/x-m4b`); `.mbr`/`.zpl` have no registered type and are left
    `null` — the Linux handler skips those two rather than inventing a
    vendor MIME type that isn't actually registered anywhere.

- `WindowsFileAssociationHandler` (`#if WINDOWS`-gated, matching the existing
  precedent in `RegistryProviderFactory`) — real logic via
  `HKCU\Software\Classes\<ext>` (default value = ProgId) +
  `HKCU\Software\Classes\<ProgId>\shell\open\command` (points at
  `Environment.ProcessPath`), through `IRegistryProvider`. Deliberately does
  **not** attempt to write the Vista+
  `...\Explorer\FileExts\<ext>\UserChoice` key: since Windows 8 that key is
  protected by an undocumented per-write hash Explorer verifies, so
  programmatic writes to it are silently ignored by the shell — this is
  well-documented, verifiable OS behavior (not a project-specific guess), and
  is exactly the kind of constraint `CanAssociationBeChanged()` exists to
  report. `CanAssociationBeChanged()` itself is a **functional probe**
  (attempt to open `HKCU\Software\Classes` for write) rather than an
  OS-version check, since that's what actually determines whether
  `SetFileAssociationInfo` can do anything, and keeps the implementation from
  asserting a specific Windows-version cutoff it doesn't have verified
  evidence for.

- `XdgFileAssociationHandler` (Linux, selected at runtime via
  `RuntimeInformation.IsOSPlatform(OSPlatform.Linux)` in the factory —
  matching the pattern already used for OS dispatch in
  `ZuneShell/ZuneUI/WebHelpCommand.cs`, as opposed to the `#if WINDOWS`
  compile-time gating used for the registry code, since this needs a
  three-way Windows/Linux/other split rather than a two-way one) — shells out
  to the freedesktop.org `xdg-utils` `xdg-mime` CLI (`query default
  <mime>` / `default <desktop-file> <mime>`). `CanAssociationBeChanged()` is
  again a functional probe: whether `xdg-mime` resolves on `PATH`.
  **Known limitation, documented via `// TODO` and XML `<remarks>` in the
  file, not silently swallowed:** `xdg-mime` has no "clear default"
  operation, only "set default" — so un-checking a file type in
  `SetFileAssociationInfo` cannot be honored on Linux (only `IsCurrentlyOwned
  == true` entries are acted on); and no `.desktop` file is shipped by this
  repo/packaging yet, so the `DesktopFileId` constant (`zuneshell.desktop`)
  this class records associations against does not resolve to an installed
  application yet — that's a packaging task, not something this API surface
  can fix.

- `NullFileAssociationHandler` (new) — honest no-op for any OS that is
  neither Windows nor Linux (macOS today). Reports
  `CanAssociationBeChanged() == false` and an empty association list rather
  than throwing, so `ZuneUI.Management`'s settings UI degrades gracefully
  instead of crashing. `// TODO` left for a real macOS Launch Services
  handler (`duti`/`lsregister`) per CLAUDE.md's *Dealing with unknowns and
  uncertainty* — no macOS-specific research was done this session.

- `FileAssociationHandlerFactory.CreateFileAssociationHandler()` now wires all
  three together (`#if WINDOWS` → `WindowsFileAssociationHandler`; else
  runtime Linux check → `XdgFileAssociationHandler`; else →
  `NullFileAssociationHandler`). No public signature changed.

**Verified:** `dotnet build ZuneDBApi/ZuneDBApi.csproj` (net8.0): 0 errors, no
new warnings (only pre-existing CA1416/CS0649/CS0067 elsewhere).
`dotnet build ZuneShell/ZuneShell.csproj` (net8.0, the actual consumer of
`IFileAssociationHandler` via `ZuneUI.Management`): 0 errors. The
`#if WINDOWS`-gated `WindowsFileAssociationHandler` could not be
build-verified on this Linux box (no `net8.0-windows`/`net472` TFM available
here, same limitation already noted in `RegistryAbstraction.md`) — reviewed
by inspection only, and it's a straightforward, small consumer of
`IRegistryProvider`/`RegistryProviderFactory`, which were already
build-verified for `WINDOWS` in that earlier session.

**Unknowns not resolved this session (documented, not guessed further):**
the real original `ProgId`/description strings and the full native extension
table (if it differs from `_defaultFileTypeExtensions`) remain unrecovered —
would need a Ghidra pass over the native `ZuneDBApi.dll`'s string constants,
which is explicitly the stage-1 work the user asked to skip for this feature.
