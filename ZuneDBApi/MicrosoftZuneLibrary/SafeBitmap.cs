using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class SafeBitmap : IDisposable
{
    private readonly IntPtr m_hBitmap;
    private bool m_disposed;

    public IntPtr Handle => m_hBitmap;

    public SafeBitmap(IntPtr hBitmap)
    {
        m_hBitmap = hBitmap;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing)
    {
        if (!m_disposed)
        {
            if (disposing && m_hBitmap != IntPtr.Zero)
            {
                // GDI delete stub
            }
            m_disposed = true;
        }
    }

    ~SafeBitmap()
    {
        Dispose(false);
    }
}
