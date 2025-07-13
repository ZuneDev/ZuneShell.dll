using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util;

public class JumpListSession : IDisposable
{
	private readonly CComPtrMgd_003CIJumpList_003E m_spJumpList;

	public unsafe bool IsAlive
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001b: Expected I, but got I8
			IJumpList* p = m_spJumpList.p;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int num);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 72)))((nint)p, &num);
			return num == 1;
		}
	}

	public unsafe int GetDisallowedDestinations(out List<JumpListEntry> disallowedDestinationList)
	{
		//IL_0043: Expected I, but got I8
		//IL_0085: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		//IL_00d3: Expected I, but got I8
		//IL_00c3: Expected I, but got I8
		//IL_00c3: Expected I, but got I8
		if (m_spJumpList.p == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 214u);
			return -2147418113;
		}
		disallowedDestinationList = null;
		uint num = 0u;
		IJumpList* p = m_spJumpList.p;
		int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &num);
		if (num2 >= 0)
		{
			IJumpListEntry** ptr = (IJumpListEntry**)global::_003CModule_003E.new_005B_005D((ulong)num * 8uL);
			num2 = (((long)(nint)ptr == 0) ? (-2147024882) : num2);
			if (num2 >= 0)
			{
				p = m_spJumpList.p;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IJumpListEntry**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, num, ptr);
				if (num2 >= 0)
				{
					disallowedDestinationList = new List<JumpListEntry>((int)num);
					uint num3 = 0u;
					if (0 < num)
					{
						IJumpListEntry** ptr2 = ptr;
						do
						{
							disallowedDestinationList.Add(new JumpListEntry((IJumpListEntry*)(*(ulong*)ptr2)));
							long num4 = *(long*)ptr2;
							if (0 != num4)
							{
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)num4 + 16)))((nint)num4);
								*(long*)ptr2 = 0L;
							}
							num3++;
							ptr2 = (IJumpListEntry**)((ulong)(nint)ptr2 + 8uL);
						}
						while (num3 < num);
					}
				}
			}
			if (0L != (nint)ptr)
			{
				global::_003CModule_003E.delete_005B_005D(ptr);
			}
		}
		return num2;
	}

	public unsafe int CreateTask(out JumpListEntry task)
	{
		//IL_0020: Expected I, but got I8
		//IL_002a: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIJumpListEntry_003E cComPtrNtv_003CIJumpListEntry_003E);
		*(long*)(&cComPtrNtv_003CIJumpListEntry_003E) = 0L;
		try
		{
			IJumpList* p = m_spJumpList.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IJumpListEntry**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, (IJumpListEntry**)(&cComPtrNtv_003CIJumpListEntry_003E));
			task = new JumpListEntry((IJumpListEntry*)(*(ulong*)(&cComPtrNtv_003CIJumpListEntry_003E)));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIJumpListEntry_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIJumpListEntry_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIJumpListEntry_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIJumpListEntry_003E_002ERelease(&cComPtrNtv_003CIJumpListEntry_003E);
		return 0;
	}

	public unsafe int CreateCategory(out JumpListCategory category)
	{
		//IL_0020: Expected I, but got I8
		//IL_002a: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIJumpListCategory_003E cComPtrNtv_003CIJumpListCategory_003E);
		*(long*)(&cComPtrNtv_003CIJumpListCategory_003E) = 0L;
		try
		{
			IJumpList* p = m_spJumpList.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IJumpListCategory**, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (IJumpListCategory**)(&cComPtrNtv_003CIJumpListCategory_003E));
			category = new JumpListCategory((IJumpListCategory*)(*(ulong*)(&cComPtrNtv_003CIJumpListCategory_003E)));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIJumpListCategory_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIJumpListCategory_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIJumpListCategory_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIJumpListCategory_003E_002ERelease(&cComPtrNtv_003CIJumpListCategory_003E);
		return 0;
	}

	public unsafe int Commit()
	{
		//IL_0031: Expected I, but got I8
		IJumpList* p = m_spJumpList.p;
		if (p == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 303u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 56)))((nint)p);
	}

	public unsafe int Cancel()
	{
		//IL_0031: Expected I, but got I8
		IJumpList* p = m_spJumpList.p;
		if (p == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 319u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 64)))((nint)p);
	}

	internal unsafe JumpListSession(IJumpList* pJumpList)
	{
		CComPtrMgd_003CIJumpList_003E spJumpList = new CComPtrMgd_003CIJumpList_003E();
		try
		{
			m_spJumpList = spJumpList;
			base._002Ector();
			m_spJumpList.op_Assign(pJumpList);
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)m_spJumpList).Dispose();
			throw;
		}
	}

	public void _007EJumpListSession()
	{
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				return;
			}
			finally
			{
				((IDisposable)m_spJumpList).Dispose();
			}
		}
		Finalize();
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
