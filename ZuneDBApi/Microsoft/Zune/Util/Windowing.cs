using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Zune.Util
{
	public class Windowing
	{
		public unsafe static void ForceSetForegroundWindow(IntPtr hwnd)
		{
			//IL_000a: Expected I4, but got I8
			INPUT tagINPUT;
            // IL initblk instruction
            Unsafe.InitBlock(ref Unsafe.AddByteOffset(ref tagINPUT, 8), 0, 32);
			*(int*)(&tagINPUT) = 1;
            Unsafe.As<INPUT, short>(ref Unsafe.AddByteOffset(ref tagINPUT, 8)) = 0;
			Module.SendInput(1u, &tagINPUT, 40);
			Module.SetForegroundWindow((HWND*)hwnd.ToPointer());
		}
	}
}
