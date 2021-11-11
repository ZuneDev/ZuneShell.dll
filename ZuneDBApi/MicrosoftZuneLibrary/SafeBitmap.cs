using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace MicrosoftZuneLibrary
{
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public class SafeBitmap : SafeHandleZeroOrMinusOneIsInvalid
	{
		public unsafe SafeBitmap(HBITMAP__* hBitmap)
			: base(ownsHandle: true)
		{
			try
			{
				IntPtr intPtr = (handle = (IntPtr)hBitmap);
			}
			catch
			{
				//try-fault
				base.Dispose(disposing: true);
				throw;
			}
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[return: MarshalAs(UnmanagedType.U1)]
		protected unsafe override bool ReleaseHandle()
		{
			return (_003CModule_003E.DeleteObject((void*)handle) != 0) ? true : false;
		}
	}
}
