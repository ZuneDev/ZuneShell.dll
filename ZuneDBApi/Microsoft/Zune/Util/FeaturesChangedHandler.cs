using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public delegate void FeaturesChangedHandler([MarshalAs(UnmanagedType.U1)] bool featuresHaveChanged);
}
