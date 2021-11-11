using System.Runtime.InteropServices;

namespace Microsoft.Zune.UserCredential
{
	[return: MarshalAs(UnmanagedType.U1)]
	public delegate bool UserCredentialHandler(UserCredentialRequestArguments args);
}
