using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CIPassportIdentity_003E : IDisposable
{
	public unsafe IPassportIdentity* p = null;

	private void _007ECComPtrMgd_003CIPassportIdentity_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CIPassportIdentity_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		IPassportIdentity* ptr = p;
		IPassportIdentity* ptr2 = ptr;
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
			base.Finalize();
		}
	}

	public sealed override void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~CComPtrMgd_003CIPassportIdentity_003E()
	{
		Dispose(false);
	}

	public unsafe implicit operator IPassportIdentity*()
	{
		return p;
	}

	[SpecialName]
	public unsafe IPassportIdentity* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe IPassportIdentity* op_Assign(IPassportIdentity* lp)
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

	public unsafe int QueryInterface_003CIPassportIdentity_003E(IPassportIdentity** pp)
	{
		//IL_002e: Expected I, but got I8
		IPassportIdentity* ptr = p;
		if (ptr == null)
		{
			Module._ZuneShipAssert(1001u, 186u);
			return -2147467261;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr)))((nint)ptr, (_GUID*)Unsafe.AsPointer(ref Module._GUID_655b468c_1224_467d_b720_3bac7f99b6ba), (void**)pp);
	}
}
