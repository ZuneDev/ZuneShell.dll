using System;

internal unsafe class CComPtrNtv<TPtr> : IDisposable where TPtr : unmanaged
{
    private bool disposedValue;
    public TPtr* p = (TPtr*)IntPtr.Zero.ToPointer();

    public bool IsNullPtr => p == null || p == IntPtr.Zero.ToPointer();

    internal CComPtrNtv()
    {
        p = (TPtr*)IntPtr.Zero.ToPointer();
    }

    internal CComPtrNtv(TPtr* lp) => set_p(lp);

    internal void set_p(TPtr* lp)
    {
        *(long*)p = (nint)lp;
        if (lp != null)
        {
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)lp + 8)))((nint)lp);
        }
    }

    public static implicit operator TPtr*(CComPtrNtv<TPtr> obj)
    {
        return obj.p;
    }

    /// <summary>
    /// Returns true if the pointer is not null.
    /// </summary>
    public static implicit operator bool(CComPtrNtv<TPtr> obj) => obj.IsNullPtr;

    public TPtr** GetPtrToPtr()
    {
        fixed (TPtr** ptr = &p)
        {
            return ptr;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
            }

            // Free unmanaged resources (unmanaged objects) and override finalizer
            // Set large fields to null
            long num = *(long*)p;
            DeviceMediator* ptr = (DeviceMediator*)num;
            if (num != 0L)
            {
                *(long*)p = 0L;
                ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 16)))((nint)ptr);
            }

            disposedValue = true;
        }
    }

    ~CComPtrNtv()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
