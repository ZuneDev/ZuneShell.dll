using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class CComPtrMgd_003CIFirmwareUpdater_003E : IDisposable
{
	public unsafe IFirmwareUpdater* p = null;

	private void _007ECComPtrMgd_003CIFirmwareUpdater_003E()
	{
		Release();
	}

	private void _0021CComPtrMgd_003CIFirmwareUpdater_003E()
	{
		Release();
	}

	public unsafe void Release()
	{
		//IL_0014: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		IFirmwareUpdater* ptr = p;
		IFirmwareUpdater* ptr2 = ptr;
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

	~CComPtrMgd_003CIFirmwareUpdater_003E()
	{
		Dispose(false);
	}

	public static unsafe implicit operator IFirmwareUpdater*(CComPtrMgd_003CIFirmwareUpdater_003E obj)
	{
		return obj.p;
	}

	[SpecialName]
	public unsafe IFirmwareUpdater* op_MemberSelection()
	{
		return p;
	}

	[SpecialName]
	public unsafe IFirmwareUpdater* op_Assign(IFirmwareUpdater* lp)
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

	public unsafe int CopyTo(IFirmwareUpdater** ppT)
	{
		//IL_0020: Expected I8, but got I
		//IL_0036: Expected I, but got I8
		if (ppT == null)
		{
			Module._ZuneShipAssert(1001u, 168u);
			return -2147467261;
		}
		*(long*)ppT = (nint)p;
		IFirmwareUpdater* ptr = p;
		if (ptr != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 8)))((nint)ptr);
		}
		return 0;
	}

	public unsafe int QueryInterfaceIFirmwareUpdater2(IFirmwareUpdater2** pp)
	{
		//IL_002e: Expected I, but got I8
		IFirmwareUpdater* ptr = p;
		if (ptr == null)
		{
			Module._ZuneShipAssert(1001u, 186u);
			return -2147467261;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr)))((nint)ptr, &Module.GUID_IFirmwareUpdater2, (void**)pp);
	}
}
