using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

internal class LibraryDataProviderQueryResult : DataProviderObject
{
	protected LibraryVirtualList m_virtualListResultSet;

	protected bool m_isEmpty;

	public LibraryDataProviderQueryResult(LibraryDataProviderQuery owner, LibraryVirtualList virtualListResultSet, object resultTypeCookie)
		: base(owner, resultTypeCookie)
	{
		m_virtualListResultSet = virtualListResultSet;
		if (virtualListResultSet == null)
		{
			return;
		}
		foreach (DataProviderMapping value in base.Mappings.Values)
		{
			if (value.UnderlyingCollectionTypeCookie != null)
			{
				m_virtualListResultSet.SetItemTypeCookie(value.UnderlyingCollectionTypeName, value.UnderlyingCollectionTypeCookie);
			}
		}
	}

	public override object GetProperty(string propertyName)
	{
		DataProviderMapping value = null;
		if (propertyName == "IsEmpty")
		{
			return m_isEmpty;
		}
		if (base.Mappings.TryGetValue(propertyName, out value))
		{
			return m_virtualListResultSet;
		}
		return null;
	}

	public override void SetProperty(string propertyName, object value)
	{
		throw new NotSupportedException();
	}

	public void SetIsEmpty([MarshalAs(UnmanagedType.U1)] bool isEmpty)
	{
		if (m_isEmpty != isEmpty)
		{
			m_isEmpty = isEmpty;
			FirePropertyChanged("IsEmpty");
		}
	}
}
