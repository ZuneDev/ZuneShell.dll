using System;

namespace Microsoft.Zune.Util;

[Flags]
public enum ETaskbarPlayerState
{
    PS_Stopped = 1,
    PS_Playing = 2,
    PS_Paused = 4,
    PS_Minimized = 8,
    PS_RatingNotRated = 16,
    PS_RatingLoveIt = 32,
    PS_RatingHateIt = 64,
    PS_CanPlay = 256,
    PS_CanPause = 512,
    PS_CanForward = 1024,
    PS_CanBack = 2048,
    PS_CanRate = 4096,
}
