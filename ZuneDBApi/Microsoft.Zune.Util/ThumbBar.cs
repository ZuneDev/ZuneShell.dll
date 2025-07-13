using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util;

public class ThumbBar : IDisposable
{
	private readonly CComPtrMgd_003CIThumbBar_003E m_spThumbBar;

	public unsafe int CreateButton(out ThumbBarButton button)
	{
		//IL_0042: Expected I, but got I8
		//IL_004c: Expected I, but got I8
		if (m_spThumbBar.p == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 193u);
			return -2147418113;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIThumbBarButton_003E cComPtrNtv_003CIThumbBarButton_003E);
		*(long*)(&cComPtrNtv_003CIThumbBarButton_003E) = 0L;
		try
		{
			IThumbBar* p = m_spThumbBar.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IThumbBarButton**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, (IThumbBarButton**)(&cComPtrNtv_003CIThumbBarButton_003E));
			button = new ThumbBarButton((IThumbBarButton*)(*(ulong*)(&cComPtrNtv_003CIThumbBarButton_003E)));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIThumbBarButton_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIThumbBarButton_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIThumbBarButton_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIThumbBarButton_003E_002ERelease(&cComPtrNtv_003CIThumbBarButton_003E);
		return 0;
	}

	public unsafe int UpdateThumbBar()
	{
		//IL_0031: Expected I, but got I8
		IThumbBar* p = m_spThumbBar.p;
		if (p == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1002u, 215u);
			return -2147418113;
		}
		return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 32)))((nint)p);
	}

	internal unsafe ThumbBar(IThumbBar* thumbBar)
	{
		CComPtrMgd_003CIThumbBar_003E spThumbBar = new CComPtrMgd_003CIThumbBar_003E();
		try
		{
			m_spThumbBar = spThumbBar;
			base._002Ector();
			m_spThumbBar.op_Assign(thumbBar);
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)m_spThumbBar).Dispose();
			throw;
		}
	}

	public void _007EThumbBar()
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
				((IDisposable)m_spThumbBar).Dispose();
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
