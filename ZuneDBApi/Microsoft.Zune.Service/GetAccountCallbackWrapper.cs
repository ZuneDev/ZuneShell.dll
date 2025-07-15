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
    private long _003Calignment_0020member_003E;

    // +8
    public int _refCount;

    // +16
    public gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E* _completeCallback;

    // +24
    public gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E* _errorCallback;

    // {ctor}
    internal static GetAccountCallbackWrapper* _007Bctor_007D(GetAccountCallbackWrapper* P_0, GetAccountCompleteCallback completeCallback, AccountManagementErrorCallback errorCallback)
    {
        //IL_000d: Expected I, but got I8
        //IL_0049: Expected I, but got I8
        //IL_005c: Expected I, but got I8
        //IL_001a: Expected I, but got I8
        *(long*)P_0 = (nint)Unsafe.AsPointer(ref _003F_003F_7GetAccountCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);
        gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_007Bctor_007D(P_0->_completeCallback);
        
        try
        {
            gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E_002E_007Bctor_007D(P_0->_errorCallback);
            try
            {
                P_0->_refCount = 1;
                gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_003D(P_0->_completeCallback, completeCallback);
                gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E_002E_003D(P_0->_errorCallback, errorCallback);
                return P_0;
            }
            catch
            {
                //try-fault
                ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E*, void>)(&gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E_002E_007Bdtor_007D), P_0->_errorCallback);
                throw;
            }
        }
        catch
        {
            //try-fault
            ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E*, void>)(&gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_007Bdtor_007D), P_0->_completeCallback);
            throw;
        }
    }

    // {dtor}
    internal static void _007Bdtor_007D(GetAccountCallbackWrapper* P_0)
    {
        //IL_0023: Expected I, but got I8
        //IL_0011: Expected I, but got I8
        //IL_002e: Expected I, but got I8
        *(long*)P_0 = (nint)Unsafe.AsPointer(ref _003F_003F_7GetAccountCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);
        try
        {
            gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E_002E_007Bdtor_007D((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E*)((ulong)(nint)P_0 + 24uL));
        }
        catch
        {
            //try-fault
            ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E*, void>)(&gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 16uL));
            throw;
        }
        gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_007Bdtor_007D((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E*)((ulong)(nint)P_0 + 16uL));
    }

    internal static int QueryInterface(GetAccountCallbackWrapper* P_0, _GUID* riid, void** ppUnknown)
    {
        //IL_0023: Expected I8, but got I
        //IL_002f: Expected I, but got I8
        if (IsEqualGUID(riid, (_GUID*)Unsafe.AsPointer(ref _IID_IUnknown)) == 0 && IsEqualGUID(riid, (_GUID*)Unsafe.AsPointer(ref _GUID_223a83b5_e8ac_4aad_882a_14ee6634fc33)) == 0)
        {
            return unchecked((int)0x80004002);
        }

        *(long*)ppUnknown = (nint)P_0;
        var pFunc = (delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)P_0 + 8));
        pFunc((nint)P_0);
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
        //IL_0016: Expected I, but got I8
        AccountUser accountUser = null;
        if (pAccountUser != null)
        {
            accountUser = new AccountUser(pAccountUser);
        }
        gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E_002E_002EPE_0024AAVGetAccountCompleteCallback_0040Service_0040Zune_0040Microsoft_0040_0040((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AGetAccountCompleteCallback_0020_005E_003E*)((ulong)(nint)P_0 + 16uL))(accountUser);
        return 0;
    }

    internal static int OnError(GetAccountCallbackWrapper* P_0, int hr, IServiceError* pServiceError)
    {
        //IL_001d: Expected I, but got I8
        ServiceError serviceError = null;
        if (pServiceError != null)
        {
            serviceError = new ServiceError(pServiceError);
        }
        HRESULT hr2 = hr;
        gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E_002E_002EPE_0024AAVAccountManagementErrorCallback_0040Service_0040Zune_0040Microsoft_0040_0040((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAccountManagementErrorCallback_0020_005E_003E*)((ulong)(nint)P_0 + 24uL))(hr2, serviceError);
        return 0;
    }
}
