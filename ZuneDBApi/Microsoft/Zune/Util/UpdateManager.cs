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
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_updateManager == null)
						{
							UpdateManager updateManager = new();
							Thread.MemoryBarrier();
							sm_updateManager = updateManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
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
			try
			{
				Monitor.Enter(sm_lock);
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
				Monitor.Exit(sm_lock);
			}
		}

		public unsafe void CancelUpdateCheck()
		{
			//IL_002f: Expected I, but got I8
			try
			{
				Monitor.Enter(sm_lock);
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
				Monitor.Exit(sm_lock);
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
			try
			{
				Monitor.Enter(sm_lock);
				IUpdateManager* p = m_spUpdateManager.p;
				if (0L == (nint)p)
				{
					CComPtrNtv_003CIUpdateManager_003E cComPtrNtv_003CIUpdateManager_003E;
					*(long*)(&cComPtrNtv_003CIUpdateManager_003E) = 0L;
					try
					{
						if (Module.GetSingleton(Module.GUID_UpdateProxy, (void**)(&cComPtrNtv_003CIUpdateManager_003E)) >= 0)
						{
							IUpdateManager* ptr4 = (IUpdateManager*)(*(ulong*)(&cComPtrNtv_003CIUpdateManager_003E));
							m_spUpdateManager.op_Assign((IUpdateManager*)(*(ulong*)(&cComPtrNtv_003CIUpdateManager_003E)));
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIUpdateManager_003E*, void>)(&Module.CComPtrNtv_003CIUpdateManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIUpdateManager_003E);
						throw;
					}
					Module.CComPtrNtv_003CIUpdateManager_003E_002ERelease(&cComPtrNtv_003CIUpdateManager_003E);
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
				Module.SafeRelease_003Cclass_0020Microsoft_003A_003AZune_003A_003AUtil_003A_003AUpdateProxy_003E(&ptr3);
				m_spUpdateManager.Release();
				Monitor.Exit(sm_lock);
			}
		}

		private UpdateManager()
		{
			CComPtrMgd<IUpdateManager> spUpdateManager = new CComPtrMgd<IUpdateManager>();
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
					((IDisposable)m_spUpdateManager).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
