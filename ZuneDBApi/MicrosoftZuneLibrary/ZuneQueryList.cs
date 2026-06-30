using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class ZuneQueryList : IDisposable
{
    private readonly object m_pResults; // Stub for IDatabaseQueryResults*
    private readonly object m_pRelay; // Stub for ResultSetEventRelay*
    private string m_bstrQueryName;
    private int m_RefCount = 0;
    private bool m_disposed = false;

    public bool IsDisposed
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_disposed; }
    }

    public bool IsEmpty
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_pResults == null; }
    }

    public int Count
    {
        get { return 0; }
    }

    internal ZuneQueryList()
    {
    }

    public uint AddRef()
    {
        return (uint)(++m_RefCount);
    }

    public uint Release()
    {
        int num = --m_RefCount;
        if (num == 0)
        {
            ((IDisposable)this).Dispose();
        }
        return (uint)num;
    }

    [return: MarshalAs(UnmanagedType.U1)]
    public bool CheckItemIndex(uint index)
    {
        return false;
    }

    public void Advise(IQueryListEvents listener)
    {
    }

    public void Unadvise(IQueryListEvents listener)
    {
    }

    public void EndBulkEventsComplete([MarshalAs(UnmanagedType.U1)] bool fAbandon)
    {
    }

    public object GetFieldValue(uint index, Type type, string AtomName, object defaultValue)
    {
        return defaultValue;
    }

    public object GetFieldValue(uint index, Type type, string AtomName)
    {
        return GetFieldValue(index, type, AtomName, null);
    }

    public object GetFieldValue(uint index, Type type, uint Atom, object defaultValue)
    {
        if (m_disposed)
        {
            return defaultValue;
        }
        return defaultValue;
    }

    public object GetFieldValue(uint index, Type type, uint Atom)
    {
        return GetFieldValue(index, type, Atom, null);
    }

    public int SetFieldValue(uint index, string AtomName, object Value)
    {
        return -2147024809;
    }

    public int SetFieldValue(uint index, uint Atom, object Value)
    {
        return -2147024809;
    }

    public uint GetIndexForLibraryId(int indexHint, int LibraryId)
    {
        return uint.MaxValue;
    }

    public ArrayList GetUniqueIds()
    {
        return null;
    }

    public int SearchForString(uint Atom, [MarshalAs(UnmanagedType.U1)] bool ascending, string SearchString)
    {
        return -1;
    }

    public int ClientBusy([MarshalAs(UnmanagedType.U1)] bool fBusy)
    {
        return 0;
    }

    public static object MarshalResult(Type type, object propVariant, object defaultValue)
    {
        return defaultValue;
    }

    public static int ConvertTypeToPropVariant(Type type, object value, object propVariant)
    {
        return -2147467259;
    }

    public static int AtomNameToAtom(string AtomName)
    {
        return -1;
    }

    public static string AtomToAtomName(int atom)
    {
        return null;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public virtual sealed void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ZuneQueryList()
    {
        Dispose(false);
    }
}
