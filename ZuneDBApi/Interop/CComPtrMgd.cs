using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZuneDBApi.Interop;

internal class CComPtrMgd<T> : IDisposable where T : unmanaged
{
    public unsafe T* p = null;

    private void _007ECComPtrMgd()
    {
        Release();
    }

    private void _0021CComPtrMgd()
    {
        Release();
    }

    public unsafe void Release()
    {
        //IL_0014: Expected I, but got I8
        //IL_0021: Expected I, but got I8
        T* ptr = p;
        T* ptr2 = ptr;
        if (ptr != null)
        {
            p = null;
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
        }
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
    {
        if (P_0)
        {
            Release();
            return;
        }
        try
        {
            Release();
        }
        finally
        {
            base.Finalize();
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

    public static unsafe implicit operator T*(CComPtrMgd<T> comPtr)
    {
        return comPtr.p;
    }

    [SpecialName]
    public unsafe T* op_MemberSelection()
    {
        return p;
    }

    [SpecialName]
    public unsafe T* op_Assign(T* lp)
    {
        //IL_001c: Expected I, but got I8
        Release();
        p = lp;
        if (lp != null)
        {
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)lp + 8)))((nint)lp);
        }
        return p;
    }

    [return: MarshalAs(UnmanagedType.U1)]
    public unsafe bool operator !()
    {
        return (long)(nint)p == 0;
    }
}
