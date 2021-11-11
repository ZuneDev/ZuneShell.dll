using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class CountryBaseDetails
	{
		private string m_abbreviation;

		private string[] m_languageAbbreviations;

		private int m_teenagerAge;

		private int m_adultAge;

		private bool m_showNewsletterOptions;

		private bool m_usageCollection;

		private CountryFieldValidator[] m_validators;

		public CountryFieldValidator[] Validators => m_validators;

		public bool UsageCollection
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_usageCollection;
			}
		}

		public bool ShowNewsletterOptions
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_showNewsletterOptions;
			}
		}

		public int AdultAge => m_adultAge;

		public int TeenagerAge => m_teenagerAge;

		public string[] LanguageAbbreviations => m_languageAbbreviations;

		public string Abbreviation => m_abbreviation;

		public CountryBaseDetails(string abbreviation, string[] languageAbbreviations, int teenagerAge, int adultAge, [MarshalAs(UnmanagedType.U1)] bool showNewsletterOptions, [MarshalAs(UnmanagedType.U1)] bool usageCollection, CountryFieldValidator[] validators)
		{
			m_abbreviation = abbreviation;
			m_languageAbbreviations = languageAbbreviations;
			m_teenagerAge = teenagerAge;
			m_adultAge = adultAge;
			m_showNewsletterOptions = showNewsletterOptions;
			m_usageCollection = usageCollection;
			m_validators = validators;
		}
	}
}
