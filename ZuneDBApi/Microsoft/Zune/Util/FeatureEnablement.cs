using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class FeatureEnablement
	{
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool IsFeatureEnabled(Features eFeature)
		{
			//IL_0003: Expected I, but got I8
			//IL_0024: Expected I, but got I8
			//IL_0037: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			bool result = false;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFeatures, bool*, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (EFeatures)eFeature, &result);
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return result;
		}

		public unsafe static void ForceFeatureOn(Features eFeature)
		{
			//IL_0003: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFeatures, int>)(*(ulong*)(*(long*)ptr + 32)))((nint)intPtr, (EFeatures)eFeature);
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static string GetRegion()
		{
			//IL_0003: Expected I, but got I8
			//IL_0008: Expected I, but got I8
			//IL_0026: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			string result = null;
			ushort* ptr2 = null;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)ptr + 40)))((nint)intPtr, &ptr2) >= 0)
				{
					result = new string((char*)ptr2);
					Module.SysFreeString(ptr2);
				}
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return result;
		}

		public unsafe static uint GetGeoId()
		{
			//IL_0003: Expected I, but got I8
			//IL_0025: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			uint num = uint.MaxValue;
			uint num2 = uint.MaxValue;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr, &num2) >= 0) ? num2 : num);
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return num;
		}

		public unsafe static uint GetInvalidGeoId()
		{
			//IL_0003: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			int result = -1;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				result = (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 64)))((nint)intPtr);
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return (uint)result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool HasValidRegionAndLanguage()
		{
			//IL_0003: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			IFeatureEnablementManager* ptr = null;
			bool result = false;
			if (Module.GetSingleton((_GUID)Module._GUID_9581b41a_b5cf_4ebf_9d1a_975477e081ca, (void**)(&ptr)) >= 0)
			{
				IFeatureEnablementManager* intPtr = ptr;
				bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 72)))((nint)intPtr) != 0) ? true : false);
				result = flag;
			}
			if (0L != (nint)ptr)
			{
				IFeatureEnablementManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return result;
		}

		public unsafe static string GetMarketplaceCulture()
		{
			//IL_003c: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
			*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
			string result;
			try
			{
				int singleton = Module.GetSingleton((_GUID)Module._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
				result = null;
				if (singleton >= 0)
				{
					WBSTRString wBSTRString;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
					try
					{
						CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E;
						*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
						try
						{
							long num = *(long*)(&cComPtrNtv_003CIService_003E);
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 168)))((nint)num) != 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 176)))((nint)num2, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CITunerConfig_003E);
									Module.WString_002EAttachBSTR((WString*)(&wBSTRString), ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 72)))((nint)num3));
								}
							}
							result = new string((char*)(*(ulong*)(&wBSTRString)));
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&Module.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
							throw;
						}
						Module.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
			return result;
		}

		public unsafe static string GetLynxCulture()
		{
			//IL_003a: Expected I, but got I8
			//IL_003a: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_0068: Expected I, but got I8
			//IL_0068: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
			*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
			string result;
			try
			{
				if (Module.GetSingleton((_GUID)Module._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E)) >= 0)
				{
					WBSTRString wBSTRString;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
					try
					{
						CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E;
						*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
						try
						{
							long num = *(long*)(&cComPtrNtv_003CIService_003E);
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 168)))((nint)num) != 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 176)))((nint)num2, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CITunerConfig_003E);
									Module.WString_002EAttachBSTR((WString*)(&wBSTRString), ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 80)))((nint)num3));
								}
							}
							result = new string((char*)(*(ulong*)(&wBSTRString)));
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&Module.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
							throw;
						}
						Module.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
					goto IL_00b4;
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			string result2;
			try
			{
				result2 = null;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
			return result2;
			IL_00b4:
			Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
			return result;
		}

		public unsafe static string GetTaxString()
		{
			//IL_003c: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
			*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
			string result;
			try
			{
				int singleton = Module.GetSingleton((_GUID)Module._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
				result = null;
				if (singleton >= 0)
				{
					WBSTRString wBSTRString;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
					try
					{
						CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E;
						*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
						try
						{
							long num = *(long*)(&cComPtrNtv_003CIService_003E);
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 168)))((nint)num) != 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 176)))((nint)num2, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CITunerConfig_003E);
									Module.WString_002EAttachBSTR((WString*)(&wBSTRString), ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 88)))((nint)num3));
								}
							}
							result = new string((char*)(*(ulong*)(&wBSTRString)));
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&Module.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
							throw;
						}
						Module.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
			return result;
		}

		public unsafe static string GetCreditCardValidationString()
		{
			//IL_003c: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
			*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
			string result;
			try
			{
				int singleton = Module.GetSingleton((_GUID)Module._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
				result = null;
				if (singleton >= 0)
				{
					WBSTRString wBSTRString;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
					try
					{
						CComPtrNtv_003CITunerConfig_003E cComPtrNtv_003CITunerConfig_003E;
						*(long*)(&cComPtrNtv_003CITunerConfig_003E) = 0L;
						try
						{
							long num = *(long*)(&cComPtrNtv_003CIService_003E);
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 168)))((nint)num) != 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ITunerConfig**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 176)))((nint)num2, (ITunerConfig**)(&cComPtrNtv_003CITunerConfig_003E)) >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CITunerConfig_003E);
									Module.WString_002EAttachBSTR((WString*)(&wBSTRString), ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CITunerConfig_003E)) + 96)))((nint)num3));
								}
							}
							result = new string((char*)(*(ulong*)(&wBSTRString)));
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CITunerConfig_003E*, void>)(&Module.CComPtrNtv_003CITunerConfig_003E_002E_007Bdtor_007D), &cComPtrNtv_003CITunerConfig_003E);
							throw;
						}
						Module.CComPtrNtv_003CITunerConfig_003E_002ERelease(&cComPtrNtv_003CITunerConfig_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
			return result;
		}

		private FeatureEnablement()
		{
		}
	}
}
