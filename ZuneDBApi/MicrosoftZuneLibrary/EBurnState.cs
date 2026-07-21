namespace MicrosoftZuneLibrary;

public enum EBurnState
{
    ebsUnknown = 0,
    ebsBusy = 1,
    ebsReady = 2,
    ebsWaitingForDisc = 3,
    ebsPlaylistItemErrors = 4,
    ebsRefreshStatusPending = 5,
    ebsPreparingToBurn = 6,
    ebsBurning = 7,
    ebsStopped = 8,
    ebsErasing = 9,
}
