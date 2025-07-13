namespace MicrosoftZuneLibrary;

public interface IDatabaseMedia
{
	void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType);
}
