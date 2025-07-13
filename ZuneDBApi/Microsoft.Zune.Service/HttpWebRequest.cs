using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Service;

public class HttpWebRequest
{
	private unsafe class AsyncRequest(IHttpWebRequest* pRequest, [MarshalAs(UnmanagedType.U1)] bool cancelOnShutdown)
	{
		public unsafe IHttpWebRequest* m_pRequest = pRequest;

		public bool m_cancelOnShutdown = cancelOnShutdown;
	}

	private Uri _uri;

	private string _method;

	private string _contentType;

	private string _acceptLanguage;

	private string _authorization;

	private string _browserCookieUrl;

	private bool _acceptGZipEncoding;

	private bool _keepAlive;

	private long _contentLength;

	private long _maxResponseLength;

	private HttpRequestCachePolicy _cachePolicy;

	private bool _cancelOnShutdown;

	private MemoryStream _requestStream;

	private static object _lock = new object();

	private static ArrayList _asyncRequests;

	private unsafe static void* m_hRequestsCompleteEvent;

	private static bool _shutdown;

	public bool CancelOnShutdown
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _cancelOnShutdown;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			_cancelOnShutdown = value;
		}
	}

	public bool KeepAlive
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _keepAlive;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			_keepAlive = value;
		}
	}

	public string ContentType
	{
		get
		{
			return _contentType;
		}
		set
		{
			_contentType = value;
		}
	}

	public HttpRequestCachePolicy CachePolicy
	{
		get
		{
			return _cachePolicy;
		}
		set
		{
			_cachePolicy = value;
		}
	}

	public long MaxResponseLength
	{
		get
		{
			return _maxResponseLength;
		}
		set
		{
			_maxResponseLength = value;
		}
	}

	public string BrowserCookieUrl
	{
		get
		{
			return _browserCookieUrl;
		}
		set
		{
			_browserCookieUrl = value;
		}
	}

	public string Authorization
	{
		get
		{
			return _authorization;
		}
		set
		{
			_authorization = value;
		}
	}

	public bool AcceptGZipEncoding
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _acceptGZipEncoding;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			_acceptGZipEncoding = value;
		}
	}

	public string AcceptLanguage
	{
		get
		{
			return _acceptLanguage;
		}
		set
		{
			_acceptLanguage = value;
		}
	}

	public long ContentLength
	{
		get
		{
			return _contentLength;
		}
		set
		{
			_contentLength = value;
		}
	}

	public Uri RequestUri => _uri;

	public string Method
	{
		get
		{
			return _method;
		}
		set
		{
			_method = value;
		}
	}

	public static HttpWebRequest Create(string uri)
	{
		return new HttpWebRequest(new Uri(uri));
	}

	public static HttpWebRequest Create(Uri uri)
	{
		return new HttpWebRequest(uri);
	}

	public Stream GetRequestStream()
	{
		if (_requestStream == null)
		{
			_requestStream = new MemoryStream(new byte[(int)_contentLength]);
		}
		return _requestStream;
	}

	public unsafe HttpWebResponse GetResponse()
	{
		//IL_0016: Expected I, but got I8
		//IL_0027: Expected I, but got I8
		//IL_002a: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_005b: Expected I, but got I8
		//IL_0084: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_00a8: Expected I, but got I8
		int num = 0;
		num = (_shutdown ? (-2147418113) : num);
		IHttpWebRequest* ptr = null;
		if (num >= 0)
		{
			num = ConstructNativeRequest(&ptr);
		}
		IStream* ptr2 = null;
		IHttpWebResponse* ptr3 = null;
		if (num >= 0)
		{
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IStream**, IHttpWebResponse**, int>)(*(ulong*)(*(long*)ptr + 160)))((nint)ptr, &ptr2, &ptr3);
		}
		HttpStatusCode httpStatusCode = HttpStatusCode.OK;
		if (num >= 0)
		{
			IHttpWebResponse* intPtr = ptr3;
			httpStatusCode = (HttpStatusCode)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 24)))((nint)intPtr);
		}
		HttpWebResponse httpWebResponse = null;
		if (num >= 0)
		{
			httpWebResponse = new HttpWebResponse(_uri, httpStatusCode, ptr3, ptr2);
		}
		if (ptr != null)
		{
			IHttpWebRequest* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		if (ptr3 != null)
		{
			IHttpWebResponse* intPtr3 = ptr3;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
		}
		if (ptr2 != null)
		{
			IStream* intPtr4 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
		}
		if (num >= 0 && httpStatusCode == HttpStatusCode.OK)
		{
			return httpWebResponse;
		}
		if (httpWebResponse != null)
		{
			throw new HttpWebException(httpWebResponse);
		}
		throw new HttpWebException(num);
	}

	public unsafe void GetResponseAsync(AsyncRequestComplete responseComplete, object stateInfo)
	{
		//IL_0032: Expected I, but got I8
		//IL_003f: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		//IL_00f2: Expected I, but got I8
		//IL_0105: Expected I, but got I8
		//IL_0082: Expected I, but got I8
		//IL_00c6: Expected I, but got I8
		//IL_00e1: Expected I, but got I8
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_lock);
		try
		{
			managedLock = managedLock2;
			if (!_shutdown)
			{
				goto IL_002f;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		goto IL_0123;
		IL_0123:
		try
		{
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		IL_002f:
		try
		{
			IHttpWebRequest* ptr = null;
			int num = ConstructNativeRequest(&ptr);
			IStream* ptr2 = null;
			if (num >= 0)
			{
				num = global::_003CModule_003E.CreateStreamOnHGlobal(null, 1, &ptr2);
				if (num >= 0)
				{
					CWebRequestCallbackWrapper* ptr3 = (CWebRequestCallbackWrapper*)global::_003CModule_003E.@new(56uL);
					CWebRequestCallbackWrapper* ptr4;
					try
					{
						if (ptr3 != null)
						{
							Uri uri = _uri;
							ptr4 = global::_003CModule_003E.Microsoft_002EZune_002EService_002ECWebRequestCallbackWrapper_002E_007Bctor_007D(ptr3, uri, ptr, ptr2, responseComplete, stateInfo);
						}
						else
						{
							ptr4 = null;
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr3);
						throw;
					}
					int num2 = (((long)(nint)ptr4 != 0) ? num : (-2147024882));
					num = num2;
					if (num2 >= 0)
					{
						bool cancelOnShutdown = _cancelOnShutdown;
						OnAsyncRequestStart(ptr, cancelOnShutdown);
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IStream*, IHttpWebRequestCallback*, int>)(*(ulong*)(*(long*)ptr + 168)))((nint)ptr, ptr2, (IHttpWebRequestCallback*)ptr4);
						if (num < 0)
						{
							OnAsyncRequestComplete(ptr);
						}
					}
					if (ptr4 != null)
					{
						CWebRequestCallbackWrapper* intPtr = ptr4;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
					}
				}
			}
			if (ptr != null)
			{
				IHttpWebRequest* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			if (ptr2 != null)
			{
				IStream* intPtr3 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
			}
			if (num < 0)
			{
				throw new HttpWebException(num);
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		goto IL_0123;
	}

	public unsafe static void Shutdown()
	{
		//IL_008d: Expected I, but got I8
		//IL_008d: Expected I, but got I8
		//IL_0070: Expected I, but got I8
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_lock);
		try
		{
			managedLock = managedLock2;
			_shutdown = true;
			if (_asyncRequests != null && _asyncRequests.Count > 0)
			{
				int num = 0;
				if (0 < _asyncRequests.Count)
				{
					do
					{
						AsyncRequest asyncRequest = (AsyncRequest)_asyncRequests[num];
						if (asyncRequest.cancelOnShutdown)
						{
							IHttpWebRequest* pRequest = asyncRequest.pRequest;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pRequest + 176)))((nint)pRequest);
						}
						num++;
					}
					while (num < _asyncRequests.Count);
				}
				m_hRequestsCompleteEvent = global::_003CModule_003E.CreateEventW(null, 1, 0, null);
				managedLock.Unlock();
				if (m_hRequestsCompleteEvent != null)
				{
					global::_003CModule_003E.WaitForSingleObject(m_hRequestsCompleteEvent, 120000u);
					global::_003CModule_003E.CloseHandle(m_hRequestsCompleteEvent);
				}
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
	}

	internal unsafe static void OnAsyncRequestComplete(IHttpWebRequest* pRequest)
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_lock);
		try
		{
			managedLock = managedLock2;
			int num = 0;
			if (0 < _asyncRequests.Count)
			{
				do
				{
					if (((AsyncRequest)_asyncRequests[num]).pRequest != pRequest)
					{
						num++;
						continue;
					}
					_asyncRequests.RemoveAt(num);
					break;
				}
				while (num < _asyncRequests.Count);
			}
			if (m_hRequestsCompleteEvent != null && _asyncRequests.Count == 0)
			{
				managedLock.Unlock();
				global::_003CModule_003E.SetEvent(m_hRequestsCompleteEvent);
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
	}

	private HttpWebRequest(Uri uri)
	{
		_uri = uri;
		_method = "GET";
		_keepAlive = true;
		_cachePolicy = HttpRequestCachePolicy.Default;
	}

	private unsafe int ConstructNativeRequest(IHttpWebRequest** ppRequest)
	{
		//IL_0077: Expected I, but got I8
		//IL_0077: Expected I, but got I8
		//IL_00b3: Expected I, but got I8
		//IL_024b: Expected I, but got I8
		//IL_024b: Expected I, but got I8
		//IL_021d: Expected I, but got I8
		//IL_021d: Expected I, but got I8
		//IL_00fc: Expected I, but got I8
		//IL_00e7: Expected I, but got I8
		//IL_0257: Expected I, but got I8
		//IL_0260: Expected I8, but got I
		//IL_011a: Expected I, but got I8
		//IL_011a: Expected I, but got I8
		//IL_0177: Expected I, but got I8
		//IL_0177: Expected I, but got I8
		//IL_0162: Expected I, but got I8
		//IL_0162: Expected I, but got I8
		//IL_014d: Expected I, but got I8
		//IL_014d: Expected I, but got I8
		//IL_01ae: Expected I, but got I8
		//IL_01ae: Expected I, but got I8
		if (ppRequest == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 774u);
			return -2147467261;
		}
		*(long*)ppRequest = 0L;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(_uri.AbsoluteUri)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(_method)))
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E);
				*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
				int num;
				try
				{
					num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIHttpWebRequest_003E cComPtrNtv_003CIHttpWebRequest_003E);
					*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 976;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IHttpWebRequest**, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CIService_003E)), ptr, ptr2, (IHttpWebRequest**)(&cComPtrNtv_003CIHttpWebRequest_003E));
							if (num >= 0)
							{
								if (_authorization != null && _authorization.Length > 0)
								{
									global::_003CModule_003E.Microsoft_002EZune_002EService_002E_003FA0x12f4bdec_002ESetHeader((IHttpWebRequest*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)), "Authorization: " + _authorization);
								}
								if (_acceptLanguage != null && _acceptLanguage.Length > 0)
								{
									global::_003CModule_003E.Microsoft_002EZune_002EService_002E_003FA0x12f4bdec_002ESetHeader((IHttpWebRequest*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)), "Accept-Language: " + _acceptLanguage);
								}
								if (_keepAlive)
								{
									global::_003CModule_003E.Microsoft_002EZune_002EService_002E_003FA0x12f4bdec_002ESetHeader((IHttpWebRequest*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)), "Proxy-Connection: Keep-Alive");
								}
								if (_acceptGZipEncoding)
								{
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 104)))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), 1);
								}
								if (num >= 0)
								{
									switch (_cachePolicy)
									{
									case HttpRequestCachePolicy.Refresh:
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EHttpCacheLevel, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), (EHttpCacheLevel)2);
										break;
									case HttpRequestCachePolicy.Default:
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EHttpCacheLevel, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), (EHttpCacheLevel)1);
										break;
									case HttpRequestCachePolicy.BypassCache:
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EHttpCacheLevel, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), (EHttpCacheLevel)0);
										break;
									}
									if (num >= 0 && !string.IsNullOrEmpty(_browserCookieUrl))
									{
										fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(_browserCookieUrl)))
										{
											try
											{
												long num3 = *(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 144;
												num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num3))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), ptr3);
											}
											catch
											{
												//try-fault
												ptr3 = null;
												throw;
											}
										}
									}
								}
							}
						}
						fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(_contentType)))
						{
							if (num >= 0)
							{
								MemoryStream requestStream = _requestStream;
								if (requestStream != null && requestStream.Length > 0)
								{
									byte[] array = _requestStream.ToArray();
									uint num4 = (uint)array.Length;
									fixed (byte* ptr5 = &array[0])
									{
										try
										{
											long num5 = *(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 136;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int, byte*, int>)(*(ulong*)num5))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), ptr4, (int)num4, ptr5);
										}
										catch
										{
											//try-fault
											ptr5 = null;
											throw;
										}
									}
								}
								if (num >= 0)
								{
									long maxResponseLength = _maxResponseLength;
									if (maxResponseLength > 0)
									{
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E)) + 88)))((nint)(*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E)), (ulong)maxResponseLength);
									}
									if (num >= 0)
									{
										IHttpWebRequest* ptr6 = (IHttpWebRequest*)(*(ulong*)(&cComPtrNtv_003CIHttpWebRequest_003E));
										*(long*)(&cComPtrNtv_003CIHttpWebRequest_003E) = 0L;
										*(long*)ppRequest = (nint)ptr6;
									}
								}
							}
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIHttpWebRequest_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIHttpWebRequest_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIHttpWebRequest_003E);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CIHttpWebRequest_003E_002ERelease(&cComPtrNtv_003CIHttpWebRequest_003E);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
				return num;
			}
		}
	}

	private unsafe static void OnAsyncRequestStart(IHttpWebRequest* pRequest, [MarshalAs(UnmanagedType.U1)] bool cancelOnShutdown)
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_lock);
		try
		{
			managedLock = managedLock2;
			AsyncRequest value = new AsyncRequest(pRequest, cancelOnShutdown);
			if (_asyncRequests == null)
			{
				_asyncRequests = new ArrayList();
			}
			_asyncRequests.Add(value);
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
	}
}
