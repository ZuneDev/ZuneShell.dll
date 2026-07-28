// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.MemoryFonts
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

#if WINDOWS

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune;

internal class Win32MemoryFonts : MemoryFonts
{
    public const uint LOAD_LIBRARY_AS_DATAFILE = 2;
    public const int RT_DATA = 10;

    public override bool TryLoadFromResourceCore(string resourceDllName, string fontResourceName)
    {
        var instanceHandle = IntPtr.Zero;
        try
        {
            instanceHandle = LoadLibraryEx(resourceDllName, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            if (instanceHandle == IntPtr.Zero)
                return false;
                
            var resource = FindResource(instanceHandle, fontResourceName, RT_DATA);
            if (resource == IntPtr.Zero)
                return false;
                
            var resourceHandle = LoadResource(instanceHandle, resource);
            var fontHandle = AddFontMemResourceEx(LockResource(resourceHandle),
                SizeofResource(instanceHandle, resource), IntPtr.Zero, out _);
                
            return resourceHandle != IntPtr.Zero && fontHandle != IntPtr.Zero;
        }
        finally
        {
            if (instanceHandle != IntPtr.Zero)
                FreeLibrary(instanceHandle);
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr LoadLibraryEx(string moduleName, IntPtr reserved, uint flags);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool FreeLibrary(IntPtr instanceHandle);

    [DllImport("kernel32.dll", EntryPoint = "FindResourceW", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindResource(
        IntPtr instanceHandle,
        string resource,
        IntPtr type);

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadResource(IntPtr instanceHandle, IntPtr resourceHandle);

    [DllImport("kernel32.dll")]
    public static extern int SizeofResource(IntPtr instanceHandle, IntPtr resourceHandle);

    [DllImport("kernel32.dll")]
    public static extern IntPtr LockResource(IntPtr resourceHandle);

    [DllImport("gdi32.dll")]
    public static extern IntPtr AddFontMemResourceEx(
        IntPtr fontBuffer,
        int fontButtonSize,
        IntPtr reserved,
        out uint fontsInstalled);
}

#endif