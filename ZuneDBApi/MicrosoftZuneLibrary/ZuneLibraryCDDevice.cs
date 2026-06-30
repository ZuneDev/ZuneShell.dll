using System;
using System.Runtime.InteropServices;
using System.Text;
using ZuneUI;

namespace MicrosoftZuneLibrary;

public class ZuneLibraryCDDevice : IDisposable
{
    private readonly object m_pDevice; // Stub for IWMPCDDevice*
    private readonly object m_pBurnPublisher; // Stub for IBurnPublisher*
    private int m_hrBurnPublisherCreate;
    private int m_fIsBurner = 0;
    private bool _disposed;
    private uint m_dwBurnAdviseCookie;

    public static bool IsImapiv2Installed => true;

    public EBurnState CurrentBurnState => EBurnState.ebsUnknown;

    public bool IsBurner => false;

    public long SpaceAvailable => 0L;

    public uint TimeAvailable => 0u;

    public bool IsDoorOpen => false;

    public bool IsDVD => false;

    public bool IsCDRW => false;

    public bool IsWriteable => false;

    public bool IsBlank => false;

    public bool IsDriveReady => false;

    public bool IsMediaLoaded => false;

    public char DrivePath => '\0';

    public string TOC => null;

    public virtual event OnQueryCancelHandler QueryCancelHandler;
    public virtual event OnSetDriveLockedForBurningHandler SetDriveLockedForBurningHandler;
    public virtual event OnBurnStateChangeHandler BurnStateChangeHandler;
    public virtual event OnItemErrorHandler ItemErrorHandler;
    public virtual event OnItemProgressHandler ItemProgressHandler;
    public virtual event OnSessionProgressHandler SessionProgressHandler;

    public int GetTrackUrl(uint dwTrackNum, StringBuilder strBuilder)
    {
        strBuilder.Length = 0;
        return -2147418113;
    }

    public HRESULT SetBurnPlaylist(int iPlaylistId)
    {
        return HRESULT._S_OK;
    }

    public HRESULT StartBurn()
    {
        return HRESULT._S_OK;
    }

    public int StopBurn()
    {
        return 0;
    }

    public HRESULT EraseDisc()
    {
        return HRESULT._S_OK;
    }

    public int SetActive([MarshalAs(UnmanagedType.U1)] bool fActive)
    {
        return 0;
    }

    public int Eject()
    {
        return -2147418113;
    }

    public HRESULT SetVolumeLabelW(string strVolumeLabel)
    {
        if (string.IsNullOrEmpty(strVolumeLabel))
        {
            return new HRESULT(-2147024809);
        }
        return HRESULT._S_OK;
    }

    public int Close()
    {
        return -2147418113;
    }

    internal void ItemProgress(int lMediaIndex, EBurnProgressStatus status, int nPercent)
    {
        ItemProgressHandler?.Invoke(lMediaIndex, status, nPercent);
    }

    internal void ItemError(int lMediaIndex, int hrError)
    {
        ItemErrorHandler?.Invoke(lMediaIndex, hrError);
    }

    internal void SessionProgress(int lSessionSecondsRemaining, int lTotalSessionSeconds)
    {
        SessionProgressHandler?.Invoke(lSessionSecondsRemaining, lTotalSessionSeconds);
    }

    internal void BurnStateChange(EBurnState burnState)
    {
        BurnStateChangeHandler?.Invoke(burnState);
        if (burnState == EBurnState.ebsStopped)
        {
            UnadviseForBurnPublisherEvents();
            SetActive(fActive: false);
        }
    }

    internal void SetDriveLockedForBurning([MarshalAs(UnmanagedType.U1)] bool fLocked)
    {
        SetDriveLockedForBurningHandler?.Invoke(fLocked);
    }

    internal unsafe void QueryCancel([MarshalAs(UnmanagedType.LPBool)] bool pfCancel)
    {
        QueryCancelHandler?.Invoke(&pfCancel);
    }

    private int AdviseForBurnPublisherEvents()
    {
        return 0;
    }

    private void UnadviseForBurnPublisherEvents()
    {
        m_dwBurnAdviseCookie = 0u;
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
    {
        if (P_0)
        {
            !ZuneLibraryCDDevice();
            return;
        }
        try
        {
            !ZuneLibraryCDDevice();
        }
        finally
        {
            base.Finalize();
        }
    }

    private void !ZuneLibraryCDDevice()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
    }

    public virtual sealed void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ZuneLibraryCDDevice()
    {
        Dispose(false);
    }
}
