namespace Microsoft.Zune.Service;

public class CountryFieldValidator
{
	private string m_name;

	private string m_regex;

	private string m_friendlyFormat;

	private string m_nameStringId;

	public string NameStringId => m_nameStringId;

	public string FriendlyFormat => m_friendlyFormat;

	public string Regex => m_regex;

	public string Name => m_name;

	public CountryFieldValidator(string name, string regex, string friendlyFormat, string nameStringId)
	{
		m_name = name;
		m_regex = regex;
		m_friendlyFormat = friendlyFormat;
		m_nameStringId = nameStringId;
	}
}
