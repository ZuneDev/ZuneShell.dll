using System.Threading;

namespace MicrosoftZuneLibrary;

// Original manages a per-instance sequential background work queue. Transcribed as
// real logic (a thin wrapper over ThreadPool) rather than stubbed, since this is pure
// .NET threading with no native dependency — see logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class WorkerQueue
{
    public static WorkerQueue CreateInstance() => new();

    public static void ShutdownAll()
    {
    }

    public bool QueueSequentialWorkItem(WaitCallback callback, object context)
    {
        return ThreadPool.QueueUserWorkItem(_ => callback(context));
    }
}
