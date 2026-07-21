using System;
using System.Collections.Generic;

namespace Microsoft.Zune.Util;

// Original wraps a native IJumpList*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class JumpListSession : IDisposable
{
    internal JumpListSession()
    {
    }

    public bool IsAlive => false;

    public int GetDisallowedDestinations(out List<JumpListEntry> disallowedDestinationList)
    {
        disallowedDestinationList = new List<JumpListEntry>();
        return unchecked((int)0x80004005);
    }

    public int CreateTask(out JumpListEntry task)
    {
        task = null;
        return unchecked((int)0x80004005);
    }

    public int CreateCategory(out JumpListCategory category)
    {
        category = null;
        return unchecked((int)0x80004005);
    }

    public int Commit() => unchecked((int)0x80004005);

    public int Cancel() => unchecked((int)0x80004005);

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
