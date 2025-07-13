using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util;

public class TelemetryAPI
{
	public unsafe static void SendDatapoint(string command, IDictionary dictionary)
	{
		//IL_01b1: Expected I, but got I8
		//IL_01b1: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(command)))
		{
			int count = dictionary.Keys.Count;
			tagSAFEARRAY* ptr2 = global::_003CModule_003E.SafeArrayCreateVector(12, 0, (uint)count);
			tagSAFEARRAY* ptr3 = global::_003CModule_003E.SafeArrayCreateVector(12, 0, (uint)count);
			int num = 0;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out tagVARIANT tagVARIANT);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out tagVARIANT tagVARIANT2);
			foreach (DictionaryEntry item in dictionary)
			{
				int num2 = 0;
				if (((DictionaryEntry)item).Key.GetType() == typeof(string))
				{
					IntPtr pDstNativeVariant = (IntPtr)(&tagVARIANT);
					Marshal.GetNativeVariantForObject(((DictionaryEntry)item).Key, pDstNativeVariant);
					if (((DictionaryEntry)item).Value.GetType() == typeof(Guid))
					{
						string obj = ((DictionaryEntry)item).Value.ToString();
						IntPtr pDstNativeVariant2 = (IntPtr)(&tagVARIANT2);
						Marshal.GetNativeVariantForObject(obj, pDstNativeVariant2);
					}
					else
					{
						if (((DictionaryEntry)item).Value.GetType() != typeof(string) && ((DictionaryEntry)item).Value.GetType() != typeof(int) && ((DictionaryEntry)item).Value.GetType() != typeof(long))
						{
							num2 = -2147024809;
							continue;
						}
						IntPtr pDstNativeVariant3 = (IntPtr)(&tagVARIANT2);
						Marshal.GetNativeVariantForObject(((DictionaryEntry)item).Value, pDstNativeVariant3);
					}
					global::_003CModule_003E.SafeArrayPutElement(ptr2, &num, &tagVARIANT);
					global::_003CModule_003E.SafeArrayPutElement(ptr3, &num, &tagVARIANT2);
					num++;
				}
				else
				{
					num2 = -2147024809;
				}
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E);
			*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
			try
			{
				if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
				{
					long num3 = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 48;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, tagSAFEARRAY*, tagSAFEARRAY*, int>)(*(ulong*)num3))((nint)(*(long*)(&cComPtrNtv_003CITelemetryManager_003E)), ptr, ptr2, ptr3);
				}
				if (ptr2 != null)
				{
					global::_003CModule_003E.SafeArrayDestroy(ptr2);
				}
				if (ptr3 != null)
				{
					global::_003CModule_003E.SafeArrayDestroy(ptr3);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
		}
	}

	public unsafe static void AddToSessionEvent(ETelemetryEvent evt, string key, int value)
	{
		//IL_0033: Expected I, but got I8
		//IL_0033: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(key)))
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E);
			*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
			try
			{
				if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
				{
					long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 64;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int, int>)(*(ulong*)num))((nint)(*(long*)(&cComPtrNtv_003CITelemetryManager_003E)), evt, ptr, value);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
		}
	}

	public unsafe static void SendEvent(ETelemetryEvent eEvent, string eventParameter)
	{
		//IL_0032: Expected I, but got I8
		//IL_0032: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(eventParameter)))
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E);
			*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
			try
			{
				if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
				{
					long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 56;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int>)(*(ulong*)num))((nint)(*(long*)(&cComPtrNtv_003CITelemetryManager_003E)), eEvent, ptr);
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
		}
	}
}
