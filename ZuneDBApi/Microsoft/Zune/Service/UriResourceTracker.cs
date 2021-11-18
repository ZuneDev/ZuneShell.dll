using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class UriResourceTracker : IDisposable
	{
		private static UriResourceTracker m_singletonInstance = null;

		private unsafe IUriResourceTracker* m_pUriResourceTracker = null;

		public static UriResourceTracker Instance
		{
			get
			{
				if (m_singletonInstance == null)
				{
					m_singletonInstance = new UriResourceTracker();
				}
				return m_singletonInstance;
			}
		}

		private unsafe UriResourceTracker()
		{
			//IL_0008: Expected I, but got I8
			//IL_0011: Expected I, but got I8
			IUriResourceTracker* pUriResourceTracker = null;
			if (Module.GetSingleton((_GUID)Module._GUID_ddbb9148_dea1_47dd_a0c1_1fdcf002c1e2, (void**)(&pUriResourceTracker)) >= 0)
			{
				m_pUriResourceTracker = pUriResourceTracker;
			}
		}

		private void _007EUriResourceTracker()
		{
			_0021UriResourceTracker();
		}

		private unsafe void _0021UriResourceTracker()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IUriResourceTracker* pUriResourceTracker = m_pUriResourceTracker;
			if (pUriResourceTracker != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pUriResourceTracker + 16)))((nint)pUriResourceTracker);
				m_pUriResourceTracker = null;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool SetResourceModified(string strUrlResource, [MarshalAs(UnmanagedType.U1)] bool fModified)
		{
			//IL_002c: Expected I, but got I8
			bool result = false;
			if (m_pUriResourceTracker != null)
			{
				fixed (char* strUrlResourcePtr = strUrlResource.ToCharArray())
				{
					ushort* ptr = (ushort*)strUrlResourcePtr;
					try
					{
						long num = *(long*)m_pUriResourceTracker + 24;
						IUriResourceTracker* pUriResourceTracker = m_pUriResourceTracker;
						result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, byte, int>)(*(ulong*)num))((nint)pUriResourceTracker, ptr, fModified ? ((byte)1) : ((byte)0)) >= 0) ? 1u : 0u) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool IsResourceModified(string strUrlResource)
		{
			//IL_002b: Expected I, but got I8
			bool result = false;
			if (m_pUriResourceTracker != null)
			{
				fixed (char* strUrlResourcePtr = strUrlResource.ToCharArray())
				{
					ushort* ptr = (ushort*)strUrlResourcePtr;
					try
					{
						long num = *(long*)m_pUriResourceTracker + 32;
						IUriResourceTracker* pUriResourceTracker = m_pUriResourceTracker;
						result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, byte>)(*(ulong*)num))((nint)pUriResourceTracker, ptr) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021UriResourceTracker();
				return;
			}
			try
			{
				_0021UriResourceTracker();
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

		~UriResourceTracker()
		{
			Dispose(false);
		}
	}
}
