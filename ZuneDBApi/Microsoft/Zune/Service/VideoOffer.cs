using System;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.Iris;

namespace Microsoft.Zune.Service
{
	public class VideoOffer : Offer
	{
		private string m_previewImageUrl;

		private string m_productionCompany;

		private Image m_previewImage;

		private string m_genre;

		private string m_seriesTitle;

		private int m_seasonNumber;

		private int m_episodeNumber;

		private int m_releaseYear;

		private bool m_isHD;

		private bool m_isRental;

		private bool m_isStream;

		private bool m_isMusicVideo;

		private bool m_isSeasonPurchase;

		private DateTime? m_expirationDate;

		private Guid m_albumId;

		public DateTime? ExpirationDate => m_expirationDate;

		public bool IsSeasonPurchase
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_isSeasonPurchase;
			}
		}

		public bool IsMusicVideo
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_isMusicVideo;
			}
		}

		public bool IsStream
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_isStream;
			}
		}

		public bool IsRental
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_isRental;
			}
		}

		public bool IsHD
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_isHD;
			}
		}

		public string Genre => m_genre;

		public int EpisodeNumber => m_episodeNumber;

		public int SeasonNumber => m_seasonNumber;

		public string SeriesTitle => m_seriesTitle;

		public int ReleaseYear => m_releaseYear;

		public string ProductionCompany => m_productionCompany;

		public Image PreviewImage
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

		public Guid AlbumId => m_albumId;

		public VideoOffer(Guid id, string title, string seriesTitle, int seasonNumber, int episodeNumber, string artist, Guid albumId, string genre, int releaseYear, string previewImageUrl, string productionCompany, PriceInfo priceInfo, [MarshalAs(UnmanagedType.U1)] bool isHD, [MarshalAs(UnmanagedType.U1)] bool isRental, [MarshalAs(UnmanagedType.U1)] bool isStream, [MarshalAs(UnmanagedType.U1)] bool isMusicVideo, [MarshalAs(UnmanagedType.U1)] bool isSeasonPurchase, [MarshalAs(UnmanagedType.U1)] bool previouslyPurchased, [MarshalAs(UnmanagedType.U1)] bool inCollection, string expirationDate)
			: base(id, title, artist, priceInfo, isMP3: false, previouslyPurchased, inCollection, null)
		{
			m_previewImageUrl = previewImageUrl;
			m_productionCompany = productionCompany;
			m_previewImage = null;
			m_genre = genre;
			m_seriesTitle = seriesTitle;
			m_seasonNumber = seasonNumber;
			m_episodeNumber = episodeNumber;
			m_releaseYear = releaseYear;
			m_isHD = isHD;
			m_isRental = isRental;
			m_isStream = isStream;
			m_isMusicVideo = isMusicVideo;
			m_isSeasonPurchase = isSeasonPurchase;
			m_albumId = albumId;
			if (!string.IsNullOrEmpty(expirationDate))
			{
				try
				{
					DateTime value = XmlConvert.ToDateTime(expirationDate, XmlDateTimeSerializationMode.Utc);
					DateTime? dateTime = (m_expirationDate = value);
				}
				catch (FormatException)
				{
				}
			}
		}
	}
}
