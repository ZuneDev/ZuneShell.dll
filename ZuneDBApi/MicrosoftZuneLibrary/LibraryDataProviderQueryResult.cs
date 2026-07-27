using System;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Transcribed verbatim from the original decompiled body: pure C#/Iris DataProviderObject
// logic with no native dependency.
internal class LibraryDataProviderQueryResult : DataProviderObject
{
    protected LibraryVirtualList m_virtualListResultSet;

    protected bool m_isEmpty;

    public LibraryDataProviderQueryResult(LibraryDataProviderQuery owner, LibraryVirtualList virtualListResultSet, object resultTypeCookie)
        : base(owner, resultTypeCookie)
    {
        m_virtualListResultSet = virtualListResultSet;
        if (virtualListResultSet == null)
            return;

        foreach (DataProviderMapping mapping in Mappings.Values)
        {
            if (mapping.UnderlyingCollectionTypeCookie != null)
                m_virtualListResultSet.SetItemTypeCookie(mapping.UnderlyingCollectionTypeName, mapping.UnderlyingCollectionTypeCookie);
        }
    }

    public override object GetProperty(string propertyName)
    {
        if (propertyName == "IsEmpty")
            return m_isEmpty;
        if (Mappings.TryGetValue(propertyName, out _))
            return m_virtualListResultSet;
        return null;
    }

    public override void SetProperty(string propertyName, object value) => throw new NotSupportedException();

    public void SetIsEmpty(bool isEmpty)
    {
        if (m_isEmpty != isEmpty)
        {
            m_isEmpty = isEmpty;
            FirePropertyChanged("IsEmpty");
        }
    }
}
