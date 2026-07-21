using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IRecordManager* (WMP's CD/radio recording engine). Not
// reverse engineered — see logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class ZuneLibraryCDRecorder : IDisposable
{
    internal ZuneLibraryCDRecorder()
    {
    }

    public event OnRecordResumeHandler RecordResumeHandler;
    public event OnRecordPauseHandler RecordPauseHandler;
    public event OnRecordStopHandler RecordStopHandler;
    public event OnRecordProgressHandler RecordProgressHandler;
    public event OnRecordStartHandler RecordStartHandler;

    public uint AddRef() => 1;

    public uint Release() => 0;

    public int AddRecordingRequest(ZuneLibraryCDDevice device, uint dwTrackNumber) => unchecked((int)0x80004005);

    public int RemoveRecordingRequest(ZuneLibraryCDDevice device, uint dwTrackNumber) => unchecked((int)0x80004005);

    public bool IsScheduledForRecording(ZuneLibraryCDDevice device, uint dwTrackNumber) => false;

    public int StopRecording() => unchecked((int)0x80004005);

    public int AsyncAddRecordingRequest(ZuneLibraryCDDevice device, uint dwTrackNumber) => unchecked((int)0x80004005);

    public int AsyncRemoveRecordingRequest(ZuneLibraryCDDevice device, uint dwTrackNumber) => unchecked((int)0x80004005);

    internal void OnRecordStart(string pwszUrl)
    {
        RecordStartHandler?.Invoke(pwszUrl);
    }

    internal void OnRecordProgress(string pwszUrl, int iTicks)
    {
        RecordProgressHandler?.Invoke(pwszUrl, iTicks);
    }

    internal void OnRecordStop(string pwszUrl, int hr)
    {
        RecordStopHandler?.Invoke(pwszUrl, hr);
    }

    internal void OnRecordPause(string pwszUrl)
    {
        RecordPauseHandler?.Invoke(pwszUrl);
    }

    internal void OnRecordResume(string pwszUrl)
    {
        RecordResumeHandler?.Invoke(pwszUrl);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
