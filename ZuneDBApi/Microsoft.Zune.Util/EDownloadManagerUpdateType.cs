namespace Microsoft.Zune.Util;

public enum EDownloadManagerUpdateType
{
	QueueResumed = 7,
	QueuePaused = 6,
	TaskProgressChanged = 5,
	TaskCancelled = 4,
	TaskFailed = 3,
	TaskAdded = 2,
	TaskCompleted = 1,
	Unknown = 0
}
