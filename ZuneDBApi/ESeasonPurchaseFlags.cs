// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this as a native [NativeCppClass] enum with no reflectable members,
// like DRMQueryState.cs — see that file's header comment for the full rationale).
//
// TODO: only two integer literals are ever cast to this type, in
// VideoOfferCollection.Init's (corrupted — see logs/Microsoft.Zune/Service/OfferCollection.md)
// decompiled body: 1 (when the current purchase-tier format is not "season", i.e. a
// single-episode purchase) and 2 (when it is). Names below are inferred from that
// binary choice, not recovered. Recovering the real member names requires Ghidra
// analysis of the native definition.
internal enum ESeasonPurchaseFlags
{
    Episode = 1,
    Season = 2,
}
