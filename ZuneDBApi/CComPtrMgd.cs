using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal unsafe class CComPtrMgd<TPtr> : IDisposable where TPtr : unmanaged
{
    public TPtr* p = null;

    public CComPtrMgd() { }

    public CComPtrMgd(IntPtr p) : this((TPtr*)p.ToPointer()) { }

    public CComPtrMgd(TPtr* p) => op_Assign(p);

    private void _007ECComPtrMgd()
    {
        Release();
    }

    private void _0021CComPtrMgd()
    {
        Release();
    }

    public void Release()
    {
        TPtr* ptr = p;
        TPtr* ptr2 = ptr;
        if (ptr != null)
        {
            p = null;
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
        }
    }

    public static implicit operator TPtr*(CComPtrMgd<TPtr> obj)
    {
        return obj.p;
    }

    [SpecialName]
    public TPtr* op_MemberSelection()
    {
        return p;
    }

    [SpecialName]
    public TPtr* op_Assign(TPtr* lp)
    {
        Release();
        p = lp;
        if (lp != null)
        {
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)lp + 8)))((nint)lp);
        }
        return p;
    }

    public unsafe int CopyTo(TPtr** ppT)
    {
        if (ppT == null)
        {
            Module._ZuneShipAssert(1001u, 168u);
            return -2147467261;
        }
        *(long*)ppT = (nint)p;
        TPtr* ptr = p;
        if (ptr != null)
        {
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 8)))((nint)ptr);
        }
        return 0;
    }

    public int QueryInterface<TPtr2>(TPtr2** pp) where TPtr2 : unmanaged
    {
        TPtr* ptr = p;
        if (ptr == null)
        {
            Module._ZuneShipAssert(1001u, 186u);
            return -2147467261;
        }

        // Use reflection on Module to get the CLSID
        string guidFieldName = "GUID_" + typeof(TPtr2).Name;
        _GUID guid = (Guid)typeof(Module).GetField(guidFieldName).GetValue(null);

        return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr)))((nint)ptr, &guid, (void**)pp);
    }

    public void Attach(TPtr* p2)
    {
        TPtr* ptr = p;
        if (ptr != p2)
        {
            if (ptr != null)
            {
                ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 16)))((nint)ptr);
            }
            p = p2;
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

        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    ~CComPtrMgd()
    {
        Dispose(false);
    }
}
