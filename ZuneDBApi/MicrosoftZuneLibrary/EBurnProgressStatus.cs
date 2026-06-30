namespace MicrosoftZuneLibrary;

public enum EBurnProgressStatus
{
    ebsUnknown = 0,
    ebsMediaCheck = 1,
    ebsPreparerating = 2,
    ebsPregrooving = 3,
    ebsWriting = 4,
    ebsVerifying = 5,
    ebsFinalizing = 6,
    ebsCompleted = 7,
    ebsError = 8,
    ebsPaused = 9,
    ebsReady = 10,
    ebsStopped = 11
}
