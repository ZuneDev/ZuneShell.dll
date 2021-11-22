using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class JumpListEntry : IDisposable
	{
		private readonly CComPtrMgd_003CIJumpListEntry_003E m_spEntry;

		public unsafe int IconIndex
		{
			get
			{
				//IL_001d: Expected I, but got I8
				int result = 0;
				IJumpListEntry* p = m_spEntry.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, &result);
				return result;
			}
			set
			{
				//IL_001a: Expected I, but got I8
				IJumpListEntry* p = m_spEntry.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, value);
			}
		}

		public unsafe string CommandLineArguments
		{
			get
			{
				//IL_0003: Expected I, but got I8
				//IL_001f: Expected I, but got I8
				ushort* ptr = null;
				object result = null;
				IJumpListEntry* p = m_spEntry.p;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
				}
				Module.SysFreeString(ptr);
				return (string)result;
			}
			set
			{
				//IL_0023: Expected I, but got I8
				fixed (char* valuePtr = value.ToCharArray())
				{
					ushort* ptr = (ushort*)valuePtr;
					IJumpListEntry* p = m_spEntry.p;
					long num = *(long*)p + 48;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)p, ptr);
				}
			}
		}

		public unsafe string Name
		{
			get
			{
				//IL_0003: Expected I, but got I8
				//IL_001f: Expected I, but got I8
				ushort* ptr = null;
				object result = null;
				IJumpListEntry* p = m_spEntry.p;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
				}
				Module.SysFreeString(ptr);
				return (string)result;
			}
			set
			{
				//IL_0023: Expected I, but got I8
				fixed (char* valuePtr = value.ToCharArray())
				{
					ushort* ptr = (ushort*)valuePtr;
					IJumpListEntry* p = m_spEntry.p;
					long num = *(long*)p + 32;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)p, ptr);
				}
			}
		}

		internal unsafe JumpListEntry(IJumpListEntry* pEntry)
		{
			CComPtrMgd_003CIJumpListEntry_003E spEntry = new CComPtrMgd_003CIJumpListEntry_003E();
			try
			{
				m_spEntry = spEntry;
				m_spEntry.op_Assign(pEntry);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spEntry).Dispose();
				throw;
			}
		}

		public void _007EJumpListEntry()
		{
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
				}
				finally
				{
					((IDisposable)m_spEntry).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
