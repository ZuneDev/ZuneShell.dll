using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicrosoftZuneLibrary;

public class ZuneLibraryCDDeviceList : IDisposable
{
    private readonly object m_pDeviceList; // Stub for IWMPCDDeviceList*
    private uint m_dwAdviseCookie;
    private bool m_fAdvised;
    private bool m_disposed;

    private OnMediaChangedHandler m_MediaChangedHandler;
    private int m_RefCount;

    public int Count => 0;

    [System.Runtime.CompilerServices.SpecialName]
    public virtual event OnMediaChangedHandler MediaChangedHandler
    {
        add { m_MediaChangedHandler = (OnMediaChangedHandler)System.Delegate.Combine(m_MediaChangedHandler, value); }
        remove { m_MediaChangedHandler = (OnMediaChangedHandler)System.Delegate.Remove(m_MediaChangedHandler, value); }
    }

    internal ZuneLibraryCDDeviceList()
    {
        m_fAdvised = false;
        m_disposed = false;
        m_RefCount = 0;
    }

    private void ~ZuneLibraryCDDeviceList()
    {
        m_MediaChangedHandler = null;
        !ZuneLibraryCDDeviceList();
    }

    private void !ZuneLibraryCDDeviceList()
    {
        if (m_disposed)
        {
            return;
        }
        if (m_fAdvised)
        {
            m_fAdvised = false;
        }
        m_pDeviceList = null;
        m_disposed = true;
    }

    public uint AddRef()
    {
        return (uint)Interlocked.Increment(ref m_RefCount);
    }

    public uint Release()
    {
        int num = Interlocked.Decrement(ref m_RefCount);
        if (num == 0)
        {
            !ZuneLibraryCDDeviceList();
        }
        return (uint)num;
    }

    public ZuneLibraryCDDevice GetItem(int idx)
    {
        if (idx >= Count)
        {
            return null;
        }
        return null;
    }

    internal void OnMediaChanged(char driveLetter, bool fMediaArrived)
    {
        m_MediaChangedHandler?.Invoke(driveLetter, fMediaArrived);
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
    {
        if (P_0)
        {
            ~ZuneLibraryCDDeviceList();
            return;
        }
        try
        {
            !ZuneLibraryCDDeviceList();
        }
        finally
        {
            base.Finalize();
        }
    }

    public virtual sealed void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ZuneLibraryCDDeviceList()
    {
        Dispose(false);
    }
}
