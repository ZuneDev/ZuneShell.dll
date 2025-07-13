using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class HttpWebResponse : IDisposable
{
	private unsafe IStream* m_pStream;

	private HttpWebRequest _request;

	private string _contentType;

	private long _contentLength;

	private Stream _stream;

	private Uri _responseUri;

	private HttpStatusCode _statusCode;

	private DateTime _expires;

	public HttpStatusCode StatusCode => _statusCode;

	public DateTime Expires => _expires;

	public string ContentType => _contentType;

	public long ContentLength => _contentLength;

	public Version ProtocolVersion => HttpVersion.Version11;

	public Uri ResponseUri => _responseUri;

	private void _007EHttpWebResponse()
	{
		_0021HttpWebResponse();
	}

	private unsafe void _0021HttpWebResponse()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		IStream* pStream = m_pStream;
		if (pStream != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pStream + 16)))((nint)pStream);
			m_pStream = null;
		}
	}

	public unsafe Stream GetResponseStream()
	{
		if (_stream == null)
		{
			_stream = new HttpResponseStream(m_pStream, _contentLength);
		}
		return _stream;
	}

	public unsafe void Close()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		IStream* pStream = m_pStream;
		if (pStream != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pStream + 16)))((nint)pStream);
			m_pStream = null;
		}
	}

	internal unsafe HttpWebResponse(Uri requestUri, HttpStatusCode statusCode, IHttpWebResponse* pResponse, IStream* pStream)
	{
		//IL_0199: Expected I, but got I8
		//IL_0032: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		//IL_007e: Expected I, but got I8
		//IL_00a5: Expected I, but got I8
		//IL_00c8: Expected I, but got I8
		//IL_00eb: Expected I, but got I8
		//IL_010d: Expected I, but got I8
		m_pStream = pStream;
		base._002Ector();
		_statusCode = statusCode;
		if (pResponse != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
			global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
			try
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pResponse + 104)))((nint)pResponse, (ushort**)(&wBSTRString));
				bool flag = *(long*)(&wBSTRString) != 0L && ((*(ushort*)(*(ulong*)(&wBSTRString)) != 0) ? true : false);
				if (flag)
				{
					_responseUri = new Uri(new string((char*)(*(ulong*)(&wBSTRString))));
				}
				else
				{
					_responseUri = requestUri;
				}
				ulong contentLength = 0uL;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, int>)(*(ulong*)(*(long*)pResponse + 72)))((nint)pResponse, &contentLength) >= 0)
				{
					_contentLength = (long)contentLength;
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString2);
				global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString2);
				try
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort**, int>)(*(ulong*)(*(long*)pResponse + 112)))((nint)pResponse, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_1BK_0040DOFICLFP_0040_003F_0024AAC_003F_0024AAo_003F_0024AAn_003F_0024AAt_003F_0024AAe_003F_0024AAn_003F_0024AAt_003F_0024AA_003F9_003F_0024AAT_003F_0024AAy_003F_0024AAp_003F_0024AAe_003F_0024AA_003F_0024AA_0040), (ushort**)(&wBSTRString2));
					bool flag2 = *(long*)(&wBSTRString2) != 0L && ((*(ushort*)(*(ulong*)(&wBSTRString2)) != 0) ? true : false);
					if (flag2)
					{
						_contentType = new string((char*)(*(ulong*)(&wBSTRString2)));
					}
					System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString3);
					global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString3);
					try
					{
						string s = null;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort**, int>)(*(ulong*)(*(long*)pResponse + 112)))((nint)pResponse, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_1BA_0040FICLJLAN_0040_003F_0024AAE_003F_0024AAx_003F_0024AAp_003F_0024AAi_003F_0024AAr_003F_0024AAe_003F_0024AAs_003F_0024AA_003F_0024AA_0040), (ushort**)(&wBSTRString3));
						bool flag3 = *(long*)(&wBSTRString3) != 0L && ((*(ushort*)(*(ulong*)(&wBSTRString3)) != 0) ? true : false);
						if (flag3)
						{
							s = new string((char*)(*(ulong*)(&wBSTRString3)));
						}
						if (!DateTime.TryParse(s, out _expires))
						{
							_expires = DateTime.MaxValue;
						}
						else
						{
							DateTime expires = _expires.ToUniversalTime();
							_expires = expires;
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString3);
						throw;
					}
					global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString3);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString2);
					throw;
				}
				global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString2);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
				throw;
			}
			global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
		}
		IStream* pStream2 = m_pStream;
		if (pStream2 != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pStream2 + 8)))((nint)pStream2);
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021HttpWebResponse();
			return;
		}
		try
		{
			_0021HttpWebResponse();
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

	~HttpWebResponse()
	{
		Dispose(false);
	}
}
