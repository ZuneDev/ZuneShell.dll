using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	[DefaultMember("Item")]
	public class LibraryVirtualList : VirtualDatabaseList, ISearchableList
	{
		private LibraryDataProviderQuery m_owner;

		private new ZuneQueryList m_pQueryList;

		private string m_itemTypeName;

		private object m_itemTypeCookie;

		private bool m_autoRefresh;

		private bool m_useSlowDataRequests;

		private bool m_antialiasImageEdges;

		internal LibraryDataProviderQuery QueryOwner => m_owner;

		public bool AntialiasImageEdges
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_antialiasImageEdges;
			}
		}

		public virtual ZuneQueryList QueryList => m_pQueryList;

		internal LibraryVirtualList(LibraryDataProviderQuery owner, ZuneQueryList pQueryList, [MarshalAs(UnmanagedType.U1)] bool autoRefresh, [MarshalAs(UnmanagedType.U1)] bool antialiasEdges)
			: base(pQueryList, enableSlowDataRequests: true)
		{
			try
			{
				m_owner = owner;
                StoreQueryResults = true;
				m_pQueryList = pQueryList;
				pQueryList.AddRef();
				m_fBlockChanges = false;
				m_fEndBulkArrivedDuringBlock = false;
				m_antialiasImageEdges = antialiasEdges;
				m_autoRefresh = autoRefresh;
				if (autoRefresh)
				{
					m_pQueryList.Advise(this);
				}
                Count = m_pQueryList.Count;
			}
			catch
			{
				//try-fault
				base.Dispose(true);
				throw;
			}
		}

		private void _007ELibraryVirtualList()
		{
			if (m_autoRefresh)
			{
				m_pQueryList.Unadvise(this);
			}
			m_pQueryList.Release();
		}

		internal void SetItemTypeCookie(string itemTypeName, object itemTypeCookie)
		{
			m_itemTypeName = itemTypeName;
			m_itemTypeCookie = itemTypeCookie;
		}

		public unsafe virtual int SearchForString(string searchString)
		{
			bool[] ascendings = null;
			string[] sorts = null;
			if (m_owner.GetSortAttributes(out sorts, out ascendings))
			{
				int result;
				fixed (char* sorts0Ptr = sorts[0].ToCharArray())
				{
					ushort* wszName = (ushort*)sorts0Ptr;
					try
					{
						uint atom = (uint)ZuneDBApi.Module.CSchemaMap_002EGetIndex(wszName);
						result = m_pQueryList.SearchForString(atom, ascendings[0], searchString);
					}
					catch
					{
						//try-fault
						wszName = null;
						throw;
					}
				}
				return result;
			}
			return -1;
		}

		public override string ToString()
		{
			return m_itemTypeName + " List";
		}

		public void DisableAutoRefresh()
		{
			if (m_autoRefresh)
			{
				m_pQueryList.Unadvise(this);
				m_autoRefresh = false;
			}
		}

		public ArrayList GetUniqueIds()
		{
			return m_pQueryList.GetUniqueIds();
		}

		protected override object OnRequestItem(int index)
		{
			if (!m_fDisposed && m_pQueryList.CheckItemIndex((uint)index))
			{
				LibraryDataProviderListItem libraryDataProviderListItem = new LibraryDataProviderListItem(m_owner, this, m_itemTypeCookie, m_QueryRN, index);
				libraryDataProviderListItem.SetSlowDataThumbnailExtraction(SlowDataRequestsEnabled);
				m_fItemsAdded = true;
				return libraryDataProviderListItem;
			}
			return new UnavailableLibraryListItem(m_owner, this, m_itemTypeCookie, index);
		}

		protected override void OnRequestSlowData(int index)
		{
			((LibraryDataProviderListItem)this[index]).OnRequestSlowData();
		}

		protected override void InvalidateItem(int index)
		{
			((LibraryDataProviderListItem)this[index]).InvalidateAllProperties();
		}

		protected override void OnPropertyChanged(string property)
		{
			base.OnPropertyChanged(property);
			if (!IsDisposed && property == "Count")
			{
				LibraryDataProviderQueryResult libraryDataProviderQueryResult = (LibraryDataProviderQueryResult)m_owner.Result;
				if (libraryDataProviderQueryResult != null)
				{
					byte isEmpty = ((Count == 0) ? ((byte)1) : ((byte)0));
					libraryDataProviderQueryResult.SetIsEmpty(isEmpty != 0);
				}
			}
		}

		protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007ELibraryVirtualList();
				}
				finally
				{
					base.Dispose(true);
				}
			}
			else
			{
				base.Dispose(false);
			}
		}
	}
}
