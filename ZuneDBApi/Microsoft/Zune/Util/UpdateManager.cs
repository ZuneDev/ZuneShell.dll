using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util
{
	public class UpdateManager : IDisposable
	{
		private readonly CComPtrMgd<IUpdateManager> m_spUpdateManager;

		private static UpdateManager sm_updateManager = null;

		private static object sm_lock = new object();

		public static UpdateManager Instance
		{
			get
			{
				if (sm_updateManager == null)
				{
					lock (sm_lock)
                    {
						if (sm_updateManager == null)
						{
							UpdateManager updateManager = new();
							Thread.MemoryBarrier();
							sm_updateManager = updateManager;
						}
					}
				}
				return sm_updateManager;
			}
		}

		public unsafe void BeginUpdateCheck(UpdateProgressHandler updateProgressHandler)
		{
			//IL_0003: Expected I, but got I8
			//IL_001c: Expected I, but got I8
			//IL_0068: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			//IL_0092: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			IUpdateManager* ptr = null;
			UpdateProxy* ptr2 = (UpdateProxy*)Module.@new(24uL);
			UpdateProxy* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EUtil_002EUpdateProxy_002E_007Bctor_007D(ptr2, updateProgressHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr2);
				throw;
			}
			UpdateProxy* ptr4 = ptr3;
			lock (sm_lock)
            {
				try
                {
					if (Module.GetSingleton(Module.GUID_UpdateProxy, (void**)(&ptr)) >= 0)
					{
						m_spUpdateManager.op_Assign(ptr);
						IUpdateManager* p = m_spUpdateManager.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUpdateProgress*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, (IUpdateProgress*)ptr3);
					}
				}
                finally
                {
					if (0L != (nint)ptr4)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr4 + 16)))((nint)ptr4);
					}
					if (0L != (nint)ptr)
					{
						IUpdateManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
						ptr = null;
					}
				}
            }
		}

		public unsafe void CancelUpdateCheck()
		{
            lock (sm_lock)
            {
				try
                {
					CComPtrMgd<IUpdateManager> spUpdateManager = m_spUpdateManager;
					IUpdateManager* p = spUpdateManager.p;
					if (0L != (nint)p)
					{
						IUpdateManager* p2 = spUpdateManager.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p2 + 32)))((nint)p2);
					}
				}
                finally
				{
					m_spUpdateManager.Release();
				}
            }
		}

		public unsafe void InstallUpdate(UpdateProgressHandler updateProgressHandler)
		{
			//IL_0019: Expected I, but got I8
			//IL_005d: Expected I, but got I8
			//IL_006b: Expected I, but got I8
			//IL_00ad: Expected I, but got I8
			UpdateProxy* ptr = (UpdateProxy*)Module.@new(24uL);
			UpdateProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002EUpdateProxy_002E_007Bctor_007D(ptr, updateProgressHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			UpdateProxy* ptr3 = ptr2;
            lock (sm_lock)
            {
				try
				{
					IUpdateManager* p = m_spUpdateManager.p;
					if (0L == (nint)p)
					{
						CComPtrNtv<IUpdateManager> cComPtrNtv_003CIUpdateManager_003E = new();
						try
						{
							if (Module.GetSingleton(Module.GUID_UpdateProxy, (void**)(cComPtrNtv_003CIUpdateManager_003E.p)) >= 0)
							{
								IUpdateManager* ptr4 = (IUpdateManager*)(*(ulong*)(cComPtrNtv_003CIUpdateManager_003E.p));
								m_spUpdateManager.op_Assign((IUpdateManager*)(*(ulong*)(cComPtrNtv_003CIUpdateManager_003E.p)));
							}
						}
						finally
						{
							cComPtrNtv_003CIUpdateManager_003E.Dispose();
						}
					}
					CComPtrMgd<IUpdateManager> spUpdateManager = m_spUpdateManager;
					IUpdateManager* p2 = spUpdateManager.p;
					if (0L != (nint)p2)
					{
						IUpdateManager* p3 = spUpdateManager.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUpdateProgress*, int>)(*(ulong*)(*(long*)p3 + 40)))((nint)p3, (IUpdateProgress*)ptr2);
					}
				}
				finally
				{
					m_spUpdateManager.Release();
				}
			}
		}

		private UpdateManager()
		{
			CComPtrMgd<IUpdateManager> spUpdateManager = new();
			try
			{
				m_spUpdateManager = spUpdateManager;
			}
			catch
			{
				//try-fault
				((IDisposable)m_spUpdateManager).Dispose();
				throw;
			}
		}

		private void _007EUpdateManager()
		{
			m_spUpdateManager.Release();
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EUpdateManager();
				}
				finally
				{
					m_spUpdateManager.Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
