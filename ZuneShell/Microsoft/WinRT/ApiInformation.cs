#if WINDOWS8

using Microsoft.Win32;
using System;

namespace Microsoft.WinRT
{
    /// <summary>
    /// Polyfills for <c>Windows.Foundation.Metadata.ApiInformation</c>
    /// </summary>
    internal static class ApiInformation
    {
        private static readonly Version Win8Version = new(6, 2, 0, 0);
        private static readonly Version Win81Version = new(6, 3, 0, 0);
        private static readonly Version Win10Version = new(10, 0, 0, 0);

        /// <summary>
        /// Returns <see langword="true"/> or <see langword="false"/> to indicate whether a specified type is present.
        /// </summary>
        /// <param name="typeName">
        /// The namespace-qualified name of the type.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the specified type is present; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsTypePresent(string typeName)
        {
            if (Environment.OSVersion.Version >= Win10Version)
            {
                return Windows.Foundation.Metadata.ApiInformation.IsTypePresent(typeName);
            }
            else
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\WindowsRuntime\ActivatableClassId\" + typeName, false);
                return key != null;
            }
        }
    }
}

#endif
