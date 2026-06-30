using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneInterop;

public class QueryPropertyBag : IDisposable
{
    private readonly object m_pBag; // Stub for IQueryPropertyBag*
    private bool _disposed;

    public IQueryPropertyBag* GetIQueryPropertyBag()
    {
        return (IQueryPropertyBag*)m_pBag;
    }

    internal QueryPropertyBag()
    {
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
    {
        if (P_0)
        {
            _disposed = true;
            return;
        }
        try
        {
            _disposed = true;
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

    ~QueryPropertyBag()
    {
        Dispose(false);
    }
}
