// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ETranscodeOptimization
{
    toOptimizeInvalid = -1,
    toOptimizeForSize = 0,
    toOptimizeForQuality = 1,
    toOptimizeForWVGA = 2,
}
