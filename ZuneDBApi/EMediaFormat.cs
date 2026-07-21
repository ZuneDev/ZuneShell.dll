// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this as a native [NativeCppClass] enum with no reflectable members,
// like DRMQueryState.cs — see that file's header comment for the full rationale).
//
// TODO: the only evidence of member values comes from integer literals cast to
// (EMediaFormat) at native call sites decompiled elsewhere in this assembly
// (0 through 5 were observed across album/track/video offer collection Init methods),
// but none of those call sites named the members. No code in this solution currently
// references named members of this enum (only the type itself, as a parameter in
// Service.GetContentUri), so recovering the real names is deferred to Ghidra analysis
// of the native definition. See logs/Microsoft.Zune/Service/Service.md.
public enum EMediaFormat
{
    Invalid = -1,
}
