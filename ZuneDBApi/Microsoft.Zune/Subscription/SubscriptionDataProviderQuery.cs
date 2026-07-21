using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

public class SubscriptionDataProviderQuery : DataProviderQuery
{
    internal SubscriptionDataProviderQuery(object typeCookie)
        : base(typeCookie)
    {
    }

    protected override void BeginExecute()
    {
        Status = DataProviderQueryStatus.Error;
    }

    protected override void OnDispose()
    {
    }
}
