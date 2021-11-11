namespace MicrosoftZuneLibrary
{
	public enum EBurnProgressStatus
	{
		ebpsComplete = 8,
		ebpsFinalizing = 7,
		ebpsReadyToWriteToCD = 6,
		ebpsTAOConvertingAndWriting = 5,
		ebpsConvertingTrack = 4,
		ebpsInspectingFile = 3,
		ebpsWritingImageToCD = 2,
		ebpsAddingFileToCDImage = 1,
		ebpsConvertingFile = 0
	}
}
