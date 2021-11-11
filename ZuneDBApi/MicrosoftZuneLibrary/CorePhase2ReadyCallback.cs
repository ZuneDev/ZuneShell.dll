using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public delegate void CorePhase2ReadyCallback(int hr, [MarshalAs(UnmanagedType.U1)] bool success);
}
