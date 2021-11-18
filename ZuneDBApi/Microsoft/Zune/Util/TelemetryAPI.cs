using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class TelemetryAPI
	{
		public unsafe static void SendDatapoint(string command, IDictionary dictionary)
		{
			//IL_01b1: Expected I, but got I8
			//IL_01b1: Expected I, but got I8
			fixed (char* commandPtr = command.ToCharArray())
			{
				ushort* ptr3 = (ushort*)commandPtr;
				int count = dictionary.Keys.Count;
				tagSAFEARRAY* ptr = Module.SafeArrayCreateVector(12, 0, (uint)count);
				tagSAFEARRAY* ptr2 = Module.SafeArrayCreateVector(12, 0, (uint)count);
				int num = 0;
				foreach (DictionaryEntry item in dictionary)
				{
					int num2 = 0;
					if (((DictionaryEntry)item).Key.GetType() == typeof(string))
					{
						tagVARIANT tagVARIANT;
						IntPtr pDstNativeVariant = (IntPtr)(&tagVARIANT);
						Marshal.GetNativeVariantForObject(((DictionaryEntry)item).Key, pDstNativeVariant);
						tagVARIANT tagVARIANT2;
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
						Module.SafeArrayPutElement(ptr, &num, &tagVARIANT);
						Module.SafeArrayPutElement(ptr2, &num, &tagVARIANT2);
						num++;
					}
					else
					{
						num2 = -2147024809;
					}
				}
				CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E;
				*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
				try
				{
					if (Module.GetSingleton((_GUID)Module._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
					{
						long num3 = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 48;
						long num4 = *(long*)(&cComPtrNtv_003CITelemetryManager_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, tagSAFEARRAY*, tagSAFEARRAY*, int>)(*(ulong*)num3))((nint)num4, ptr3, ptr, ptr2);
					}
					if (ptr != null)
					{
						Module.SafeArrayDestroy(ptr);
					}
					if (ptr2 != null)
					{
						Module.SafeArrayDestroy(ptr2);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&Module.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
					throw;
				}
				Module.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
			}
		}

		public unsafe static void AddToSessionEvent(ETelemetryEvent evt, string key, int value)
		{
			//IL_0033: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			fixed (char* keyPtr = key.ToCharArray())
			{
				ushort* ptr = (ushort*)keyPtr;
				CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E;
				*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
				try
				{
					if (Module.GetSingleton((_GUID)Module._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
					{
						long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 64;
						long num2 = *(long*)(&cComPtrNtv_003CITelemetryManager_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int, int>)(*(ulong*)num))((nint)num2, evt, ptr, value);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&Module.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
					throw;
				}
				Module.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
			}
		}

		public unsafe static void SendEvent(ETelemetryEvent eEvent, string eventParameter)
		{
			//IL_0032: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			fixed (char* eventParameterPtr = eventParameter.ToCharArray())
			{
				ushort* ptr = (ushort*)eventParameterPtr;
				CComPtrNtv_003CITelemetryManager_003E cComPtrNtv_003CITelemetryManager_003E;
				*(long*)(&cComPtrNtv_003CITelemetryManager_003E) = 0L;
				try
				{
					if (Module.GetSingleton((_GUID)Module._GUID_ab28333b_a55c_4312_a7a3_2dd60d4a7154, (void**)(&cComPtrNtv_003CITelemetryManager_003E)) >= 0)
					{
						long num = *(long*)(*(ulong*)(&cComPtrNtv_003CITelemetryManager_003E)) + 56;
						long num2 = *(long*)(&cComPtrNtv_003CITelemetryManager_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int>)(*(ulong*)num))((nint)num2, eEvent, ptr);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITelemetryManager_003E*, void>)(&Module.CComPtrNtv_003CITelemetryManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITelemetryManager_003E);
					throw;
				}
				Module.CComPtrNtv_003CITelemetryManager_003E_002ERelease(&cComPtrNtv_003CITelemetryManager_003E);
			}
		}
	}
}
