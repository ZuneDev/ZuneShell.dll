using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CISyncEngine_003E : IDisposable
{
	public unsafe ISyncEngine* p = null;

	private void _007ECComPtrMgd_003CISyncEngine_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CISyncEngine_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		ISyncEngine* ptr = p;
		ISyncEngine* ptr2 = ptr;
		if (ptr != null)
		{
			p = null;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	public unsafe implicit operator ISyncEngine*()
	{
		return p;
	}

	[SpecialName]
	public unsafe ISyncEngine* op_MemberSelection()
	{
		return p;
	}

	public unsafe void Attach(ISyncEngine* p2)
	{
		//IL_001b: Expected I, but got I8
		ISyncEngine* ptr = p;
		if (ptr != p2)
		{
			if (ptr != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 16)))((nint)ptr);
			}
			p = p2;
		}
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
			base.Finalize();
		}
	}

	public sealed override void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~CComPtrMgd_003CISyncEngine_003E()
	{
		Dispose(false);
	}
}
