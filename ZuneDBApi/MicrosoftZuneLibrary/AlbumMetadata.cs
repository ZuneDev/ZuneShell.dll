using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class AlbumMetadata : IDisposable
	{
		private unsafe IAlbumInfo* m_pAlbumInfo;

		private bool m_fDisposed;

		internal unsafe IAlbumInfo* AlbumInfo => m_pAlbumInfo;

		public unsafe bool ExactMatch
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001e: Expected I, but got I8
				bool result = false;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					int num;
					int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pAlbumInfo + 176)))((nint)pAlbumInfo, &num);
					if (num2 < 0)
					{
						throw new COMException("IAlbumInfo::IsExactMatch failed", num2);
					}
					bool flag = ((num != 0) ? true : false);
					result = flag;
				}
				return result;
			}
		}

		public unsafe uint TrackCount
		{
			get
			{
				//IL_001e: Expected I, but got I8
				uint result = 0u;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)pAlbumInfo + 128)))((nint)pAlbumInfo, &result);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetTrackCount failed", num);
					}
				}
				return result;
			}
		}

		public unsafe uint WMISTrackCount
		{
			get
			{
				//IL_001e: Expected I, but got I8
				uint result = 0u;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)pAlbumInfo + 168)))((nint)pAlbumInfo, &result);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetWMISTrackCount failed", num);
					}
				}
				return result;
			}
		}

		public unsafe int MediaId
		{
			get
			{
				//IL_001e: Expected I, but got I8
				int result = -1;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pAlbumInfo + 160)))((nint)pAlbumInfo, &result);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetMediaId failed", num);
					}
				}
				return result;
			}
		}

		public unsafe string CoverUrl
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_0021: Expected I, but got I8
				string result = null;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pAlbumInfo + 144)))((nint)pAlbumInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetCoverUrl failed", num);
					}
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//Discarded unreachable code: IL_0054
				//IL_003c: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(value)))
					{
						try
						{
							ushort* ptr2 = _003CModule_003E.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pAlbumInfo + 152)))((nint)pAlbumInfo, ptr2);
							_003CModule_003E.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("IAlbumInfo::SetCoverUrl failed", num);
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				try
				{
				}
				catch
				{
					//try-fault
					throw;
				}
			}
		}

		public unsafe int ReleaseYear
		{
			get
			{
				//IL_001b: Expected I, but got I8
				int result = -1;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pAlbumInfo + 112)))((nint)pAlbumInfo, &result);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetReleaseYear failed", num);
					}
				}
				return result;
			}
			set
			{
				//IL_0036: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					switch (value)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 14:
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 21:
					case 22:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
						value += 2000;
						break;
					case 30:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
					case 40:
					case 41:
					case 42:
					case 43:
					case 44:
					case 45:
					case 46:
					case 47:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
					case 58:
					case 59:
					case 60:
					case 61:
					case 62:
					case 63:
					case 64:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
					case 70:
					case 71:
					case 72:
					case 73:
					case 74:
					case 75:
					case 76:
					case 77:
					case 78:
					case 79:
					case 80:
					case 81:
					case 82:
					case 83:
					case 84:
					case 85:
					case 86:
					case 87:
					case 88:
					case 89:
					case 90:
					case 91:
					case 92:
					case 93:
					case 94:
					case 95:
					case 96:
					case 97:
					case 98:
					case 99:
						value += 1900;
						break;
					}
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pAlbumInfo + 120)))((nint)pAlbumInfo, value);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::SetReleaseYear failed", num);
					}
				}
			}
		}

		public unsafe string AlbumArtistYomi
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pAlbumInfo + 80)))((nint)pAlbumInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetAlbumArtistYomi failed", num);
					}
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//Discarded unreachable code: IL_0051
				//IL_0039: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(value)))
					{
						try
						{
							ushort* ptr2 = _003CModule_003E.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pAlbumInfo + 88)))((nint)pAlbumInfo, ptr2);
							_003CModule_003E.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("IAlbumInfo::SetAlbumArtistYomi failed", num);
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				try
				{
				}
				catch
				{
					//try-fault
					throw;
				}
			}
		}

		public unsafe string AlbumArtist
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pAlbumInfo + 64)))((nint)pAlbumInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetAlbumArtist failed", num);
					}
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//Discarded unreachable code: IL_0051
				//IL_0039: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(value)))
					{
						try
						{
							ushort* ptr2 = _003CModule_003E.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pAlbumInfo + 72)))((nint)pAlbumInfo, ptr2);
							_003CModule_003E.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("IAlbumInfo::SetAlbumArtist failed", num);
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				try
				{
				}
				catch
				{
					//try-fault
					throw;
				}
			}
		}

		public unsafe string AlbumTitleYomi
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pAlbumInfo + 48)))((nint)pAlbumInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetTitleYomi failed", num);
					}
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//Discarded unreachable code: IL_0051
				//IL_0039: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(value)))
					{
						try
						{
							ushort* ptr2 = _003CModule_003E.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pAlbumInfo + 56)))((nint)pAlbumInfo, ptr2);
							_003CModule_003E.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("IAlbumInfo::SetTitleYomi failed", num);
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				try
				{
				}
				catch
				{
					//try-fault
					throw;
				}
			}
		}

		public unsafe string AlbumTitle
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pAlbumInfo + 32)))((nint)pAlbumInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("IAlbumInfo::GetTitle failed", num);
					}
					result = new string((char*)ptr);
					_003CModule_003E.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//Discarded unreachable code: IL_0051
				//IL_0039: Expected I, but got I8
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(value)))
					{
						try
						{
							ushort* ptr2 = _003CModule_003E.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pAlbumInfo + 40)))((nint)pAlbumInfo, ptr2);
							_003CModule_003E.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("IAlbumInfo::SetTitle failed", num);
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				try
				{
				}
				catch
				{
					//try-fault
					throw;
				}
			}
		}

		public unsafe AlbumMetadata(IAlbumInfo* pAlbumInfo)
		{
			//IL_001c: Expected I, but got I8
			m_pAlbumInfo = pAlbumInfo;
			if (pAlbumInfo != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pAlbumInfo + 8)))((nint)pAlbumInfo);
			}
		}

		private void _007EAlbumMetadata()
		{
			_0021AlbumMetadata();
		}

		private unsafe void _0021AlbumMetadata()
		{
			//IL_001f: Expected I, but got I8
			//IL_0028: Expected I, but got I8
			if (!m_fDisposed)
			{
				IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
				if (pAlbumInfo != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pAlbumInfo + 16)))((nint)pAlbumInfo);
					m_pAlbumInfo = null;
				}
			}
		}

		public unsafe TrackMetadata GetTrack(uint index)
		{
			//IL_000f: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			//IL_0047: Expected I, but got I8
			TrackMetadata result = null;
			IAlbumInfo* pAlbumInfo = m_pAlbumInfo;
			if (pAlbumInfo != null)
			{
				ITrackInfo* ptr = null;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, ITrackInfo**, int>)(*(ulong*)(*(long*)pAlbumInfo + 136)))((nint)pAlbumInfo, index, &ptr);
				if (num < 0)
				{
					throw new COMException("IAlbumInfo::GetTrack failed", num);
				}
				result = new TrackMetadata(ptr);
				ITrackInfo* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			return result;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021AlbumMetadata();
				return;
			}
			try
			{
				_0021AlbumMetadata();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~AlbumMetadata()
		{
			Dispose(false);
		}
	}
}
