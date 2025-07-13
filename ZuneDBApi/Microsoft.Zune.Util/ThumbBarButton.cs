using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util;

public class ThumbBarButton : IDisposable
{
	private readonly CComPtrMgd_003CIThumbBarButton_003E m_spButton;

	public unsafe bool ShowBackground
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001d: Expected I, but got I8
			int num = 0;
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 104)))((nint)p, &num);
			return num == 1;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			//IL_0023: Expected I, but got I8
			int num = (value ? 1 : 0);
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, num);
		}
	}

	public unsafe bool IsHidden
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001d: Expected I, but got I8
			int num = 0;
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, &num);
			return num == 1;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			//IL_0023: Expected I, but got I8
			int num = (value ? 1 : 0);
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)p + 96)))((nint)p, num);
		}
	}

	public unsafe bool IsEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001d: Expected I, but got I8
			int num = 0;
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 72)))((nint)p, &num);
			return num == 1;
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			//IL_0023: Expected I, but got I8
			int num = (value ? 1 : 0);
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)p + 80)))((nint)p, num);
		}
	}

	public unsafe ThumbBarButtonIcons Icon
	{
		get
		{
			//IL_001e: Expected I, but got I8
			EThumbBarButtonIcons result = (EThumbBarButtonIcons)10;
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EThumbBarButtonIcons*, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, &result);
			return (ThumbBarButtonIcons)result;
		}
		set
		{
			//IL_001a: Expected I, but got I8
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EThumbBarButtonIcons, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, (EThumbBarButtonIcons)value);
		}
	}

	public unsafe string Tooltip
	{
		get
		{
			//IL_0003: Expected I, but got I8
			//IL_001f: Expected I, but got I8
			ushort* ptr = null;
			object result = null;
			IThumbBarButton* p = m_spButton.p;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr) >= 0)
			{
				result = new string((char*)ptr);
			}
			global::_003CModule_003E.SysFreeString(ptr);
			return (string)result;
		}
		set
		{
			//IL_0023: Expected I, but got I8
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(value)))
			{
				IThumbBarButton* p = m_spButton.p;
				long num = *(long*)p + 48;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)p, ptr);
			}
		}
	}

	public unsafe uint UniqueID
	{
		get
		{
			//IL_001d: Expected I, but got I8
			uint result = 0u;
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &result);
			return result;
		}
		set
		{
			//IL_001a: Expected I, but got I8
			IThumbBarButton* p = m_spButton.p;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, value);
		}
	}

	internal unsafe ThumbBarButton(IThumbBarButton* pButton)
	{
		CComPtrMgd_003CIThumbBarButton_003E spButton = new CComPtrMgd_003CIThumbBarButton_003E();
		try
		{
			m_spButton = spButton;
			base._002Ector();
			m_spButton.op_Assign(pButton);
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)m_spButton).Dispose();
			throw;
		}
	}

	public void _007EThumbBarButton()
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
				((IDisposable)m_spButton).Dispose();
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
