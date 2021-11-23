#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
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

		internal unsafe SafeBitmapWithData(void* pData, HBITMAP* hBitmap)
		{
			//Discarded unreachable code: IL_00b9
			_pImageData = pData;
			base._002Ector(hBitmap);
			int num;
			try
			{
				byte condition = (byte)(((long)(nint)pData != 0) ? 1 : 0);
				Debug.Assert(condition != 0);
				tagBITMAP tagBITMAP;
				if (Module.GetObjectW(hBitmap, 32, &tagBITMAP) != 0)
				{
					goto IL_004c;
				}
				uint lastError = Module.GetLastError();
				num = (((int)lastError > 0) ? ((int)(lastError & 0xFFFF) | -2147024896) : ((int)lastError));
				if (num >= 0)
				{
					goto IL_004c;
				}
				goto end_IL_000e;
				IL_004c:
				_iWidth = Unsafe.As<tagBITMAP, int>(ref Unsafe.AddByteOffset(ref tagBITMAP, 4));
				_iHeight = Unsafe.As<tagBITMAP, int>(ref Unsafe.AddByteOffset(ref tagBITMAP, 8));
				goto IL_00c3;
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
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
				{
					Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x4101eecd_002EWPP_SafeBitmap_cpp_Traceguids), (uint)num);
				}
				throw new ApplicationException(Module.GetErrorDescription(num));
			}
			catch
			{
				//try-fault
				base.Dispose(disposing: true);
				throw;
			}
			IL_00c3:
			try
			{
			}
			catch
			{
				//try-fault
				base.Dispose(disposing: true);
				throw;
			}
		}

		internal unsafe SafeBitmapWithData(int iHeight, int iWidth, void* pData, HBITMAP* hBitmap)
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
			HBITMAP* hBitmap = null;
			void* pData = null;
			if (Module.CopyThumbnailBitmapData((HBITMAP*)handle.ToInt64(), srcX, srcY, srcWidth, srcHeight, dstWidth, dstHeight, &hBitmap, &pData) >= 0)
			{
				result = new SafeBitmapWithData(dstHeight, dstWidth, pData, hBitmap);
			}
			return (SafeBitmapWithData)result;
		}

		public unsafe static SafeBitmapWithData CreateThumbnailBitmap(string strFilename)
		{
			object result = null;
			fixed (char* strFilenamePtr = strFilename.ToCharArray())
			{
				int iWidth;
				int iHeight;
				void* pData;
				HBITMAP* hBitmap;
				if (Module.GetThumbnailBitmapData((ushort*)strFilenamePtr, &iWidth, &iHeight, &pData, &hBitmap) >= 0)
				{
					result = new SafeBitmapWithData(iHeight, iWidth, pData, hBitmap);
				}
				return (SafeBitmapWithData)result;
			}
		}
	}
}
