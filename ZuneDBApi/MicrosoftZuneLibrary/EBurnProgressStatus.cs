namespace MicrosoftZuneLibrary;

public enum EBurnProgressStatus
{
    ebpsConvertingFile = 0,
    ebpsAddingFileToCDImage = 1,
    ebpsWritingImageToCD = 2,
    ebpsInspectingFile = 3,
    ebpsConvertingTrack = 4,
    ebpsTAOConvertingAndWriting = 5,
    ebpsReadyToWriteToCD = 6,
    ebpsFinalizing = 7,
    ebpsComplete = 8,
}
