using Microsoft.Iris;

namespace Microsoft.Zune.Subscription
{
	public class SubscriptionDataProviderQuery : DataProviderQuery
	{
		internal SubscriptionDataProviderQuery(object queryTypeCookie)
			: base(queryTypeCookie)
		{
		}

		protected override void BeginExecute()
		{
			string text = (string)GetProperty("FeedUrl");
			string text2 = (string)GetProperty("Sort");
			string serviceId = (string)GetProperty("ServiceID");
			if (Result != null)
			{
				VirtualSubscriptionEpisodeList virtualSubscriptionEpisodeList = (VirtualSubscriptionEpisodeList)((SubscriptionDataProviderQueryResult)Result).GetProperty("Items");
				if (null != virtualSubscriptionEpisodeList && virtualSubscriptionEpisodeList.FeedUrl == text)
				{
					virtualSubscriptionEpisodeList.Sort(text2);
					FirePropertyChanged("Result");
				}
				else
				{
					((SubscriptionDataProviderQueryResult)Result).OnDispose();
					Result = new SubscriptionDataProviderQueryResult(this, ResultTypeCookie, text, serviceId, text2);
				}
			}
			else
			{
				Result = new SubscriptionDataProviderQueryResult(this, ResultTypeCookie, text, serviceId, text2);
			}
		}

		protected override void OnDispose()
		{
			if (Result != null)
			{
				((SubscriptionDataProviderQueryResult)Result).OnDispose();
			}
			Result = null;
		}
	}
}
