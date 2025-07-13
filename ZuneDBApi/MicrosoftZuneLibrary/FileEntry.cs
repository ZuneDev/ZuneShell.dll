namespace MicrosoftZuneLibrary;

public class FileEntry
{
	private EMediaTypes _mediaType;

	private string _path;

	public string Path => _path;

	public EMediaTypes MediaType => _mediaType;

	public unsafe FileEntry(ushort* path, EMediaTypes mediaType)
	{
		_path = new string((char*)path);
		_mediaType = mediaType;
	}
}
