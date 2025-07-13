using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public delegate void OnSetDriveLockedForBurningHandler([MarshalAs(UnmanagedType.U1)] bool fLocked);
