namespace System.Runtime.InteropServices
{
    internal static class MarshalEx
    {
        public static int SizeOf<T>()
        {
#if NETSTANDARD1_0_OR_GREATER
            return Marshal.SizeOf<T>();
#else
            return Marshal.SizeOf(typeof(T));
#endif
        }

        public static T PtrToStructure<T>(IntPtr ptr)
        {
#if NETSTANDARD1_0_OR_GREATER
            return Marshal.PtrToStructure<T>(ptr);
#else
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
#endif
        }
    }
}
