using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CSyncRulesViewMediator_003E : IDisposable
{
	public unsafe SyncRulesViewMediator* p = null;

	private void _007ECComPtrMgd_003CSyncRulesViewMediator_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CSyncRulesViewMediator_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		SyncRulesViewMediator* ptr = p;
		SyncRulesViewMediator* ptr2 = ptr;
		if (ptr != null)
		{
			p = null;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
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
			//base.Finalize();
		}
	}

	public void Dispose()
	{
		Dispose(true);
	}

	~CComPtrMgd_003CSyncRulesViewMediator_003E()
	{
		Dispose(false);
	}

	public static unsafe implicit operator SyncRulesViewMediator*(CComPtrMgd_003CSyncRulesViewMediator_003E obj)
	{
		return obj.p;
	}

	[SpecialName]
	public unsafe SyncRulesViewMediator* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe SyncRulesViewMediator* op_Assign(SyncRulesViewMediator* lp)
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
}
