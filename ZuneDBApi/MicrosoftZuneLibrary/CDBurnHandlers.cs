namespace MicrosoftZuneLibrary;

// Grouped together since each is a one-line delegate declaration recovered from
// ZuneLibraryCDDevice/ZuneLibraryCDDeviceList/ZuneLibraryCDRecorder's event fields —
// see logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public delegate void OnSessionProgressHandler(int lSessionTimeRemaining, int lSessionTotalTime);
public delegate void OnItemProgressHandler(int lMediaIndex, EBurnProgressStatus eStatus, int nPercent);
public delegate void OnItemErrorHandler(int lMediaIndex, int hrError);
public delegate void OnBurnStateChangeHandler(EBurnState eBurnState);
public delegate void OnSetDriveLockedForBurningHandler(bool fLocked);
public unsafe delegate void OnQueryCancelHandler(bool* pfCancel);
public delegate void OnMediaChangedHandler(char driveLetter, bool fMediaArrived);
public delegate void OnRecordStartHandler(string sourceUrl);
public delegate void OnRecordProgressHandler(string sourceUrl, int iTicks);
public delegate void OnRecordStopHandler(string sourceUrl, int hr);
public delegate void OnRecordPauseHandler(string sourceUrl);
public delegate void OnRecordResumeHandler(string sourceUrl);
