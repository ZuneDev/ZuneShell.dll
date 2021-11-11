using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CIWinLiveAvailableInformation_003E : IDisposable
{
	public unsafe IWinLiveAvailableInformation* p = null;

	private void _007ECComPtrMgd_003CIWinLiveAvailableInformation_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CIWinLiveAvailableInformation_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		IWinLiveAvailableInformation* ptr = p;
		IWinLiveAvailableInformation* ptr2 = ptr;
		if (ptr != null)
		{
			p = null;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
	}

	public unsafe implicit operator IWinLiveAvailableInformation*()
	{
		return p;
	}

	[SpecialName]
	public unsafe IWinLiveAvailableInformation* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe IWinLiveAvailableInformation* op_Assign(IWinLiveAvailableInformation* lp)
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
			base.Finalize();
		}
	}

	public sealed override void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~CComPtrMgd_003CIWinLiveAvailableInformation_003E()
	{
		Dispose(false);
	}
}
