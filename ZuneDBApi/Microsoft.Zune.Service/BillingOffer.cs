using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class BillingOffer
{
	private ulong m_id;

	private EBillingOfferType m_eBillingOfferType;

	private string m_offerName;

	private string m_displayPrice;

	private uint m_points;

	private float m_price;

	private bool m_taxes;

	private bool m_trial;

	public bool Trial
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_trial;
		}
	}

	public bool Taxes
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_taxes;
		}
	}

	public string DisplayPrice => m_displayPrice;

	public float Price => m_price;

	public uint Points => m_points;

	public string OfferName => m_offerName;

	public EBillingOfferType OfferType => m_eBillingOfferType;

	public ulong Id => m_id;

	public BillingOffer(ulong id, EBillingOfferType eBillingOfferType, string offerName)
	{
		Initialize(id, eBillingOfferType, offerName, null, 0u, 0f, taxes: false, trial: false);
	}

	public BillingOffer(ulong id, EBillingOfferType eBillingOfferType, string offerName, string displayPrice, uint points, float price, [MarshalAs(UnmanagedType.U1)] bool taxes, [MarshalAs(UnmanagedType.U1)] bool trial)
	{
		Initialize(id, eBillingOfferType, offerName, displayPrice, points, price, taxes, trial);
	}

	private void Initialize(ulong id, EBillingOfferType eBillingOfferType, string offerName, string displayPrice, uint points, float price, [MarshalAs(UnmanagedType.U1)] bool taxes, [MarshalAs(UnmanagedType.U1)] bool trial)
	{
		m_id = id;
		m_eBillingOfferType = eBillingOfferType;
		m_offerName = offerName;
		m_displayPrice = displayPrice;
		m_points = points;
		m_price = price;
		m_taxes = taxes;
		m_trial = trial;
		if (offerName != null)
		{
			m_offerName = m_offerName.Trim();
		}
	}
}
