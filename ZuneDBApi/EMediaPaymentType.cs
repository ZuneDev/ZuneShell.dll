using System.Runtime.CompilerServices;
using Microsoft.VisualC;

[DebugInfoInPDB]
[MiscellaneousBits(64)]
[NativeCppClass]
internal enum EMediaPaymentType
{
    Unknown = -1,
    CreditCard = 1,
    Points = 3,
    Token = 4,
}
