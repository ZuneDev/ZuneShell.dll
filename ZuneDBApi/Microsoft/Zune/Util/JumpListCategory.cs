using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class JumpListCategory : IDisposable
	{
		private readonly CComPtrMgd<IJumpListCategory> m_spCategory;

		public unsafe string Name
		{
			get
			{
				//IL_0003: Expected I, but got I8
				//IL_001f: Expected I, but got I8
				ushort* ptr = null;
				object result = null;
				IJumpListCategory* p = m_spCategory.p;
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
					IJumpListCategory* p = m_spCategory.p;
					long num = *(long*)p + 32;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)p, ptr);
				}
			}
		}

		public unsafe int CreateDestination(out JumpListEntry destination)
		{
			//IL_0020: Expected I, but got I8
			//IL_002a: Expected I, but got I8
			CComPtrNtv_003CIJumpListEntry_003E cComPtrNtv_003CIJumpListEntry_003E;
			*(long*)(&cComPtrNtv_003CIJumpListEntry_003E) = 0L;
			try
			{
				IJumpListCategory* p = m_spCategory.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IJumpListEntry**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, (IJumpListEntry**)(&cComPtrNtv_003CIJumpListEntry_003E));
				destination = new JumpListEntry((IJumpListEntry*)(*(ulong*)(&cComPtrNtv_003CIJumpListEntry_003E)));
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIJumpListEntry_003E*, void>)(&Module.CComPtrNtv_003CIJumpListEntry_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIJumpListEntry_003E);
				throw;
			}
			Module.CComPtrNtv_003CIJumpListEntry_003E_002ERelease(&cComPtrNtv_003CIJumpListEntry_003E);
			return 0;
		}

		internal unsafe JumpListCategory(IJumpListCategory* pCategory)
		{
			CComPtrMgd<IJumpListCategory> spCategory = new CComPtrMgd<IJumpListCategory>();
			try
			{
				m_spCategory = spCategory;
				m_spCategory.op_Assign(pCategory);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spCategory).Dispose();
				throw;
			}
		}

		public void _007EJumpListCategory()
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
					((IDisposable)m_spCategory).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
