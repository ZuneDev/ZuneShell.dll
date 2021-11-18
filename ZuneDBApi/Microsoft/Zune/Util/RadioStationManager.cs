using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util
{
	public class RadioStationManager : IDisposable
	{
		private unsafe IRadioStationManager* m_pRadioStationManager = null;

		private static RadioStationManager sm_radioStationManager = null;

		private static object sm_lock = new object();

		public static RadioStationManager Instance
		{
			get
			{
				if (sm_radioStationManager == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_radioStationManager == null)
						{
							RadioStationManager radioStationManager = new RadioStationManager();
							Thread.MemoryBarrier();
							sm_radioStationManager = radioStationManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_radioStationManager;
			}
		}

		private void _007ERadioStationManager()
		{
			_0021RadioStationManager();
		}

		private unsafe void _0021RadioStationManager()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IRadioStationManager* pRadioStationManager = m_pRadioStationManager;
			if (0L != (nint)pRadioStationManager)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pRadioStationManager + 16)))((nint)pRadioStationManager);
				m_pRadioStationManager = null;
			}
		}

		public unsafe RadioPlaylist GetRadioPlaylist(string uri)
		{
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			IRadioStationManager* ptr = null;
			IRadioPlaylist* ptr2 = null;
			RadioPlaylist result = null;
			fixed (char* uriPtr = uri.ToCharArray())
			{
				ushort* ptr3 = (ushort*)uriPtr;
				int singleton = Module.GetSingleton((_GUID)Module._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr));
				if (singleton >= 0)
				{
					long num = *(long*)ptr + 24;
					IRadioStationManager* intPtr = ptr;
					singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IRadioPlaylist**, int>)(*(ulong*)num))((nint)intPtr, ptr3, &ptr2);
					if (singleton >= 0)
					{
						result = new RadioPlaylist(ptr2);
					}
				}
				if (0L != (nint)ptr)
				{
					IRadioStationManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					ptr = null;
				}
				if (0L != (nint)ptr2)
				{
					IRadioPlaylist* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
				}
				return result;
			}
		}

		public unsafe void AddStation(string title, string sourceUrl, string imageUrl, RadioStationProgressHandler radioStationProgressHandler)
		{
			//IL_001a: Expected I, but got I8
			//IL_0060: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0086: Expected I, but got I8
			RadioStationProxy* ptr = (RadioStationProxy*)Module.@new(24uL);
			RadioStationProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002ERadioStationProxy_002E_007Bctor_007D(ptr, radioStationProgressHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			fixed (char* titlePtr = title.ToCharArray())
			{
				ushort* ptr4 = (ushort*)titlePtr;
				fixed (char* sourceUrlPtr = sourceUrl.ToCharArray())
				{
					ushort* ptr5 = (ushort*)sourceUrlPtr;
					fixed (char* imageUrlPtr = imageUrl.ToCharArray())
					{
						ushort* ptr6 = (ushort*)imageUrlPtr;
						IRadioStationManager* ptr3;
						if (Module.GetSingleton((_GUID)Module._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr3)) >= 0)
						{
							long num = *(long*)ptr3 + 32;
							IRadioStationManager* intPtr = ptr3;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, IAsyncCallback*, int>)(*(ulong*)num))((nint)intPtr, ptr4, ptr5, ptr6, (IAsyncCallback*)ptr2);
						}
						if (0L != (nint)ptr2)
						{
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
						}
						if (0L != (nint)ptr3)
						{
							IRadioStationManager* intPtr2 = ptr3;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
						}
					}
				}
			}
		}

		public unsafe void DeleteStation(string title, RadioStationProgressHandler radioStationProgressHandler)
		{
			//IL_0019: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			RadioStationProxy* ptr = (RadioStationProxy*)Module.@new(24uL);
			RadioStationProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002ERadioStationProxy_002E_007Bctor_007D(ptr, radioStationProgressHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			fixed (char* titlePtr = title.ToCharArray())
			{
				ushort* ptr4 = (ushort*)titlePtr;
				IRadioStationManager* ptr3;
				if (Module.GetSingleton((_GUID)Module._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr3)) >= 0)
				{
					long num = *(long*)ptr3 + 40;
					IRadioStationManager* intPtr = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IAsyncCallback*, int>)(*(ulong*)num))((nint)intPtr, ptr4, (IAsyncCallback*)ptr2);
				}
				if (0L != (nint)ptr2)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
				}
				if (0L != (nint)ptr3)
				{
					IRadioStationManager* intPtr2 = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
			}
		}

		private unsafe RadioStationManager()
		{
		}//IL_0008: Expected I, but got I8


		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021RadioStationManager();
				return;
			}
			try
			{
				_0021RadioStationManager();
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

		~RadioStationManager()
		{
			Dispose(false);
		}
	}
}
