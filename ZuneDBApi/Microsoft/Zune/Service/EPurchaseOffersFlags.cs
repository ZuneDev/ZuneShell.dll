using System;

namespace Microsoft.Zune.Service
{
	[Flags]
	public enum EPurchaseOffersFlags
	{
		PurchaseHD = 0x1,
		PurchaseTrials = 0x8,
		StreamVideos = 0x4,
		RentVideos = 0x2,
		None = 0x0
	}
}
