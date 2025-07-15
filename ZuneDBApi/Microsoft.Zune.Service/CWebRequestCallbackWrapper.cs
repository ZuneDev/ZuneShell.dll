using Microsoft.VisualC;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using static _003CModule_003E;

namespace Microsoft.Zune.Service;

[StructLayout(LayoutKind.Sequential, Size = 56)]
[MiscellaneousBits(64)]
[NativeCppClass]
[DebugInfoInPDB]
internal struct CWebRequestCallbackWrapper
{
	private long _003Calignment_0020member_003E;

    internal unsafe static CWebRequestCallbackWrapper* Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E_007Bctor_007D(CWebRequestCallbackWrapper* P_0, Uri requestUri, IHttpWebRequest* pRequest, IStream* pStream, AsyncRequestComplete requestComplete, object stateInfo)
    {
        //IL_000e: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        //IL_00b6: Expected I, but got I8
        //IL_00c9: Expected I, but got I8
        //IL_001d: Expected I, but got I8
        //IL_002c: Expected I, but got I8
        //IL_0040: Expected I, but got I8
        //IL_0043: Expected I8, but got I
        //IL_0049: Expected I, but got I8
        //IL_004c: Expected I8, but got I
        //IL_007b: Expected I, but got I8
        //IL_007b: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        *(long*)P_0 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003F_003F_7CWebRequestCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);
        CWebRequestCallbackWrapper* ptr = (CWebRequestCallbackWrapper*)((ulong)(nint)P_0 + 32uL);
        gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_007Bctor_007D((gcroot_003CSystem_003A_003AUri_0020_005E_003E*)ptr);
        try
        {
            CWebRequestCallbackWrapper* ptr2 = (CWebRequestCallbackWrapper*)((ulong)(nint)P_0 + 40uL);
            gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_007Bctor_007D((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*)ptr2);
            try
            {
                CWebRequestCallbackWrapper* ptr3 = (CWebRequestCallbackWrapper*)((ulong)(nint)P_0 + 48uL);
                gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_007Bctor_007D((gcroot_003CSystem_003A_003AObject_0020_005E_003E*)ptr3);
                try
                {
                    *(int*)((ulong)(nint)P_0 + 8uL) = 1;
                    CWebRequestCallbackWrapper* ptr4 = (CWebRequestCallbackWrapper*)((ulong)(nint)P_0 + 24uL);
                    *(long*)ptr4 = (nint)pStream;
                    CWebRequestCallbackWrapper* ptr5 = (CWebRequestCallbackWrapper*)((ulong)(nint)P_0 + 16uL);
                    *(long*)ptr5 = (nint)pRequest;
                    gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_003D((gcroot_003CSystem_003A_003AUri_0020_005E_003E*)ptr, requestUri);
                    gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_003D((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*)ptr2, requestComplete);
                    gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_003D((gcroot_003CSystem_003A_003AObject_0020_005E_003E*)ptr3, stateInfo);
                    ulong num = *(ulong*)ptr4;
                    if (num != 0L)
                    {
                        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num + 8)))((nint)num);
                    }
                    ulong num2 = *(ulong*)ptr5;
                    if (num2 != 0L)
                    {
                        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num2 + 8)))((nint)num2);
                    }
                }
                catch
                {
                    //try-fault
                    ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CSystem_003A_003AObject_0020_005E_003E*, void>)(&gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 48uL));
                    throw;
                }
            }
            catch
            {
                //try-fault
                ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*, void>)(&gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 40uL));
                throw;
            }
        }
        catch
        {
            //try-fault
            ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CSystem_003A_003AUri_0020_005E_003E*, void>)(&gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 32uL));
            throw;
        }
        return P_0;
    }

    internal unsafe static void Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E_007Bdtor_007D(CWebRequestCallbackWrapper* P_0)
    {
        //IL_004b: Expected I, but got I8
        //IL_0068: Expected I, but got I8
        //IL_0085: Expected I, but got I8
        //IL_001e: Expected I, but got I8
        //IL_001e: Expected I, but got I8
        //IL_0038: Expected I, but got I8
        //IL_0038: Expected I, but got I8
        //IL_0056: Expected I, but got I8
        //IL_0073: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        *(long*)P_0 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003F_003F_7CWebRequestCallbackWrapper_0040Service_0040Zune_0040Microsoft_0040_00406B_0040);
        try
        {
            try
            {
                try
                {
                    ulong num = *(ulong*)((ulong)(nint)P_0 + 24uL);
                    if (num != 0L)
                    {
                        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num + 16)))((nint)num);
                    }
                    ulong num2 = *(ulong*)((ulong)(nint)P_0 + 16uL);
                    if (num2 != 0L)
                    {
                        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num2 + 16)))((nint)num2);
                    }
                }
                catch
                {
                    //try-fault
                    ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CSystem_003A_003AObject_0020_005E_003E*, void>)(&gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 48uL));
                    throw;
                }
                gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_007Bdtor_007D((gcroot_003CSystem_003A_003AObject_0020_005E_003E*)((ulong)(nint)P_0 + 48uL));
            }
            catch
            {
                //try-fault
                ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*, void>)(&gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 40uL));
                throw;
            }
            gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_007Bdtor_007D((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*)((ulong)(nint)P_0 + 40uL));
        }
        catch
        {
            //try-fault
            ___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<gcroot_003CSystem_003A_003AUri_0020_005E_003E*, void>)(&gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_007Bdtor_007D), (void*)((ulong)(nint)P_0 + 32uL));
            throw;
        }
        gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_007Bdtor_007D((gcroot_003CSystem_003A_003AUri_0020_005E_003E*)((ulong)(nint)P_0 + 32uL));
    }

    internal unsafe static int Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002EQueryInterface(CWebRequestCallbackWrapper* P_0, _GUID* riid, void** ppUnknown)
    {
        //IL_0023: Expected I8, but got I
        //IL_002f: Expected I, but got I8
        if (IsEqualGUID(riid, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _IID_IUnknown)) == 0 && IsEqualGUID(riid, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _GUID_a3138a7c_be4e_4aa1_999f_f2fdd4b3f428)) == 0)
        {
            return -2147467262;
        }
        *(long*)ppUnknown = (nint)P_0;
        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)P_0 + 8)))((nint)P_0);
        return 0;
    }

    internal unsafe static uint Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002EAddRef(CWebRequestCallbackWrapper* P_0)
    {
        return (uint)Interlocked.Increment(ref *(int*)((long)(nint)P_0 + 8L));
    }

    internal unsafe static uint Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002ERelease(CWebRequestCallbackWrapper* P_0)
    {
        int num = Interlocked.Decrement(ref *(int*)((long)(nint)P_0 + 8L));
        if (num == 0 && P_0 != null)
        {
            Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E__delDtor(P_0, 1u);
        }
        return (uint)num;
    }

    internal unsafe static void* Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E__delDtor(CWebRequestCallbackWrapper* P_0, uint P_1)
    {
        Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E_007Bdtor_007D(P_0);
        if ((P_1 & 1) != 0)
        {
            delete(P_0);
        }
        return P_0;
    }

    internal unsafe static int Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002ERedirected(CWebRequestCallbackWrapper* P_0, IHttpWebResponse* P_1, ushort* P_2)
    {
        return 0;
    }

    internal unsafe static int Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002EHeadersAvailable(CWebRequestCallbackWrapper* P_0, IHttpWebResponse* P_1)
    {
        return 0;
    }

    internal unsafe static int Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002EResponseProgress(CWebRequestCallbackWrapper* P_0, IHttpWebResponse* P_1, ulong P_2, ulong P_3)
    {
        return 0;
    }

    internal unsafe static int Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002EResponseCompleted(CWebRequestCallbackWrapper* P_0, IHttpWebResponse* pResponse, int __unnamed001)
    {
        //IL_000d: Expected I, but got I8
        //IL_0018: Expected I, but got I8
        //IL_0022: Expected I, but got I8
        //IL_002d: Expected I, but got I8
        //IL_003b: Expected I, but got I8
        //IL_004f: Expected I, but got I8
        //IL_0061: Expected I, but got I8
        //IL_0061: Expected I, but got I8
        //IL_007c: Expected I, but got I8
        //IL_007c: Expected I, but got I8
        HttpStatusCode statusCode = (HttpStatusCode)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pResponse + 24)))((nint)pResponse);
        Uri requestUri = gcroot_003CSystem_003A_003AUri_0020_005E_003E_002E_002EPE_0024AAVUri_0040System_0040_0040((gcroot_003CSystem_003A_003AUri_0020_005E_003E*)((ulong)(nint)P_0 + 32uL));
        AsyncRequestComplete asyncRequestComplete = gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E_002E_002EPE_0024AAVAsyncRequestComplete_0040Service_0040Zune_0040Microsoft_0040_0040((gcroot_003CMicrosoft_003A_003AZune_003A_003AService_003A_003AAsyncRequestComplete_0020_005E_003E*)((ulong)(nint)P_0 + 40uL));
        object stateInfo = gcroot_003CSystem_003A_003AObject_0020_005E_003E_002E_002EPE_0024AAVObject_0040System_0040_0040((gcroot_003CSystem_003A_003AObject_0020_005E_003E*)((ulong)(nint)P_0 + 48uL));
        Microsoft.Zune.Service.HttpWebResponse response = new Microsoft.Zune.Service.HttpWebResponse(requestUri, statusCode, pResponse, (IStream*)(*(ulong*)((ulong)(nint)P_0 + 24uL)));
        asyncRequestComplete(response, stateInfo);
        Microsoft.Zune.Service.HttpWebRequest.OnAsyncRequestComplete((IHttpWebRequest*)(*(ulong*)((ulong)(nint)P_0 + 16uL)));
        long num = *(long*)((ulong)(nint)P_0 + 24uL);
        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num + 16)))((nint)num);
        *(long*)((ulong)(nint)P_0 + 24uL) = 0L;
        long num2 = *(long*)((ulong)(nint)P_0 + 16uL);
        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num2 + 16)))((nint)num2);
        *(long*)((ulong)(nint)P_0 + 16uL) = 0L;
        return 0;
    }
}
