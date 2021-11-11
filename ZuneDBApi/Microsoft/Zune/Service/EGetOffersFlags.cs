using System;

namespace Microsoft.Zune.Service
{
	[Flags]
	public enum EGetOffersFlags
	{
		SeasonPurchase = 0x2,
		None = 0x0,
		SubscriptionFreeTracks = 0x1
	}
}
