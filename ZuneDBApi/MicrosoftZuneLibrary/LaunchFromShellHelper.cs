using System.Collections.Generic;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Original spins up a background thread (ThreadProc) that enumerates a shell
// IDataObject/URL into a native file list, marshaling results back onto the app
// thread. Not reverse engineered (the native shell-interop side isn't visible to
// ILSpy) — see logs/MicrosoftZuneLibrary/ZuneLibrary.md. Go() completes immediately
// with an empty file list rather than actually enumerating anything.
public class LaunchFromShellHelper
{
    private DeferredInvokeHandler _completeHandler;
    private string _startParam;
    private string _eventName;
    private bool _startParamIsDataObject;
    private bool _cancelled;
    private List<FileEntry> _files = new();

    public string TaskName { get; set; }

    public List<FileEntry> Files => _files;

    public LaunchFromShellHelper(string taskName, string marshalledDataObject, string eventName)
    {
        Init(taskName, marshalledDataObject, eventName, startParamIsDataObject: true);
    }

    public LaunchFromShellHelper(string taskName, string initialUrl)
    {
        Init(taskName, initialUrl, null, startParamIsDataObject: false);
    }

    private void Init(string taskName, string startParam, string eventName, bool startParamIsDataObject)
    {
        TaskName = taskName;
        _startParam = startParam;
        _eventName = eventName;
        _startParamIsDataObject = startParamIsDataObject;
    }

    public void Go(DeferredInvokeHandler completeHandler)
    {
        _completeHandler = completeHandler;
        if (!_cancelled)
            _completeHandler?.Invoke(this);
    }

    public void Cancel()
    {
        _cancelled = true;
    }
}
