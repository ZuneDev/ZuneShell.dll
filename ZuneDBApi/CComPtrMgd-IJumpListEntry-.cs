using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CIJumpListEntry_003E : IDisposable
{
	public unsafe IJumpListEntry* p = null;

	private void _007ECComPtrMgd_003CIJumpListEntry_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CIJumpListEntry_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		IJumpListEntry* ptr = p;
		IJumpListEntry* ptr2 = ptr;
		if (ptr != null)
		{
			p = null;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	[SpecialName]
	public unsafe IJumpListEntry* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe IJumpListEntry* op_Assign(IJumpListEntry* lp)
	{
		//IL_001c: Expected I, but got I8
		Release();
		p = lp;
		if (lp != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)lp + 8)))((nint)lp);
		}
		return p;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			Release();
			return;
		}
		try
		{
			Release();
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

	~CComPtrMgd_003CIJumpListEntry_003E()
	{
		Dispose(false);
	}
}
