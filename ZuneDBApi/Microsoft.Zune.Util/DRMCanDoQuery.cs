using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZuneDBApi.Interop;

namespace Microsoft.Zune.Util;

public class DRMCanDoQuery : IDisposable
{
	private unsafe IDRMQuery* _pDRMQuery;

	public unsafe DRMCanDoQuery()
	{
		System.Runtime.CompilerServices.Unsafe.SkipInit(out IDRMQuery* pDRMQuery);
		int num = ZuneLibraryExports.CreateDRMQuery(&pDRMQuery);
		if (num < 0)
		{
			throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
		}
		_pDRMQuery = pDRMQuery;
	}

	private void _007EDRMCanDoQuery()
	{
		_0021DRMCanDoQuery();
	}

	private unsafe void _0021DRMCanDoQuery()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		IDRMQuery* pDRMQuery = _pDRMQuery;
		if (pDRMQuery != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDRMQuery + 16)))((nint)pDRMQuery);
			_pDRMQuery = null;
		}
	}

	public unsafe void SetDeviceInfo([MarshalAs(UnmanagedType.U1)] bool fHasSerialNumber, string deviceCert)
	{
		//IL_0022: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(deviceCert)))
		{
			long num = *(long*)_pDRMQuery + 24;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, ushort*, int>)(*(ulong*)num))((nint)_pDRMQuery, fHasSerialNumber ? ((byte)1) : ((byte)0), ptr);
			if (num2 < 0)
			{
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num2));
			}
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanBurnFile(string path)
	{
		//IL_0023: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(path)))
		{
			long num = *(long*)_pDRMQuery + 56;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out bool flag);
			int num2 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, bool*, int>)(*(ulong*)num))((nint)_pDRMQuery, ptr, &flag) >= 0 && flag) ? 1 : 0);
			return (byte)num2 != 0;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanBurnKID(string DRMKeyID)
	{
		//IL_0023: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(DRMKeyID)))
		{
			long num = *(long*)_pDRMQuery + 64;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out bool result);
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, bool*, int>)(*(ulong*)num))((nint)_pDRMQuery, ptr, &result);
			if (num2 < 0)
			{
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num2));
			}
			return result;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanSyncFile(string path)
	{
		//IL_0023: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(path)))
		{
			long num = *(long*)_pDRMQuery + 72;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out bool result);
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, bool*, int>)(*(ulong*)num))((nint)_pDRMQuery, ptr, &result);
			if (num2 < 0)
			{
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num2));
			}
			return result;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool CanSyncKID(string DRMKeyID)
	{
		//IL_0023: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(DRMKeyID)))
		{
			long num = *(long*)_pDRMQuery + 80;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out bool result);
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, bool*, int>)(*(ulong*)num))((nint)_pDRMQuery, ptr, &result);
			if (num2 < 0)
			{
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num2));
			}
			return result;
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021DRMCanDoQuery();
			return;
		}
		try
		{
			_0021DRMCanDoQuery();
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

	~DRMCanDoQuery()
	{
		Dispose(false);
	}
}
