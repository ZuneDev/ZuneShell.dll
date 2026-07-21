# HRESULT — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-20 — Full decompilation

**Source:** ILSpy `decompile_type` on `ZuneUI.HRESULT` in `ZuneShell/lib/ZuneDBApi.dll`
(mixed-mode C++/CLI assembly), via `mcp__ilspy__decompile_type` with query
"full source code including static field initializer int values, constructor,
and all method bodies verbatim". `get_type_members` was used first to confirm
the full field/method/property list before decompiling.

This is a pure managed struct (no native interop), so the ILSpy decompilation
is fully accurate and was used essentially verbatim. The original expresses
all 110 named HRESULT constants via a `static HRESULT()` static constructor
that assigns `.hr` on each field individually (a decompiler artifact of how
the C++/CLI compiler lowers static field initializers) — this was simplified
to ordinary C# field initializers (`public static readonly HRESULT _S_OK = 0;`
etc.), which compiles to equivalent IL.

**Value transcription:** The static constructor gives each constant as a
signed 32-bit decimal literal (e.g. `e_ABORT.hr = -2147467260;`). These were
copied verbatim as decimal literals into the field initializers — this was
deliberately chosen over hand-converting to hex, after an earlier attempt at
manual decimal-to-hex conversion (done without tooling, for "readability")
introduced errors in ~90% of the 110 constants, including one so broken it
left a non-compiling placeholder (`0xC0EA0099 + 8`) in the first draft. A
Python script was used to verify hex round-trips before the mistake was
caught and the file was reverted to plain decimal literals, which needed no
conversion step and therefore carry no transcription risk beyond copy-paste.

**Also simplified from the decompiler's raw output:**
- `Unsafe.SkipInit` + field assignment → direct field initializer.
- `(byte)((hrA.hr != hrB.hr) ? 1u : 0u) != 0` → plain `hrA.hr != hrB.hr`
  (the decompiler's convoluted bool-via-byte expression is an artifact of the
  `[return: MarshalAs(UnmanagedType.U1)]` attribute forcing an 8-bit bool
  representation at the ABI boundary; the C# `bool` expression is identical
  once compiled, and the `MarshalAs` attribute is preserved on the operator
  itself so the ABI-level representation is unaffected).
- `sealed override` → `override readonly` (struct member, `sealed` is
  redundant on a struct override and not idiomatic C#).

No unknowns; this type required no assumptions.
