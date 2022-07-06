using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyCompany("Microsoft Corporation")]
//[assembly: AssemblyKeyFile("e:\\temp\\471135\\public.amd64fre\\internal\\strongnamekeys\\fake\\36MSApp1024.snk")]
[assembly: ComVisible(false)]
[assembly: AssemblyProduct("Microsoft (R) Windows (R) Operating System")]
[assembly: AssemblyCopyright("Copyright (c) Microsoft Corporation. All rights reserved.")]
//[assembly: AssemblyDelaySign(true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

#if OPENZUNE
[assembly: AssemblyFileVersion("5.0.0.0")]
[assembly: AssemblyVersion("5.0.0.0")]
#else
[assembly: AssemblyFileVersion("4.8.2345.0")]
[assembly: AssemblyVersion("4.7.0.0")]
#endif