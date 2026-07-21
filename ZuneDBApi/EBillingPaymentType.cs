// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this as a native [NativeCppClass] enum with no reflectable members,
// like DRMQueryState.cs — see that file's header comment for the full rationale).
//
// TODO: CreditCardCollection.Init reads this out of ICreditCardCollection::GetItem
// but never uses the value afterward (the constructed CreditCard object only carries
// ECreditCardType, not this field) — no integer literal or comparison against it
// appears anywhere in managed code, so neither values nor names can be inferred.
// Recovering them requires Ghidra analysis of the native definition. See
// logs/Microsoft.Zune/Service/OfferCollection.md.
internal enum EBillingPaymentType
{
    Invalid = -1,
}
