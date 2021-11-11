using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class RadioPlaylist : IDisposable
	{
		private unsafe IRadioPlaylist* m_pRadioPlaylist;

		public unsafe RadioPlaylist(IRadioPlaylist* pRadioPlaylist)
		{
			//IL_001e: Expected I, but got I8
			m_pRadioPlaylist = pRadioPlaylist;
			base._002Ector();
			IRadioPlaylist* pRadioPlaylist2 = m_pRadioPlaylist;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pRadioPlaylist2 + 8)))((nint)pRadioPlaylist2);
		}

		private void _007ERadioPlaylist()
		{
			_0021RadioPlaylist();
		}

		private unsafe void _0021RadioPlaylist()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IRadioPlaylist* pRadioPlaylist = m_pRadioPlaylist;
			if (0L != (nint)pRadioPlaylist)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pRadioPlaylist + 16)))((nint)pRadioPlaylist);
				m_pRadioPlaylist = null;
			}
		}

		public unsafe string GetNextUri()
		{
			//IL_0004: Expected I, but got I8
			//IL_001a: Expected I, but got I8
			object result = null;
			ushort* ptr = null;
			IRadioPlaylist* pRadioPlaylist = m_pRadioPlaylist;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pRadioPlaylist + 24)))((nint)pRadioPlaylist, &ptr) >= 0)
			{
				result = new string((char*)ptr);
			}
			_003CModule_003E.SysFreeString(ptr);
			return (string)result;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021RadioPlaylist();
				return;
			}
			try
			{
				_0021RadioPlaylist();
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

		~RadioPlaylist()
		{
			Dispose(false);
		}
	}
}
