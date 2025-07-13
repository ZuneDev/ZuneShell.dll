namespace Microsoft.Zune.Util;

public enum ETaskbarPlayerState
{
	PS_CanRate = 4096,
	PS_CanBack = 2048,
	PS_CanForward = 1024,
	PS_CanPause = 512,
	PS_CanPlay = 256,
	PS_RatingHateIt = 64,
	PS_RatingLoveIt = 32,
	PS_RatingNotRated = 16,
	PS_Minimized = 8,
	PS_Paused = 4,
	PS_Playing = 2,
	PS_Stopped = 1
}
