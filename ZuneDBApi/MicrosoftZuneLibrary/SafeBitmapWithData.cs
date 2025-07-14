#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Iris;
using ZuneDBApi.Interop;

namespace MicrosoftZuneLibrary;

[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
public class SafeBitmapWithData : SafeBitmap
{
	private unsafe void* _pImageData;

	private int _iHeight;

	private int _iWidth;

	private int DataSize => ScanLineWidth * _iHeight;

	public int Height => _iHeight;

	public int Width => _iWidth;

	public int ScanLineWidth => ((_iWidth + 1) * 3) & -4;

	internal unsafe SafeBitmapWithData(void* pData, HBITMAP__* hBitmap)
	{
		_pImageData = pData;
		base._002Ector(hBitmap);
		int num;
		try
		{
			byte condition = (byte)(((long)(nint)pData != 0) ? 1 : 0);
			Debug.Assert(condition != 0);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out tagBITMAP tagBITMAP);
			if (global::_003CModule_003E.GetObjectW(hBitmap, 32, &tagBITMAP) != 0)
			{
				goto IL_004c;
			}
			uint lastError = global::_003CModule_003E.GetLastError();
			num = (((int)lastError > 0) ? ((int)(lastError & 0xFFFF) | -2147024896) : ((int)lastError));
			if (num >= 0)
			{
				goto IL_004c;
			}
			goto end_IL_000e;
			IL_004c:
			_iWidth = System.Runtime.CompilerServices.Unsafe.As<tagBITMAP, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagBITMAP, 4));
			_iHeight = System.Runtime.CompilerServices.Unsafe.As<tagBITMAP, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagBITMAP, 8));
			goto IL_00c4;
			end_IL_000e:;
		}
		catch
		{
			//try-fault
			base.Dispose(disposing: true);
			throw;
		}
		try
		{
			if (global::_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E.WPP_GLOBAL_Control) && (*(int*)((ulong)(nint)global::_003CModule_003E.WPP_GLOBAL_Control + 28uL) & 2) != 0 && (uint)(*(byte*)((ulong)(nint)global::_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				global::_003CModule_003E.WPP_SF_D(*(ulong*)((ulong)(nint)global::_003CModule_003E.WPP_GLOBAL_Control + 16uL), 10, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003FA0x4101eecd_002EWPP_SafeBitmap_cpp_Traceguids), (uint)num);
			}
			throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
		}
		catch
		{
			//try-fault
			base.Dispose(disposing: true);
			throw;
		}
		IL_00c4:
		try
		{
			return;
		}
		catch
		{
			//try-fault
			base.Dispose(disposing: true);
			throw;
		}
	}

	internal unsafe SafeBitmapWithData(int iHeight, int iWidth, void* pData, HBITMAP__* hBitmap)
	{
		_pImageData = pData;
		_iHeight = iHeight;
		_iWidth = iWidth;
		base._002Ector(hBitmap);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	[return: MarshalAs(UnmanagedType.U1)]
	protected unsafe override bool ReleaseHandle()
	{
		//IL_0008: Expected I, but got I8
		_pImageData = null;
		return base.ReleaseHandle();
	}

	public unsafe IntPtr GetData()
	{
		return (IntPtr)_pImageData;
	}

	public Image CreateImage([MarshalAs(UnmanagedType.U1)] bool antialiasEdges)
	{
		Image result = null;
		IntPtr data = GetData();
		if (Width != 0 && Height != 0 && data != IntPtr.Zero)
		{
			result = new Image(null, Width, Height, -ScanLineWidth, RawImageFormat.R8G8B8, data, 0, 0, flippable: false, antialiasEdges);
		}
		return result;
	}

	public Image CreateImage()
	{
		return CreateImage(antialiasEdges: false);
	}

	public unsafe SafeBitmapWithData Clone(int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
	{
		//IL_0004: Expected I, but got I8
		//IL_0007: Expected I, but got I8
		//IL_0024: Expected I, but got I8
		object result = null;
		HBITMAP__* hBitmap = null;
		void* pData = null;
		if (ZuneLibraryExports.CopyThumbnailBitmapData((HBITMAP__*)handle.ToInt64(), srcX, srcY, srcWidth, srcHeight, dstWidth, dstHeight, &hBitmap, &pData) >= 0)
		{
			result = new SafeBitmapWithData(dstHeight, dstWidth, pData, hBitmap);
		}
		return (SafeBitmapWithData)result;
	}

	public unsafe static SafeBitmapWithData CreateThumbnailBitmap(string strFilename)
	{
		object result = null;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strFilename)))
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int iWidth);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int iHeight);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out void* pData);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out HBITMAP__* hBitmap);
			if (ZuneLibraryExports.GetThumbnailBitmapData(ptr, &iWidth, &iHeight, &pData, &hBitmap) >= 0)
			{
				result = new SafeBitmapWithData(iHeight, iWidth, pData, hBitmap);
			}
			return (SafeBitmapWithData)result;
		}
	}
}
