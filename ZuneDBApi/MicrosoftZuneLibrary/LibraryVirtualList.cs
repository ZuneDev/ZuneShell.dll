using System.Collections;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Original constructor wires this list up to a LibraryDataProviderQuery owner and a
// live ZuneQueryList (advising for change notifications, seeding Count from the query's
// row count). Since ZuneQueryList is always empty here (no native database — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md), the owner/advise wiring was dropped:
// OnRequestItem can never actually be called with Count permanently at 0. The
// non-native surface (AntialiasImageEdges, QueryList, GetUniqueIds, DisableAutoRefresh)
// is otherwise a direct transcription.
public class LibraryVirtualList : VirtualDatabaseList, ISearchableList
{
    private string m_itemTypeName;
    private bool m_antialiasImageEdges;

    public bool AntialiasImageEdges => m_antialiasImageEdges;

    public virtual ZuneQueryList QueryList => m_pQueryList;

    internal LibraryVirtualList(ZuneQueryList pQueryList, bool autoRefresh, bool antialiasEdges)
        : base(pQueryList, true)
    {
        m_antialiasImageEdges = antialiasEdges;
        Count = pQueryList.Count;
    }

    internal void SetItemTypeCookie(string itemTypeName, object itemTypeCookie)
    {
        m_itemTypeName = itemTypeName;
    }

    public virtual int SearchForString(string searchString) => -1;

    public override string ToString() => m_itemTypeName + " List";

    public void DisableAutoRefresh()
    {
    }

    public ArrayList GetUniqueIds() => m_pQueryList.GetUniqueIds();

    protected override object OnRequestItem(int index)
    {
        throw new System.IndexOutOfRangeException(nameof(index));
    }
}
