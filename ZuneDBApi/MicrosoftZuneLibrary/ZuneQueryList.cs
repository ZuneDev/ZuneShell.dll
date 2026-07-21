using System;
using System.Collections;

namespace MicrosoftZuneLibrary;

// Original wraps a native IDatabaseQueryResults*/ResultSetEventRelay* pair — the
// live-updating result set backing every library query (QueryDatabase,
// GetTracksByArtist, etc). Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md. Always empty since there's no native
// database engine behind it.
public class ZuneQueryList : IDisposable
{
    internal ZuneQueryList()
    {
    }

    public bool IsDisposed { get; private set; }

    public bool IsEmpty => Count == 0;

    public int Count => 0;

    public uint AddRef() => 1;

    public uint Release() => 0;

    public bool CheckItemIndex(uint index) => false;

    public void Advise(IQueryListEvents listener)
    {
    }

    public void Unadvise(IQueryListEvents listener)
    {
    }

    public void EndBulkEventsComplete(bool fAbandon)
    {
    }

    public object GetFieldValue(uint index, Type type, string atomName, object defaultValue) => defaultValue;

    public object GetFieldValue(uint index, Type type, string atomName) => null;

    public object GetFieldValue(uint index, Type type, uint atom, object defaultValue) => defaultValue;

    public object GetFieldValue(uint index, Type type, uint atom) => null;

    public int SetFieldValue(uint index, string atomName, object value) => unchecked((int)0x80004005);

    public int SetFieldValue(uint index, uint atom, object value) => unchecked((int)0x80004005);

    public uint GetIndexForLibraryId(int indexHint, int libraryId) => 0;

    public ArrayList GetUniqueIds() => new();

    public int SearchForString(uint atom, bool ascending, string searchString) => -1;

    public int ClientBusy(bool fBusy) => unchecked((int)0x80004005);

    public static int AtomNameToAtom(string atomName) => -1;

    public static string AtomToAtomName(int atom) => null;

    protected virtual void Dispose(bool disposing)
    {
        IsDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
