using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicrosoftZuneLibrary
{
	public class ZuneLibraryCDDeviceList : IDisposable
	{
		private unsafe IWMPCDDeviceList* m_pDeviceList;

		private uint m_dwAdviseCookie;

		private bool m_fAdvised;

		private bool m_disposed;

		private OnMediaChangedHandler m_MediaChangedHandler;

		private int m_RefCount;

		public unsafe int Count
		{
			get
			{
				//IL_001b: Expected I, but got I8
				uint result = 0u;
				IWMPCDDeviceList* pDeviceList = m_pDeviceList;
				if (pDeviceList != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)pDeviceList + 24)))((nint)pDeviceList, &result);
				}
				return (int)result;
			}
		}

		[SpecialName]
		public virtual event OnMediaChangedHandler MediaChangedHandler
		{
			add
			{
				m_MediaChangedHandler = (OnMediaChangedHandler)Delegate.Combine(m_MediaChangedHandler, value);
			}
			remove
			{
				m_MediaChangedHandler = (OnMediaChangedHandler)Delegate.Remove(m_MediaChangedHandler, value);
			}
		}

		public unsafe ZuneLibraryCDDeviceList(IWMPCDDeviceList* pDeviceList)
		{
			//IL_003e: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_009b: Expected I, but got I8
			//IL_00b2: Expected I, but got I8
			m_pDeviceList = pDeviceList;
			m_fAdvised = false;
			m_disposed = false;
			m_RefCount = 0;
			base._002Ector();
			IWMPCDDeviceList* pDeviceList2 = m_pDeviceList;
			if (pDeviceList2 == null)
			{
				return;
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDeviceList2 + 8)))((nint)pDeviceList2);
			CDDeviceCallback* ptr = (CDDeviceCallback*)Module.@new(24uL);
			CDDeviceCallback* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ECDDeviceCallback_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			IZuneCDDeviceCallback* ptr3;
			if (ptr2 == null || ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr2)))((nint)ptr2, (_GUID*)Unsafe.AsPointer(ref Module.IID_IUnknown), (void**)(&ptr3)) < 0)
			{
				return;
			}
			fixed (uint* ptr4 = &m_dwAdviseCookie)
			{
				try
				{
					long num = *(long*)m_pDeviceList + 48;
					IWMPCDDeviceList* pDeviceList3 = m_pDeviceList;
					IZuneCDDeviceCallback* intPtr = ptr3;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IZuneCDDeviceCallback*, uint*, int>)(*(ulong*)num))((nint)pDeviceList3, intPtr, ptr4) >= 0)
					{
						m_fAdvised = true;
					}
					IZuneCDDeviceCallback* intPtr2 = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
				catch
				{
					//try-fault
					ptr4 = null;
					throw;
				}
			}
		}

		private void _007EZuneLibraryCDDeviceList()
		{
			m_MediaChangedHandler = null;
			_0021ZuneLibraryCDDeviceList();
		}

		private unsafe void _0021ZuneLibraryCDDeviceList()
		{
			//IL_002d: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			if (m_disposed)
			{
				return;
			}
			IWMPCDDeviceList* pDeviceList = m_pDeviceList;
			if (pDeviceList != null)
			{
				if (m_fAdvised)
				{
					IWMPCDDeviceList* intPtr = pDeviceList;
					uint dwAdviseCookie = m_dwAdviseCookie;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)pDeviceList + 56)))((nint)intPtr, dwAdviseCookie);
					m_fAdvised = false;
				}
				pDeviceList = m_pDeviceList;
				IWMPCDDeviceList* intPtr2 = pDeviceList;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				m_pDeviceList = null;
			}
			m_disposed = true;
		}

		public uint AddRef()
		{
			return (uint)Interlocked.Increment(ref m_RefCount);
		}

		public uint Release()
		{
			int num = Interlocked.Decrement(ref m_RefCount);
			if (0 == num)
			{
				_0021ZuneLibraryCDDeviceList();
			}
			return (uint)num;
		}

		public unsafe ZuneLibraryCDDevice GetItem(int idx)
		{
			//IL_0018: Expected I, but got I8
			//IL_002f: Expected I, but got I8
			if (m_pDeviceList == null)
			{
				return null;
			}
			if (idx >= Count)
			{
				return null;
			}
			IWMPCDDevice* pDevice = null;
			IWMPCDDeviceList* pDeviceList = m_pDeviceList;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IWMPCDDevice**, int>)(*(ulong*)(*(long*)pDeviceList + 32)))((nint)pDeviceList, (uint)idx, &pDevice) < 0)
			{
				return null;
			}
			return new ZuneLibraryCDDevice(pDevice);
		}

		internal void OnMediaChanged(ushort driveLetter, int fMediaPresent)
		{
			OnMediaChangedHandler mediaChangedHandler = m_MediaChangedHandler;
			if (mediaChangedHandler != null)
			{
				bool fMediaArrived = ((fMediaPresent != 0) ? true : false);
				mediaChangedHandler((char)driveLetter, fMediaArrived);
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EZuneLibraryCDDeviceList();
				return;
			}
			try
			{
				_0021ZuneLibraryCDDeviceList();
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

		~ZuneLibraryCDDeviceList()
		{
			Dispose(false);
		}
	}
}
