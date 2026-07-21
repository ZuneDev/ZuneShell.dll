// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum EItemDownloadState
{
    eDownloadStateNone = 0,
    eDownloadStateError = 1,
    eDownloadStateDownloading = 2,
    eDownloadStateDownloaded = 3,
}
