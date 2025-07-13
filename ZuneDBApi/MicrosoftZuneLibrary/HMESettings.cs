using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class HMESettings : IDisposable
{
	private unsafe IHMESettings* m_pSettings;

	private unsafe INSSManager* m_pNSSManager;

	private unsafe INSSDevices* m_pDeviceList;

	private unsafe NSSMediator* m_pNSSMediator;

	private NSSDeviceListChangeHandler m_NSSDeviceListChange;

	public unsafe bool VelaSharingEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001c: Expected I, but got I8
			IHMESettings* pSettings = m_pSettings;
			if (pSettings == null)
			{
				return false;
			}
			return (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 168)))((nint)pSettings) != 0) ? true : false;
		}
	}

	public unsafe bool SharingEnableRequiresLoginAsAdmin
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_0019: Expected I, but got I8
			IHMESettings* pSettings = m_pSettings;
			if (pSettings == null)
			{
				return true;
			}
			return (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 112)))((nint)pSettings) != 0) ? true : false;
		}
	}

	public unsafe bool SharingEnableRequiresElevation
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_0019: Expected I, but got I8
			IHMESettings* pSettings = m_pSettings;
			if (pSettings == null)
			{
				return true;
			}
			return (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 104)))((nint)pSettings) != 0) ? true : false;
		}
	}

	public unsafe bool SharingEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001c: Expected I, but got I8
			IHMESettings* pSettings = m_pSettings;
			if (pSettings == null)
			{
				return false;
			}
			return (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 152)))((nint)pSettings) != 0) ? true : false;
		}
	}

	public bool SharingBroken
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return false;
		}
	}

	[SpecialName]
	public virtual event NSSDeviceListChangeHandler NSSDeviceListChangeEvent
	{
		add
		{
			m_NSSDeviceListChange = (NSSDeviceListChangeHandler)Delegate.Combine(m_NSSDeviceListChange, value);
		}
		remove
		{
			m_NSSDeviceListChange = (NSSDeviceListChangeHandler)Delegate.Remove(m_NSSDeviceListChange, value);
		}
	}

	public unsafe HMESettings()
	{
		//IL_000e: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		//IL_001e: Expected I, but got I8
		m_pSettings = null;
		m_pNSSManager = null;
		m_pDeviceList = null;
	}

	private void _007EHMESettings()
	{
		_0021HMESettings();
	}

	private unsafe void _0021HMESettings()
	{
		//IL_001c: Expected I, but got I8
		//IL_0025: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		//IL_0047: Expected I, but got I8
		//IL_0060: Expected I, but got I8
		//IL_0069: Expected I, but got I8
		//IL_0092: Expected I, but got I8
		//IL_009b: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (0L != (nint)pSettings)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pSettings + 16)))((nint)pSettings);
			m_pSettings = null;
		}
		INSSManager* pNSSManager = m_pNSSManager;
		if (0L != (nint)pNSSManager)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNSSManager + 16)))((nint)pNSSManager);
			m_pNSSManager = null;
		}
		INSSDevices* pDeviceList = m_pDeviceList;
		if (0L != (nint)pDeviceList)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDeviceList + 16)))((nint)pDeviceList);
			m_pDeviceList = null;
		}
		NSSMediator* pNSSMediator = m_pNSSMediator;
		if (pNSSMediator != null)
		{
			global::_003CModule_003E.NSSMediator_002EShutdown(pNSSMediator);
		}
		NSSMediator* pNSSMediator2 = m_pNSSMediator;
		if (0L != (nint)pNSSMediator2)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNSSMediator2 + 16)))((nint)pNSSMediator2);
			m_pNSSMediator = null;
		}
	}

	public unsafe int Init()
	{
		//IL_003d: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0065: Expected I, but got I8
		fixed (IHMESettings** pSettings = &m_pSettings)
		{
			int num = global::_003CModule_003E.CreateHMESettings(pSettings);
			fixed (INSSManager** pNSSManager = &m_pNSSManager)
			{
				if (num >= 0)
				{
					num = global::_003CModule_003E.CreateNSSManager(pNSSManager);
				}
				NSSMediator* pNSSMediator = m_pNSSMediator;
				if (0L != (nint)pNSSMediator)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNSSMediator + 16)))((nint)pNSSMediator);
					m_pNSSMediator = null;
				}
				NSSMediator* ptr = (NSSMediator*)global::_003CModule_003E.@new(40uL);
				NSSMediator* ptr2;
				try
				{
					ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NSSMediator_002E_007Bctor_007D(ptr, m_pNSSManager, this));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr);
					throw;
				}
				m_pNSSMediator = ptr2;
				global::_003CModule_003E.SafeAddRef_003Cclass_0020NSSMediator_003E(ptr2);
				return num;
			}
		}
	}

	public unsafe int RepairSharing()
	{
		//IL_002c: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 76u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 136)))((nint)pSettings);
	}

	public unsafe int EnableSharingForUser()
	{
		//IL_0029: Expected I, but got I8
		//IL_005d: Expected I, but got I8
		//IL_0040: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 119u);
			return -2147418113;
		}
		IHMESettings* intPtr = pSettings;
		int num;
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 96)))((nint)intPtr) == 0)
		{
			pSettings = m_pSettings;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 120)))((nint)pSettings, 1);
			if (num < 0)
			{
				goto IL_0078;
			}
		}
		pSettings = m_pSettings;
		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 160)))((nint)pSettings, 1);
		if (num >= 0)
		{
			pSettings = m_pSettings;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)pSettings + 24)))((nint)pSettings, 0);
		}
		goto IL_0078;
		IL_0078:
		return num;
	}

	public unsafe int DisableSharingForUser()
	{
		//IL_0030: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 144u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 160)))((nint)pSettings, 0);
	}

	public unsafe int DisableSharingForMachine()
	{
		//IL_002d: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 153u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 120)))((nint)pSettings, 0);
	}

	public unsafe int SetSharedFoldersList([MarshalAs(UnmanagedType.U1)] bool fForce)
	{
		//IL_002d: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 162u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)pSettings + 24)))((nint)pSettings, fForce ? ((byte)1) : ((byte)0));
	}

	public unsafe int GetDisplayName(ref string strName)
	{
		//IL_0031: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 181u);
			return -2147418113;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ushort* ptr);
		int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pSettings + 224)))((nint)pSettings, &ptr);
		if (num >= 0)
		{
			strName = new string((char*)ptr);
			global::_003CModule_003E.SysFreeString(ptr);
		}
		return num;
	}

	public unsafe int SetDisplayName(string strName)
	{
		//IL_0044: Expected I, but got I8
		if (m_pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 197u);
			return -2147418113;
		}
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strName)))
		{
			int result;
			if (ptr != null)
			{
				long num = *(long*)m_pSettings + 232;
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pSettings, ptr);
			}
			else
			{
				result = -2147418113;
			}
			return result;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetSharingEnabledForMediaType(EMediaTypes mediaType)
	{
		//IL_004e: Expected I, but got I8
		//IL_003b: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			return false;
		}
		int num;
		if (mediaType != EMediaTypes.eMediaTypeAudio)
		{
			if (mediaType != EMediaTypes.eMediaTypeVideo)
			{
				if (mediaType != EMediaTypes.eMediaTypeImage)
				{
					goto IL_0056;
				}
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 192)))((nint)pSettings);
			}
			else
			{
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 208)))((nint)pSettings);
			}
		}
		else
		{
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pSettings + 176)))((nint)pSettings);
		}
		if (num != 0)
		{
			return true;
		}
		goto IL_0056;
		IL_0056:
		return false;
	}

	public unsafe int SetSharingEnabledForMediaType(EMediaTypes mediaType, [MarshalAs(UnmanagedType.U1)] bool bEnabled)
	{
		//IL_0075: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_004d: Expected I, but got I8
		IHMESettings* pSettings = m_pSettings;
		if (pSettings == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 248u);
			return -2147418113;
		}
		int num = (bEnabled ? 1 : 0);
		return mediaType switch
		{
			EMediaTypes.eMediaTypeImage => ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 200)))((nint)pSettings, num), 
			EMediaTypes.eMediaTypeVideo => ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 216)))((nint)pSettings, num), 
			EMediaTypes.eMediaTypeAudio => ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pSettings + 184)))((nint)pSettings, num), 
			_ => -2147024809, 
		};
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetAllDevicesEnabled()
	{
		//IL_001b: Expected I, but got I8
		INSSManager* pNSSManager = m_pNSSManager;
		if (pNSSManager == null)
		{
			return false;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out short num);
		if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, short*, int>)(*(ulong*)(*(long*)pNSSManager + 56)))((nint)pNSSManager, &num) >= 0)
		{
			return num == -1;
		}
		return false;
	}

	public unsafe int SetAllDevicesEnabled([MarshalAs(UnmanagedType.U1)] bool bEnabled)
	{
		//IL_0034: Expected I, but got I8
		INSSManager* pNSSManager = m_pNSSManager;
		if (pNSSManager == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 297u);
			return -2147418113;
		}
		int num = -1;
		if (!bEnabled)
		{
			num = ~num;
		}
		short num2 = (short)num;
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, short, int>)(*(ulong*)(*(long*)pNSSManager + 64)))((nint)pNSSManager, num2);
	}

	public unsafe uint GetDeviceCount()
	{
		//IL_005a: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		if (m_pNSSManager == null)
		{
			return 0u;
		}
		uint num = 0u;
		if (m_pDeviceList == null)
		{
			fixed (INSSDevices** ptr = &m_pDeviceList)
			{
				try
				{
					long num2 = *(long*)m_pNSSManager + 88;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INSSDevices**, int>)(*(ulong*)num2))((nint)m_pNSSManager, ptr);
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}
		INSSDevices* pDeviceList = m_pDeviceList;
		if (pDeviceList != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num3);
			num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pDeviceList + 64)))((nint)pDeviceList, &num3) >= 0) ? ((uint)num3) : num);
		}
		return num;
	}

	public unsafe int GetDeviceProps(uint dwIndex, ref string strName, ref string strMAC, ref string strSerialNumber)
	{
		//IL_0020: Expected I, but got I8
		//IL_0023: Expected I, but got I8
		//IL_0026: Expected I, but got I8
		//IL_002a: Expected I, but got I8
		//IL_0057: Expected I, but got I8
		//IL_0076: Expected I, but got I8
		//IL_01f5: Expected I, but got I8
		//IL_01f9: Expected I, but got I8
		//IL_020b: Expected I, but got I8
		//IL_020f: Expected I, but got I8
		//IL_009d: Expected I, but got I8
		//IL_0221: Expected I, but got I8
		//IL_0225: Expected I, but got I8
		//IL_00b4: Expected I, but got I8
		//IL_00e3: Expected I, but got I8
		//IL_00ed: Expected I, but got I8
		//IL_0117: Expected I, but got I8
		//IL_016e: Expected I, but got I8
		//IL_0133: Expected I, but got I8
		//IL_0178: Expected I, but got I8
		//IL_014e: Expected I, but got I8
		//IL_0152: Expected I, but got I8
		//IL_019f: Expected I, but got I8
		//IL_01b9: Expected I, but got I8
		//IL_01d4: Expected I, but got I8
		//IL_01d8: Expected I, but got I8
		if (m_pDeviceList == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 338u);
			return -2147418113;
		}
		INSSDevice* ptr = null;
		INSSProperties* ptr2 = null;
		INSSProperty* ptr3 = null;
		ushort* ptr4 = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out tagVARIANT tagVARIANT);
		fixed (tagVARIANT* ptr5 = &System.Runtime.CompilerServices.Unsafe.AsRef<tagVARIANT>(&tagVARIANT))
		{
			global::_003CModule_003E.VariantInit(ptr5);
			fixed (INSSDevice** ptr6 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSDevice*>(&ptr))
			{
				long num = *(long*)m_pDeviceList + 56;
				int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, INSSDevice**, int>)(*(ulong*)num))((nint)m_pDeviceList, (int)dwIndex, ptr6);
				if (num2 >= 0)
				{
					fixed (ushort** ptr7 = &System.Runtime.CompilerServices.Unsafe.AsRef<ushort*>(&ptr4))
					{
						try
						{
							long num3 = *(long*)ptr + 56;
							num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)num3))((nint)ptr, ptr7);
						}
						catch
						{
							//try-fault
							ptr7 = null;
							throw;
						}
					}
					if (num2 >= 0)
					{
						strMAC = new string((char*)ptr4);
						global::_003CModule_003E.SysFreeString(ptr4);
						ptr4 = null;
						fixed (INSSProperties** ptr8 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSProperties*>(&ptr2))
						{
							try
							{
								long num4 = *(long*)ptr + 96;
								num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INSSProperties**, int>)(*(ulong*)num4))((nint)ptr, ptr8);
							}
							catch
							{
								//try-fault
								ptr8 = null;
								throw;
							}
						}
						if (num2 >= 0)
						{
							fixed (INSSProperty** ptr9 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSProperty*>(&ptr3))
							{
								try
								{
									long num5 = *(long*)ptr2 + 72;
									num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, INSSProperty**, int>)(*(ulong*)num5))((nint)ptr2, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_1BK_0040BFIEKNFP_0040_003F_0024AAF_003F_0024AAr_003F_0024AAi_003F_0024AAe_003F_0024AAn_003F_0024AAd_003F_0024AAl_003F_0024AAy_003F_0024AAN_003F_0024AAa_003F_0024AAm_003F_0024AAe_003F_0024AA_003F_0024AA_0040), ptr9);
									if (num2 < 0)
									{
										ptr3 = null;
										num2 = 0;
									}
								}
								catch
								{
									//try-fault
									ptr9 = null;
									throw;
								}
							}
							if (num2 >= 0)
							{
								if (ptr3 != null)
								{
									long num6 = *(long*)ptr3 + 64;
									num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, tagVARIANT*, int>)(*(ulong*)num6))((nint)ptr3, ptr5);
								}
								if (num2 >= 0)
								{
									if (ptr3 != null)
									{
										if (*(ushort*)(&tagVARIANT) == 8)
										{
											strName = new string((char*)System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8)));
										}
										global::_003CModule_003E.VariantClear(ptr5);
										if (0L != (nint)ptr3)
										{
											INSSProperty* intPtr = ptr3;
											((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
											ptr3 = null;
										}
									}
									fixed (INSSProperty** ptr10 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSProperty*>(&ptr3))
									{
										try
										{
											long num7 = *(long*)ptr2 + 72;
											num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, INSSProperty**, int>)(*(ulong*)num7))((nint)ptr2, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_1BK_0040GBMCOKGG_0040_003F_0024AAS_003F_0024AAe_003F_0024AAr_003F_0024AAi_003F_0024AAa_003F_0024AAl_003F_0024AAN_003F_0024AAu_003F_0024AAm_003F_0024AAb_003F_0024AAe_003F_0024AAr_003F_0024AA_003F_0024AA_0040), ptr10);
											if (num2 < 0)
											{
												ptr3 = null;
												num2 = 0;
											}
										}
										catch
										{
											//try-fault
											ptr10 = null;
											throw;
										}
									}
									if (num2 >= 0)
									{
										if (ptr3 != null)
										{
											long num8 = *(long*)ptr3 + 64;
											num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, tagVARIANT*, int>)(*(ulong*)num8))((nint)ptr3, ptr5);
										}
										if (num2 >= 0 && ptr3 != null)
										{
											if (*(ushort*)(&tagVARIANT) == 8)
											{
												strSerialNumber = new string((char*)System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8)));
											}
											global::_003CModule_003E.VariantClear(ptr5);
											if (0L != (nint)ptr3)
											{
												INSSProperty* intPtr2 = ptr3;
												((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
												ptr3 = null;
											}
										}
									}
								}
							}
						}
					}
				}
				if (ptr4 != null)
				{
					global::_003CModule_003E.SysFreeString(ptr4);
				}
				if (0L != (nint)ptr)
				{
					INSSDevice* intPtr3 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr = null;
				}
				if (0L != (nint)ptr2)
				{
					INSSProperties* intPtr4 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr2 = null;
				}
				if (0L != (nint)ptr3)
				{
					INSSProperty* intPtr5 = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
					ptr3 = null;
				}
				global::_003CModule_003E.VariantClear(ptr5);
				return num2;
			}
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetDeviceEnabled(uint dwIndex)
	{
		//IL_000f: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		//IL_0078: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		if (m_pDeviceList == null)
		{
			return false;
		}
		bool flag = false;
		INSSDevice* ptr = null;
		AuthorizationStatus authorizationStatus = (AuthorizationStatus)0;
		fixed (INSSDevice** ptr2 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSDevice*>(&ptr))
		{
			long num = *(long*)m_pDeviceList + 56;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, INSSDevice**, int>)(*(ulong*)num))((nint)m_pDeviceList, (int)dwIndex, ptr2);
			if (num2 >= 0)
			{
				fixed (AuthorizationStatus* ptr3 = &System.Runtime.CompilerServices.Unsafe.AsRef<AuthorizationStatus>(&authorizationStatus))
				{
					try
					{
						long num3 = *(long*)ptr + 64;
						num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, AuthorizationStatus*, int>)(*(ulong*)num3))((nint)ptr, ptr3);
					}
					catch
					{
						//try-fault
						ptr3 = null;
						throw;
					}
				}
				if (num2 >= 0)
				{
					flag = authorizationStatus == (AuthorizationStatus)2 || flag;
				}
			}
			if (0L != (nint)ptr)
			{
				INSSDevice* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			return flag;
		}
	}

	public unsafe int EnableDevice(uint dwIndex, [MarshalAs(UnmanagedType.U1)] bool bEnabled)
	{
		//IL_0020: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		//IL_0070: Expected I, but got I8
		//IL_005d: Expected I, but got I8
		if (m_pDeviceList == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 470u);
			return -2147418113;
		}
		INSSDevice* ptr = null;
		fixed (INSSDevice** ptr2 = &System.Runtime.CompilerServices.Unsafe.AsRef<INSSDevice*>(&ptr))
		{
			long num = *(long*)m_pDeviceList + 56;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, INSSDevice**, int>)(*(ulong*)num))((nint)m_pDeviceList, (int)dwIndex, ptr2);
			if (num2 >= 0)
			{
				AuthorizationStatus authorizationStatus = (bEnabled ? ((AuthorizationStatus)2) : ((AuthorizationStatus)3));
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, AuthorizationStatus, int>)(*(ulong*)(*(long*)ptr + 72)))((nint)ptr, authorizationStatus);
			}
			if (0L != (nint)ptr)
			{
				INSSDevice* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			return num2;
		}
	}

	internal void NSSDeviceListChange()
	{
		if (m_NSSDeviceListChange != null)
		{
			m_NSSDeviceListChange();
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021HMESettings();
			return;
		}
		try
		{
			_0021HMESettings();
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

	~HMESettings()
	{
		Dispose(false);
	}
}
