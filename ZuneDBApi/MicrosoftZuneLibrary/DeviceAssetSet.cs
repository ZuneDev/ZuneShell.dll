namespace MicrosoftZuneLibrary;

// Original wraps a native IDeviceAssetProvider*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/Device.md.
public class DeviceAssetSet
{
    private string[] m_ImageUris = System.Array.Empty<string>();
    private string[] m_DefaultImageUris = System.Array.Empty<string>();
    private uint[] m_Colors = System.Array.Empty<uint>();

    public uint[] Colors => m_Colors;

    public string[] ImageUris => m_ImageUris;

    public string[] DefaultImageUris => m_DefaultImageUris;
}
