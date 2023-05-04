using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Permissions;

[assembly: AssemblyCompany("Microsoft Corporation")]
//[assembly: AssemblyKeyFile("e:\\temp\\471135\\public.amd64fre\\internal\\strongnamekeys\\fake\\36MSApp1024.snk")]
[assembly: ComVisible(false)]
[assembly: AssemblyProduct("Microsoft (R) Windows (R) Operating System")]
[assembly: AssemblyCopyright("Copyright (c) Microsoft Corporation. All rights reserved.")]
//[assembly: AssemblyDelaySign(true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

[assembly: AssemblyFileVersion("5.0.0.0")]
[assembly: AssemblyVersion("5.0.0.0")]

// Workaround for CA1416
#if WINDOWS10
[assembly: SupportedOSPlatform("windows10.0.10240")]
#elif WINDOWS8_0_OR_GREATER
[assemby: SupportedOSPlatform("windows8.0.0")]
#endif