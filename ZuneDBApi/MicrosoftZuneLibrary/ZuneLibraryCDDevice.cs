using System;
using System.Text;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original wraps a native IWMPCDDevice*/IBurnPublisher* pair (CD/DVD burning via
// Windows Media Player's burn engine). Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class ZuneLibraryCDDevice : IDisposable
{
    internal ZuneLibraryCDDevice()
    {
    }

    public event OnQueryCancelHandler QueryCancelHandler;
    public event OnSetDriveLockedForBurningHandler SetDriveLockedForBurningHandler;
    public event OnBurnStateChangeHandler BurnStateChangeHandler;
    public event OnItemErrorHandler ItemErrorHandler;
    public event OnItemProgressHandler ItemProgressHandler;
    public event OnSessionProgressHandler SessionProgressHandler;

    public static bool IsImapiv2Installed => false;

    public EBurnState CurrentBurnState => EBurnState.ebsUnknown;

    public bool IsBurner => false;

    public long SpaceAvailable => 0;

    public uint TimeAvailable => 0;

    public bool IsDoorOpen => false;

    public bool IsDVD => false;

    public bool IsCDRW => false;

    public bool IsWriteable => false;

    public bool IsBlank => false;

    public bool IsDriveReady => false;

    public bool IsMediaLoaded => false;

    public char DrivePath => '\0';

    public string TOC => null;

    public int GetTrackUrl(uint dwTrackNum, StringBuilder strBuilder) => unchecked((int)0x80004005);

    public HRESULT SetBurnPlaylist(int iPlaylistId) => HRESULT._E_FAIL;

    public HRESULT StartBurn() => HRESULT._E_FAIL;

    public int StopBurn() => unchecked((int)0x80004005);

    public HRESULT EraseDisc() => HRESULT._E_FAIL;

    public int SetActive(bool fActive) => unchecked((int)0x80004005);

    public int Eject() => unchecked((int)0x80004005);

    public HRESULT SetVolumeLabelW(string strVolumeLabel) => HRESULT._E_FAIL;

    public int Close() => unchecked((int)0x80004005);

    internal void ItemProgress(int lMediaIndex, EBurnProgressStatus status, int nPercent)
    {
        ItemProgressHandler?.Invoke(lMediaIndex, status, nPercent);
    }

    internal void ItemError(int lMediaIndex, int hrError)
    {
        ItemErrorHandler?.Invoke(lMediaIndex, hrError);
    }

    internal void SessionProgress(int lSessonSecondsRemaining, int lTotalSessionSeconds)
    {
        SessionProgressHandler?.Invoke(lSessonSecondsRemaining, lTotalSessionSeconds);
    }

    internal void BurnStateChange(EBurnState burnState)
    {
        BurnStateChangeHandler?.Invoke(burnState);
    }

    internal void SetDriveLockedForBurning(bool fLocked)
    {
        SetDriveLockedForBurningHandler?.Invoke(fLocked);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
