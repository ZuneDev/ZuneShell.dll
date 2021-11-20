using System;

namespace Microsoft.Zune.Service
{
	[Flags]
	public enum EPurchaseOffersFlags
	{
		PurchaseTrials = 0x8,
		StreamVideos = 0x4,
		RentVideos = 0x2,
		PurchaseHD = 0x1,
		None = 0x0
	}
}
