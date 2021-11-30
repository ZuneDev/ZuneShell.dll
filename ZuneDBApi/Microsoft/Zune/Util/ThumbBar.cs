using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class ThumbBar : IDisposable
	{
		private readonly CComPtrMgd<IThumbBar> m_spThumbBar;

		public unsafe int CreateButton(out ThumbBarButton button)
		{
			//IL_0042: Expected I, but got I8
			//IL_004c: Expected I, but got I8
			if (m_spThumbBar.p == null)
			{
				Module._ZuneShipAssert(1002u, 193u);
				button = null;
				return -2147418113;
			}
			CComPtrNtv<IThumbBarButton> cComPtrNtv_003CIThumbBarButton_003E = new();
			try
			{
				IThumbBar* p = m_spThumbBar.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IThumbBarButton**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, cComPtrNtv_003CIThumbBarButton_003E.GetPtrToPtr());
				button = new ThumbBarButton((IThumbBarButton*)(*(ulong*)(cComPtrNtv_003CIThumbBarButton_003E.p)));
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIThumbBarButton_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIThumbBarButton_003E.Dispose();
			return 0;
		}

		public unsafe int UpdateThumbBar()
		{
			//IL_0031: Expected I, but got I8
			IThumbBar* p = m_spThumbBar.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 215u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 32)))((nint)p);
		}

		internal unsafe ThumbBar(IThumbBar* thumbBar)
		{
			try
			{
				m_spThumbBar = new(thumbBar);
			}
            catch
            {
				m_spThumbBar.Dispose();
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
				m_spThumbBar.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
