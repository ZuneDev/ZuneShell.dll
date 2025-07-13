using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Service;

public class AppOffer : Offer
{
	private string m_publisher;

	private string m_developer;

	private string m_genre;

	private string m_version;

	private string m_previewImageUrl;

	private string m_ratingImageUrl;

	private Image m_previewImage;

	private Image m_ratingImage;

	private DateTime m_releaseDateTime;

	private EClientTypeFlags m_clientTypes;

	private bool m_isTrialPurchase;

	public bool IsTrialPurchase
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_isTrialPurchase;
		}
	}

	public Image RatingImage
	{
		get
		{
			if (m_ratingImage == null && m_ratingImageUrl != null)
			{
				m_ratingImage = new Image(m_ratingImageUrl);
			}
			return m_ratingImage;
		}
	}

	public Image CoverArt
	{
		get
		{
			if (m_previewImage == null && m_previewImageUrl != null)
			{
				m_previewImage = new Image(m_previewImageUrl);
			}
			return m_previewImage;
		}
	}

	public DateTime ReleaseDate => m_releaseDateTime;

	public string Version => m_version;

	public string Genre => m_genre;

	public string Developer => m_developer;

	public string Publisher => m_publisher;

	public AppOffer(Guid id, string title, string publisher, string developer, string genre, string version, string previewImageUrl, string ratingImageUrl, PriceInfo priceInfo, DateTime releaseDateTime, [MarshalAs(UnmanagedType.U1)] bool previouslyPurchased, [MarshalAs(UnmanagedType.U1)] bool inCollection, [MarshalAs(UnmanagedType.U1)] bool isTrialPurchase)
		: base(id, title, null, priceInfo, isMP3: false, previouslyPurchased, inCollection, null)
	{
		m_publisher = publisher;
		m_developer = developer;
		m_genre = genre;
		m_version = version;
		m_previewImageUrl = previewImageUrl;
		m_ratingImageUrl = ratingImageUrl;
		m_releaseDateTime = releaseDateTime;
		m_previewImage = null;
		m_isTrialPurchase = isTrialPurchase;
	}
}
