// C# adaptation of original gcroot template
// https://github.com/icestudent/vc-19-changes/blob/e9f49e36a28463963e8199ff7bc14222910598df/msclr/gcroot.h

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.VisualC;

internal unsafe struct gcroot<T> : IDisposable
{
    // Don't let anyone copy the handle value directly, or make a copy
    // by taking the address of this object and pointing to it from
    // somewhere else.  The root will be freed when the dtor of this
    // object gets called, and anyone pointing to it still will
    // cause serious harm to the Garbage Collector.
    private void* _handle;

    // always allocate a new handle during construction (see above)
    //
    // Initializes to a NULL handle, which is always safe
    [DebuggerStepThrough]
    [SecuritySafeCritical]
    public gcroot()
    {
        _handle = __GCHANDLE_TO_VOIDPTR(GCHandle.Alloc(null));
    }

    // this can't be T& here because & does not yet work on managed types
    // (T should be a pointer anyway).
    //
    public gcroot(T t)
    {
        _handle = __GCHANDLE_TO_VOIDPTR(GCHandle.Alloc(t));
    }

    public gcroot(gcroot<T> r)
    {
        // don't copy a handle, copy what it points to (see above)
        _handle = __GCHANDLE_TO_VOIDPTR(
                                        GCHandle.Alloc(
                                                __VOIDPTR_TO_GCHANDLE(r._handle).Target));
    }

    // Since C++ objects and handles are allocated 1-to-1, we can
    // free the handle when the object is destroyed
    //
    [DebuggerStepThrough]
    [SecurityCritical]
    public void Dispose()
    {
        GCHandle g = __VOIDPTR_TO_GCHANDLE(_handle);
        g.Free();
        _handle = (void*)0; // should fail if reconstituted
    }

    [DebuggerStepThrough]
    [SecurityCritical]
    public gcroot<T> Assign(T t)
    {
        // no need to check for valid handle; was allocated in ctor
        var gcHandle = __VOIDPTR_TO_GCHANDLE(_handle);
        gcHandle.Target = t;
        return this;
    }

    public static explicit operator T(gcroot<T> r) => (T)__VOIDPTR_TO_GCHANDLE(r._handle).Target;

    public readonly T Get() => (T)this;

    public static void Create(gcroot<T>* p) => *p = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void* __GCHANDLE_TO_VOIDPTR(GCHandle handle) => ((IntPtr)handle).ToPointer();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GCHandle __VOIDPTR_TO_GCHANDLE(void* ptr) => (GCHandle)new IntPtr(ptr);
}
