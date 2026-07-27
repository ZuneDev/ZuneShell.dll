namespace MicrosoftZuneLibrary;

// Transcribed verbatim from the original decompiled body: a plain argument bundle passed
// through Application.DeferredInvoke from LibraryDataProviderQuery.BeginExecuteWorker to
// LibraryDataProviderQuery.DeferredSetResult.
internal class DeferredSetResultArgs(int requestGeneration, ZuneQueryList queryList, bool retainedList)
{
    public int RequestGeneration = requestGeneration;

    public ZuneQueryList QueryList = queryList;

    public bool RetainedList = retainedList;
}
