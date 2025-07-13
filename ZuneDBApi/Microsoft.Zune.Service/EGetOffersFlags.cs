using System;

namespace Microsoft.Zune.Service;

[Flags]
public enum EGetOffersFlags
{
	SeasonPurchase = 2,
	None = 0,
	SubscriptionFreeTracks = 1
}
