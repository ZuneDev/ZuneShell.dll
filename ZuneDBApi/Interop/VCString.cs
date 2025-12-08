using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZuneDBApi.Interop;

/// <summary>
/// Encodes and decodes text in formats equivalent to Visual C++ and C++/CLI.
/// </summary>
internal static class VCString
{
    public static unsafe ushort[] ToWide(string str)
    {
        ushort[] array = new ushort[str.Length + 1];

        var chars = str.ToCharArray();

        fixed (ushort* ptr = array)
        {
            Marshal.Copy(chars, 0, new IntPtr(ptr), str.Length);
        }

        array[str.Length] = 0;

        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ushort* AsPtr(ref ushort[] str) => (ushort*)Unsafe.AsPointer(ref str);
}
