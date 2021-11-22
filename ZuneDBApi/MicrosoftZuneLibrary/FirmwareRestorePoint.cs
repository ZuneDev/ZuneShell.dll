using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class FirmwareRestorePoint : IDisposable
	{
		private readonly CComPtrMgd_003CIFirmwareRestorePoint_003E m_spRestorePoint;

		internal unsafe IFirmwareRestorePoint* NativeRestorePointPtr => m_spRestorePoint.p;

		public unsafe TimeSpan EstimatedRestoreTime
		{
			get
			{
				//IL_0026: Expected I, but got I8
				TimeSpan result = TimeSpan.Zero;
				uint num = 0u;
				IFirmwareRestorePoint* p = m_spRestorePoint.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &num) >= 0)
				{
					result = TimeSpan.FromMilliseconds(num);
				}
				return result;
			}
		}

		public unsafe DateTime CreationDate
		{
			get
			{
				//IL_0019: Expected I4, but got I8
				//IL_0037: Expected I, but got I8
				DateTime result = default(DateTime);
				_SYSTEMTIME stValue;
				*(short*)(&stValue) = 0;
                // IL initblk instruction
                Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref stValue, 2), 0, 14);
				IFirmwareRestorePoint* p = m_spRestorePoint.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _SYSTEMTIME*, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &stValue) >= 0)
				{
					return Module.SystemTimeToDateTime(stValue);
				}
				return result;
			}
		}

		public unsafe string OSVersion
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareRestorePoint* p = m_spRestorePoint.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
		}

		internal unsafe FirmwareRestorePoint(IFirmwareRestorePoint* pRestorePoint)
		{
			CComPtrMgd_003CIFirmwareRestorePoint_003E spRestorePoint = new CComPtrMgd_003CIFirmwareRestorePoint_003E();
			try
			{
				m_spRestorePoint = spRestorePoint;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_q(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 64, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), pRestorePoint);
				}
				m_spRestorePoint.op_Assign(pRestorePoint);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 65, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spRestorePoint).Dispose();
				throw;
			}
		}

		private unsafe void _007EFirmwareRestorePoint()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 66, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_spRestorePoint.Release();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 67, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareRestorePoint();
				}
				finally
				{
					((IDisposable)m_spRestorePoint).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
