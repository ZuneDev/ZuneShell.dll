using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class FirmwareRestorePointCollection : IDisposable
	{
		private readonly CComPtrMgd<IFirmwareRestorePointCollection> m_spRestorePointCollection;

		public unsafe int Count
		{
			get
			{
				//IL_0022: Expected I, but got I8
				int num = 0;
				uint num2 = 0u;
				IFirmwareRestorePointCollection* p = m_spRestorePointCollection.p;
				if (p != null)
				{
					num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &num2) >= 0) ? ((int)num2) : num);
				}
				return num;
			}
		}

		public unsafe FirmwareRestorePoint GetRestorePoint(int index)
		{
			//IL_002f: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			FirmwareRestorePoint result = null;
			CComPtrNtv<IFirmwareRestorePoint> cComPtrNtv_003CIFirmwareRestorePoint_003E = new();
			try
			{
				CComPtrMgd<IFirmwareRestorePointCollection> spRestorePointCollection = m_spRestorePointCollection;
				if (spRestorePointCollection.p != null)
				{
					IFirmwareRestorePointCollection* p = spRestorePointCollection.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IFirmwareRestorePoint**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, (uint)index, (IFirmwareRestorePoint**)(cComPtrNtv_003CIFirmwareRestorePoint_003E.p)) >= 0)
					{
						result = new FirmwareRestorePoint((IFirmwareRestorePoint*)(*(ulong*)(cComPtrNtv_003CIFirmwareRestorePoint_003E.p)));
					}
				}
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIFirmwareRestorePoint_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIFirmwareRestorePoint_003E.Dispose();
			return result;
		}

		internal unsafe FirmwareRestorePointCollection(IFirmwareRestorePointCollection* pRestorePointCollection)
		{
			CComPtrMgd<IFirmwareRestorePointCollection> spRestorePointCollection = new CComPtrMgd<IFirmwareRestorePointCollection>();
			try
			{
				m_spRestorePointCollection = spRestorePointCollection;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_q(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 68, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), pRestorePointCollection);
				}
				m_spRestorePointCollection.op_Assign(pRestorePointCollection);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 69, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spRestorePointCollection).Dispose();
				throw;
			}
		}

		private unsafe void _007EFirmwareRestorePointCollection()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 70, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_spRestorePointCollection.Release();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 71, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareRestorePointCollection();
				}
				finally
				{
					((IDisposable)m_spRestorePointCollection).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
