using System;
using System.Runtime.InteropServices;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class FirmwareUpdateErrorInfo : IDisposable
	{
		private readonly CComPtrMgd_003CIFirmwareUpdateErrorInfo_003E m_spErrorInfo;

		public unsafe string Url
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareUpdateErrorInfo* p = m_spErrorInfo.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe string Description
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareUpdateErrorInfo* p = m_spErrorInfo.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe HRESULT HrStatus
		{
			get
			{
				//IL_0024: Expected I, but got I8
				int num = -2147467259;
				IFirmwareUpdateErrorInfo* p = m_spErrorInfo.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &num);
				}
				return num;
			}
		}

		internal unsafe FirmwareUpdateErrorInfo(IFirmwareUpdateErrorInfo* pErrorInfo)
		{
			CComPtrMgd_003CIFirmwareUpdateErrorInfo_003E spErrorInfo = new CComPtrMgd_003CIFirmwareUpdateErrorInfo_003E();
			try
			{
				m_spErrorInfo = spErrorInfo;
				m_spErrorInfo.op_Assign(pErrorInfo);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spErrorInfo).Dispose();
				throw;
			}
		}

		private void _007EFirmwareUpdateErrorInfo()
		{
			m_spErrorInfo.Release();
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareUpdateErrorInfo();
				}
				finally
				{
					((IDisposable)m_spErrorInfo).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
