using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service
{
	public class ServiceError : IDisposable
	{
		private readonly CComPtrMgd<IServiceError> m_spServiceError;

		private IList<PropertyError> m_propertyErrors;

		public unsafe IList<PropertyError> PropertyErrors
		{
			get
			{
				//IL_002d: Expected I, but got I8
				//IL_0047: Expected I, but got I8
				//IL_0065: Expected I, but got I8
				if (m_propertyErrors == null)
				{
					IServiceError* p = m_spServiceError.p;
					if (p != null)
					{
						int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 32)))((nint)p);
						m_propertyErrors = new List<PropertyError>(num);
						int num2 = 0;
						if (0 < num)
						{
							do
							{
								int num3 = 0;
								ushort* ptr = null;
								IServiceError* p2 = m_spServiceError.p;
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int*, int>)(*(ulong*)(*(long*)p2 + 40)))((nint)p2, num2, &ptr, &num3) >= 0)
								{
									PropertyError propertyError = new PropertyError();
									propertyError.Name = new string((char*)ptr);
									HRESULT hRESULT2 = (propertyError.Hr = num3);
									m_propertyErrors.Add(propertyError);
								}
								Module.SysFreeString(ptr);
								num2++;
							}
							while (num2 < num);
						}
					}
				}
				return m_propertyErrors;
			}
		}

		public unsafe HRESULT RootError
		{
			get
			{
				//IL_0026: Expected I, but got I8
				HRESULT s_OK = HRESULT._S_OK;
				IServiceError* p = m_spServiceError.p;
				if (p != null)
				{
					IServiceError* ptr = p;
					s_OK.hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)ptr);
				}
				return s_OK;
			}
		}

		internal unsafe ServiceError(IServiceError* pServiceError)
		{
			CComPtrMgd<IServiceError> spServiceError = new CComPtrMgd<IServiceError>();
			try
			{
				m_spServiceError = spServiceError;
				m_spServiceError.op_Assign(pServiceError);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spServiceError).Dispose();
				throw;
			}
		}

		public void _007EServiceError()
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
					((IDisposable)m_spServiceError).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
