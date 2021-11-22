using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class InteropNotifications : IDisposable
	{
		private bool m_fDisposed = false;

		private bool m_fAdvised = false;

		private ulong m_AdviseCookie;

		internal OnShowErrorDialogHandler m_ShowErrorDialogHandler;

		[SpecialName]
		public virtual event OnShowErrorDialogHandler ShowErrorDialog
		{
			add
			{
				m_ShowErrorDialogHandler = (OnShowErrorDialogHandler)Delegate.Combine(m_ShowErrorDialogHandler, value);
			}
			remove
			{
				m_ShowErrorDialogHandler = (OnShowErrorDialogHandler)Delegate.Remove(m_ShowErrorDialogHandler, value);
			}
		}

		public unsafe InteropNotifications()
		{
			//IL_002d: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			NativeInteropNotifications* ptr = (NativeInteropNotifications*)Module.@new(24uL);
			NativeInteropNotifications* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ENativeInteropNotifications_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			IInteropNotify* ptr3;
			if (ptr2 == null || ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr2)))((nint)ptr2, (_GUID*)Unsafe.AsPointer(ref Module._GUID_3fb2d757_8ddb_46a9_9dd2_3424e2903e46), (void**)(&ptr3)) < 0)
			{
				return;
			}
			fixed (ulong* ptr4 = &m_AdviseCookie)
			{
				try
				{
					if (Module.InteropNotifyAdvise(ptr3, ptr4) >= 0)
					{
						m_fAdvised = true;
					}
					IInteropNotify* intPtr = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
				catch
				{
					//try-fault
					ptr4 = null;
					throw;
				}
			}
		}

		private void _007EInteropNotifications()
		{
			_0021InteropNotifications();
		}

		private void _0021InteropNotifications()
		{
			if (!m_fDisposed)
			{
				m_fDisposed = true;
				if (m_fAdvised)
				{
					Module.InteropNotifyUnAdvise(m_AdviseCookie);
					m_fAdvised = false;
				}
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021InteropNotifications();
				return;
			}
			try
			{
				_0021InteropNotifications();
			}
			finally
			{
				base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~InteropNotifications()
		{
			Dispose(false);
		}
	}
}
