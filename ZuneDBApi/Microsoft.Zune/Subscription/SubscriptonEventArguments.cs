namespace Microsoft.Zune.Subscription;

public class SubscriptonEventArguments
{
    private SubscriptionAction m_Action;
    private string m_SubscriptionTitle;
    private EMediaTypes m_eMediaType;
    private bool m_userInitiated;

    public bool UserInitiated => m_userInitiated;

    public EMediaTypes MediaType => m_eMediaType;

    public string SubscriptionTitle => m_SubscriptionTitle;

    public SubscriptionAction Action => m_Action;

    internal SubscriptonEventArguments(SubscriptionAction action, string subscriptionTitle, EMediaTypes eMediaType, bool userInitiated)
    {
        m_Action = action;
        m_SubscriptionTitle = subscriptionTitle;
        m_eMediaType = eMediaType;
        m_userInitiated = userInitiated;
    }
}
