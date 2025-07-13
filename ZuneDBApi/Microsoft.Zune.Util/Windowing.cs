using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Zune.Util;

public class Windowing
{
	public unsafe static void ForceSetForegroundWindow(IntPtr hwnd)
	{
		//IL_000a: Expected I4, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out tagINPUT tagINPUT);
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlock(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagINPUT, 8), 0, 32);
		*(int*)(&tagINPUT) = 1;
		System.Runtime.CompilerServices.Unsafe.As<tagINPUT, short>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagINPUT, 8)) = 0;
		global::_003CModule_003E.SendInput(1u, &tagINPUT, 40);
		global::_003CModule_003E.SetForegroundWindow((HWND__*)hwnd.ToPointer());
	}
}
