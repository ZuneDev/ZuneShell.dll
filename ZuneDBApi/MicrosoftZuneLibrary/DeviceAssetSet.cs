namespace MicrosoftZuneLibrary;

public class DeviceAssetSet
{
	private string[] m_ImageUris;

	private string[] m_DefaultImageUris;

	private uint[] m_Colors;

	public uint[] Colors => m_Colors;

	public string[] ImageUris => m_ImageUris;

	public string[] DefaultImageUris => m_DefaultImageUris;

	public DeviceAssetSet(string[] imageUris, string[] defaultImageUris, uint[] colors)
	{
		m_DefaultImageUris = defaultImageUris;
		m_ImageUris = imageUris;
		m_Colors = colors;
	}
}
