using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class TrackMetadata : INotifyPropertyChanged, IDisposable
	{
		private unsafe ITrackInfo* m_pTrackInfo;

		private PropertyChangedEventHandler m_PropertyChanged;

		private bool m_fDisposed;

		internal unsafe ITrackInfo* TrackInfo => m_pTrackInfo;

		public unsafe int MediaId
		{
			get
			{
				//IL_001e: Expected I, but got I8
				int result = -1;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pTrackInfo + 144)))((nint)pTrackInfo, &result);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetDiscNumber failed", num);
					}
				}
				return result;
			}
			set
			{
				//IL_0024: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && MediaId != value)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pTrackInfo + 152)))((nint)pTrackInfo, value);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::SetMediaId failed", num);
					}
					FirePropertyChanged("MediaId");
				}
			}
		}

		public unsafe int DiscNumber
		{
			get
			{
				//IL_001b: Expected I, but got I8
				int result = -1;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pTrackInfo + 120)))((nint)pTrackInfo, &result);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetDiscNumber failed", num);
					}
				}
				return result;
			}
			set
			{
				//IL_0024: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && DiscNumber != value)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pTrackInfo + 128)))((nint)pTrackInfo, value);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::SetDiscNumber failed", num);
					}
					FirePropertyChanged("DiscNumber");
				}
			}
		}

		public unsafe int TrackNumber
		{
			get
			{
				//IL_001b: Expected I, but got I8
				int result = -1;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pTrackInfo + 104)))((nint)pTrackInfo, &result);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetTrackNumber failed", num);
					}
				}
				return result;
			}
			set
			{
				//IL_0021: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && TrackNumber != value)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pTrackInfo + 112)))((nint)pTrackInfo, value);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::SetTrackNumber failed", num);
					}
					FirePropertyChanged("TrackNumber");
				}
			}
		}

		public unsafe string Composer
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pTrackInfo + 24)))((nint)pTrackInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetComposer failed", num);
					}
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//IL_0047: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && Composer.CompareTo(value) != 0)
				{
					fixed (char* valuePtr = value.ToCharArray())
					{
						ushort* ptr = (ushort*)valuePtr;
						try
						{
							ushort* ptr2 = Module.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pTrackInfo + 32)))((nint)pTrackInfo, ptr2);
							Module.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("ITrackInfo::SetComposer failed", num);
							}
							FirePropertyChanged("Composer");
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

		public unsafe string Conductor
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pTrackInfo + 40)))((nint)pTrackInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetConductor failed", num);
					}
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//IL_0047: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && Conductor.CompareTo(value) != 0)
				{
					fixed (char* valuePtr = value.ToCharArray())
					{
						ushort* ptr = (ushort*)valuePtr;
						try
						{
							ushort* ptr2 = Module.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pTrackInfo + 48)))((nint)pTrackInfo, ptr2);
							Module.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("ITrackInfo::SetConductor failed", num);
							}
							FirePropertyChanged("Conductor");
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

		public unsafe string Genre
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pTrackInfo + 88)))((nint)pTrackInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetGenre failed", num);
					}
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//IL_0047: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && Genre.CompareTo(value) != 0)
				{
					fixed (char* valuePtr = value.ToCharArray())
					{
						ushort* ptr = (ushort*)valuePtr;
						try
						{
							ushort* ptr2 = Module.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pTrackInfo + 96)))((nint)pTrackInfo, ptr2);
							Module.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("ITrackInfo::SetGenre failed", num);
							}
							FirePropertyChanged("Genre");
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

		public unsafe string TrackArtist
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pTrackInfo + 56)))((nint)pTrackInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetArtist failed", num);
					}
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//IL_0047: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && TrackArtist.CompareTo(value) != 0)
				{
					fixed (char* valuePtr = value.ToCharArray())
					{
						ushort* ptr = (ushort*)valuePtr;
						try
						{
							ushort* ptr2 = Module.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pTrackInfo + 64)))((nint)pTrackInfo, ptr2);
							Module.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("ITrackInfo::SetArtist failed", num);
							}
							FirePropertyChanged("TrackArtist");
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

		public unsafe string TrackTitle
		{
			get
			{
				//IL_000f: Expected I, but got I8
				//IL_001e: Expected I, but got I8
				string result = null;
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					ushort* ptr = null;
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pTrackInfo + 72)))((nint)pTrackInfo, &ptr);
					if (num < 0)
					{
						throw new COMException("ITrackInfo::GetTitle failed", num);
					}
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
			set
			{
				//IL_0047: Expected I, but got I8
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null && TrackTitle.CompareTo(value) != 0)
				{
					fixed (char* valuePtr = value.ToCharArray())
					{
						ushort* ptr = (ushort*)valuePtr;
						try
						{
							ushort* ptr2 = Module.SysAllocString(ptr);
							if (ptr2 == null)
							{
								throw new COMException("SysAllocString failed", -2147024882);
							}
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)pTrackInfo + 80)))((nint)pTrackInfo, ptr2);
							Module.SysFreeString(ptr2);
							if (num != 0)
							{
								throw new COMException("ITrackInfo::SetTitle failed", num);
							}
							FirePropertyChanged("TrackTitle");
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

		[SpecialName]
		public virtual event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				m_PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(m_PropertyChanged, value);
			}
			remove
			{
				m_PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(m_PropertyChanged, value);
			}
		}

		public unsafe TrackMetadata(ITrackInfo* pTrackInfo)
		{
			//IL_001c: Expected I, but got I8
			m_pTrackInfo = pTrackInfo;
			if (pTrackInfo != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pTrackInfo + 8)))((nint)pTrackInfo);
			}
		}

		private void _007ETrackMetadata()
		{
			_0021TrackMetadata();
		}

		private unsafe void _0021TrackMetadata()
		{
			//IL_001f: Expected I, but got I8
			//IL_0028: Expected I, but got I8
			if (!m_fDisposed)
			{
				ITrackInfo* pTrackInfo = m_pTrackInfo;
				if (pTrackInfo != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pTrackInfo + 16)))((nint)pTrackInfo);
					m_pTrackInfo = null;
				}
			}
		}

		private void FirePropertyChanged(string propName)
		{
			if (m_PropertyChanged != null)
			{
				m_PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021TrackMetadata();
				return;
			}
			try
			{
				_0021TrackMetadata();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~TrackMetadata()
		{
			Dispose(false);
		}
	}
}
