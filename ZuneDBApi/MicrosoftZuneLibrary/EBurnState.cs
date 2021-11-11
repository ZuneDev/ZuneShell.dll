namespace MicrosoftZuneLibrary
{
	public enum EBurnState
	{
		ebsErasing = 9,
		ebsStopped = 8,
		ebsBurning = 7,
		ebsPreparingToBurn = 6,
		ebsRefreshStatusPending = 5,
		ebsPlaylistItemErrors = 4,
		ebsWaitingForDisc = 3,
		ebsReady = 2,
		ebsBusy = 1,
		ebsUnknown = 0
	}
}
