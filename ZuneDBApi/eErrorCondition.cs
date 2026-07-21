// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum eErrorCondition
{
    eEC_None = 0,
    eEC_SignIn = 1,
    eEC_Purchase = 2,
    eEC_Download = 3,
    eEC_WinLive = 4,
    eEC_FWUpdater = 5,
    eEC_Cart = 6,
    eEC_Last = 7,
}
