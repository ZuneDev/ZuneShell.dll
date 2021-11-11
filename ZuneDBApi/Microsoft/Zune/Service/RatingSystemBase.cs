using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class RatingSystemBase
	{
		private RatingValue[] m_ratings;

		private Dictionary<string, Dictionary<string, string>> m_strings;

		private string m_name;

		private string m_title;

		private string m_description;

		private string m_blockText;

		private string m_defaultLanguage;

		private bool m_useImages;

		private bool m_showBlockUnrated;

		public RatingValue[] Ratings => m_ratings;

		public Dictionary<string, Dictionary<string, string>> Strings => m_strings;

		public bool ShowBlockUnrated
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_showBlockUnrated;
			}
		}

		public bool UseImages
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_useImages;
			}
		}

		public string DefaultLanguage => m_defaultLanguage;

		public string BlockText => m_blockText;

		public string Description => m_description;

		public string Title => m_title;

		public string Name => m_name;

		public RatingSystemBase(string name, string title, string description, string blockText, [MarshalAs(UnmanagedType.U1)] bool useImages, [MarshalAs(UnmanagedType.U1)] bool showBlockUnrated, string defaultLanguage, Dictionary<string, Dictionary<string, string>> strings, RatingValue[] ratings)
		{
			m_name = name;
			m_title = title;
			m_description = description;
			m_blockText = blockText;
			m_defaultLanguage = defaultLanguage;
			m_useImages = useImages;
			m_showBlockUnrated = showBlockUnrated;
			m_strings = strings;
			m_ratings = ratings;
		}
	}
}
