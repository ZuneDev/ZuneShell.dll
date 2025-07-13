using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class RatingValue
{
	private string m_name;

	private int m_order;

	private string m_text;

	private string m_toolTip;

	private string m_description;

	private string m_image;

	private bool m_treatAsUnrated;

	public bool TreatAsUnrated
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_treatAsUnrated;
		}
	}

	public string ImageId => m_image;

	public string DescriptionId => m_description;

	public string ToolTipId => m_toolTip;

	public string TextId => m_text;

	public int Order => m_order;

	public string Rating => m_name;

	public RatingValue(string name, int order, string text, string toolTip, string description, string image, [MarshalAs(UnmanagedType.U1)] bool treatAsUnrated)
	{
		m_name = name;
		m_order = order;
		m_text = text;
		m_toolTip = toolTip;
		m_description = description;
		m_image = image;
		m_treatAsUnrated = treatAsUnrated;
	}
}
