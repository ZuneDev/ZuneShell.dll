using Microsoft.Iris;

namespace Microsoft.Zune.Subscription
{
	public class SubscriptionDataProvider
	{
		public static void Register()
		{
			Application.RegisterDataProvider("DynamicRssView", ConstructQuery);
		}

		public static DataProviderQuery ConstructQuery(object queryTypeCookie)
		{
			return new SubscriptionDataProviderQuery(queryTypeCookie);
		}

		private SubscriptionDataProvider()
		{
		}
	}
}
