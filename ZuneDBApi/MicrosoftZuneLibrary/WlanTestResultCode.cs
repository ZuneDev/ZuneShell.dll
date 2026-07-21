namespace MicrosoftZuneLibrary;

public enum WlanTestResultCode
{
    wlanTestResultSuccess = 0,
    wlanTestResultRunning = 1,
    wlanTestResultFailNoConfig = 2,
    wlanTestResultFailAssociate = 3,
    wlanTestResultFailDhcp = 4,
    wlanTestResultTimeout = 5,
    wlanTestResultCancelled = 6,
    wlanTestResultFailInternal = 7,
    wlanTestResultUDPConnectivityFailure = 8,
    wlanTestResultTCPIPConnectivityFailure = 9,
}
