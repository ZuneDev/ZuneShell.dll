using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;
using Microsoft.Zune.Util;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class AppInitializationSequencer
	{
		public delegate void Phase3CompleteCallback(int hr);

		private CorePhase2ReadyCallback m_GcCorePhase2ReadyCallback;

		public AppInitializationSequencer(CorePhase2ReadyCallback corePhase2ReadyCallback)
		{
			m_GcCorePhase2ReadyCallback = corePhase2ReadyCallback;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool UIReady()
		{
			//IL_0024: Expected I, but got I8
			//IL_0043: Expected I, but got I8
			AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)Module.@new(24uL);
			AsyncCallbackWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, CorePhase2Ready));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			int num2;
			if (ptr2 != null)
			{
				int num = Module.ZuneLibraryExports_002EPhase3Initialization((IAsyncCallback*)ptr2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
				if (num >= 0)
				{
					num2 = 1;
					goto IL_004d;
				}
			}
			num2 = 0;
			goto IL_004d;
			IL_004d:
			return (byte)num2 != 0;
		}

		public void CorePhase2Ready(HRESULT hr)
		{
			object[] array = new object[2] { hr.hr, null };
			byte b = (byte)((hr.hr >= 0) ? 1 : 0);
			array[1] = b != 0;
			Application.DeferredInvoke(CorePhase2ReadyMarshalled, array, DeferredInvokePriority.Normal);
		}

		private void CorePhase2ReadyMarshalled(object args)
		{
			object[] array = (object[])args;
			m_GcCorePhase2ReadyCallback((int)array[0], (bool)array[1]);
		}
	}
}
