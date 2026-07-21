namespace MicrosoftZuneLibrary;

public class FileEntry
{
    private EMediaTypes _mediaType;
    private string _path;

    public string Path => _path;

    public EMediaTypes MediaType => _mediaType;

    internal FileEntry(string path, EMediaTypes mediaType)
    {
        _path = path;
        _mediaType = mediaType;
    }
}
