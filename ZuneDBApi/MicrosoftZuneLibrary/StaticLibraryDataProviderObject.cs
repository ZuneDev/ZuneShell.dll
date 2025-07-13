using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;
using MicrosoftZuneInterop;

namespace MicrosoftZuneLibrary;

public class StaticLibraryDataProviderObject : LibraryDataProviderItemBase
{
	private int m_LibraryId;

	private QueryPropertyBag m_QueryContext;

	private EListType m_eListType;

	private string m_thumbnailFallbackImageUrl;

	internal StaticLibraryDataProviderObject(DataProviderQuery query, object typeCookie, EListType eListType, int libraryId, int userId, int deviceId, string thumbnailFallbackImageUrl)
		: base(query, typeCookie)
	{
		m_LibraryId = libraryId;
		m_eListType = eListType;
		m_thumbnailFallbackImageUrl = thumbnailFallbackImageUrl;
		(m_QueryContext = new QueryPropertyBag()).SetValue("QueryView", 0);
		m_QueryContext.SetValue("UserId", userId);
		m_QueryContext.SetValue("DeviceId", deviceId);
	}

	protected override object GetFieldValue(Type type, uint Atom)
	{
		return GetFieldValue(type, Atom, null);
	}

	protected override object GetFieldValue(Type type, string AtomName, object defaultValue)
	{
		return GetFieldValue(type, (uint)ZuneQueryList.AtomNameToAtom(AtomName), defaultValue);
	}

	protected override object GetFieldValue(Type type, uint Atom, object defaultValue)
	{
		int[] columnIndexes = new int[1] { (int)Atom };
		object[] array = new object[1] { defaultValue };
		ZuneLibrary.GetFieldValues(m_LibraryId, m_eListType, 1, columnIndexes, array, m_QueryContext);
		return array[0];
	}

	protected override int SetFieldValue(string AtomName, object Value)
	{
		return SetFieldValue((uint)ZuneQueryList.AtomNameToAtom(AtomName), Value);
	}

	protected override int SetFieldValue(uint Atom, object Value)
	{
		ZuneLibrary.SetFieldValues(columnIndexes: new int[1] { (int)Atom }, fieldValues: new object[1] { Value }, iMediaId: m_LibraryId, eList: m_eListType, cValues: 1, propertyBag: m_QueryContext);
		return 0;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	protected override bool AntialiasImageEdges()
	{
		return true;
	}

	protected override string ThumbnailFallbackImageUrl()
	{
		return m_thumbnailFallbackImageUrl;
	}
}
