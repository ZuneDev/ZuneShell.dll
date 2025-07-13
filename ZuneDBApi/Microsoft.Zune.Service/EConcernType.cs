using ZuneUI;

namespace Microsoft.Zune.Service;

public enum EConcernType
{
	[Description(49222u)]
	StaleImage = 5,
	[Description(49221u)]
	Stale = 4,
	[Description(49220u)]
	PlayOrDownloadErrors = 3,
	[Description(49219u)]
	Miscategorized = 2,
	[Description(49223u)]
	OffensiveImage = 1,
	[Description(49218u)]
	OffensiveContent = 0,
	Unknown = -1
}
