using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class FirmwareUpdatePackage : IDisposable
	{
		private readonly CComPtrMgd_003CIFirmwareMetadata_003E m_spFirmwareMetadata;

		private bool m_fSelected;

		internal bool Selected
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fSelected;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				m_fSelected = value;
			}
		}

		public unsafe TimeSpan UpdateEstimatedTime
		{
			get
			{
				//IL_002f: Expected I, but got I8
				TimeSpan result = default(TimeSpan);
				CComPtrMgd_003CIFirmwareMetadata_003E spFirmwareMetadata = m_spFirmwareMetadata;
				if (spFirmwareMetadata.p != null)
				{
					uint num = 0u;
					IFirmwareMetadata* p = spFirmwareMetadata.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 72)))((nint)p, &num) >= 0)
					{
						return TimeSpan.FromMilliseconds(num);
					}
				}
				return result;
			}
		}

		public unsafe string MoreInfoURL
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe string EULAContent
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe string Version
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
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
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe string Name
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
		}

		public unsafe FirmwareUpdateType Type
		{
			get
			{
				//IL_0020: Expected I, but got I8
				EFirmwareUpdateType result = (EFirmwareUpdateType)(-1);
				IFirmwareMetadata* p = m_spFirmwareMetadata.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFirmwareUpdateType*, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, &result);
				}
				return (FirmwareUpdateType)result;
			}
		}

		internal unsafe FirmwareUpdatePackage(IFirmwareMetadata* pFirmwareMetadata)
		{
			CComPtrMgd_003CIFirmwareMetadata_003E spFirmwareMetadata = new CComPtrMgd_003CIFirmwareMetadata_003E();
			try
			{
				m_spFirmwareMetadata = spFirmwareMetadata;
				base._002Ector();
				m_spFirmwareMetadata.op_Assign(pFirmwareMetadata);
				m_fSelected = false;
			}
			catch
			{
				//try-fault
				((IDisposable)m_spFirmwareMetadata).Dispose();
				throw;
			}
		}

		private void _007EFirmwareUpdatePackage()
		{
			m_spFirmwareMetadata.Release();
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareUpdatePackage();
				}
				finally
				{
					((IDisposable)m_spFirmwareMetadata).Dispose();
				}
			}
			else
			{
				Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
