using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003CCppImplementationDetails_003E;
using Microsoft.Iris;
using Microsoft.Zune.Util;
using ZuneDBApi.Interop;
using ZuneUI;

namespace Microsoft.Zune.Service;

public class Service : IDisposable
{
	private static Service m_singletonInstance = null;

	private unsafe IService* m_pService = null;

	public static Service Instance
	{
		get
		{
			if (m_singletonInstance == null)
			{
				m_singletonInstance = new Service();
			}
			return m_singletonInstance;
		}
	}

	private unsafe Service()
	{
		//IL_0008: Expected I, but got I8
		//IL_0011: Expected I, but got I8
		IService* pService = null;
		if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&pService)) >= 0)
		{
			m_pService = pService;
		}
	}

	private unsafe static int PaymentTypeToBillingPaymentType(PaymentType ePaymentType, EBillingPaymentType* pePaymentType)
	{
		if (pePaymentType == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 501u);
			return -2147467261;
		}
		int result = 0;
		switch (ePaymentType)
		{
		default:
			global::_003CModule_003E._ZuneShipAssert(1003u, 532u);
			result = -2147024809;
			break;
		case PaymentType.Token:
			*pePaymentType = (EBillingPaymentType)4;
			break;
		case PaymentType.Wholesale:
			*pePaymentType = (EBillingPaymentType)3;
			break;
		case PaymentType.DirectDebit:
			*pePaymentType = (EBillingPaymentType)2;
			break;
		case PaymentType.CreditCard:
			*pePaymentType = (EBillingPaymentType)1;
			break;
		case PaymentType.None:
			*pePaymentType = (EBillingPaymentType)0;
			break;
		case PaymentType.Unknown:
			*pePaymentType = (EBillingPaymentType)(-1);
			break;
		}
		return result;
	}

	private unsafe static int PaymentTypeToMediaPaymentType(PaymentType ePaymentType, EMediaPaymentType* pePaymentType)
	{
		if (pePaymentType == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 545u);
			return -2147467261;
		}
		int result = 0;
		switch (ePaymentType)
		{
		default:
			global::_003CModule_003E._ZuneShipAssert(1003u, 568u);
			result = -2147024809;
			break;
		case PaymentType.Points:
			*pePaymentType = (EMediaPaymentType)3;
			break;
		case PaymentType.Token:
			*pePaymentType = (EMediaPaymentType)4;
			break;
		case PaymentType.CreditCard:
			*pePaymentType = (EMediaPaymentType)1;
			break;
		case PaymentType.Unknown:
			*pePaymentType = (EMediaPaymentType)(-1);
			break;
		}
		return result;
	}

	public unsafe static string GetEndPointUri(EServiceEndpointId eServiceEndpointId)
	{
		//IL_0004: Expected I, but got I8
		object result = null;
		ushort* ptr = null;
		if (global::_003CModule_003E.GetServiceEndPointUri((global::EServiceEndpointId)eServiceEndpointId, &ptr) >= 0)
		{
			result = Marshal.PtrToStringBSTR((IntPtr)ptr);
		}
		if (ptr != null)
		{
			global::_003CModule_003E.SysFreeString(ptr);
		}
		return (string)result;
	}

	public unsafe int Phase3Initialize()
	{
		//IL_0020: Expected I, but got I8
		int result = -2147467259;
		IService* pService = m_pService;
		if (pService != null)
		{
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 136)))((nint)pService);
		}
		return result;
	}

	public unsafe int InitializeWMISEndpointCollection()
	{
		//IL_0020: Expected I, but got I8
		int result = -2147467259;
		IService* pService = m_pService;
		if (pService != null)
		{
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 888)))((nint)pService);
		}
		return result;
	}

	public unsafe string GetWMISEndPointUri(string strEndPointName)
	{
		//IL_0017: Expected I, but got I8
		//IL_0032: Expected I, but got I8
		string result = null;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strEndPointName)))
		{
			IService* pService = m_pService;
			if (pService != null)
			{
				ushort* ptr2 = null;
				long num = *(long*)pService + 896;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort**, int>)(*(ulong*)num))((nint)m_pService, ptr, &ptr2) >= 0)
				{
					if (ptr2 == null)
					{
						goto IL_004d;
					}
					result = Marshal.PtrToStringBSTR((IntPtr)ptr2);
				}
				if (ptr2 != null)
				{
					global::_003CModule_003E.SysFreeString(ptr2);
				}
			}
			goto IL_004d;
			IL_004d:
			return result;
		}
	}

	private void _007EService()
	{
		_0021Service();
	}

	private unsafe void _0021Service()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		IService* pService = m_pService;
		if (pService != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pService + 16)))((nint)pService);
			m_pService = null;
		}
	}

	public unsafe void SignIn(string strUsername, string strPassword, [MarshalAs(UnmanagedType.U1)] bool fRememberUsername, [MarshalAs(UnmanagedType.U1)] bool fRememberPassword, [MarshalAs(UnmanagedType.U1)] bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler)
	{
		//IL_0028: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		//IL_008d: Expected I, but got I8
		if (m_pService == null)
		{
			return;
		}
		int num = 0;
		AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
		AsyncCallbackWrapper* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, eventHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strUsername)))
		{
			try
			{
				fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPassword)))
				{
					try
					{
						if (num >= 0)
						{
							long num2 = *(long*)m_pService + 288;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, int, int, int, IAsyncCallback*, int>)(*(ulong*)num2))((nint)m_pService, ptr3, ptr4, fRememberUsername ? 1 : 0, fRememberPassword ? 1 : 0, fAutomaticallySignInAtStartup ? 1 : 0, (IAsyncCallback*)ptr2);
						}
						if (ptr2 != null)
						{
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
						}
					}
					catch
					{
						//try-fault
						ptr4 = null;
						throw;
					}
				}
			}
			catch
			{
				//try-fault
				ptr3 = null;
				throw;
			}
		}
	}

	public unsafe void RefreshAccount(AsyncCompleteHandler eventHandler)
	{
		//IL_0021: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		if (m_pService != null)
		{
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, eventHandler));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IAsyncCallback*, int>)(*(ulong*)(*(long*)pService + 304)))((nint)pService, 1, (IAsyncCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsSigningIn()
	{
		//IL_001c: Expected I, but got I8
		bool result = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 208)))((nint)pService) != 0) ? true : false);
			result = flag;
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsSignedIn()
	{
		//IL_001c: Expected I, but got I8
		bool result = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 200)))((nint)pService) != 0) ? true : false);
			result = flag;
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsSignedInWithSubscription()
	{
		//IL_001c: Expected I, but got I8
		bool result = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 216)))((nint)pService) != 0) ? true : false);
			result = flag;
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool BlockExplicitContent()
	{
		//IL_001c: Expected I, but got I8
		bool result = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 248)))((nint)pService) != 0) ? true : false);
			result = flag;
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool BlockRatedContent(string system, string rating)
	{
		//IL_0038: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(system)))
			{
				try
				{
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(rating)))
					{
						try
						{
							long num = *(long*)m_pService + 256;
							bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, int>)(*(ulong*)num))((nint)m_pService, ptr, ptr2) != 0) ? true : false);
							result = flag;
						}
						catch
						{
							//try-fault
							ptr2 = null;
							throw;
						}
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanSignedInUserPostUsageData()
	{
		//IL_001c: Expected I, but got I8
		bool result = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 264)))((nint)pService) != 0) ? true : false);
			result = flag;
		}
		return result;
	}

	public unsafe string GetSignedInUsername()
	{
		//IL_000f: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		string result = null;
		IService* pService = m_pService;
		if (pService != null)
		{
			ushort* ptr = null;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pService + 272)))((nint)pService, &ptr) >= 0)
			{
				if (ptr == null)
				{
					goto IL_003c;
				}
				result = Marshal.PtrToStringBSTR((IntPtr)ptr);
			}
			if (ptr != null)
			{
				global::_003CModule_003E.SysFreeString(ptr);
			}
		}
		goto IL_003c;
		IL_003c:
		return result;
	}

	public unsafe uint GetSignedInGeoId()
	{
		//IL_001c: Expected I, but got I8
		int result = 0;
		IService* pService = m_pService;
		if (pService != null)
		{
			result = (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pService + 280)))((nint)pService);
		}
		return (uint)result;
	}

	public unsafe void CancelSignIn()
	{
		//IL_001a: Expected I, but got I8
		IService* pService = m_pService;
		if (pService != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 328)))((nint)pService);
		}
	}

	public unsafe void SignOut()
	{
		//IL_001a: Expected I, but got I8
		IService* pService = m_pService;
		if (pService != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 336)))((nint)pService);
		}
	}

	public unsafe IList GetPersistedUsernames()
	{
		//IL_0046: Expected I, but got I8
		//IL_006e: Expected I, but got I8
		IList list = null;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out DynamicArray_003Cunsigned_0020short_0020_002A_003E obj);
			*(long*)(&obj) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7_003F_0024DynamicArray_0040PEAG_0040_00406B_0040);
			System.Runtime.CompilerServices.Unsafe.As<DynamicArray_003Cunsigned_0020short_0020_002A_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
			System.Runtime.CompilerServices.Unsafe.As<DynamicArray_003Cunsigned_0020short_0020_002A_003E, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16)) = 0;
			System.Runtime.CompilerServices.Unsafe.As<DynamicArray_003Cunsigned_0020short_0020_002A_003E, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 20)) = 0;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, DynamicArray_003Cunsigned_0020short_0020_002A_003E*, int>)(*(ulong*)(*(long*)pService + 392)))((nint)pService, &obj) >= 0)
				{
					int num = System.Runtime.CompilerServices.Unsafe.As<DynamicArray_003Cunsigned_0020short_0020_002A_003E, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16));
					list = new ArrayList(System.Runtime.CompilerServices.Unsafe.As<DynamicArray_003Cunsigned_0020short_0020_002A_003E, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16)));
					int num2 = 0;
					if (0 < num)
					{
						do
						{
							ushort* ptr = (ushort*)(*(ulong*)global::_003CModule_003E.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_005B_005D(&obj, num2));
							list.Add(new string((char*)ptr));
							global::_003CModule_003E.SysFreeString(ptr);
							num2++;
						}
						while (num2 < num);
					}
					global::_003CModule_003E.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002ERemoveAllElements(&obj, true);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&global::_003CModule_003E.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&obj);
		}
		return list;
	}

	public unsafe void RemovePersistedUsername(string strUsername)
	{
		//IL_002c: Expected I, but got I8
		if (m_pService == null)
		{
			return;
		}
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strUsername)))
		{
			try
			{
				long num = *(long*)m_pService + 400;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pService, ptr);
			}
			catch
			{
				//try-fault
				ptr = null;
				throw;
			}
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SignInPasswordRequired(string strUsername)
	{
		//IL_0033: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strUsername)))
			{
				try
				{
					int num = 0;
					long num2 = *(long*)m_pService + 408;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num2))((nint)m_pService, ptr, &num) >= 0)
					{
						bool flag = ((num != 0) ? true : false);
						result = flag;
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SignInAtStartup(string strUsername)
	{
		//IL_0033: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strUsername)))
			{
				try
				{
					int num = 0;
					long num2 = *(long*)m_pService + 416;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num2))((nint)m_pService, ptr, &num) >= 0)
					{
						bool flag = ((num != 0) ? true : false);
						result = flag;
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	public unsafe string GetSignInAtStartupUsername()
	{
		//IL_000f: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		string result = null;
		IService* pService = m_pService;
		if (pService != null)
		{
			ushort* ptr = null;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pService + 424)))((nint)pService, &ptr) >= 0)
			{
				if (ptr == null)
				{
					goto IL_003c;
				}
				result = Marshal.PtrToStringBSTR((IntPtr)ptr);
			}
			if (ptr != null)
			{
				global::_003CModule_003E.SysFreeString(ptr);
			}
		}
		goto IL_003c;
		IL_003c:
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanDownloadSubscriptionContent()
	{
		//IL_0028: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		bool result = false;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 184)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0) ? true : false);
					result = flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId)
	{
		//IL_002a: Expected I, but got I8
		activeSubscription = false;
		subscriptionId = 0uL;
		IService* pService = m_pService;
		if (pService != null)
		{
			int num = 0;
			ulong num2 = 0uL;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, ulong*, int>)(*(ulong*)(*(long*)pService + 376)))((nint)pService, &num, &num2) >= 0)
			{
				bool flag = ((num != 0) ? true : false);
				activeSubscription = flag;
				subscriptionId = num2;
			}
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid)
	{
		//IL_0028: Expected I, but got I8
		bool flag = false;
		_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
		int num = 0;
		IService* pService = m_pService;
		if (pService != null)
		{
			flag = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)pService + 384)))((nint)pService, &gUID_NULL, &num) >= 0 || flag;
		}
		iUserId = num;
		Guid guid = global::_003CModule_003E.GUIDToGuid(gUID_NULL);
		guidUserGuid = guid;
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool ClearLastSignedInUser()
	{
		//IL_001c: Expected I, but got I8
		bool flag = false;
		IService* pService = m_pService;
		if (pService != null)
		{
			flag = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pService + 360)))((nint)pService) >= 0 || flag;
		}
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId)
	{
		//IL_0035: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		bool flag = false;
		_GUID gUID = global::_003CModule_003E.GuidToGUID(guidUserGuid);
		int num = 0;
		IService* pService = m_pService;
		if (pService != null)
		{
			flag = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, _GUID*, ushort*, ushort*, int, int*, int>)(*(ulong*)(*(long*)pService + 352)))((nint)pService, 0uL, &gUID, null, null, 0, &num) >= 0 || flag;
		}
		iUserId = num;
		return flag;
	}

	public unsafe string GetXboxPuid()
	{
		//IL_0021: Expected I, but got I8
		//IL_0033: Expected I4, but got I8
		string result = null;
		IService* pService = m_pService;
		if (pService != null)
		{
			ulong num = 0uL;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, int>)(*(ulong*)(*(long*)pService + 944)))((nint)pService, &num);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024ArrayType_0024_0024_0024BY0BO_0040G _0024ArrayType_0024_0024_0024BY0BO_0040G);
			*(short*)(&_0024ArrayType_0024_0024_0024BY0BO_0040G) = 0;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0BO_0040G, 2), 0, 58);
			if (num2 >= 0)
			{
				num2 = ((global::_003CModule_003E._i64tow_s((long)num, (ushort*)(&_0024ArrayType_0024_0024_0024BY0BO_0040G), 30uL, 10) != 0) ? (-2147467259) : num2);
				if (num2 >= 0)
				{
					result = new string((char*)(&_0024ArrayType_0024_0024_0024BY0BO_0040G));
				}
			}
		}
		return result;
	}

	public unsafe void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
	{
		//IL_001b: Expected I, but got I8
		//IL_003f: Expected I, but got I8
		//IL_004d: Expected I, but got I8
		DownloadCallbackWrapper* ptr = (DownloadCallbackWrapper*)global::_003CModule_003E.@new(40uL);
		DownloadCallbackWrapper* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EDownloadCallbackWrapper_002E_007Bctor_007D(ptr, eventHandler, progressHandler, allPendingHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		if (ptr2 != null)
		{
			IService* pService = m_pService;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IDownloadCallback*, int>)(*(ulong*)(*(long*)pService + 600)))((nint)pService, (IDownloadCallback*)ptr2);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	public unsafe void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
	{
		//IL_0038: Expected I, but got I8
		//IL_06b8: Expected I, but got I8
		//IL_06cb: Expected I, but got I8
		//IL_005b: Expected I, but got I8
		//IL_06e0: Expected I, but got I8
		//IL_007c: Expected I, but got I8
		//IL_0612: Expected I, but got I8
		//IL_0637: Expected I, but got I8
		//The blocks IL_05e2, IL_0649 are reachable both inside and outside the pinned region starting at IL_05e0. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_05c0, IL_05c5, IL_05c7, IL_0657 are reachable both inside and outside the pinned region starting at IL_05be. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_05aa, IL_05ae, IL_0662 are reachable both inside and outside the pinned region starting at IL_05a8. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_0596, IL_059f, IL_066d are reachable both inside and outside the pinned region starting at IL_0594. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_0582, IL_058b, IL_0678 are reachable both inside and outside the pinned region starting at IL_0580. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_00a4, IL_00aa, IL_00b2, IL_00be, IL_00e6, IL_010b, IL_011b, IL_012b, IL_013b, IL_014b, IL_015b, IL_016b, IL_017b, IL_018b, IL_019b, IL_01ab, IL_01bb, IL_01c8, IL_021c, IL_022c, IL_023c, IL_024c, IL_025c, IL_026c, IL_0279, IL_02bb, IL_02c8, IL_02d8, IL_031a, IL_035e, IL_0376, IL_03cc, IL_03e4, IL_0414, IL_041c, IL_0458, IL_0460, IL_0493, IL_049b, IL_04c5, IL_04da, IL_04f3, IL_04fd, IL_0513, IL_0524, IL_054e, IL_0556, IL_0566, IL_0575, IL_0577, IL_0683, IL_068c, IL_0691, IL_06b9, IL_06bd, IL_06cc, IL_06d2 are reachable both inside and outside the pinned region starting at IL_00a2. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		string text = null;
		if (m_pService == null)
		{
			return;
		}
		/*pinned*/ref ushort reference = ref *(ushort*)null;
		try
		{
			int num = 0;
			DownloadCallbackWrapper* ptr = (DownloadCallbackWrapper*)global::_003CModule_003E.@new(40uL);
			DownloadCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EDownloadCallbackWrapper_002E_007Bctor_007D(ptr, eventHandler, progressHandler, allPendingHandler));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			DownloadCallbackWrapper* ptr3 = ptr2;
			num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
			IMediaCollection* ptr4 = null;
			if (num < 0)
			{
				goto IL_06cc;
			}
			IService* pService = m_pService;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection**, int>)(*(ulong*)(*(long*)pService + 32)))((nint)pService, &ptr4);
			if (num < 0)
			{
				goto IL_06b9;
			}
			_GUID gUID;
			ref ushort reference2;
			ref ushort reference3;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E);
			if (!(deviceEndpointId != null) || deviceEndpointId.Equals(""))
			{
				if (items != null)
				{
					IEnumerator enumerator = items.GetEnumerator();
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						Guid guid = Guid.Empty;
						string s = null;
						string s2 = null;
						string s3 = null;
						text = null;
						global::EContentType eContentType = (global::EContentType)(-1);
						if (current is DataProviderObject)
						{
							DataProviderObject dataProviderObject = current as DataProviderObject;
							string typeName = dataProviderObject.TypeName;
							try
							{
								text = (string)dataProviderObject.GetProperty("ReferrerContext");
							}
							catch (KeyNotFoundException ex)
							{
							}
							if (!typeName.Equals("Album") && !typeName.Equals("AlbumData") && !typeName.Equals("RecommendedAlbum"))
							{
								if (!typeName.Equals("Track") && !typeName.Equals("ChannelTrack") && !typeName.Equals("ProfileTrack") && !typeName.Equals("PlaylistTrack") && !typeName.Equals("ZplTrack") && !typeName.Equals("RecommendedTrack") && !typeName.Equals("TrackPurchaseHistory") && !typeName.Equals("TrackDownloadHistory"))
								{
									if (typeName.Equals("PlaylistContentItem"))
									{
										eContentType = (global::EContentType)0;
										guid = (Guid)dataProviderObject.GetProperty("ZuneMediaId");
										s = (string)dataProviderObject.GetProperty("Title");
										s2 = (string)dataProviderObject.GetProperty("AlbumName");
										s3 = (string)dataProviderObject.GetProperty("ArtistName");
									}
									else if (!typeName.Equals("MusicVideo") && !typeName.Equals("MusicVideoHistory") && !typeName.Equals("Episode") && !typeName.Equals("Short") && !typeName.Equals("VideoHistory"))
									{
										if (typeName.Equals("Video"))
										{
											eContentType = (global::EContentType)3;
											guid = (Guid)dataProviderObject.GetProperty("ZuneMediaId");
											s = (string)dataProviderObject.GetProperty("Title");
											s3 = (string)dataProviderObject.GetProperty("Title");
										}
										else if (typeName.Equals("AppData") || typeName.Equals("ZuneHDAppData"))
										{
											eContentType = (global::EContentType)7;
											guid = (Guid)dataProviderObject.GetProperty("Id");
											s = (string)dataProviderObject.GetProperty("Title");
											s3 = (string)dataProviderObject.GetProperty("Author");
										}
									}
									else
									{
										eContentType = (global::EContentType)3;
										guid = (Guid)dataProviderObject.GetProperty("Id");
										s = (string)dataProviderObject.GetProperty("Title");
										DataProviderObject dataProviderObject2 = (DataProviderObject)dataProviderObject.GetProperty("PrimaryArtist");
										if (dataProviderObject2 != null)
										{
											s3 = (string)dataProviderObject2.GetProperty("Title");
										}
									}
								}
								else
								{
									eContentType = (global::EContentType)0;
									guid = (Guid)dataProviderObject.GetProperty("Id");
									s = (string)dataProviderObject.GetProperty("Title");
									s2 = (string)dataProviderObject.GetProperty("AlbumTitle");
									DataProviderObject dataProviderObject3 = (DataProviderObject)dataProviderObject.GetProperty("PrimaryArtist");
									if (dataProviderObject3 != null)
									{
										s3 = (string)dataProviderObject3.GetProperty("Title");
									}
								}
							}
							else
							{
								eContentType = (global::EContentType)1;
								guid = (Guid)dataProviderObject.GetProperty("Id");
								s = (string)dataProviderObject.GetProperty("Title");
							}
						}
						else if (current is TrackOffer)
						{
							eContentType = (global::EContentType)0;
							TrackOffer trackOffer = current as TrackOffer;
							guid = trackOffer.Id;
							s = trackOffer.Title;
							s2 = trackOffer.Album;
							s3 = trackOffer.Artist;
							text = trackOffer.ServiceContext;
						}
						else if (current is VideoOffer)
						{
							eContentType = (global::EContentType)3;
							VideoOffer videoOffer = current as VideoOffer;
							guid = videoOffer.Id;
							s = videoOffer.Title;
							s2 = videoOffer.SeriesTitle;
							s3 = videoOffer.Artist;
						}
						else if (current is AlbumOffer)
						{
							eContentType = (global::EContentType)1;
							AlbumOffer albumOffer = current as AlbumOffer;
							guid = albumOffer.Id;
							s = albumOffer.Title;
							text = albumOffer.ServiceContext;
						}
						else if (current.GetType() == typeof(DownloadTask))
						{
							DownloadTask downloadTask = (DownloadTask)current;
							string property = ((DownloadTask)current).GetProperty("Type");
							if (!string.IsNullOrEmpty(property))
							{
								eContentType = (global::EContentType)GetContentType(property);
							}
							string property2 = ((DownloadTask)current).GetProperty("ServiceId");
							if (!string.IsNullOrEmpty(property2))
							{
								try
								{
									Guid guid2 = new Guid(property2);
									guid = guid2;
								}
								catch (FormatException ex2)
								{
								}
							}
							s = downloadTask.GetProperty("Title");
							s2 = downloadTask.GetProperty("Album");
							s3 = downloadTask.GetProperty("Artist");
						}
						if (num < 0 || !(guid != Guid.Empty))
						{
							continue;
						}
						gUID = global::_003CModule_003E.GuidToGUID(guid);
						fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s)))
						{
							try
							{
								if (ptr5 == null)
								{
									fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
									{
										fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s2)))
										{
											try
											{
												if (ptr6 == null)
												{
													fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
													{
														fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
														{
															try
															{
																if (ptr7 == null)
																{
																	fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
																	{
																		reference2 = ref *(ushort*)null;
																		try
																		{
																			if (!string.IsNullOrEmpty(text))
																			{
																				fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																				{
																					global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																					reference3 = ref *(ushort*)null;
																					try
																					{
																						if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																						{
																							eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																							fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																							{
																								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																								try
																								{
																									long num2 = *(long*)m_pService + 120;
																									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																									long num3 = *(long*)ptr4 + 40;
																									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																								}
																								catch
																								{
																									//try-fault
																									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																									throw;
																								}
																								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																							}
																						}
																						else
																						{
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																							try
																							{
																								long num2 = *(long*)m_pService + 120;
																								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																								long num3 = *(long*)ptr4 + 40;
																								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																							}
																							catch
																							{
																								//try-fault
																								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																								throw;
																							}
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																						}
																					}
																					catch
																					{
																						//try-fault
																						reference3 = ref *(ushort*)null;
																						throw;
																					}
																					reference3 = ref *(ushort*)null;
																				}
																			}
																			else
																			{
																				global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																				reference3 = ref *(ushort*)null;
																				try
																				{
																					if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																					{
																						eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																						fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																						{
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																							try
																							{
																								long num2 = *(long*)m_pService + 120;
																								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																								long num3 = *(long*)ptr4 + 40;
																								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																							}
																							catch
																							{
																								//try-fault
																								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																								throw;
																							}
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																						}
																					}
																					else
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				catch
																				{
																					//try-fault
																					reference3 = ref *(ushort*)null;
																					throw;
																				}
																				reference3 = ref *(ushort*)null;
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference2 = ref *(ushort*)null;
																			throw;
																		}
																		reference2 = ref *(ushort*)null;
																	}
																	continue;
																}
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															catch
															{
																//try-fault
																ptr7 = null;
																throw;
															}
														}
													}
													continue;
												}
												fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
												{
													try
													{
														if (ptr7 == null)
														{
															fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
															{
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															continue;
														}
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													catch
													{
														//try-fault
														ptr7 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr6 = null;
												throw;
											}
										}
									}
									continue;
								}
								fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s2)))
								{
									try
									{
										if (ptr6 == null)
										{
											fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
											{
												fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
												{
													try
													{
														if (ptr7 == null)
														{
															fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
															{
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															continue;
														}
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													catch
													{
														//try-fault
														ptr7 = null;
														throw;
													}
												}
											}
											continue;
										}
										fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
										{
											try
											{
												if (ptr7 == null)
												{
													fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
													{
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													continue;
												}
												reference2 = ref *(ushort*)null;
												try
												{
													if (!string.IsNullOrEmpty(text))
													{
														fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
														{
															global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
															reference3 = ref *(ushort*)null;
															try
															{
																if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																{
																	eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																	fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																else
																{
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																	try
																	{
																		long num2 = *(long*)m_pService + 120;
																		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																		long num3 = *(long*)ptr4 + 40;
																		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																	}
																	catch
																	{
																		//try-fault
																		global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																		throw;
																	}
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																}
															}
															catch
															{
																//try-fault
																reference3 = ref *(ushort*)null;
																throw;
															}
															reference3 = ref *(ushort*)null;
														}
													}
													else
													{
														global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
														reference3 = ref *(ushort*)null;
														try
														{
															if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
															{
																eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																{
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																	try
																	{
																		long num2 = *(long*)m_pService + 120;
																		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																		long num3 = *(long*)ptr4 + 40;
																		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																	}
																	catch
																	{
																		//try-fault
																		global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																		throw;
																	}
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																}
															}
															else
															{
																global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																try
																{
																	long num2 = *(long*)m_pService + 120;
																	((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																	long num3 = *(long*)ptr4 + 40;
																	num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																}
																catch
																{
																	//try-fault
																	global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																	throw;
																}
																global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
															}
														}
														catch
														{
															//try-fault
															reference3 = ref *(ushort*)null;
															throw;
														}
														reference3 = ref *(ushort*)null;
													}
												}
												catch
												{
													//try-fault
													reference2 = ref *(ushort*)null;
													throw;
												}
												reference2 = ref *(ushort*)null;
											}
											catch
											{
												//try-fault
												ptr7 = null;
												throw;
											}
										}
									}
									catch
									{
										//try-fault
										ptr6 = null;
										throw;
									}
								}
							}
							catch
							{
								//try-fault
								ptr5 = null;
								throw;
							}
						}
					}
				}
				if (num >= 0)
				{
					long num4 = *(long*)m_pService + 608;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection*, global::EDownloadFlags, int, ushort*, int, IDownloadCallback*, int>)(*(ulong*)num4))((nint)m_pService, ptr4, (global::EDownloadFlags)eDownloadFlags, -1, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference), 1, (IDownloadCallback*)ptr3);
				}
				goto IL_06b9;
			}
			IMediaCollection* intPtr;
			DownloadCallbackWrapper* intPtr2;
			fixed (ushort* ptr10 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(deviceEndpointId)))
			{
				if (items != null)
				{
					IEnumerator enumerator = items.GetEnumerator();
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						Guid guid = Guid.Empty;
						string s = null;
						string s2 = null;
						string s3 = null;
						text = null;
						global::EContentType eContentType = (global::EContentType)(-1);
						if (current is DataProviderObject)
						{
							DataProviderObject dataProviderObject = current as DataProviderObject;
							string typeName = dataProviderObject.TypeName;
							try
							{
								text = (string)dataProviderObject.GetProperty("ReferrerContext");
							}
							catch (KeyNotFoundException ex)
							{
							}
							if (!typeName.Equals("Album") && !typeName.Equals("AlbumData") && !typeName.Equals("RecommendedAlbum"))
							{
								if (!typeName.Equals("Track") && !typeName.Equals("ChannelTrack") && !typeName.Equals("ProfileTrack") && !typeName.Equals("PlaylistTrack") && !typeName.Equals("ZplTrack") && !typeName.Equals("RecommendedTrack") && !typeName.Equals("TrackPurchaseHistory") && !typeName.Equals("TrackDownloadHistory"))
								{
									if (typeName.Equals("PlaylistContentItem"))
									{
										eContentType = (global::EContentType)0;
										guid = (Guid)dataProviderObject.GetProperty("ZuneMediaId");
										s = (string)dataProviderObject.GetProperty("Title");
										s2 = (string)dataProviderObject.GetProperty("AlbumName");
										s3 = (string)dataProviderObject.GetProperty("ArtistName");
									}
									else if (!typeName.Equals("MusicVideo") && !typeName.Equals("MusicVideoHistory") && !typeName.Equals("Episode") && !typeName.Equals("Short") && !typeName.Equals("VideoHistory"))
									{
										if (typeName.Equals("Video"))
										{
											eContentType = (global::EContentType)3;
											guid = (Guid)dataProviderObject.GetProperty("ZuneMediaId");
											s = (string)dataProviderObject.GetProperty("Title");
											s3 = (string)dataProviderObject.GetProperty("Title");
										}
										else if (typeName.Equals("AppData") || typeName.Equals("ZuneHDAppData"))
										{
											eContentType = (global::EContentType)7;
											guid = (Guid)dataProviderObject.GetProperty("Id");
											s = (string)dataProviderObject.GetProperty("Title");
											s3 = (string)dataProviderObject.GetProperty("Author");
										}
									}
									else
									{
										eContentType = (global::EContentType)3;
										guid = (Guid)dataProviderObject.GetProperty("Id");
										s = (string)dataProviderObject.GetProperty("Title");
										DataProviderObject dataProviderObject2 = (DataProviderObject)dataProviderObject.GetProperty("PrimaryArtist");
										if (dataProviderObject2 != null)
										{
											s3 = (string)dataProviderObject2.GetProperty("Title");
										}
									}
								}
								else
								{
									eContentType = (global::EContentType)0;
									guid = (Guid)dataProviderObject.GetProperty("Id");
									s = (string)dataProviderObject.GetProperty("Title");
									s2 = (string)dataProviderObject.GetProperty("AlbumTitle");
									DataProviderObject dataProviderObject3 = (DataProviderObject)dataProviderObject.GetProperty("PrimaryArtist");
									if (dataProviderObject3 != null)
									{
										s3 = (string)dataProviderObject3.GetProperty("Title");
									}
								}
							}
							else
							{
								eContentType = (global::EContentType)1;
								guid = (Guid)dataProviderObject.GetProperty("Id");
								s = (string)dataProviderObject.GetProperty("Title");
							}
						}
						else if (current is TrackOffer)
						{
							eContentType = (global::EContentType)0;
							TrackOffer trackOffer = current as TrackOffer;
							guid = trackOffer.Id;
							s = trackOffer.Title;
							s2 = trackOffer.Album;
							s3 = trackOffer.Artist;
							text = trackOffer.ServiceContext;
						}
						else if (current is VideoOffer)
						{
							eContentType = (global::EContentType)3;
							VideoOffer videoOffer = current as VideoOffer;
							guid = videoOffer.Id;
							s = videoOffer.Title;
							s2 = videoOffer.SeriesTitle;
							s3 = videoOffer.Artist;
						}
						else if (current is AlbumOffer)
						{
							eContentType = (global::EContentType)1;
							AlbumOffer albumOffer = current as AlbumOffer;
							guid = albumOffer.Id;
							s = albumOffer.Title;
							text = albumOffer.ServiceContext;
						}
						else if (current.GetType() == typeof(DownloadTask))
						{
							DownloadTask downloadTask = (DownloadTask)current;
							string property = ((DownloadTask)current).GetProperty("Type");
							if (!string.IsNullOrEmpty(property))
							{
								eContentType = (global::EContentType)GetContentType(property);
							}
							string property2 = ((DownloadTask)current).GetProperty("ServiceId");
							if (!string.IsNullOrEmpty(property2))
							{
								try
								{
									Guid guid2 = new Guid(property2);
									guid = guid2;
								}
								catch (FormatException ex2)
								{
								}
							}
							s = downloadTask.GetProperty("Title");
							s2 = downloadTask.GetProperty("Album");
							s3 = downloadTask.GetProperty("Artist");
						}
						if (num < 0 || !(guid != Guid.Empty))
						{
							continue;
						}
						gUID = global::_003CModule_003E.GuidToGUID(guid);
						fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s)))
						{
							try
							{
								if (ptr5 == null)
								{
									fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
									{
										fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s2)))
										{
											try
											{
												if (ptr6 == null)
												{
													fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
													{
														fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
														{
															try
															{
																if (ptr7 == null)
																{
																	fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
																	{
																		reference2 = ref *(ushort*)null;
																		try
																		{
																			if (!string.IsNullOrEmpty(text))
																			{
																				fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																				{
																					global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																					reference3 = ref *(ushort*)null;
																					try
																					{
																						if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																						{
																							eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																							fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																							{
																								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																								try
																								{
																									long num2 = *(long*)m_pService + 120;
																									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																									long num3 = *(long*)ptr4 + 40;
																									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																								}
																								catch
																								{
																									//try-fault
																									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																									throw;
																								}
																								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																							}
																						}
																						else
																						{
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																							try
																							{
																								long num2 = *(long*)m_pService + 120;
																								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																								long num3 = *(long*)ptr4 + 40;
																								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																							}
																							catch
																							{
																								//try-fault
																								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																								throw;
																							}
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																						}
																					}
																					catch
																					{
																						//try-fault
																						reference3 = ref *(ushort*)null;
																						throw;
																					}
																					reference3 = ref *(ushort*)null;
																				}
																			}
																			else
																			{
																				global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																				reference3 = ref *(ushort*)null;
																				try
																				{
																					if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																					{
																						eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																						fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																						{
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																							try
																							{
																								long num2 = *(long*)m_pService + 120;
																								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																								long num3 = *(long*)ptr4 + 40;
																								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																							}
																							catch
																							{
																								//try-fault
																								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																								throw;
																							}
																							global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																						}
																					}
																					else
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				catch
																				{
																					//try-fault
																					reference3 = ref *(ushort*)null;
																					throw;
																				}
																				reference3 = ref *(ushort*)null;
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference2 = ref *(ushort*)null;
																			throw;
																		}
																		reference2 = ref *(ushort*)null;
																	}
																	continue;
																}
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															catch
															{
																//try-fault
																ptr7 = null;
																throw;
															}
														}
													}
													continue;
												}
												fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
												{
													try
													{
														if (ptr7 == null)
														{
															fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
															{
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															continue;
														}
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													catch
													{
														//try-fault
														ptr7 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr6 = null;
												throw;
											}
										}
									}
									continue;
								}
								fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s2)))
								{
									try
									{
										if (ptr6 == null)
										{
											fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
											{
												fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
												{
													try
													{
														if (ptr7 == null)
														{
															fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
															{
																reference2 = ref *(ushort*)null;
																try
																{
																	if (!string.IsNullOrEmpty(text))
																	{
																		fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																		{
																			global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																			reference3 = ref *(ushort*)null;
																			try
																			{
																				if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																				{
																					eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																					{
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																						try
																						{
																							long num2 = *(long*)m_pService + 120;
																							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																							long num3 = *(long*)ptr4 + 40;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																						}
																						catch
																						{
																							//try-fault
																							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																							throw;
																						}
																						global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																					}
																				}
																				else
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			catch
																			{
																				//try-fault
																				reference3 = ref *(ushort*)null;
																				throw;
																			}
																			reference3 = ref *(ushort*)null;
																		}
																	}
																	else
																	{
																		global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																		reference3 = ref *(ushort*)null;
																		try
																		{
																			if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																			{
																				eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																				fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																				{
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																					try
																					{
																						long num2 = *(long*)m_pService + 120;
																						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																						long num3 = *(long*)ptr4 + 40;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																					}
																					catch
																					{
																						//try-fault
																						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																						throw;
																					}
																					global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																				}
																			}
																			else
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		catch
																		{
																			//try-fault
																			reference3 = ref *(ushort*)null;
																			throw;
																		}
																		reference3 = ref *(ushort*)null;
																	}
																}
																catch
																{
																	//try-fault
																	reference2 = ref *(ushort*)null;
																	throw;
																}
																reference2 = ref *(ushort*)null;
															}
															continue;
														}
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													catch
													{
														//try-fault
														ptr7 = null;
														throw;
													}
												}
											}
											continue;
										}
										fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(s3)))
										{
											try
											{
												if (ptr7 == null)
												{
													fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, ushort>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040))
													{
														reference2 = ref *(ushort*)null;
														try
														{
															if (!string.IsNullOrEmpty(text))
															{
																fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
																{
																	global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																	reference3 = ref *(ushort*)null;
																	try
																	{
																		if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																		{
																			eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																			fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																			{
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																				try
																				{
																					long num2 = *(long*)m_pService + 120;
																					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																					long num3 = *(long*)ptr4 + 40;
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																				}
																				catch
																				{
																					//try-fault
																					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																					throw;
																				}
																				global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																			}
																		}
																		else
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	catch
																	{
																		//try-fault
																		reference3 = ref *(ushort*)null;
																		throw;
																	}
																	reference3 = ref *(ushort*)null;
																}
															}
															else
															{
																global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
																reference3 = ref *(ushort*)null;
																try
																{
																	if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																	{
																		eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																		fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																		{
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																			try
																			{
																				long num2 = *(long*)m_pService + 120;
																				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																				long num3 = *(long*)ptr4 + 40;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																			}
																			catch
																			{
																				//try-fault
																				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																				throw;
																			}
																			global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																		}
																	}
																	else
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																catch
																{
																	//try-fault
																	reference3 = ref *(ushort*)null;
																	throw;
																}
																reference3 = ref *(ushort*)null;
															}
														}
														catch
														{
															//try-fault
															reference2 = ref *(ushort*)null;
															throw;
														}
														reference2 = ref *(ushort*)null;
													}
													continue;
												}
												reference2 = ref *(ushort*)null;
												try
												{
													if (!string.IsNullOrEmpty(text))
													{
														fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
														{
															global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
															reference3 = ref *(ushort*)null;
															try
															{
																if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
																{
																	eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																	fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																	{
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																		try
																		{
																			long num2 = *(long*)m_pService + 120;
																			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																			long num3 = *(long*)ptr4 + 40;
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																			throw;
																		}
																		global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																	}
																}
																else
																{
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																	try
																	{
																		long num2 = *(long*)m_pService + 120;
																		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr8, eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																		long num3 = *(long*)ptr4 + 40;
																		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																	}
																	catch
																	{
																		//try-fault
																		global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																		throw;
																	}
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																}
															}
															catch
															{
																//try-fault
																reference3 = ref *(ushort*)null;
																throw;
															}
															reference3 = ref *(ushort*)null;
														}
													}
													else
													{
														global::EDownloadContextEvent eDownloadContextEvent = (global::EDownloadContextEvent)(-1);
														reference3 = ref *(ushort*)null;
														try
														{
															if (clientContextEvent != EDownloadContextEvent.Unknown && !string.IsNullOrEmpty(clientContextEventData))
															{
																eDownloadContextEvent = (global::EDownloadContextEvent)clientContextEvent;
																fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(clientContextEventData)))
																{
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																	try
																	{
																		long num2 = *(long*)m_pService + 120;
																		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, ptr9, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																		long num3 = *(long*)ptr4 + 40;
																		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																	}
																	catch
																	{
																		//try-fault
																		global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																		throw;
																	}
																	global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
																}
															}
															else
															{
																global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
																try
																{
																	long num2 = *(long*)m_pService + 120;
																	((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), eDownloadContextEvent, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
																	long num3 = *(long*)ptr4 + 40;
																	num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num3))((nint)ptr4, &gUID, eContentType, ptr5, ptr6, ptr7, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
																}
																catch
																{
																	//try-fault
																	global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
																	throw;
																}
																global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
															}
														}
														catch
														{
															//try-fault
															reference3 = ref *(ushort*)null;
															throw;
														}
														reference3 = ref *(ushort*)null;
													}
												}
												catch
												{
													//try-fault
													reference2 = ref *(ushort*)null;
													throw;
												}
												reference2 = ref *(ushort*)null;
											}
											catch
											{
												//try-fault
												ptr7 = null;
												throw;
											}
										}
									}
									catch
									{
										//try-fault
										ptr6 = null;
										throw;
									}
								}
							}
							catch
							{
								//try-fault
								ptr5 = null;
								throw;
							}
						}
					}
				}
				if (num >= 0)
				{
					long num4 = *(long*)m_pService + 608;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection*, global::EDownloadFlags, int, ushort*, int, IDownloadCallback*, int>)(*(ulong*)num4))((nint)m_pService, ptr4, (global::EDownloadFlags)eDownloadFlags, -1, ptr10, 1, (IDownloadCallback*)ptr3);
				}
				if (ptr4 != null)
				{
					intPtr = ptr4;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
				if (ptr3 != null)
				{
					intPtr2 = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
			}
			goto end_IL_0012;
			IL_06cc:
			if (ptr3 != null)
			{
				intPtr2 = ptr3;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			goto end_IL_0012;
			IL_06b9:
			if (ptr4 != null)
			{
				intPtr = ptr4;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			goto IL_06cc;
			end_IL_0012:;
		}
		catch
		{
			//try-fault
			reference = ref *(ushort*)null;
			throw;
		}
		reference = ref *(ushort*)null;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden)
	{
		//IL_003a: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaId);
			IService* pService = m_pService;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, global::EContentType, int*, int*, int*, int>)(*(ulong*)(*(long*)pService + 616)))((nint)pService, gUID, (global::EContentType)eContentType, &num, &num2, &num3) >= 0)
			{
				bool flag = ((num != 0 || num2 != 0) ? true : false);
				result = flag;
				bool flag2 = ((num2 != 0) ? true : false);
				fIsDownloadPending = flag2;
				bool flag3 = ((num3 != 0) ? true : false);
				fIsHidden = flag3;
			}
		}
		return result;
	}

	public unsafe void CancelDownload(Guid guidMediaId, EContentType eContentType)
	{
		//IL_0028: Expected I, but got I8
		if (m_pService != null)
		{
			_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaId);
			IService* pService = m_pService;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, global::EContentType, int>)(*(ulong*)(*(long*)pService + 624)))((nint)pService, gUID, (global::EContentType)eContentType);
		}
	}

	public unsafe HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut)
	{
		//IL_0016: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		int num = 0;
		string text = null;
		_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
		if (m_pService != null)
		{
			ushort* ptr = null;
			_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaId);
			IService* pService = m_pService;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, global::EContentType, global::EContentUriFlags, EMediaFormat, EMediaRights, ushort**, _GUID*, int>)(*(ulong*)(*(long*)pService + 584)))((nint)pService, gUID, (global::EContentType)eContentType, (global::EContentUriFlags)eContentUriFlags, eMediaFormat, eMediaRights, &ptr, &gUID_NULL);
			if (num >= 0)
			{
				if (ptr == null)
				{
					goto IL_005e;
				}
				text = Marshal.PtrToStringBSTR((IntPtr)ptr);
			}
			if (ptr != null)
			{
				global::_003CModule_003E.SysFreeString(ptr);
			}
		}
		goto IL_005e;
		IL_005e:
		uriOut = text;
		Guid guid = global::_003CModule_003E.GUIDToGuid(gUID_NULL);
		mediaInstanceIdOut = guid;
		return num;
	}

	public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, [MarshalAs(UnmanagedType.U1)] bool fIsHD, [MarshalAs(UnmanagedType.U1)] bool fIsRental, out string uriOut)
	{
		EMediaFormat eMediaFormat = (fIsHD ? ((EMediaFormat)2) : ((EMediaFormat)3));
		EMediaRights eMediaRights = (fIsRental ? ((EMediaRights)9) : ((EMediaRights)8));
		Guid mediaInstanceIdOut = default(Guid);
		return GetContentUri(guidMediaId, eContentType, eContentUriFlags, eMediaFormat, eMediaRights, out uriOut, out mediaInstanceIdOut);
	}

	public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut)
	{
		return GetContentUri(guidMediaId, eContentType, eContentUriFlags, (EMediaFormat)(-1), (EMediaRights)(-1), out uriOut, out mediaInstanceIdOut);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType)
	{
		int dbMediaId;
		bool fHidden;
		return InCompleteCollection(guidMediaId, eContentType, null, out dbMediaId, out fHidden);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId)
	{
		int dbMediaId;
		bool fHidden;
		return InCompleteCollection(guidMediaId, eContentType, strDeviceEndpointId, out dbMediaId, out fHidden);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden)
	{
		return InCompleteCollection(guidMediaId, eContentType, null, out dbMediaId, out fHidden);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden)
	{
		//IL_004a: Expected I, but got I8
		bool result = false;
		int num = -1;
		if (m_pService != null)
		{
			int num2 = 0;
			int num3 = 0;
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strDeviceEndpointId)))
			{
				try
				{
					_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaId);
					long num4 = *(long*)m_pService + 640;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, global::EContentType, ushort*, int*, int*, int*, int>)(*(ulong*)num4))((nint)m_pService, gUID, (global::EContentType)eContentType, ptr, &num2, &num, &num3) >= 0)
					{
						bool flag = ((num2 != 0) ? true : false);
						result = flag;
						bool flag2 = ((num3 != 0) ? true : false);
						fHidden = flag2;
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		dbMediaId = num;
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType)
	{
		if (InCompleteCollection(guidMediaId, eContentType, null, out var _, out var fHidden))
		{
			return !fHidden;
		}
		return false;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId)
	{
		if (InCompleteCollection(guidMediaId, eContentType, null, out dbMediaId, out var fHidden))
		{
			return !fHidden;
		}
		return false;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool InHiddenCollection(Guid guidMediaId, EContentType eContentType)
	{
		int dbMediaId;
		bool fHidden;
		return InCompleteCollection(guidMediaId, eContentType, null, out dbMediaId, out fHidden) && fHidden;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext)
	{
		//IL_005d: Expected I, but got I8
		//IL_005d: Expected I, but got I8
		//IL_009f: Expected I, but got I8
		//IL_009f: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strTitle)))
			{
				try
				{
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strAlbum)))
					{
						try
						{
							fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strArtist)))
							{
								try
								{
									fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strGenre)))
									{
										try
										{
											fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strServiceContext)))
											{
												try
												{
													System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E);
													*(long*)(&cComPtrNtv_003CIContextData_003E) = 0L;
													try
													{
														long num = *(long*)m_pService + 120;
														((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num))((nint)m_pService, ptr5, (global::EDownloadContextEvent)(-1), null, (IContextData**)(&cComPtrNtv_003CIContextData_003E));
														_GUID gUID = global::_003CModule_003E.GuidToGUID(guidAlbumMediaId);
														_GUID gUID2 = global::_003CModule_003E.GuidToGUID(guidTrackMediaId);
														long num2 = *(long*)m_pService + 904;
														result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, _GUID*, _GUID*, int, ushort*, int, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)num2))((nint)m_pService, iUserId, iRating, &gUID2, &gUID, iTrackNumber, ptr, msDuration, ptr2, ptr3, ptr4, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E))) >= 0;
													}
													catch
													{
														//try-fault
														global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
														throw;
													}
													global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002ERelease(&cComPtrNtv_003CIContextData_003E);
												}
												catch
												{
													//try-fault
													ptr5 = null;
													throw;
												}
											}
										}
										catch
										{
											//try-fault
											ptr4 = null;
											throw;
										}
									}
								}
								catch
								{
									//try-fault
									ptr3 = null;
									throw;
								}
							}
						}
						catch
						{
							//try-fault
							ptr2 = null;
							throw;
						}
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle)
	{
		//IL_003a: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strTitle)))
			{
				try
				{
					_GUID gUID = global::_003CModule_003E.GuidToGUID(guidArtistMediaId);
					long num = *(long*)m_pService + 912;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, _GUID*, ushort*, int>)(*(ulong*)num))((nint)m_pService, iUserId, iRating, &gUID, ptr) >= 0;
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, [In][Out] ref int piRating)
	{
		//IL_002f: Expected I, but got I8
		bool flag = false;
		if (m_pService != null)
		{
			_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaId);
			IService* pService = m_pService;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num);
			flag = 0 == ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, _GUID*, global::EContentType, int*, int>)(*(ulong*)(*(long*)pService + 920)))((nint)pService, iUserId, &gUID, (global::EContentType)eContentType, &num);
			if (flag)
			{
				piRating = num;
			}
		}
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler)
	{
		//IL_0023: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		bool flag = false;
		if (m_pService != null)
		{
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, eventHandler));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAsyncCallback*, int>)(*(ulong*)(*(long*)pService + 648)))((nint)pService, (IAsyncCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
				flag = num >= 0 || flag;
			}
		}
		return flag;
	}

	public unsafe string GetSubscriptionDirectory()
	{
		//IL_000f: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		string result = null;
		IService* pService = m_pService;
		if (pService != null)
		{
			ushort* ptr = null;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pService + 656)))((nint)pService, &ptr) >= 0)
			{
				result = Marshal.PtrToStringBSTR((IntPtr)ptr);
			}
			if (ptr != null)
			{
				global::_003CModule_003E.SysFreeString(ptr);
			}
		}
		return result;
	}

	public static EListType ContentTypeToListType(EContentType contentType)
	{
		int result;
		switch (contentType)
		{
		case EContentType.MusicTrack:
			return EListType.eTrackList;
		case EContentType.MusicAlbum:
			return EListType.eAlbumList;
		case EContentType.Video:
			return EListType.eVideoList;
		case EContentType.PodcastEpisode:
			return EListType.ePodcastEpisodeList;
		case EContentType.PodcastSeries:
			return EListType.ePodcastList;
		case EContentType.Artist:
			return EListType.eArtistList;
		default:
			result = 23;
			break;
		case EContentType.App:
			result = 20;
			break;
		}
		return (EListType)result;
	}

	public unsafe EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType)
	{
		//IL_00d3: Expected I, but got I8
		//IL_003c: Expected I, but got I8
		int dbMediaId = -1;
		bool fIsDownloadPending = false;
		EMediaStatus result = EMediaStatus.StatusNotAvailable;
		bool fIsHidden;
		if (InVisibleCollection(guidMediaId, eContentType, out dbMediaId))
		{
			EMediaTypes eMediaTypes = EMediaTypes.eMediaTypeInvalid;
			if (eContentType == EContentType.Video)
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040 _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040);
				global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bctor_007D((DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040), 177u);
				try
				{
					if (ZuneLibraryExports.GetFieldValues(dbMediaId, ContentTypeToListType(EContentType.Video), 1, (DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040), null) >= 0 && System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 4)) >= 0)
					{
						eMediaTypes = ((System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, ushort>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 8)) == 3) ? System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, EMediaTypes>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 16)) : eMediaTypes);
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindVecDtor((delegate*<void*, ulong, int, delegate*<void*, void>, void>)(&global::_003CModule_003E.__ehvec_dtor), (void*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040), 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D));
					throw;
				}
				global::_003CModule_003E.__ehvec_dtor(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D));
				if (eMediaTypes == EMediaTypes.eMediaTypeVideoMBR)
				{
					if (IsDownloading(guidMediaId, EContentType.Video, out fIsDownloadPending, out fIsHidden))
					{
						EMediaStatus eMediaStatus = (fIsDownloadPending ? EMediaStatus.StatusDownloadPending : EMediaStatus.StatusDownloading);
						result = eMediaStatus;
					}
					else
					{
						result = EMediaStatus.StatusInCollectionShortcut;
					}
					goto IL_016f;
				}
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040 _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402);
			global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bctor_007D((DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402), 149u);
			try
			{
				if (ZuneLibraryExports.GetFieldValues(dbMediaId, ContentTypeToListType(eContentType), 1, (DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402), null) < 0 || System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 4)) < 0 || System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, ushort>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 8)) != 3)
				{
					goto IL_0118;
				}
				if (System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 16)) != 10)
				{
					if (System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 16)) != 20 && System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 16)) != 26)
					{
						if (System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 16)) != 30 && System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 16)) != 40)
						{
							goto IL_0118;
						}
						result = EMediaStatus.StatusInCollectionOwned;
					}
					else
					{
						result = EMediaStatus.StatusInCollectionExpiring;
					}
				}
				else
				{
					result = EMediaStatus.StatusInCollectionNoLicense;
				}
				goto end_IL_00c2;
				IL_0118:
				result = EMediaStatus.StatusInCollectionUnknown;
				end_IL_00c2:;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindVecDtor((delegate*<void*, ulong, int, delegate*<void*, void>, void>)(&global::_003CModule_003E.__ehvec_dtor), (void*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402), 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D));
				throw;
			}
			global::_003CModule_003E.__ehvec_dtor(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_00402, 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D));
		}
		else if (IsDownloading(guidMediaId, eContentType, out fIsDownloadPending, out fIsHidden))
		{
			EMediaStatus eMediaStatus2 = (fIsDownloadPending ? EMediaStatus.StatusDownloadPending : EMediaStatus.StatusDownloading);
			result = eMediaStatus2;
		}
		goto IL_016f;
		IL_016f:
		return result;
	}

	public unsafe string GetZuneTag()
	{
		//IL_0033: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		string result = null;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 96)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &ptr);
					if (num < 0)
					{
						goto IL_0066;
					}
					if (ptr != null)
					{
						result = Marshal.PtrToStringBSTR((IntPtr)ptr);
						goto IL_0066;
					}
				}
				goto end_IL_001a;
				IL_0066:
				if (ptr != null)
				{
					global::_003CModule_003E.SysFreeString(ptr);
				}
				end_IL_001a:;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe string GetPassportTicket(EPassportPolicyId ePassportPolicy)
	{
		//IL_0036: Expected I, but got I8
		//IL_003a: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		string result = null;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPassportPolicy, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 40)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), (EPassportPolicy)ePassportPolicy, &ptr);
					if (num < 0)
					{
						goto IL_006a;
					}
					if (ptr != null)
					{
						result = Marshal.PtrToStringBSTR((IntPtr)ptr);
						goto IL_006a;
					}
				}
				goto end_IL_001d;
				IL_006a:
				if (ptr != null)
				{
					global::_003CModule_003E.SysFreeString(ptr);
				}
				end_IL_001d:;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity)
	{
		//IL_0048: Expected I, but got I8
		//IL_0059: Expected I, but got I8
		int num = -2147467259;
		passportIdentity = null;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(username)))
			{
				try
				{
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(password)))
					{
						try
						{
							System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
							*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
							try
							{
								long num2 = *(long*)m_pService + 440;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, EPassportPolicy, IPassportIdentity**, int>)(*(ulong*)num2))((nint)m_pService, ptr, ptr2, (EPassportPolicy)ePassportPolicyId, (IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
								if (num >= 0)
								{
									passportIdentity = new PassportIdentity((IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)));
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
								throw;
							}
							global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
						}
						catch
						{
							//try-fault
							ptr2 = null;
							throw;
						}
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return new HRESULT(num);
	}

	public unsafe string GetXboxTicket()
	{
		//IL_0033: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		string result = null;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 32)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &ptr);
					if (num < 0)
					{
						goto IL_0066;
					}
					if (ptr != null)
					{
						result = Marshal.PtrToStringBSTR((IntPtr)ptr);
						goto IL_0066;
					}
				}
				goto end_IL_001a;
				IL_0066:
				if (ptr != null)
				{
					global::_003CModule_003E.SysFreeString(ptr);
				}
				end_IL_001a:;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe ulong GetPassportPuid()
	{
		//IL_0031: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		ulong result = 0uL;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 80)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe ValueType GetUserGuid()
	{
		//IL_0034: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 64)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &gUID_NULL);
					if (num < 0)
					{
						gUID_NULL = global::_003CModule_003E.GUID_NULL;
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return global::_003CModule_003E.GUIDToGuid(gUID_NULL);
	}

	public unsafe string GetLocale()
	{
		//IL_0033: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		string result = null;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 104)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &ptr);
					if (num < 0)
					{
						goto IL_0066;
					}
					if (ptr != null)
					{
						result = Marshal.PtrToStringBSTR((IntPtr)ptr);
						goto IL_0066;
					}
				}
				goto end_IL_001a;
				IL_0066:
				if (ptr != null)
				{
					global::_003CModule_003E.SysFreeString(ptr);
				}
				end_IL_001a:;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool HasSignInLabelTakedown()
	{
		//IL_0030: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		bool result = false;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 248)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0) ? true : false);
					result = flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool HasSignInBillingViolation()
	{
		//IL_0030: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		bool result = false;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 200)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0) ? true : false);
					result = flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe int GetPointsBalance()
	{
		//IL_0030: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		int result = 0;
		if (m_pService != null && IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 144)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback)
	{
		//IL_0022: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		if (m_pService != null)
		{
			GetBalancesCallbackWrapper* ptr = (GetBalancesCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetBalancesCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetBalancesCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IGetBalancesCallback*, int>)(*(ulong*)(*(long*)pService + 672)))((nint)pService, (IGetBalancesCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	public unsafe int GetSubscriptionFreeTrackBalance()
	{
		//IL_0030: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		int result = 0;
		if (m_pService != null && IsSignedInWithSubscription())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 152)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state)
	{
		//IL_004a: Expected I, but got I8
		//IL_0090: Expected I, but got I8
		//IL_0090: Expected I, but got I8
		int num = 0;
		num = (((long)(nint)m_pService == 0) ? (-2147467259) : num);
		_GUID offerId2 = global::_003CModule_003E.GuidToGUID(offerId);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIGetOfferDetailsCallback_003E cComPtrNtv_003CIGetOfferDetailsCallback_003E);
		*(long*)(&cComPtrNtv_003CIGetOfferDetailsCallback_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0)
			{
				GetOfferDetailsCallbackWrapper* ptr = (GetOfferDetailsCallbackWrapper*)global::_003CModule_003E.@new(64uL);
				GetOfferDetailsCallbackWrapper* p;
				try
				{
					p = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetOfferDetailsCallbackWrapper_002E_007Bctor_007D(ptr, offerId2, state, m_pService, completeCallback, errorCallback));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIGetOfferDetailsCallback_003E_002EAttach(&cComPtrNtv_003CIGetOfferDetailsCallback_003E, (IGetOfferDetailsCallback*)p);
				num = ((*(long*)(&cComPtrNtv_003CIGetOfferDetailsCallback_003E) == 0) ? (-2147024882) : num);
				if (num >= 0)
				{
					IService* pService = m_pService;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, _GUID*, IGetOfferDetailsCallback*, int>)(*(ulong*)(*(long*)pService + 688)))((nint)pService, 1, &offerId2, (IGetOfferDetailsCallback*)(*(ulong*)(&cComPtrNtv_003CIGetOfferDetailsCallback_003E)));
				}
			}
			result = new HRESULT(num);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIGetOfferDetailsCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIGetOfferDetailsCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIGetOfferDetailsCallback_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIGetOfferDetailsCallback_003E_002ERelease(&cComPtrNtv_003CIGetOfferDetailsCallback_003E);
		return result;
	}

	public unsafe void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback)
	{
		//IL_0032: Expected I, but got I8
		//IL_004d: Expected I, but got I8
		//IL_006d: Expected I, but got I8
		//IL_035a: Expected I, but got I8
		//IL_0347: Expected I, but got I8
		//IL_029e: Expected I, but got I8
		//IL_029e: Expected I, but got I8
		//IL_029e: Expected I, but got I8
		//IL_029e: Expected I, but got I8
		//IL_029e: Expected I, but got I8
		//IL_0336: Expected I, but got I8
		//IL_02f2: Expected I, but got I8
		//IL_02f2: Expected I, but got I8
		//IL_02f2: Expected I, but got I8
		//IL_02f2: Expected I, but got I8
		//IL_02f2: Expected I, but got I8
		//IL_011a: Expected I, but got I8
		//IL_011a: Expected I, but got I8
		//IL_013d: Expected I, but got I8
		//IL_013d: Expected I, but got I8
		//IL_013d: Expected I, but got I8
		//IL_013d: Expected I, but got I8
		//IL_013d: Expected I, but got I8
		//IL_0206: Expected I, but got I8
		//IL_0206: Expected I, but got I8
		//IL_0229: Expected I, but got I8
		//IL_0229: Expected I, but got I8
		//IL_0229: Expected I, but got I8
		//IL_0229: Expected I, but got I8
		//IL_0229: Expected I, but got I8
		//The blocks IL_00eb, IL_014e are reachable both inside and outside the pinned region starting at IL_00e9. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_01e0, IL_023a are reachable both inside and outside the pinned region starting at IL_01df. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		//The blocks IL_0312, IL_0337, IL_033a, IL_0348, IL_034d are reachable both inside and outside the pinned region starting at IL_0310. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		if (m_pService == null)
		{
			return;
		}
		/*pinned*/ref ushort reference = ref *(ushort*)null;
		try
		{
			int num = 0;
			GetOffersCallbackWrapper* ptr = (GetOffersCallbackWrapper*)global::_003CModule_003E.@new(40uL);
			GetOffersCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetOffersCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback, mapIdToContext));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
			IMediaCollection* ptr3 = null;
			if (num < 0)
			{
				goto IL_0348;
			}
			IService* pService = m_pService;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection**, int>)(*(ulong*)(*(long*)pService + 32)))((nint)pService, &ptr3);
			if (num < 0)
			{
				goto IL_0337;
			}
			if (albumGuids != null)
			{
				IEnumerator enumerator = albumGuids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E);
					do
					{
						if (num < 0)
						{
							continue;
						}
						Guid guid = (Guid)enumerator.Current;
						string text = null;
						if (mapIdToContext != null && mapIdToContext.Contains(guid))
						{
							text = (string)mapIdToContext[guid];
						}
						/*pinned*/ref ushort reference2 = ref *(ushort*)null;
						try
						{
							_GUID gUID;
							if (!string.IsNullOrEmpty(text))
							{
								fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text)))
								{
									global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
									try
									{
										long num2 = *(long*)m_pService + 120;
										((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, ptr4, (global::EDownloadContextEvent)(-1), null, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
										gUID = global::_003CModule_003E.GuidToGUID(guid);
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID, (global::EContentType)1, null, null, null, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E)));
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
										throw;
									}
									global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
								}
							}
							else
							{
								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bctor_007D(&cComPtrNtv_003CIContextData_003E);
								try
								{
									long num2 = *(long*)m_pService + 120;
									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num2))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference2), (global::EDownloadContextEvent)(-1), null, global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_0026(&cComPtrNtv_003CIContextData_003E));
									gUID = global::_003CModule_003E.GuidToGUID(guid);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID, (global::EContentType)1, null, null, null, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E)));
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
									throw;
								}
								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
							}
						}
						catch
						{
							//try-fault
							reference2 = ref *(ushort*)null;
							throw;
						}
						reference2 = ref *(ushort*)null;
					}
					while (enumerator.MoveNext());
				}
			}
			if (trackGuids != null)
			{
				IEnumerator enumerator2 = trackGuids.GetEnumerator();
				if (enumerator2.MoveNext())
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E2);
					do
					{
						if (num < 0)
						{
							continue;
						}
						Guid guid2 = (Guid)enumerator2.Current;
						string text2 = null;
						if (mapIdToContext != null && mapIdToContext.Contains(guid2))
						{
							text2 = (string)mapIdToContext[guid2];
						}
						/*pinned*/ref ushort reference3 = ref *(ushort*)null;
						try
						{
							_GUID gUID2;
							if (!string.IsNullOrEmpty(text2))
							{
								fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(text2)))
								{
									*(long*)(&cComPtrNtv_003CIContextData_003E2) = 0L;
									try
									{
										long num3 = *(long*)m_pService + 120;
										((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num3))((nint)m_pService, ptr5, (global::EDownloadContextEvent)(-1), null, (IContextData**)(&cComPtrNtv_003CIContextData_003E2));
										gUID2 = global::_003CModule_003E.GuidToGUID(guid2);
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID2, (global::EContentType)0, null, null, null, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E2)));
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E2);
										throw;
									}
									global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E2);
								}
							}
							else
							{
								*(long*)(&cComPtrNtv_003CIContextData_003E2) = 0L;
								try
								{
									long num3 = *(long*)m_pService + 120;
									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EDownloadContextEvent, ushort*, IContextData**, int>)(*(ulong*)num3))((nint)m_pService, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference3), (global::EDownloadContextEvent)(-1), null, (IContextData**)(&cComPtrNtv_003CIContextData_003E2));
									gUID2 = global::_003CModule_003E.GuidToGUID(guid2);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID2, (global::EContentType)0, null, null, null, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E2)));
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E2);
									throw;
								}
								global::_003CModule_003E.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E2);
							}
						}
						catch
						{
							//try-fault
							reference3 = ref *(ushort*)null;
							throw;
						}
						reference3 = ref *(ushort*)null;
					}
					while (enumerator2.MoveNext());
				}
			}
			if (videoGuids != null)
			{
				IEnumerator enumerator3 = videoGuids.GetEnumerator();
				if (enumerator3.MoveNext())
				{
					do
					{
						if (num >= 0)
						{
							_GUID gUID3 = global::_003CModule_003E.GuidToGUID((Guid)enumerator3.Current);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID3, (global::EContentType)3, null, null, null, null);
						}
					}
					while (enumerator3.MoveNext());
				}
			}
			if (appGuids != null)
			{
				IEnumerator enumerator4 = appGuids.GetEnumerator();
				if (enumerator4.MoveNext())
				{
					do
					{
						if (num >= 0)
						{
							_GUID gUID4 = global::_003CModule_003E.GuidToGUID((Guid)enumerator4.Current);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, global::EContentType, ushort*, ushort*, ushort*, IContextData*, int>)(*(ulong*)(*(long*)ptr3 + 40)))((nint)ptr3, &gUID4, (global::EContentType)7, null, null, null, null);
						}
					}
					while (enumerator4.MoveNext());
				}
			}
			if (num < 0)
			{
				goto IL_0337;
			}
			if (string.IsNullOrEmpty(deviceEndpointId))
			{
				long num4 = *(long*)m_pService + 680;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection*, IGetOffersCallback*, global::EGetOffersFlags, ushort*, int>)(*(ulong*)num4))((nint)m_pService, ptr3, (IGetOffersCallback*)ptr2, (global::EGetOffersFlags)eGetOffersFlags, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference));
				goto IL_0337;
			}
			IMediaCollection* intPtr;
			GetOffersCallbackWrapper* intPtr2;
			fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(deviceEndpointId)))
			{
				long num4 = *(long*)m_pService + 680;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMediaCollection*, IGetOffersCallback*, global::EGetOffersFlags, ushort*, int>)(*(ulong*)num4))((nint)m_pService, ptr3, (IGetOffersCallback*)ptr2, (global::EGetOffersFlags)eGetOffersFlags, ptr6);
				if (ptr3 != null)
				{
					intPtr = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
				if (ptr2 != null)
				{
					intPtr2 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
			}
			goto end_IL_000f;
			IL_0348:
			if (ptr2 != null)
			{
				intPtr2 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			goto end_IL_000f;
			IL_0337:
			if (ptr3 != null)
			{
				intPtr = ptr3;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			goto IL_0348;
			end_IL_000f:;
		}
		catch
		{
			//try-fault
			reference = ref *(ushort*)null;
			throw;
		}
		reference = ref *(ushort*)null;
	}

	public unsafe void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler)
	{
		//IL_0039: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		//IL_0068: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		//IL_008d: Expected I, but got I8
		//IL_010c: Expected I, but got I8
		//IL_011f: Expected I, but got I8
		//IL_00ee: Expected I, but got I8
		//IL_0130: Expected I, but got I8
		//IL_0141: Expected I, but got I8
		//IL_0152: Expected I, but got I8
		if (null == payment)
		{
			throw new ArgumentNullException("payment");
		}
		if (m_pService == null)
		{
			return;
		}
		int num = 0;
		PurchaseOffersCallbackWrapper* ptr = (PurchaseOffersCallbackWrapper*)global::_003CModule_003E.@new(24uL);
		PurchaseOffersCallbackWrapper* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EPurchaseOffersCallbackWrapper_002E_007Bctor_007D(ptr, purchaseOffersHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
		IMusicAlbumCollection* ptr3 = null;
		if (num >= 0 && albumOffers != null)
		{
			ptr3 = albumOffers.GetCollection();
		}
		IMusicTrackCollection* ptr4 = null;
		if (num >= 0 && trackOffers != null)
		{
			ptr4 = trackOffers.GetCollection();
		}
		IVideoCollection* ptr5 = null;
		if (num >= 0 && videoOffers != null)
		{
			ptr5 = videoOffers.GetCollection();
		}
		IAppCollection* ptr6 = null;
		if (num >= 0 && appOffers != null)
		{
			ptr6 = appOffers.GetCollection();
		}
		EMediaPaymentType eMediaPaymentType = (EMediaPaymentType)(-1);
		if (num >= 0)
		{
			num = PaymentTypeToMediaPaymentType(payment.Type, &eMediaPaymentType);
			if (num >= 0)
			{
				fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(payment.Id)))
				{
					try
					{
						long num2 = *(long*)m_pService + 712;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaPaymentType, ushort*, IMusicAlbumCollection*, IMusicTrackCollection*, IVideoCollection*, IAppCollection*, global::EPurchaseOffersFlags, IPurchaseOffersCallback*, int>)(*(ulong*)num2))((nint)m_pService, eMediaPaymentType, ptr7, ptr3, ptr4, ptr5, ptr6, (global::EPurchaseOffersFlags)ePurchaseOffersFlags, (IPurchaseOffersCallback*)ptr2);
					}
					catch
					{
						//try-fault
						ptr7 = null;
						throw;
					}
				}
			}
		}
		if (ptr3 != null)
		{
			IMusicAlbumCollection* intPtr = ptr3;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		if (ptr4 != null)
		{
			IMusicTrackCollection* intPtr2 = ptr4;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		if (ptr5 != null)
		{
			IVideoCollection* intPtr3 = ptr5;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
		}
		if (ptr6 != null)
		{
			IAppCollection* intPtr4 = ptr6;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
		}
		if (ptr2 != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback)
	{
		//IL_0027: Expected I, but got I8
		//IL_00e1: Expected I, but got I8
		//IL_00f6: Expected I, but got I8
		//IL_00af: Expected I, but got I8
		int num;
		if (m_pService != null)
		{
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, callback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 == null)
			{
				num = -2147024882;
				goto IL_0103;
			}
			_GUID gUID = global::_003CModule_003E.GuidToGUID(userId);
			ulong num2 = (ulong)artists.Count;
			_GUID* ptr3 = (_GUID*)global::_003CModule_003E.new_005B_005D((num2 > 1152921504606846975L) ? ulong.MaxValue : (num2 * 16));
			int num3 = 0;
			if (0 < artists.Count)
			{
				_GUID* ptr4 = ptr3;
				do
				{
					_GUID gUID2 = global::_003CModule_003E.GuidToGUID((Guid)artists[num3]);
					// IL cpblk instruction
					System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(ptr4, ref gUID2, 16);
					num3++;
					ptr4 = (_GUID*)((ulong)(nint)ptr4 + 16uL);
				}
				while (num3 < artists.Count);
			}
			long num4 = *(long*)m_pService + 528;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, int, _GUID*, IAsyncCallback*, int>)(*(ulong*)num4))((nint)m_pService, gUID, artists.Count, ptr3, (IAsyncCallback*)ptr2);
			global::_003CModule_003E.delete_005B_005D(ptr3);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
		else
		{
			num = -2147467259;
		}
		if (num >= 0)
		{
			return true;
		}
		goto IL_0103;
		IL_0103:
		if (callback != null)
		{
			HRESULT hr = num;
			callback(hr);
		}
		return false;
	}

	public unsafe int VerifyToken(string token, out TokenDetails tokenDetails)
	{
		//IL_0038: Expected I, but got I8
		//IL_0048: Expected I, but got I8
		int num = 0;
		tokenDetails = null;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(token)))
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITokenDetails_003E cComPtrNtv_003CITokenDetails_003E);
					*(long*)(&cComPtrNtv_003CITokenDetails_003E) = 0L;
					try
					{
						long num2 = *(long*)m_pService + 536;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ITokenDetails**, int>)(*(ulong*)num2))((nint)m_pService, ptr, (ITokenDetails**)(&cComPtrNtv_003CITokenDetails_003E));
						if (num >= 0)
						{
							tokenDetails = new TokenDetails((ITokenDetails*)(*(ulong*)(&cComPtrNtv_003CITokenDetails_003E)));
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITokenDetails_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITokenDetails_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITokenDetails_003E);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CITokenDetails_003E_002ERelease(&cComPtrNtv_003CITokenDetails_003E);
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return num;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback)
	{
		//IL_0025: Expected I, but got I8
		//IL_006d: Expected I, but got I8
		//IL_0084: Expected I, but got I8
		int num;
		if (m_pService != null)
		{
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, callback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 == null)
			{
				num = -2147024882;
				goto IL_0091;
			}
			_GUID gUID = global::_003CModule_003E.GuidToGUID(mediaId);
			fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(message)))
			{
				try
				{
					long num2 = *(long*)m_pService + 520;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, global::EConcernType, global::EContentType, _GUID, ushort*, IAsyncCallback*, int>)(*(ulong*)num2))((nint)m_pService, (global::EConcernType)concernType, (global::EContentType)contentType, gUID, ptr3, (IAsyncCallback*)ptr2);
				}
				catch
				{
					//try-fault
					ptr3 = null;
					throw;
				}
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
		else
		{
			num = -2147467259;
		}
		if (num >= 0)
		{
			return true;
		}
		goto IL_0091;
		IL_0091:
		if (callback != null)
		{
			HRESULT hr = num;
			callback(hr);
		}
		return false;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback)
	{
		//IL_0028: Expected I, but got I8
		//IL_0078: Expected I, but got I8
		//IL_0098: Expected I, but got I8
		int num;
		if (m_pService != null)
		{
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, callback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 == null)
			{
				num = -2147024882;
				goto IL_00a5;
			}
			_GUID gUID = global::_003CModule_003E.GuidToGUID(mediaId);
			fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
			{
				try
				{
					fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(comment)))
					{
						try
						{
							long num2 = *(long*)m_pService + 544;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, ushort*, int, IAsyncCallback*, int>)(*(ulong*)num2))((nint)m_pService, &gUID, ptr3, ptr4, rating, (IAsyncCallback*)ptr2);
						}
						catch
						{
							//try-fault
							ptr4 = null;
							throw;
						}
					}
				}
				catch
				{
					//try-fault
					ptr3 = null;
					throw;
				}
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
		else
		{
			num = -2147467259;
		}
		if (num >= 0)
		{
			return true;
		}
		goto IL_00a5;
		IL_00a5:
		if (callback != null)
		{
			HRESULT hr = num;
			callback(hr);
		}
		return false;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy)
	{
		//IL_0031: Expected I, but got I8
		//IL_0031: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strUrl)))
			{
				try
				{
					long num = *(long*)m_pService + 816;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, EPassportPolicy, IAsyncCallback*, int>)(*(ulong*)num))((nint)m_pService, ptr, (EPassportPolicy)ePassportPolicy, null) >= 0;
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum)
	{
		//IL_003b: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out MusicAlbumMetadata musicAlbumMetadata);
			global::_003CModule_003E.MusicAlbumMetadata_002E_007Bctor_007D(&musicAlbumMetadata);
			try
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(compId)))
				{
					try
					{
						long num = *(long*)m_pService + 832;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, MusicAlbumMetadata*, int>)(*(ulong*)num))((nint)m_pService, ptr, &musicAlbumMetadata) >= 0)
						{
							Guid guid = global::_003CModule_003E.GUIDToGuid(System.Runtime.CompilerServices.Unsafe.As<MusicAlbumMetadata, _GUID>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref musicAlbumMetadata, 8)));
							guidAlbum = guid;
							result = true;
						}
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<MusicAlbumMetadata*, void>)(&global::_003CModule_003E.MusicAlbumMetadata_002E_007Bdtor_007D), &musicAlbumMetadata);
				throw;
			}
			*(long*)(&musicAlbumMetadata) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_7MusicAlbumMetadata_0040_00406B_0040);
			global::_003CModule_003E.MusicAlbumMetadata_002EClear(&musicAlbumMetadata);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo)
	{
		//IL_0030: Expected I, but got I8
		bool result = false;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(compId)))
			{
				try
				{
					long num = *(long*)m_pService + 840;
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _GUID guid);
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, _GUID*, int>)(*(ulong*)num))((nint)m_pService, ptr, &guid) >= 0)
					{
						Guid guid2 = global::_003CModule_003E.GUIDToGuid(guid);
						guidMusicVideo = guid2;
						result = true;
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	public unsafe DRMInfo GetFileDRMInfo(string filePath)
	{
		//IL_003b: Expected I, but got I8
		DRMInfo result = null;
		if (m_pService != null)
		{
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(filePath)))
			{
				try
				{
					long num = *(long*)m_pService + 568;
					System.Runtime.CompilerServices.Unsafe.SkipInit(out DRMQueryState eCanPlay);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _FILETIME fILETIME);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out int num2);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out int num3);
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, DRMQueryState*, _FILETIME*, int*, int*, int>)(*(ulong*)num))((nint)m_pService, ptr, &eCanPlay, &fILETIME, &num2, &num3) >= 0)
					{
						long num4 = (long)(uint)System.Runtime.CompilerServices.Unsafe.As<_FILETIME, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref fILETIME, 4)) * 4294967296L + (uint)(*(int*)(&fILETIME));
						DateTime expiryDate = DateTime.MaxValue;
						if (num4 > 0)
						{
							try
							{
								expiryDate = DateTime.FromFileTime(num4);
							}
							catch (ArgumentOutOfRangeException)
							{
							}
						}
						bool canBurn = ((num3 != 0) ? true : false);
						bool canSync = ((num2 != 0) ? true : false);
						result = new DRMInfo(eCanPlay, expiryDate, canSync, canBurn);
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		return result;
	}

	public unsafe DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType)
	{
		//IL_0037: Expected I, but got I8
		DRMInfo result = null;
		if (m_pService != null)
		{
			_GUID gUID = global::_003CModule_003E.GuidToGUID(mediaId);
			IService* pService = m_pService;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out DRMQueryState eCanPlay);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out _FILETIME fILETIME);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num2);
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, global::EContentType, DRMQueryState*, _FILETIME*, int*, int*, int>)(*(ulong*)(*(long*)pService + 576)))((nint)pService, gUID, (global::EContentType)eContentType, &eCanPlay, &fILETIME, &num, &num2) >= 0)
			{
				long num3 = (long)(uint)System.Runtime.CompilerServices.Unsafe.As<_FILETIME, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref fILETIME, 4)) * 4294967296L + (uint)(*(int*)(&fILETIME));
				DateTime expiryDate = DateTime.MaxValue;
				if (num3 > 0)
				{
					try
					{
						expiryDate = DateTime.FromFileTime(num3);
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
				bool canBurn = ((num2 != 0) ? true : false);
				bool canSync = ((num != 0) ? true : false);
				result = new DRMInfo(eCanPlay, expiryDate, canSync, canBurn);
			}
		}
		return result;
	}

	public unsafe ulong GetSubscriptionOfferId()
	{
		//IL_0029: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		ulong result = 0uL;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 208)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe ulong GetSubscriptionRenewalOfferId()
	{
		//IL_0029: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		ulong result = 0uL;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 216)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe DateTime GetSubscriptionEndDate()
	{
		//IL_0030: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		DateTime result = DateTime.MaxValue;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				string text = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 224)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &ptr);
					if (num >= 0)
					{
						if (ptr == null)
						{
							goto IL_006d;
						}
						text = Marshal.PtrToStringBSTR((IntPtr)ptr);
					}
					if (ptr != null)
					{
						global::_003CModule_003E.SysFreeString(ptr);
					}
				}
				goto IL_006d;
				IL_006d:
				if (text != null && text.Length > 0)
				{
					DateTime.TryParse(text, out result);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe DateTime GetSubscriptionFreeTrackExpiration()
	{
		//IL_0030: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		DateTime result = DateTime.MaxValue;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E));
				ushort* ptr = null;
				string text = null;
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 232)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E)), &ptr);
					if (num >= 0)
					{
						if (ptr == null)
						{
							goto IL_006d;
						}
						text = Marshal.PtrToStringBSTR((IntPtr)ptr);
					}
					if (ptr != null)
					{
						global::_003CModule_003E.SysFreeString(ptr);
					}
				}
				goto IL_006d;
				IL_006d:
				if (text != null && text.Length > 0)
				{
					DateTime.TryParse(text, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out result);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool SubscriptionPendingCancel()
	{
		//IL_0028: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		bool flag = false;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					flag = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 240)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0 || flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return flag;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsParentallyControlled()
	{
		//IL_0028: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		bool result = false;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 120)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0) ? true : false);
					result = flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool IsLightWeight()
	{
		//IL_0028: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		//IL_0041: Expected I, but got I8
		bool result = false;
		if (IsSignedIn())
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CISignInState_003E cComPtrNtv_003CISignInState_003E);
			*(long*)(&cComPtrNtv_003CISignInState_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ISignInState**, int>)(*(ulong*)(*(long*)pService + 344)))((nint)pService, (ISignInState**)(&cComPtrNtv_003CISignInState_003E)) >= 0)
				{
					bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CISignInState_003E)) + 128)))((nint)(*(long*)(&cComPtrNtv_003CISignInState_003E))) != 0) ? true : false);
					result = flag;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISignInState_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISignInState_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CISignInState_003E_002ERelease(&cComPtrNtv_003CISignInState_003E);
		}
		return result;
	}

	public unsafe void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback)
	{
		//IL_0022: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		if (m_pService != null)
		{
			GetPaymentInstrumentsCallbackWrapper* ptr = (GetPaymentInstrumentsCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetPaymentInstrumentsCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetPaymentInstrumentsCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IGetPaymentInstrumentsCallback*, int>)(*(ulong*)(*(long*)pService + 792)))((nint)pService, (IGetPaymentInstrumentsCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	public unsafe int AddPaymentInstrument(PaymentInstrument paymentInstrument, AddPaymentInstrumentCompleteCallback completeCallback, AddPaymentInstrumentErrorCallback errorCallback)
	{
		//IL_003c: Expected I, but got I8
		//IL_0157: Expected I, but got I8
		//IL_01e9: Expected I, but got I8
		CreditCard creditCard = paymentInstrument as CreditCard;
		int result;
		if (m_pService != null && creditCard == null)
		{
			result = -2147467259;
		}
		else
		{
			AddPaymentInstrumentCallbackWrapper* ptr = (AddPaymentInstrumentCallbackWrapper*)global::_003CModule_003E.@new(40uL);
			AddPaymentInstrumentCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EAddPaymentInstrumentCallbackWrapper_002E_007Bctor_007D(ptr, creditCard, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 == null)
			{
				result = -2147024882;
			}
			else
			{
				_SYSTEMTIME sYSTEMTIME = global::_003CModule_003E.DateTimeToSystemTime(creditCard.ExpirationDate);
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.Street1)))
				{
					try
					{
						fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.Street2)))
						{
							try
							{
								fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.City)))
								{
									try
									{
										fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.District)))
										{
											try
											{
												fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.State)))
												{
													try
													{
														fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.PostalCode)))
														{
															try
															{
																fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhonePrefix)))
																{
																	try
																	{
																		fixed (ushort* ptr10 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhoneNumber)))
																		{
																			try
																			{
																				fixed (ushort* ptr11 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhoneExtension)))
																				{
																					try
																					{
																						fixed (ushort* ptr12 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountHolderName)))
																						{
																							try
																							{
																								fixed (ushort* ptr13 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountNumber)))
																								{
																									try
																									{
																										fixed (ushort* ptr14 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.CCVNumber)))
																										{
																											try
																											{
																												long num = *(long*)m_pService + 808;
																												result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ECreditCardType, ushort*, ushort*, ushort*, _SYSTEMTIME, IAddPaymentInstrumentCallback*, int>)(*(ulong*)num))((nint)m_pService, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, ptr9, ptr10, ptr11, (ECreditCardType)creditCard.CreditCardType, ptr12, ptr13, ptr14, sYSTEMTIME, (IAddPaymentInstrumentCallback*)ptr2);
																											}
																											catch
																											{
																												//try-fault
																												ptr14 = null;
																												throw;
																											}
																										}
																									}
																									catch
																									{
																										//try-fault
																										ptr13 = null;
																										throw;
																									}
																								}
																							}
																							catch
																							{
																								//try-fault
																								ptr12 = null;
																								throw;
																							}
																						}
																					}
																					catch
																					{
																						//try-fault
																						ptr11 = null;
																						throw;
																					}
																				}
																			}
																			catch
																			{
																				//try-fault
																				ptr10 = null;
																				throw;
																			}
																		}
																	}
																	catch
																	{
																		//try-fault
																		ptr9 = null;
																		throw;
																	}
																}
															}
															catch
															{
																//try-fault
																ptr8 = null;
																throw;
															}
														}
													}
													catch
													{
														//try-fault
														ptr7 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr6 = null;
												throw;
											}
										}
									}
									catch
									{
										//try-fault
										ptr5 = null;
										throw;
									}
								}
							}
							catch
							{
								//try-fault
								ptr4 = null;
								throw;
							}
						}
					}
					catch
					{
						//try-fault
						ptr3 = null;
						throw;
					}
				}
				if (ptr2 != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
				}
			}
		}
		return result;
	}

	public unsafe int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError)
	{
		//IL_01d5: Expected I, but got I8
		//IL_0138: Expected I, but got I8
		//IL_01c6: Expected I, but got I8
		CreditCard creditCard = paymentInstrument as CreditCard;
		int num = 0;
		if (m_pService == null || creditCard == null)
		{
			num = -2147467259;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
		global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
			*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					_SYSTEMTIME sYSTEMTIME = global::_003CModule_003E.DateTimeToSystemTime(creditCard.ExpirationDate);
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.Street1)))
					{
						try
						{
							fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.Street2)))
							{
								try
								{
									fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.City)))
									{
										try
										{
											fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.District)))
											{
												try
												{
													fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.State)))
													{
														try
														{
															fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Address.PostalCode)))
															{
																try
																{
																	fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhonePrefix)))
																	{
																		try
																		{
																			fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhoneNumber)))
																			{
																				try
																				{
																					fixed (ushort* ptr9 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhoneExtension)))
																					{
																						try
																						{
																							fixed (ushort* ptr10 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountHolderName)))
																							{
																								try
																								{
																									fixed (ushort* ptr11 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountNumber)))
																									{
																										try
																										{
																											fixed (ushort* ptr12 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.CCVNumber)))
																											{
																												try
																												{
																													long num2 = *(long*)m_pService + 800;
																													num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ECreditCardType, ushort*, ushort*, ushort*, _SYSTEMTIME, ushort**, IServiceError**, int>)(*(ulong*)num2))((nint)m_pService, ptr, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, ptr9, (ECreditCardType)creditCard.CreditCardType, ptr10, ptr11, ptr12, sYSTEMTIME, (ushort**)(&wBSTRString), global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_0026(&cComPtrNtv_003CIServiceError_003E));
																												}
																												catch
																												{
																													//try-fault
																													ptr12 = null;
																													throw;
																												}
																											}
																										}
																										catch
																										{
																											//try-fault
																											ptr11 = null;
																											throw;
																										}
																									}
																								}
																								catch
																								{
																									//try-fault
																									ptr10 = null;
																									throw;
																								}
																							}
																						}
																						catch
																						{
																							//try-fault
																							ptr9 = null;
																							throw;
																						}
																					}
																				}
																				catch
																				{
																					//try-fault
																					ptr8 = null;
																					throw;
																				}
																			}
																		}
																		catch
																		{
																			//try-fault
																			ptr7 = null;
																			throw;
																		}
																	}
																}
																catch
																{
																	//try-fault
																	ptr6 = null;
																	throw;
																}
															}
														}
														catch
														{
															//try-fault
															ptr5 = null;
															throw;
														}
													}
												}
												catch
												{
													//try-fault
													ptr4 = null;
													throw;
												}
											}
										}
										catch
										{
											//try-fault
											ptr3 = null;
											throw;
										}
									}
								}
								catch
								{
									//try-fault
									ptr2 = null;
									throw;
								}
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
					if (num >= 0)
					{
						paymentId = new string((char*)(*(ulong*)(&wBSTRString)));
					}
				}
				if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
				{
					serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
				}
				else
				{
					serviceError = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
			throw;
		}
		global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
		return num;
	}

	public unsafe void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
	{
		//IL_0022: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		if (m_pService != null)
		{
			GetBillingOffersCallbackWrapper* ptr = (GetBillingOffersCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetBillingOffersCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetBillingOffersCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IGetBillingOffersCallback*, int>)(*(ulong*)(*(long*)pService + 736)))((nint)pService, (IGetBillingOffersCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	public unsafe void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
	{
		//IL_0022: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		if (m_pService != null)
		{
			GetBillingOffersCallbackWrapper* ptr = (GetBillingOffersCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetBillingOffersCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetBillingOffersCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, IGetBillingOffersCallback*, int>)(*(ulong*)(*(long*)pService + 744)))((nint)pService, offerId, (IGetBillingOffersCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	public unsafe void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
	{
		//IL_0022: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		if (m_pService != null)
		{
			GetBillingOffersCallbackWrapper* ptr = (GetBillingOffersCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetBillingOffersCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetBillingOffersCallbackWrapper_002E_007Bctor_007D(ptr, completeCallback, errorCallback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				IService* pService = m_pService;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IGetBillingOffersCallback*, int>)(*(ulong*)(*(long*)pService + 752)))((nint)pService, (IGetBillingOffersCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
		}
	}

	public unsafe int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback)
	{
		//IL_0019: Expected I, but got I8
		//IL_00c6: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		//IL_00ac: Expected I, but got I8
		int num = 0;
		if (m_pService == null || offer == null || paymentInstrument == null)
		{
			num = -2147467259;
		}
		AsyncCallbackWrapper* ptr = null;
		if (num >= 0)
		{
			AsyncCallbackWrapper* ptr2 = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr2, callback));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr2);
				throw;
			}
			ptr = ptr3;
			num = (((long)(nint)ptr3 == 0) ? (-2147024882) : num);
		}
		EBillingPaymentType eBillingPaymentType = (EBillingPaymentType)(-1);
		if (num >= 0)
		{
			num = PaymentTypeToBillingPaymentType(paymentInstrument.Type, &eBillingPaymentType);
			if (num >= 0)
			{
				fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(paymentInstrument.Id)))
				{
					try
					{
						long num2 = *(long*)m_pService + 768;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, global::EBillingOfferType, ushort*, EBillingPaymentType, int, IAsyncCallback*, int>)(*(ulong*)num2))((nint)m_pService, offer.Id, (global::EBillingOfferType)offer.OfferType, ptr4, eBillingPaymentType, (int)offer.Points, (IAsyncCallback*)ptr);
					}
					catch
					{
						//try-fault
						ptr4 = null;
						throw;
					}
				}
			}
		}
		if (ptr != null)
		{
			AsyncCallbackWrapper* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		return num;
	}

	public unsafe int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument)
	{
		//IL_006a: Expected I, but got I8
		int num = 0;
		if (m_pService == null || offer == null || paymentInstrument == null)
		{
			num = -2147467259;
		}
		EBillingPaymentType eBillingPaymentType = (EBillingPaymentType)(-1);
		if (num >= 0)
		{
			num = PaymentTypeToBillingPaymentType(paymentInstrument.Type, &eBillingPaymentType);
			if (num >= 0)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(paymentInstrument.Id)))
				{
					try
					{
						long num2 = *(long*)m_pService + 760;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, global::EBillingOfferType, ushort*, EBillingPaymentType, int, int>)(*(ulong*)num2))((nint)m_pService, offer.Id, (global::EBillingOfferType)offer.OfferType, ptr, eBillingPaymentType, (int)offer.Points);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
		}
		return num;
	}

	public unsafe string GetMachineId()
	{
		//IL_000f: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		string result = null;
		IService* pService = m_pService;
		if (pService != null)
		{
			ushort* ptr = null;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pService + 928)))((nint)pService, &ptr) >= 0)
			{
				result = new string((char*)ptr);
				global::_003CModule_003E.SysFreeString(ptr);
			}
		}
		return result;
	}

	public unsafe int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback)
	{
		//IL_0019: Expected I, but got I8
		//IL_0051: Expected I, but got I8
		AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
		AsyncCallbackWrapper* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, callback));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(resumeHandle)))
		{
			fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(authorizationToken)))
			{
				long num = *(long*)m_pService + 728;
				return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IAsyncCallback*, int>)(*(ulong*)num))((nint)m_pService, ptr3, ptr4, (IAsyncCallback*)ptr2);
			}
		}
	}

	public unsafe CountryBaseDetails[] GetCountryDetails()
	{
		//IL_002f: Expected I, but got I8
		//IL_0048: Expected I, but got I8
		//IL_0048: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_00ee: Expected I, but got I8
		//IL_00ee: Expected I, but got I8
		//IL_010e: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0116: Expected I, but got I8
		//IL_011a: Expected I, but got I8
		//IL_0139: Expected I, but got I8
		//IL_0139: Expected I, but got I8
		//IL_021c: Expected I, but got I8
		//IL_019f: Expected I, but got I8
		//IL_01bf: Expected I, but got I8
		//IL_01cd: Expected I, but got I8
		CountryBaseDetails[] array = null;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
			*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E));
				int num2 = 0;
				if (num >= 0)
				{
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 112)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)));
					array = new CountryBaseDetails[num2];
				}
				int num3 = 0;
				if (0 < num2)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
					do
					{
						int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 120)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3);
						ushort** ptr = null;
						if (num4 != 0)
						{
							ulong num5 = (ulong)num4;
							ptr = (ushort**)global::_003CModule_003E.new_005B_005D((num5 > 2305843009213693951L) ? ulong.MaxValue : (num5 * 8));
							num = (((long)(nint)ptr == 0) ? (-2147024882) : num);
						}
						global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
						try
						{
							int teenagerAge = 0;
							int adultAge = 0;
							int num6 = 0;
							int num7 = 0;
							int num8 = 0;
							if (num >= 0)
							{
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort**, ushort**, int*, int*, int*, int*, int*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 128)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3, num4, (ushort**)(&wBSTRString), ptr, &teenagerAge, &adultAge, &num6, &num7, &num8);
								if (num >= 0)
								{
									CountryFieldValidator[] array2 = new CountryFieldValidator[num8];
									int num9 = 0;
									if (0 < num8)
									{
										do
										{
											ushort* value = null;
											ushort* value2 = null;
											ushort* value3 = null;
											ushort* value4 = null;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort**, ushort**, ushort**, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 136)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3, num9, &value, &value2, &value3, &value4);
											if (num >= 0)
											{
												CountryFieldValidator countryFieldValidator = new CountryFieldValidator(new string((char*)value), new string((char*)value2), new string((char*)value3), new string((char*)value4));
												array2[num9] = countryFieldValidator;
											}
											global::_003CModule_003E.SafeSysFreeString(&value);
											global::_003CModule_003E.SafeSysFreeString(&value2);
											global::_003CModule_003E.SafeSysFreeString(&value3);
											global::_003CModule_003E.SafeSysFreeString(&value4);
											num9++;
										}
										while (num9 < num8);
									}
									if (num >= 0)
									{
										string abbreviation = new string((char*)(*(ulong*)(&wBSTRString)));
										string[] array3 = new string[num4];
										int num10 = 0;
										if (0 < num4)
										{
											ushort** ptr2 = ptr;
											do
											{
												array3[num10] = new string((char*)(*(ulong*)ptr2));
												num10++;
												ptr2 = (ushort**)((ulong)(nint)ptr2 + 8uL);
											}
											while (num10 < num4);
										}
										bool usageCollection = ((num7 != 0) ? true : false);
										bool showNewsletterOptions = ((num6 != 0) ? true : false);
										array[num3] = new CountryBaseDetails(abbreviation, array3, teenagerAge, adultAge, showNewsletterOptions, usageCollection, array2);
									}
								}
							}
							if (ptr != null)
							{
								if (0 < num4)
								{
									ushort** ptr3 = ptr;
									int num11 = num4;
									do
									{
										global::_003CModule_003E.SafeSysFreeString(ptr3);
										ptr3 = (ushort**)((ulong)(nint)ptr3 + 8uL);
										num11 += -1;
									}
									while (num11 != 0);
								}
								global::_003CModule_003E.delete_005B_005D(ptr);
							}
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
							throw;
						}
						global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
						num3++;
					}
					while (num3 < num2);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
		}
		return array;
	}

	public unsafe RatingSystemBase[] GetRatingSystems()
	{
		//IL_002f: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		//IL_008e: Expected I, but got I8
		//IL_008e: Expected I, but got I8
		//IL_00a5: Expected I, but got I8
		//IL_00a5: Expected I, but got I8
		//IL_0105: Expected I, but got I8
		//IL_0109: Expected I, but got I8
		//IL_0146: Expected I, but got I8
		//IL_01c6: Expected I, but got I8
		//IL_01d8: Expected I, but got I8
		//IL_0201: Expected I, but got I8
		//IL_043a: Expected I, but got I8
		//IL_043f: Expected I, but got I8
		//IL_0444: Expected I, but got I8
		//IL_044c: Expected I, but got I8
		//IL_046d: Expected I, but got I8
		//IL_033f: Expected I, but got I8
		//IL_033f: Expected I, but got I8
		//IL_0223: Expected I, but got I8
		//IL_022e: Expected I, but got I8
		//IL_023a: Expected I, but got I8
		//IL_026d: Expected I, but got I8
		//IL_0356: Expected I, but got I8
		//IL_035b: Expected I, but got I8
		//IL_0360: Expected I, but got I8
		//IL_0368: Expected I, but got I8
		//IL_0387: Expected I, but got I8
		//IL_029d: Expected I, but got I8
		RatingSystemBase[] array = null;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
			*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E));
				int num2 = 0;
				if (num >= 0)
				{
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 144)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)));
					array = new RatingSystemBase[num2];
				}
				int num3 = 0;
				if (0 < num2)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString2);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString3);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString4);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString5);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString6);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString7);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString8);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString9);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString10);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString11);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out int order);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out int num18);
					do
					{
						int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 168)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3);
						int num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 160)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3);
						int num6 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 152)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3);
						Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
						global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
						try
						{
							global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString2);
							try
							{
								global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString3);
								try
								{
									global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString4);
									try
									{
										global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString5);
										try
										{
											int num7 = 0;
											int num8 = 0;
											int num9 = 0;
											if (0 < num5)
											{
												do
												{
													ITunerConfig* ptr = global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_002D_003E(&cComPtrNtv_003CITunerConfig_003E);
													int num10 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int>)(*(ulong*)(*(long*)ptr + 176)))((nint)ptr, num3, num9);
													ushort** ptr2 = null;
													if (num >= 0)
													{
														ulong num11 = (ulong)num10;
														ptr2 = (ushort**)global::_003CModule_003E.new_005B_005D((num11 > 2305843009213693951L) ? ulong.MaxValue : (num11 * 8));
														num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
													}
													ushort** ptr3 = null;
													if (num >= 0)
													{
														ulong num12 = (ulong)num10;
														ptr3 = (ushort**)global::_003CModule_003E.new_005B_005D((num12 > 2305843009213693951L) ? ulong.MaxValue : (num12 * 8));
													}
													global::_003CModule_003E.WString_002EDeleteString((WString*)(&wBSTRString));
													global::_003CModule_003E.WString_002EDeleteString((WString*)(&wBSTRString2));
													global::_003CModule_003E.WString_002EDeleteString((WString*)(&wBSTRString3));
													global::_003CModule_003E.WString_002EDeleteString((WString*)(&wBSTRString4));
													global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString6);
													try
													{
														ITunerConfig* ptr4 = global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_002D_003E(&cComPtrNtv_003CITunerConfig_003E);
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, ushort**, ushort**, ushort**, ushort**, ushort**, int*, int*, ushort**, ushort**, int>)(*(ulong*)(*(long*)ptr4 + 184)))((nint)ptr4, num3, num9, num10, (ushort**)(&wBSTRString), (ushort**)(&wBSTRString2), (ushort**)(&wBSTRString3), (ushort**)(&wBSTRString4), (ushort**)(&wBSTRString6), &num7, &num8, ptr2, ptr3);
														if (num >= 0)
														{
															string key = new string((char*)(*(ulong*)(&wBSTRString6)));
															Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
															if (num9 == num6)
															{
																global::_003CModule_003E.WString_002EDeleteString((WString*)(&wBSTRString5));
																global::_003CModule_003E.WString_002EInit((WString*)(&wBSTRString5), (ushort*)(*(ulong*)(&wBSTRString6)), ulong.MaxValue);
															}
															long num13 = num10;
															if (0 < num13)
															{
																ushort** ptr5 = ptr2;
																ushort** ptr6 = (ushort**)((byte*)ptr3 - (nuint)ptr2);
																ulong num14 = (ulong)num13;
																do
																{
																	dictionary2.Add(new string((char*)(*(ulong*)ptr5)), new string((char*)(*(ulong*)((byte*)ptr6 + (nuint)ptr5))));
																	ptr5 = (ushort**)((ulong)(nint)ptr5 + 8uL);
																	num14--;
																}
																while (num14 != 0);
															}
															dictionary.Add(key, dictionary2);
															if (ptr2 != null)
															{
																if (0 < num10)
																{
																	ushort** ptr7 = ptr2;
																	int num15 = num10;
																	do
																	{
																		global::_003CModule_003E.SafeSysFreeString(ptr7);
																		ptr7 = (ushort**)((ulong)(nint)ptr7 + 8uL);
																		num15 += -1;
																	}
																	while (num15 != 0);
																}
																global::_003CModule_003E.delete_005B_005D(ptr2);
															}
															if (ptr3 != null)
															{
																if (0 < num10)
																{
																	ushort** ptr8 = ptr3;
																	int num16 = num10;
																	do
																	{
																		global::_003CModule_003E.SafeSysFreeString(ptr8);
																		ptr8 = (ushort**)((ulong)(nint)ptr8 + 8uL);
																		num16 += -1;
																	}
																	while (num16 != 0);
																}
																global::_003CModule_003E.delete_005B_005D(ptr3);
															}
														}
													}
													catch
													{
														//try-fault
														global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString6);
														throw;
													}
													global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString6);
													num9++;
												}
												while (num9 < num5);
											}
											if (num >= 0)
											{
												RatingValue[] array2 = new RatingValue[num4];
												int num17 = 0;
												if (0 < num4)
												{
													do
													{
														global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString7);
														try
														{
															global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString8);
															try
															{
																global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString9);
																try
																{
																	global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString10);
																	try
																	{
																		global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString11);
																		try
																		{
																			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort**, int*, ushort**, ushort**, ushort**, ushort**, int*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 192)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), num3, num17, (ushort**)(&wBSTRString7), &order, (ushort**)(&wBSTRString10), (ushort**)(&wBSTRString8), (ushort**)(&wBSTRString9), (ushort**)(&wBSTRString11), &num18);
																			if (num >= 0)
																			{
																				bool treatAsUnrated = ((num18 != 0) ? true : false);
																				ushort* value = (ushort*)(*(ulong*)(&wBSTRString9));
																				ushort* value2 = (ushort*)(*(ulong*)(&wBSTRString10));
																				ushort* value3 = (ushort*)(*(ulong*)(&wBSTRString8));
																				RatingValue ratingValue = new RatingValue(new string((char*)(*(ulong*)(&wBSTRString7))), order, new string((char*)value3), new string((char*)value2), new string((char*)value), new string((char*)(*(ulong*)(&wBSTRString11))), treatAsUnrated);
																				array2[num17] = ratingValue;
																			}
																		}
																		catch
																		{
																			//try-fault
																			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString11);
																			throw;
																		}
																		global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString11);
																	}
																	catch
																	{
																		//try-fault
																		global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString10);
																		throw;
																	}
																	global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString10);
																}
																catch
																{
																	//try-fault
																	global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString9);
																	throw;
																}
																global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString9);
															}
															catch
															{
																//try-fault
																global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString8);
																throw;
															}
															global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString8);
														}
														catch
														{
															//try-fault
															global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString7);
															throw;
														}
														global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString7);
														num17++;
													}
													while (num17 < num4);
												}
												if (num >= 0)
												{
													bool showBlockUnrated = ((num8 != 0) ? true : false);
													bool useImages = ((num7 != 0) ? true : false);
													ushort* value4 = (ushort*)(*(ulong*)(&wBSTRString4));
													ushort* value5 = (ushort*)(*(ulong*)(&wBSTRString3));
													ushort* value6 = (ushort*)(*(ulong*)(&wBSTRString2));
													RatingSystemBase ratingSystemBase = new RatingSystemBase(new string((char*)(*(ulong*)(&wBSTRString))), new string((char*)value6), new string((char*)value5), new string((char*)value4), useImages, showBlockUnrated, new string((char*)(*(ulong*)(&wBSTRString5))), dictionary, array2);
													array[num3] = ratingSystemBase;
												}
											}
										}
										catch
										{
											//try-fault
											global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString5);
											throw;
										}
										global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString5);
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString4);
										throw;
									}
									global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString4);
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString3);
									throw;
								}
								global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString3);
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString2);
								throw;
							}
							global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString2);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
							throw;
						}
						global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
						num3++;
					}
					while (num3 < num2);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D(&cComPtrNtv_003CITunerConfig_003E);
		}
		return array;
	}

	public unsafe int GetRentalTermDays(string strStudio)
	{
		//IL_0030: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		int result = 14;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strStudio)))
		{
			if (m_pService != null)
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
				*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
				try
				{
					IService* pService = m_pService;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
					{
						long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 208;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), ptr, &result);
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
			}
			return result;
		}
	}

	public unsafe int GetRentalTermHours(string strStudio)
	{
		//IL_0030: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		int result = 24;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strStudio)))
		{
			if (m_pService != null)
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
				*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
				try
				{
					IService* pService = m_pService;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
					{
						long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 216;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), ptr, &result);
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
			}
			return result;
		}
	}

	public unsafe string GetPhoneClientType(string strPhoneOsVersion)
	{
		//IL_0006: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		string result = null;
		ushort* ptr = null;
		fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPhoneOsVersion)))
		{
			if (m_pService != null)
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
				*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
				try
				{
					IService* pService = m_pService;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E));
					if (num >= 0)
					{
						long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 224;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort**, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), ptr2, &ptr);
						if (num >= 0)
						{
							result = Marshal.PtrToStringBSTR((IntPtr)ptr);
						}
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
			}
			return result;
		}
	}

	public unsafe int GetSubscriptionTrialDuration()
	{
		//IL_0028: Expected I, but got I8
		//IL_0043: Expected I, but got I8
		//IL_0043: Expected I, but got I8
		int result = 0;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E);
			*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)pService + 176)))((nint)pService, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 232)))((nint)(*(long*)(&cComPtrNtv_003CITunerConfig_003E)), &result);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
		}
		return result;
	}

	public unsafe AppOfferCollection CreateEmptyAppCollection()
	{
		//IL_0025: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		AppOfferCollection appOfferCollection = null;
		if (m_pService != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAppCollection_003E cComPtrNtv_003CIAppCollection_003E);
			*(long*)(&cComPtrNtv_003CIAppCollection_003E) = 0L;
			try
			{
				IService* pService = m_pService;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAppCollection**, int>)(*(ulong*)(*(long*)pService + 72)))((nint)pService, (IAppCollection**)(&cComPtrNtv_003CIAppCollection_003E)) >= 0)
				{
					appOfferCollection = new AppOfferCollection();
					appOfferCollection.Init((IAppCollection*)(*(ulong*)(&cComPtrNtv_003CIAppCollection_003E)));
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAppCollection_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAppCollection_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAppCollection_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIAppCollection_003E_002ERelease(&cComPtrNtv_003CIAppCollection_003E);
		}
		return appOfferCollection;
	}

	public EContentType GetContentType(string contentTypeStr)
	{
		EContentType eContentType = EContentType.Unknown;
		int result;
		switch (contentTypeStr)
		{
		case "type:musictrack":
			return EContentType.MusicTrack;
		case "type:musicvideo":
			return EContentType.Video;
		case "type:podcast":
			return EContentType.PodcastEpisode;
		default:
			result = (int)eContentType;
			break;
		case "type:app":
			result = 7;
			break;
		}
		return (EContentType)result;
	}

	public unsafe void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler)
	{
		//IL_000b: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		//IL_0069: Expected I, but got I8
		//IL_0027: Expected I, but got I8
		if (m_pService == null)
		{
			return;
		}
		AsyncCallbackWrapper* ptr = null;
		if (eventHandler != null)
		{
			AsyncCallbackWrapper* ptr2 = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
			AsyncCallbackWrapper* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr2, eventHandler));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr2);
				throw;
			}
			ptr = ptr3;
			if (ptr3 == null)
			{
				goto IL_0059;
			}
		}
		_GUID gUID = global::_003CModule_003E.GuidToGUID(guidMediaInstanceId);
		IService* pService = m_pService;
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, global::EStreamingActionType, _GUID, IAsyncCallback*, int>)(*(ulong*)(*(long*)pService + 984)))((nint)pService, (global::EStreamingActionType)eStreamingActionType, gUID, (IAsyncCallback*)ptr);
		goto IL_0059;
		IL_0059:
		if (ptr != null)
		{
			AsyncCallbackWrapper* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021Service();
			return;
		}
		try
		{
			_0021Service();
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

	~Service()
	{
		Dispose(false);
	}
}
