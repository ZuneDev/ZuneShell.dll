using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using DataStructs;
using ZuneUI;

namespace Microsoft.Zune.Util;

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
						System.Runtime.CompilerServices.Unsafe.SkipInit(out IMetadataManager* ptr);
						int singleton = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
						if (singleton < 0)
						{
							throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(singleton));
						}
						System.Runtime.CompilerServices.Unsafe.SkipInit(out IFamilySettingsProvider* pProvider);
						int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)ptr, (_GUID)global::_003CModule_003E._GUID_04f38ab5_391b_4b5a_a2c1_d4b74aeb4be9, (void**)(&pProvider));
						if (num < 0)
						{
							throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
						}
						Thread.MemoryBarrier();
						familySettingsManager.m_pProvider = pProvider;
						if (0L != (nint)ptr)
						{
							IMetadataManager* intPtr = ptr;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
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
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(szRatingSystem)))
		{
			long num2 = *(long*)m_pProvider + 24;
			int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort*, int, int, int*, int>)(*(ulong*)num2))((nint)m_pProvider, nSettingId, nUserId, ptr, nRatingLevel, fBlockUnrated ? 1 : 0, &num);
			settingId = num;
			return new HRESULT(hr);
		}
	}

	public unsafe HRESULT GetSettingIdsForUser(int nUserId, out int[] rgSettingIds)
	{
		//IL_001f: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out IntSet intSet);
		global::_003CModule_003E.DataStructs_002EIntSet_002E_007Bctor_007D(&intSet);
		HRESULT result;
		try
		{
			IFamilySettingsProvider* pProvider = m_pProvider;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IntSet*, int>)(*(ulong*)(*(long*)pProvider + 32)))((nint)pProvider, nUserId, &intSet);
			if (num >= 0 && System.Runtime.CompilerServices.Unsafe.As<IntSet, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref intSet, 8)) != -1)
			{
				rgSettingIds = new int[global::_003CModule_003E.DataStructs_002EIntSet_002EMemberCount(&intSet)];
				int num2 = System.Runtime.CompilerServices.Unsafe.As<IntSet, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref intSet, 4));
				int num3 = 0;
				if (System.Runtime.CompilerServices.Unsafe.As<IntSet, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref intSet, 4)) != -1)
				{
					do
					{
						rgSettingIds[num3] = num2;
						num3++;
						num2 = global::_003CModule_003E.DataStructs_002EIntSet_002EGetNextMember(&intSet, num2);
					}
					while (num2 != -1);
				}
			}
			result = num;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IntSet*, void>)(&global::_003CModule_003E.DataStructs_002EIntSet_002E_007Bdtor_007D), &intSet);
			throw;
		}
		global::_003CModule_003E.DataStructs_002EIntSet_002EFreeData(&intSet);
		return result;
	}

	public unsafe HRESULT GetSetting(int nSettingId, out string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
	{
		//IL_0008: Expected I4, but got I8
		//IL_0023: Expected I, but got I8
		//IL_0033: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
		HRESULT result;
		try
		{
			IFamilySettingsProvider* pProvider = m_pProvider;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num2);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num3);
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, tagPROPVARIANT*, int*, int*, int>)(*(ulong*)(*(long*)pProvider + 40)))((nint)pProvider, nSettingId, (tagPROPVARIANT*)(&cComPropVariant), &num2, &num3);
			if (0 == num)
			{
				szRatingSystem = new string((char*)System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)));
				nRatingLevel = num2;
				bool flag = ((num3 != 0) ? true : false);
				fBlockUnrated = flag;
			}
			result = num;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
			throw;
		}
		global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
		return result;
	}

	public unsafe HRESULT GetSettingForSystem(int nUserId, string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
	{
		//IL_002a: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(szRatingSystem)))
		{
			long num = *(long*)m_pProvider + 48;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num3);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num4);
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, int*, int*, int>)(*(ulong*)num))((nint)m_pProvider, nUserId, ptr, &num3, &num4);
			if (0 == num2)
			{
				nRatingLevel = num3;
				bool flag = ((num4 != 0) ? true : false);
				fBlockUnrated = flag;
			}
			return num2;
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
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~FamilySettingsManager()
	{
		Dispose(false);
	}
}
