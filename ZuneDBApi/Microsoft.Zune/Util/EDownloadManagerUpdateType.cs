namespace Microsoft.Zune.Util;

public enum EDownloadManagerUpdateType
{
    Unknown = 0,
    TaskCompleted = 1,
    TaskAdded = 2,
    TaskFailed = 3,
    TaskCancelled = 4,
    TaskProgressChanged = 5,
    QueuePaused = 6,
    QueueResumed = 7,
}
