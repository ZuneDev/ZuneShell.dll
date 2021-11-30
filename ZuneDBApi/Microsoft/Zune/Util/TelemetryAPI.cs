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
			fixed (char* commandPtr = command)
			{
				ushort* ptr3 = (ushort*)commandPtr;
				int count = dictionary.Keys.Count;
				tagSAFEARRAY* ptr = Module.SafeArrayCreateVector(12, 0, (uint)count);
				tagSAFEARRAY* ptr2 = Module.SafeArrayCreateVector(12, 0, (uint)count);
				int num = 0;
				foreach (DictionaryEntry item in dictionary)
				{
					int num2 = 0;
					if (item.Key.GetType() == typeof(string))
					{
						VARIANT tagVARIANT;
						IntPtr pDstNativeVariant = (IntPtr)(&tagVARIANT);
						Marshal.GetNativeVariantForObject(item.Key, pDstNativeVariant);
						VARIANT tagVARIANT2;
						if (item.Value.GetType() == typeof(Guid))
						{
							string obj = item.Value.ToString();
							IntPtr pDstNativeVariant2 = (IntPtr)(&tagVARIANT2);
							Marshal.GetNativeVariantForObject(obj, pDstNativeVariant2);
						}
						else
						{
							if (item.Value.GetType() != typeof(string) && item.Value.GetType() != typeof(int) && item.Value.GetType() != typeof(long))
							{
								num2 = -2147024809;
								continue;
							}
							IntPtr pDstNativeVariant3 = (IntPtr)(&tagVARIANT2);
							Marshal.GetNativeVariantForObject(item.Value, pDstNativeVariant3);
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
				CComPtrNtv<ITelemetryManager> cComPtrNtv_003CITelemetryManager_003E = new();
				try
				{
					if (Module.GetSingleton(Module.GUID_ITelemetryManager, (void**)(cComPtrNtv_003CITelemetryManager_003E.p)) >= 0)
					{
						long num3 = *(long*)(*(ulong*)(cComPtrNtv_003CITelemetryManager_003E.p)) + 48;
						long num4 = *(long*)(cComPtrNtv_003CITelemetryManager_003E.p);
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
				finally
				{
					cComPtrNtv_003CITelemetryManager_003E.Dispose();
				}
			}
		}

		public unsafe static void AddToSessionEvent(ETelemetryEvent evt, string key, int value)
		{
			//IL_0033: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			fixed (char* keyPtr = key)
			{
				ushort* ptr = (ushort*)keyPtr;
				CComPtrNtv<ITelemetryManager> cComPtrNtv_003CITelemetryManager_003E = new();
				try
				{
					if (Module.GetSingleton(Module.GUID_ITelemetryManager, (void**)(cComPtrNtv_003CITelemetryManager_003E.p)) >= 0)
					{
						long num = *(long*)(*(ulong*)(cComPtrNtv_003CITelemetryManager_003E.p)) + 64;
						long num2 = *(long*)(cComPtrNtv_003CITelemetryManager_003E.p);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int, int>)(*(ulong*)num))((nint)num2, evt, ptr, value);
					}
				}
				finally
				{
					cComPtrNtv_003CITelemetryManager_003E.Dispose();
				}
			}
		}

		public unsafe static void SendEvent(ETelemetryEvent eEvent, string eventParameter)
		{
			//IL_0032: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			fixed (char* eventParameterPtr = eventParameter)
			{
				ushort* ptr = (ushort*)eventParameterPtr;
				CComPtrNtv<ITelemetryManager> cComPtrNtv_003CITelemetryManager_003E = new();
				try
				{
					if (Module.GetSingleton(Module.GUID_ITelemetryManager, (void**)(cComPtrNtv_003CITelemetryManager_003E.p)) >= 0)
					{
						long num = *(long*)(*(ulong*)(cComPtrNtv_003CITelemetryManager_003E.p)) + 56;
						long num2 = *(long*)(cComPtrNtv_003CITelemetryManager_003E.p);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ETelemetryEvent, ushort*, int>)(*(ulong*)num))((nint)num2, eEvent, ptr);
					}
				}
				finally
				{
					cComPtrNtv_003CITelemetryManager_003E.Dispose();
				}
			}
		}
	}
}
