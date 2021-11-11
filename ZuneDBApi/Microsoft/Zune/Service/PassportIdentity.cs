using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class PassportIdentity : IDisposable
	{
		private readonly CComPtrMgd_003CIPassportIdentity_003E m_spPassportIdentity;

		private string m_username;

		private string m_password;

		private string m_serviceTicket;

		public unsafe string ServiceTicket
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_serviceTicket == (string)null)
				{
					CComPtrMgd_003CIPassportIdentity_003E spPassportIdentity = m_spPassportIdentity;
					if (spPassportIdentity.p != null)
					{
						ushort* ptr = null;
						IPassportIdentity* p = spPassportIdentity.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
						{
							m_serviceTicket = new string((char*)ptr);
						}
						_003CModule_003E.SysFreeString(ptr);
					}
				}
				return m_serviceTicket;
			}
		}

		public unsafe string Password
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_password == (string)null)
				{
					CComPtrMgd_003CIPassportIdentity_003E spPassportIdentity = m_spPassportIdentity;
					if (spPassportIdentity.p != null)
					{
						ushort* ptr = null;
						IPassportIdentity* p = spPassportIdentity.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &ptr) >= 0)
						{
							m_password = new string((char*)ptr);
						}
						_003CModule_003E.SysFreeString(ptr);
					}
				}
				return m_username;
			}
		}

		public unsafe string Username
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_username == (string)null)
				{
					CComPtrMgd_003CIPassportIdentity_003E spPassportIdentity = m_spPassportIdentity;
					if (spPassportIdentity.p != null)
					{
						ushort* ptr = null;
						IPassportIdentity* p = spPassportIdentity.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
						{
							m_username = new string((char*)ptr);
						}
						_003CModule_003E.SysFreeString(ptr);
					}
				}
				return m_username;
			}
		}

		internal unsafe PassportIdentity(IPassportIdentity* pPassportIdentity)
		{
			CComPtrMgd_003CIPassportIdentity_003E spPassportIdentity = new CComPtrMgd_003CIPassportIdentity_003E();
			try
			{
				m_spPassportIdentity = spPassportIdentity;
				base._002Ector();
				m_spPassportIdentity.op_Assign(pPassportIdentity);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spPassportIdentity).Dispose();
				throw;
			}
		}

		internal unsafe int GetComPointer(IPassportIdentity** ppPassportIdentity)
		{
			CComPtrMgd_003CIPassportIdentity_003E spPassportIdentity = m_spPassportIdentity;
			return (spPassportIdentity.p == null) ? (-2147467259) : spPassportIdentity.QueryInterface_003CIPassportIdentity_003E(ppPassportIdentity);
		}

		public void _007EPassportIdentity()
		{
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
				}
				finally
				{
					((IDisposable)m_spPassportIdentity).Dispose();
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
