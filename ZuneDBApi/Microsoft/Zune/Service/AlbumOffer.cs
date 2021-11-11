using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Service
{
	public class AlbumOffer : Offer
	{
		private Image m_coverArt;

		private string m_coverArtUrl;

		private string m_genre;

		private int m_releaseYear;

		private bool m_premium;

		public bool Premium
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_premium;
			}
		}

		public string Genre => m_genre;

		public int ReleaseYear => m_releaseYear;

		public Image CoverArt
		{
			get
			{
				if (m_coverArt == null && m_coverArtUrl != (string)null)
				{
					m_coverArt = new Image(m_coverArtUrl);
				}
				return m_coverArt;
			}
		}

		public AlbumOffer(Guid id, string title, string artist, string genre, int releaseYear, string coverArtUrl, PriceInfo priceInfo, [MarshalAs(UnmanagedType.U1)] bool isMP3, [MarshalAs(UnmanagedType.U1)] bool premium, [MarshalAs(UnmanagedType.U1)] bool previouslyPurchased, [MarshalAs(UnmanagedType.U1)] bool inCollection, string serviceContext)
			: base(id, title, artist, priceInfo, isMP3, previouslyPurchased, inCollection, serviceContext)
		{
			m_coverArtUrl = coverArtUrl;
			m_coverArt = null;
			m_genre = genre;
			m_releaseYear = releaseYear;
			m_premium = premium;
		}
	}
}
