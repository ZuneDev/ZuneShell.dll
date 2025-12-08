using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct IService
{
    [FieldOffset(384)]
    public delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int> _method1;
}
