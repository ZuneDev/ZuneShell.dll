using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class CallbackOnUIThread : IDisposable
{
    private readonly IRequestCallbackOnUIThread _pUI;
    private bool _disposed;

    public CallbackOnUIThread(IRequestCallbackOnUIThread pUI)
    {
        _pUI = pUI;
    }

    public void Request(CallbackPriorityManaged priority, int id, IntPtr pv, IntPtr pNativeDeferredCallback)
    {
        _pUI.CallbackOnUIThreadRequest(priority, id, pv, pNativeDeferredCallback);
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
        }
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CallbackOnUIThread()
    {
        Dispose(false);
    }
}
