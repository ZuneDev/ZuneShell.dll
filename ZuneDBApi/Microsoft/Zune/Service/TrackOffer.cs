using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class TrackOffer : Offer
	{
		private int m_trackNumber;

		private string m_album;

		private bool m_subscriptionFree;

		public bool SubscriptionFree
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_subscriptionFree;
			}
		}

		public string Album => m_album;

		public int TrackNumber => m_trackNumber;

		public TrackOffer(Guid id, int trackNumber, string title, string album, string artist, string serviceContext, PriceInfo priceInfo, [MarshalAs(UnmanagedType.U1)] bool isMP3, [MarshalAs(UnmanagedType.U1)] bool previouslyPurchased, [MarshalAs(UnmanagedType.U1)] bool inCollection, [MarshalAs(UnmanagedType.U1)] bool subscriptionFree)
			: base(id, title, artist, priceInfo, isMP3, previouslyPurchased, inCollection, serviceContext)
		{
			m_trackNumber = trackNumber;
			m_album = album;
			m_subscriptionFree = subscriptionFree;
		}
	}
}
