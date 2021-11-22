using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CIWinLiveInformation_003E : IDisposable
{
	public unsafe IWinLiveInformation* p = null;

	private void _007ECComPtrMgd_003CIWinLiveInformation_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CIWinLiveInformation_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		IWinLiveInformation* ptr = p;
		IWinLiveInformation* ptr2 = ptr;
		if (ptr != null)
		{
			p = null;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	public static unsafe implicit operator IWinLiveInformation*(CComPtrMgd_003CIWinLiveInformation_003E obj)
	{
		return obj.p;
	}

	[SpecialName]
	public unsafe IWinLiveInformation* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe IWinLiveInformation* op_Assign(IWinLiveInformation* lp)
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

	~CComPtrMgd_003CIWinLiveInformation_003E()
	{
		Dispose(false);
	}
}
