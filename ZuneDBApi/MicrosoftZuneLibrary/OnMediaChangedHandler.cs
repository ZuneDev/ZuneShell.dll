using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public delegate void OnMediaChangedHandler([MarshalAs(UnmanagedType.U2)] char driveLetter, [MarshalAs(UnmanagedType.U1)] bool fMediaArrived);
