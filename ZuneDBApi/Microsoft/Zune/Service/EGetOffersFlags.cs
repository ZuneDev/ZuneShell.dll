using System;

namespace Microsoft.Zune.Service
{
	[Flags]
	public enum EGetOffersFlags
	{
		None = 0,
		SubscriptionFreeTracks = 1,
		SeasonPurchase = 2
	}
}
