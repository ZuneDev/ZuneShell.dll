// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type as a native [NativeCppClass] enum with no reflectable
// members, unlike most other reimplemented types). Placed at the project root
// rather than under a namespace-mapped folder for that reason (see EMediaTypes.cs).
//
// TODO: member values 0/1/2/4 are inferred from their usage in DRMInfo's original
// decompiled body (m_canPlay == (DRMQueryState)0/1/2/4 map to ValidLicense/
// NoLicense/LicenseExpired/NotProtected respectively). Value 3 is not referenced
// anywhere in managed code and its name is unknown; recovering it requires Ghidra
// analysis of the native DRMQueryState definition. See logs/Microsoft.Zune/Service/DRMInfo.md.
public enum DRMQueryState
{
    ValidLicense = 0,
    NoLicense = 1,
    LicenseExpired = 2,
    NotProtected = 4,
}
