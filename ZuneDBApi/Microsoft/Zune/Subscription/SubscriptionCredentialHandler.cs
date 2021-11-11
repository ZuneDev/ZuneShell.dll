using System.Runtime.InteropServices;

namespace Microsoft.Zune.Subscription
{
	[return: MarshalAs(UnmanagedType.U1)]
	public delegate bool SubscriptionCredentialHandler(SubscriptonCredentialRequestArguments args);
}
