using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZuneDBApi.Interop;

internal unsafe class CComPtrMgd<T> : IDisposable where T : unmanaged
{
    public T* p = null;

    public void Release()
    {
        var ptr = p;
        if (ptr != null)
        {
            p = null;
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 16)))((nint)ptr);
        }
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing)
    {
        if (disposing)
        {
            Release();
        }
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CComPtrMgd()
    {
        Dispose(false);
    }

    public static implicit operator T*(CComPtrMgd<T> comPtr)
    {
        return comPtr.p;
    }

    [SpecialName]
    public static T* op_MemberSelection(CComPtrMgd<T> comPtr)
    {
        // TODO: C# doesn't respect these operator overloads
        return comPtr.p;
    }

    [SpecialName]
    public T* op_Assign(T* lp)
    {
        Release();
        p = lp;
        if (lp != null)
        {
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)lp + 8)))((nint)lp);
        }
        return p;
    }

    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe bool operator !(CComPtrMgd<T> comPtr)
    {
        return (nint)comPtr.p == 0;
    }
}
