using System;
using System.Collections;
using System.Runtime.InteropServices;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Service
{
	public class WinLiveInformation : IDisposable
	{
		private readonly CComPtrMgd_003CIWinLiveInformation_003E m_spWinLiveInformation;

		private string m_termsOfServiceUrl;

		private string m_privacyUrl;

		private string m_hipChallenge;

		private SafeBitmapWithData m_hipImage;

		private IList m_domains;

		public unsafe IList Domains
		{
			get
			{
				//IL_002a: Expected I, but got I8
				//IL_0040: Expected I, but got I8
				//IL_005c: Expected I, but got I8
				if (m_domains == null)
				{
					IWinLiveInformation* p = m_spWinLiveInformation.p;
					if (p != null)
					{
						int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 72)))((nint)p);
						m_domains = new ArrayList(num);
						int num2 = 0;
						if (0 < num)
						{
							do
							{
								ushort* ptr = null;
								IWinLiveInformation* p2 = m_spWinLiveInformation.p;
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int>)(*(ulong*)(*(long*)p2 + 80)))((nint)p2, num2, &ptr) >= 0)
								{
									m_domains.Add(new string((char*)ptr));
								}
								Module.SysFreeString(ptr);
								num2++;
							}
							while (num2 < num);
						}
					}
				}
				return m_domains;
			}
		}

		public unsafe int HipLength
		{
			get
			{
				//IL_001e: Expected I, but got I8
				int result = 0;
				IWinLiveInformation* p = m_spWinLiveInformation.p;
				if (p != null)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 64)))((nint)p);
				}
				return result;
			}
		}

		public unsafe SafeBitmapWithData HipImage
		{
			get
			{
				//IL_002b: Expected I, but got I8
				//IL_003d: Expected I, but got I8
				//IL_0041: Expected I, but got I8
				if (m_hipImage == null)
				{
					CComPtrMgd_003CIWinLiveInformation_003E spWinLiveInformation = m_spWinLiveInformation;
					if (spWinLiveInformation.p != null)
					{
						int num = 0;
						IWinLiveInformation* p = spWinLiveInformation.p;
						HBITMAP* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HBITMAP*>)(*(ulong*)(*(long*)p + 56)))((nint)p);
						num = (((long)(nint)ptr == 0) ? (-2147467259) : num);
						HBITMAP* ptr2 = null;
						void* pData = null;
						if (num >= 0 && Module.CopyThumbnailBitmapData(ptr, &ptr2, &pData) >= 0)
						{
							try
							{
								m_hipImage = new SafeBitmapWithData(pData, ptr2);
							}
							catch (ApplicationException)
							{
								m_hipImage = null;
								if (ptr2 != null)
								{
									Module.DeleteObject(ptr2);
								}
							}
						}
					}
				}
				return m_hipImage;
			}
		}

		public unsafe string HipChallenge
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_hipChallenge == null)
				{
					CComPtrMgd_003CIWinLiveInformation_003E spWinLiveInformation = m_spWinLiveInformation;
					if (spWinLiveInformation.p != null)
					{
						ushort* ptr = null;
						IWinLiveInformation* p = spWinLiveInformation.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, &ptr) >= 0)
						{
							m_hipChallenge = new string((char*)ptr);
						}
						Module.SysFreeString(ptr);
					}
				}
				return m_hipChallenge;
			}
		}

		public unsafe string PrivacyUrl
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_privacyUrl == null)
				{
					CComPtrMgd_003CIWinLiveInformation_003E spWinLiveInformation = m_spWinLiveInformation;
					if (spWinLiveInformation.p != null)
					{
						ushort* ptr = null;
						IWinLiveInformation* p = spWinLiveInformation.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
						{
							m_privacyUrl = new string((char*)ptr);
						}
						Module.SysFreeString(ptr);
					}
				}
				return m_privacyUrl;
			}
		}

		public unsafe int TermsOfServiceVersion
		{
			get
			{
				//IL_001e: Expected I, but got I8
				int result = 0;
				IWinLiveInformation* p = m_spWinLiveInformation.p;
				if (p != null)
				{
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 32)))((nint)p);
				}
				return result;
			}
		}

		public unsafe string TermsOfServiceUrl
		{
			get
			{
				//IL_0020: Expected I, but got I8
				//IL_0036: Expected I, but got I8
				if (m_termsOfServiceUrl == null)
				{
					CComPtrMgd_003CIWinLiveInformation_003E spWinLiveInformation = m_spWinLiveInformation;
					if (spWinLiveInformation.p != null)
					{
						ushort* ptr = null;
						IWinLiveInformation* p = spWinLiveInformation.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
						{
							m_termsOfServiceUrl = new string((char*)ptr);
						}
						Module.SysFreeString(ptr);
					}
				}
				return m_termsOfServiceUrl;
			}
		}

		internal unsafe WinLiveInformation(IWinLiveInformation* pWinLiveInformation)
		{
			CComPtrMgd_003CIWinLiveInformation_003E spWinLiveInformation = new CComPtrMgd_003CIWinLiveInformation_003E();
			try
			{
				m_spWinLiveInformation = spWinLiveInformation;
				base._002Ector();
				m_spWinLiveInformation.op_Assign(pWinLiveInformation);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spWinLiveInformation).Dispose();
				throw;
			}
		}

		public void _007EWinLiveInformation()
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
					((IDisposable)m_spWinLiveInformation).Dispose();
				}
			}
			else
			{
				Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
