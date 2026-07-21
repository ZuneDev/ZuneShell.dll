# DRMQueryState — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Partial reconstruction from usage, not managed metadata

**Source:** ILSpy `decompile_type` on `DRMQueryState` in `ZuneShell/lib/ZuneDBApi.dll`
returns an empty `[NativeCppClass]` enum body (no reflectable members) — same
situation as `EMediaRights`/`EMediaFormat` (see `logs/Microsoft.Zune/Service/Service.md`).

**Recovery method:** `Microsoft.Zune.Service.DRMInfo`'s original decompiled body
(needed for `logs/Microsoft.Zune/Service/OfferCollection.md`'s `DRMInfo.cs`) compares
its `m_canPlay` field against four literal casts: `(DRMQueryState)0`, `(DRMQueryState)1`,
`(DRMQueryState)2`, and `(DRMQueryState)4`, mapped respectively to the public
`ValidLicense`, `NoLicense`, `LicenseExpired`, and `NotProtected` boolean properties.
This gives high-confidence names for four members purely from how they're consumed,
per CLAUDE.md's "assumption with fewest predicates, documented" allowance.

**Gap:** value `3` is never referenced anywhere in `ZuneDBApi.dll`'s managed code, so
its name is unknown and was not invented. `DRMQueryState.cs` (project root, global
namespace, same rationale as `EMediaTypes.cs`) only declares the four recovered members.

**Needs:** Ghidra analysis of the native `DRMQueryState` enum definition to confirm
these four names and recover the fifth (value 3).
