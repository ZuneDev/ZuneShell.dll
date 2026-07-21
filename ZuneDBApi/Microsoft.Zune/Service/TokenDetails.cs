using System;

namespace Microsoft.Zune.Service;

public class TokenDetails : PaymentInstrument
{
    private string m_offerName;
    private string m_sellText;
    private ETokenType m_eTokenType;
    private EPurchaseOfferType m_ePurchaseOfferType;
    private Guid m_mediaId;
    private Guid m_mediaOfferId;
    private ulong m_billingOfferId;

    public Guid MediaOfferId => m_mediaOfferId;

    public Guid MediaId => m_mediaId;

    public EPurchaseOfferType PurchaseOfferType => m_ePurchaseOfferType;

    public string SellText => m_sellText;

    public string OfferName => m_offerName;

    public ulong BillingOfferId => m_billingOfferId;

    public ETokenType TokenType => m_eTokenType;

    public string Token => base.Id;

    internal TokenDetails()
        : base(null, PaymentType.Token)
    {
    }
}
