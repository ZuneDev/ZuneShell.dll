using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class ZunePlaylist : IDisposable
{
    private readonly object m_pPlaylist; // Stub for IPlaylist*
    private bool _disposed;

    internal IPlaylist* Playlist => (IPlaylist*)m_pPlaylist;

    private void ~ZunePlaylist()
    {
        !ZunePlaylist();
    }

    private void !ZunePlaylist()
    {
        if (!_disposed)
        {
            IPlaylist* pPlaylist = (IPlaylist*)m_pPlaylist;
            if (pPlaylist != null)
            {
                // Release stub
                m_pPlaylist = null;
            }
            _disposed = true;
        }
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
    {
        if (P_0)
        {
            !ZunePlaylist();
            return;
        }
        try
        {
            !ZunePlaylist();
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

    ~ZunePlaylist()
    {
        Dispose(false);
    }
}
