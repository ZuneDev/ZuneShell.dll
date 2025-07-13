namespace MicrosoftZuneLibrary;

public enum WlanTestResultCode
{
	wlanTestResultTCPIPConnectivityFailure = 9,
	wlanTestResultUDPConnectivityFailure = 8,
	wlanTestResultFailInternal = 7,
	wlanTestResultCancelled = 6,
	wlanTestResultTimeout = 5,
	wlanTestResultFailDhcp = 4,
	wlanTestResultFailAssociate = 3,
	wlanTestResultFailNoConfig = 2,
	wlanTestResultRunning = 1,
	wlanTestResultSuccess = 0
}
