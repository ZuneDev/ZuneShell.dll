using CommunityToolkit.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Microsoft.Zune;

public static class AsyncHelper
{
    public static void Run(Task task)
    {
        if (task == null)
            return;

        var t = Task.Run(() => task);
        t.Wait();
    }

    public static TResult Run<TResult>(Func<Task<TResult>> action)
    {
        Guard.IsNotNull(action);

        var t = Task.Run(action);
        t.Wait();
        return t.Result;
    }
}
