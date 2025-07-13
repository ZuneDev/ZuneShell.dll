using System;

namespace Microsoft.Zune.Service;

[Flags]
public enum EPurchaseOffersFlags
{
	PurchaseHD = 1,
	PurchaseTrials = 8,
	StreamVideos = 4,
	RentVideos = 2,
	None = 0
}
