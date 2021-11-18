using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class LibraryDataProviderListItem : LibraryDataProviderItemBase
	{
		protected internal int m_LibraryId;

		protected LibraryVirtualList m_listOwner;

		protected int m_DontUseDirectly_Index;

		protected int m_QueryRN;

		internal unsafe LibraryDataProviderListItem(LibraryDataProviderQuery owner, LibraryVirtualList listOwner, object typeCookie, int QueryRN, int index)
			: base(owner, typeCookie)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_DD(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, (uint)index);
			}
			m_DontUseDirectly_Index = index;
			m_QueryRN = QueryRN;
			m_listOwner = listOwner;
			if (QueryRN != -1)
			{
				m_LibraryId = (int)m_listOwner.QueryList.GetFieldValue((uint)index, typeof(int), 355u, -1);
			}
			SetSlowDataThumbnailExtraction(useSlowData: true);
		}

		public LibraryVirtualList GetOwner()
		{
			return m_listOwner;
		}

		protected internal unsafe int GetIndex()
		{
			int queryRN = m_QueryRN;
			if (queryRN != -1)
			{
				LibraryVirtualList listOwner = m_listOwner;
				if (queryRN != listOwner.GetQueryRN())
				{
					int indexForLibraryId = (int)listOwner.QueryList.GetIndexForLibraryId(m_DontUseDirectly_Index, m_LibraryId);
					if (indexForLibraryId != -1)
					{
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
						{
							Module.WPP_SF_DDD(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 50, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, (uint)m_DontUseDirectly_Index, (uint)indexForLibraryId);
						}
						m_DontUseDirectly_Index = indexForLibraryId;
						m_QueryRN = m_listOwner.GetQueryRN();
					}
				}
			}
			return m_DontUseDirectly_Index;
		}

		protected override object GetFieldValue(Type type, uint Atom)
		{
			return m_listOwner.QueryList.GetFieldValue((uint)GetIndex(), type, Atom);
		}

		protected override object GetFieldValue(Type type, string AtomName, object defaultValue)
		{
			return m_listOwner.QueryList.GetFieldValue((uint)GetIndex(), type, AtomName, defaultValue);
		}

		protected override object GetFieldValue(Type type, uint Atom, object defaultValue)
		{
			return m_listOwner.QueryList.GetFieldValue((uint)GetIndex(), type, Atom, defaultValue);
		}

		protected override int SetFieldValue(string AtomName, object Value)
		{
			return m_listOwner.QueryList.SetFieldValue((uint)GetIndex(), AtomName, Value);
		}

		protected override int SetFieldValue(uint Atom, object Value)
		{
			return m_listOwner.QueryList.SetFieldValue((uint)GetIndex(), Atom, Value);
		}

		protected unsafe override void NotifySlowDataAcquireComplete()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 20, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			int index = GetIndex();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_DD(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 21, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, (uint)index);
			}
			m_listOwner.NotifySlowDataAcquireComplete(index);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 22, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		protected override bool AntialiasImageEdges()
		{
			return m_listOwner.AntialiasImageEdges;
		}

		protected override string ThumbnailFallbackImageUrl()
		{
			return m_listOwner.QueryOwner.ThumbnailFallbackImageUrl;
		}

		protected unsafe override void UpdateThumbnail(object args)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 48, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
			if (!m_listOwner.IsDisposed)
			{
				base.UpdateThumbnail(args);
			}
			else
			{
				((IDisposable)(AsyncGetThumbnailState)args).Dispose();
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL)) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 49, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
		}
	}
}
