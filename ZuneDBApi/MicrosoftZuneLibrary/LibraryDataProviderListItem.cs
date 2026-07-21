using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Original constructor also takes an internal LibraryDataProviderQuery owner
// (needed to construct the DataProviderObject base's typeCookie via a real query).
// Since LibraryVirtualList.OnRequestItem (the only original caller of this
// constructor) never actually runs — there's no native database to back a real
// query result, see logs/MicrosoftZuneLibrary/ZuneLibrary.md — that parameter was
// dropped from this internal-only constructor.
public class LibraryDataProviderListItem : LibraryDataProviderItemBase
{
    private LibraryVirtualList m_listOwner;
    private int m_DontUseDirectly_Index;
    private int m_QueryRN;

    internal LibraryDataProviderListItem(DataProviderQuery owner, LibraryVirtualList listOwner, object typeCookie, int queryRN, int index)
        : base(owner, typeCookie)
    {
        m_DontUseDirectly_Index = index;
        m_QueryRN = queryRN;
        m_listOwner = listOwner;
    }

    public LibraryVirtualList GetOwner() => m_listOwner;

    protected internal int GetIndex() => m_DontUseDirectly_Index;
}
