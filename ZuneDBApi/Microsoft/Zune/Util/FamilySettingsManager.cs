using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using DataStructs;
using ZuneUI;

namespace Microsoft.Zune.Util
{
	public class FamilySettingsManager : IDisposable
	{
		private unsafe IFamilySettingsProvider* m_pProvider = null;

		private static FamilySettingsManager sm_manager = null;

		private static object sm_lock = new object();

		public unsafe static FamilySettingsManager Instance
		{
			get
			{
				//IL_0052: Expected I, but got I8
				//IL_0082: Expected I, but got I8
				//IL_0086: Expected I, but got I8
				if (sm_manager == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_manager == null)
						{
							FamilySettingsManager familySettingsManager = new FamilySettingsManager();
							IMetadataManager* ptr;
							int singleton = Module.GetSingleton(Module.GUID_IMetadataManager, (void**)(&ptr));
							if (singleton < 0)
							{
								throw new ApplicationException(Module.GetErrorDescription(singleton));
							}
							IMetadataManager* intPtr = ptr;
							_GUID guid_IFamilySettingsProvider = Module.GUID_IFamilySettingsProvider;
							IFamilySettingsProvider* pProvider;
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, guid_IFamilySettingsProvider, (void**)(&pProvider));
							if (num < 0)
							{
								throw new ApplicationException(Module.GetErrorDescription(num));
							}
							Thread.MemoryBarrier();
							familySettingsManager.m_pProvider = pProvider;
							if (0L != (nint)ptr)
							{
								IMetadataManager* intPtr2 = ptr;
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
								ptr = null;
							}
							sm_manager = familySettingsManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_manager;
			}
		}

		private void _007EFamilySettingsManager()
		{
			_0021FamilySettingsManager();
		}

		private unsafe void _0021FamilySettingsManager()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IFamilySettingsProvider* pProvider = m_pProvider;
			if (0L != (nint)pProvider)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pProvider + 16)))((nint)pProvider);
				m_pProvider = null;
			}
		}

		public unsafe HRESULT AddSetting(int nSettingId, int nUserId, string szRatingSystem, int nRatingLevel, [MarshalAs(UnmanagedType.U1)] bool fBlockUnrated, out int settingId)
		{
			//IL_002c: Expected I, but got I8
			int num = -1;
			fixed (char* szRatingSystemPtr = szRatingSystem.ToCharArray())
			{
				ushort* ptr = (ushort*)szRatingSystemPtr;
				long num2 = *(long*)m_pProvider + 24;
				IFamilySettingsProvider* pProvider = m_pProvider;
				int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort*, int, int, int*, int>)(*(ulong*)num2))((nint)pProvider, nSettingId, nUserId, ptr, nRatingLevel, fBlockUnrated ? 1 : 0, &num);
				settingId = num;
				return new HRESULT(hr);
			}
		}

		public unsafe HRESULT GetSettingIdsForUser(int nUserId, out int[] rgSettingIds)
		{
			//IL_001f: Expected I, but got I8
			IntSet intSet;
			Module.DataStructs_002EIntSet_002E_007Bctor_007D(&intSet);
			HRESULT result;
			try
			{
				IFamilySettingsProvider* pProvider = m_pProvider;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IntSet*, int>)(*(ulong*)(*(long*)pProvider + 32)))((nint)pProvider, nUserId, &intSet);
				if (num >= 0 && !(Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 8)) == -1))
				{
					rgSettingIds = new int[Module.DataStructs_002EIntSet_002EMemberCount(&intSet)];
					int num2 = Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 4));
					int num3 = 0;
					if (Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 4)) != -1)
					{
						do
						{
							rgSettingIds[num3] = num2;
							num3++;
							num2 = Module.DataStructs_002EIntSet_002EGetNextMember(&intSet, num2);
						}
						while (num2 != -1);
					}
				}
				result = num;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IntSet*, void>)(&Module.DataStructs_002EIntSet_002E_007Bdtor_007D), &intSet);
				throw;
			}
			Module.DataStructs_002EIntSet_002EFreeData(&intSet);
			return result;
		}

		public unsafe HRESULT GetSetting(int nSettingId, out string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
		{
            //IL_0008: Expected I4, but got I8
            //IL_0023: Expected I, but got I8
            //IL_0033: Expected I, but got I8
            PROPVARIANT cComPropVariant = new();
			HRESULT result;
			try
			{
				IFamilySettingsProvider* pProvider = m_pProvider;
				int num;
				int num2;
				int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, PROPVARIANT, int*, int*, int>)(*(ulong*)(*(long*)pProvider + 40)))((nint)pProvider, (int)(int)nSettingId, (PROPVARIANT)(PROPVARIANT)cComPropVariant, (int*)(int*)&num, (int*)(int*)&num2);
				if (0 == num3)
				{
					szRatingSystem = new string((char*)Unsafe.As<PROPVARIANT, ulong>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)));
					nRatingLevel = num;
					bool flag = (fBlockUnrated = ((num2 != 0) ? true : false));
				}
				result = num3;
			}
			catch
			{
                //try-fault
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			cComPropVariant.Clear();
			return result;
		}

		public unsafe HRESULT GetSettingForSystem(int nUserId, string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
		{
			//IL_002a: Expected I, but got I8
			fixed (char* szRatingSystemPtr = szRatingSystem.ToCharArray())
			{
				ushort* ptr = (ushort*)szRatingSystemPtr;
				long num = *(long*)m_pProvider + 48;
				IFamilySettingsProvider* pProvider = m_pProvider;
				int num2;
				int num3;
				int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, int*, int*, int>)(*(ulong*)num))((nint)pProvider, nUserId, ptr, &num2, &num3);
				if (0 == num4)
				{
					nRatingLevel = num2;
					bool flag = (fBlockUnrated = ((num3 != 0) ? true : false));
				}
				return num4;
			}
		}

		private unsafe FamilySettingsManager()
		{
		}//IL_0008: Expected I, but got I8


		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021FamilySettingsManager();
				return;
			}
			try
			{
				_0021FamilySettingsManager();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~FamilySettingsManager()
		{
			Dispose(false);
		}
	}
}
