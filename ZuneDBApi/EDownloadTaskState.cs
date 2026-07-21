// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum EDownloadTaskState
{
    DLTaskPendingAttach = 0,
    DLTaskPending = 1,
    DLTaskDownloading = 2,
    DLTaskPaused = 3,
    DLTaskCancelled = 4,
    DLTaskFailed = 5,
    DLTaskComplete = 6,
    DLTaskNone = 7,
}
