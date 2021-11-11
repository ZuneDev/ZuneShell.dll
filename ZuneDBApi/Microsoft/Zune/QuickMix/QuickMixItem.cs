using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.QuickMix
{
	public class QuickMixItem
	{
		private int m_mediaId;

		private int m_durationSeconds;

		private bool m_inCollection;

		private string m_title;

		private string m_artistName;

		private Guid m_serviceMediaId;

		public bool InCollection
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_inCollection;
			}
		}

		public int DurationSeconds => m_durationSeconds;

		public Guid ServiceMediaId => m_serviceMediaId;

		public string ArtistName => m_artistName;

		public string Title => m_title;

		public int MediaId => m_mediaId;

		public QuickMixItem(int mediaId, string title, string artistName, int durationSeconds, [MarshalAs(UnmanagedType.U1)] bool inCollection, Guid serviceMediaId)
		{
			m_mediaId = mediaId;
			m_title = title;
			m_artistName = artistName;
			m_durationSeconds = durationSeconds;
			m_inCollection = inCollection;
			m_serviceMediaId = serviceMediaId;
		}

		public override string ToString()
		{
			object[] array = new object[5] { m_title, m_artistName, null, null, null };
			Guid serviceMediaId = m_serviceMediaId;
			array[2] = serviceMediaId;
			array[3] = m_mediaId;
			array[4] = m_inCollection;
			return string.Format("Title:{0}\n\tArtist:{1}\n\tServiceMediaId:{2}\n\tMedia Id:{3}\tInCollection:{4}", array);
		}
	}
}
