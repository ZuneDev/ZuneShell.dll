using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class UpdatePackageCollection : IDisposable
	{
		private readonly CComPtrMgd<IFirmwareUpdateCollection> m_spUpdatePackageCollection;

		private List<FirmwareUpdatePackage> m_packages;

		private FirmwareUpdatePackage m_FirmwarePackage;

		private FirmwareUpdatePackage m_GamesPackage;

		private List<FirmwareUpdatePackage> Packages
		{
			get
			{
				CachePackages();
				return m_packages;
			}
		}

		internal unsafe IFirmwareUpdateCollection* NativeCollectionPtr => m_spUpdatePackageCollection.p;

		public FirmwareUpdatePackage GamesPackage
		{
			get
			{
				CachePackages();
				return m_GamesPackage;
			}
		}

		public FirmwareUpdatePackage FirmwarePackage
		{
			get
			{
				CachePackages();
				return m_FirmwarePackage;
			}
		}

		public FirmwareUpdatePackage this[int index] => Packages[index];

		[return: MarshalAs(UnmanagedType.U1)]
		public bool get_Selected(int index) => this.Packages[index].Selected;

		public unsafe void set_Selected(int index, [MarshalAs(UnmanagedType.U1)] bool value)
		{
			//IL_001f: Expected I, but got I8
			uint num = (uint)index;
			IFirmwareUpdateCollection* p = m_spUpdatePackageCollection.p;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint*, int, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, 1u, &num, value ? 1 : 0);
			if (num2 >= 0)
			{
				Packages[index].Selected = value;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
			{
				Module.WPP_SF_dld(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), index, value ? 1 : 0, num2);
			}
		}

		public unsafe int Count
		{
			get
			{
				//IL_001d: Expected I, but got I8
				uint result = 0u;
				IFirmwareUpdateCollection* p = m_spUpdatePackageCollection.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &result);
				return (int)result;
			}
		}

		internal unsafe UpdatePackageCollection(IFirmwareUpdateCollection* pCollection)
		{
			CComPtrMgd<IFirmwareUpdateCollection> spUpdatePackageCollection = new CComPtrMgd<IFirmwareUpdateCollection>();
			try
			{
				m_spUpdatePackageCollection = spUpdatePackageCollection;
				m_spUpdatePackageCollection.op_Assign(pCollection);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spUpdatePackageCollection).Dispose();
				throw;
			}
		}

		private void _007EUpdatePackageCollection()
		{
			m_spUpdatePackageCollection.Release();
			m_FirmwarePackage = null;
			m_GamesPackage = null;
			DisposePackages();
		}

		private unsafe void CachePackages()
		{
			//IL_0037: Expected I, but got I8
			//IL_00ae: Expected I, but got I8
			//IL_00bb: Expected I, but got I8
			//IL_00ed: Expected I, but got I8
			//IL_0108: Expected I, but got I8
			//IL_0108: Expected I, but got I8
			if (m_packages != null)
			{
				return;
			}
			m_packages = new List<FirmwareUpdatePackage>();
			uint num = 0u;
			IFirmwareUpdateCollection* p = m_spUpdatePackageCollection.p;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &num);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), (int)num);
			}
			uint num3 = 0u;
			if (num2 >= 0)
			{
				while (num3 < num)
				{
					FirmwareUpdatePackage firmwareUpdatePackage = null;
					CComPtrNtv_003CIFirmwareMetadata_003E cComPtrNtv_003CIFirmwareMetadata_003E;
					*(long*)(&cComPtrNtv_003CIFirmwareMetadata_003E) = 0L;
					try
					{
						IFirmwareUpdateCollection* p2 = m_spUpdatePackageCollection.p;
						uint num4 = num3;
						num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IFirmwareMetadata**, int>)(*(ulong*)(*(long*)p2 + 32)))((nint)p2, num4, (IFirmwareMetadata**)(&cComPtrNtv_003CIFirmwareMetadata_003E));
						if (num2 >= 0)
						{
							firmwareUpdatePackage = new FirmwareUpdatePackage((IFirmwareMetadata*)(*(ulong*)(&cComPtrNtv_003CIFirmwareMetadata_003E)));
							m_packages.Add(firmwareUpdatePackage);
							IFirmwareUpdateCollection* p3 = m_spUpdatePackageCollection.p;
							bool selected = firmwareUpdatePackage.Selected;
							num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint*, int, int>)(*(ulong*)(*(long*)p3 + 40)))((nint)p3, 1u, &num3, selected ? 1 : 0);
						}
						EFirmwareUpdateType eFirmwareUpdateType = (EFirmwareUpdateType)(-1);
						if (num2 >= 0)
						{
							long num5 = *(long*)(&cComPtrNtv_003CIFirmwareMetadata_003E);
							num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFirmwareUpdateType*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIFirmwareMetadata_003E)) + 48)))((nint)num5, &eFirmwareUpdateType);
							if (num2 >= 0)
							{
								if (eFirmwareUpdateType == 0)
								{
									m_GamesPackage = firmwareUpdatePackage;
								}
								else
								{
									m_FirmwarePackage = firmwareUpdatePackage;
								}
							}
						}
						fixed (char* firmwareUpdatePackageNamePtr = firmwareUpdatePackage.Name.ToCharArray())
						{
							ushort* a = (ushort*)firmwareUpdatePackageNamePtr;
							try
							{
								if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
								{
									Module.WPP_SF_dSdl(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 12, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), (int)num3, a, (int)eFirmwareUpdateType, firmwareUpdatePackage.Selected ? 1 : 0);
								}
							}
							catch
							{
								//try-fault
								a = null;
								throw;
							}
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareMetadata_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareMetadata_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareMetadata_003E);
						throw;
					}
					Module.CComPtrNtv_003CIFirmwareMetadata_003E_002ERelease(&cComPtrNtv_003CIFirmwareMetadata_003E);
					num3++;
					if (num2 < 0)
					{
						break;
					}
				}
				if (num2 >= 0)
				{
					return;
				}
			}
			DisposePackages();
		}

		private unsafe void DisposePackages()
		{
			if (m_packages == null)
			{
				return;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 13, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = m_packages.Count - 1;
			if (num >= 0)
			{
				do
				{
					((IDisposable)m_packages[num])?.Dispose();
					num += -1;
				}
				while (num >= 0);
			}
			m_packages = null;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EUpdatePackageCollection();
				}
				finally
				{
					((IDisposable)m_spUpdatePackageCollection).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
