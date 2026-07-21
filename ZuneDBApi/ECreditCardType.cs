// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this as a native [NativeCppClass] enum with no reflectable members,
// like DRMQueryState.cs — see that file's header comment for the full rationale).
//
// Unlike EMediaRights/EMediaFormat, this one's *values* are not a guess: the original
// CreditCardCollection.Init reads this out of ICreditCardCollection::GetItem and
// immediately does `(CreditCardType)creditCardType` — a bare reinterpret cast with no
// translation table — so its members must be numerically identical to the public
// Microsoft.Zune.Service.CreditCardType enum for that cast to have been correct in the
// original. Only the *names* below are therefore a TODO (the native member identifiers
// are unrecoverable without Ghidra), not the values. See
// logs/Microsoft.Zune/Service/OfferCollection.md.
internal enum ECreditCardType
{
    Unknown = -1,
    Visa = 0,
    MasterCard = 1,
    AmEx = 2,
    Discover = 3,
    JCB = 4,
    Diners = 5,
    KLCC = 6,
}
