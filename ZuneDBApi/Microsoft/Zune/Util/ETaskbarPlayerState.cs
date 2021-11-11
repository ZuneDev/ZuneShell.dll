namespace Microsoft.Zune.Util
{
	public enum ETaskbarPlayerState
	{
		PS_CanRate = 0x1000,
		PS_CanBack = 0x800,
		PS_CanForward = 0x400,
		PS_CanPause = 0x200,
		PS_CanPlay = 0x100,
		PS_RatingHateIt = 0x40,
		PS_RatingLoveIt = 0x20,
		PS_RatingNotRated = 0x10,
		PS_Minimized = 8,
		PS_Paused = 4,
		PS_Playing = 2,
		PS_Stopped = 1
	}
}
