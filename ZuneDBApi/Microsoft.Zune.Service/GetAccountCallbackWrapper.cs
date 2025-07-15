using Microsoft.VisualC;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ZuneUI;
using static _003CModule_003E;

namespace Microsoft.Zune.Service;

[StructLayout(LayoutKind.Sequential, Size = 32)]
[MiscellaneousBits(64)]
[DebugInfoInPDB]
[NativeCppClass]
internal unsafe struct GetAccountCallbackWrapper
{
    // +0
    private vtable* _vtable;

    // +8
    public int _refCount;

    // +16
    public gcroot<GetAccountCompleteCallback>* _completeCallback;

    // +24
    public gcroot<AccountManagementErrorCallback>* _errorCallback;

    // {ctor}
    internal static GetAccountCallbackWrapper* _007Bctor_007D(GetAccountCallbackWrapper* P_0, GetAccountCompleteCallback completeCallback, AccountManagementErrorCallback errorCallback)
    {
        *(long*)P_0 = (nint)Unsafe.AsPointer(ref _003F_003F_7GetAccountCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);
        
        gcroot<GetAccountCompleteCallback>.Create(P_0->_completeCallback);
        gcroot<AccountManagementErrorCallback>.Create(P_0->_errorCallback);

        P_0->_refCount = 1;
        P_0->_completeCallback->Assign(completeCallback);
        P_0->_errorCallback->Assign(errorCallback);

        return P_0;
    }

    // {dtor}
    internal static void _007Bdtor_007D(GetAccountCallbackWrapper* P_0)
    {
        *(long*)P_0 = (nint)Unsafe.AsPointer(ref _003F_003F_7GetAccountCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);

        P_0->_errorCallback->Dispose();
        P_0->_completeCallback->Dispose();
    }

    internal static int QueryInterface(GetAccountCallbackWrapper* P_0, _GUID* riid, void** ppUnknown)
    {
        if (!IsEqualGUID(riid, (_GUID*)Unsafe.AsPointer(ref _IID_IUnknown)) && !IsEqualGUID(riid, (_GUID*)Unsafe.AsPointer(ref _IID_223a83b5_e8ac_4aad_882a_14ee6634fc33)))
        {
            return HRESULT._E_NOINTERFACE;
        }

        *(long*)ppUnknown = (nint)P_0;

        AddRef(P_0);

        return 0;
    }

    internal static uint AddRef(GetAccountCallbackWrapper* obj)
    {
        return (uint)Interlocked.Increment(ref obj->_refCount);
    }

    internal static uint Release(GetAccountCallbackWrapper* obj)
    {
        int num = Interlocked.Decrement(ref obj->_refCount);
        if (num == 0 && obj != null)
        {
            __delDtor(obj, 1u);
        }
        return (uint)num;
    }

    internal static void* __delDtor(GetAccountCallbackWrapper* P_0, uint P_1)
    {
        _007Bdtor_007D(P_0);
        if ((P_1 & 1) != 0)
        {
            delete(P_0);
        }
        return P_0;
    }

    internal static int OnSuccess(GetAccountCallbackWrapper* P_0, IAccountUser* pAccountUser)
    {
        AccountUser accountUser = null;
        if (pAccountUser != null)
        {
            accountUser = new AccountUser(pAccountUser);
        }

        var callback = P_0->_completeCallback->Get();
        callback(accountUser);

        return 0;
    }

    internal static int OnError(GetAccountCallbackWrapper* P_0, int hr, IServiceError* pServiceError)
    {
        ServiceError serviceError = null;
        if (pServiceError != null)
        {
            serviceError = new ServiceError(pServiceError);
        }

        var callback = P_0->_errorCallback->Get();
        callback((HRESULT)hr, serviceError);

        return 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct vtable
    {
        delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IntPtr, IntPtr, uint> _queryInterface;
        delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint> _addRef;
        delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint> _release;
    }
}
