using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

[DefaultMember("Item")]
public class VirtualSubscriptionEpisodeList : VirtualList, IDisposable
{
	private string m_feedUrl;

	private DataProviderQuery m_owner;

	private object m_typeCookie;

	private List<SubscriptionDataProviderItem> m_items;

	private object m_lock;

	private string m_sortProperty;

	private bool m_sortAsc;

	internal virtual string FeedUrl => m_feedUrl;

	public VirtualSubscriptionEpisodeList(DataProviderQuery owner, object typeCookie, string feedUrl, string sort)
	{
		m_feedUrl = feedUrl;
		m_owner = owner;
		m_typeCookie = typeCookie;
		base._002Ector();
		try
		{
			m_items = new List<SubscriptionDataProviderItem>();
			m_lock = new object();
			SetSortProperty(sort);
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)this).Dispose();
			throw;
		}
	}

	private void _007EVirtualSubscriptionEpisodeList()
	{
		Monitor.Enter(m_lock);
		try
		{
			List<SubscriptionDataProviderItem>.Enumerator enumerator = m_items.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnDispose();
			}
			m_items = null;
		}
		finally
		{
			Monitor.Exit(m_lock);
		}
	}

	internal unsafe void AddItem(IMSMediaSchemaPropertySet* pEpisodeItem)
	{
		Monitor.Enter(m_lock);
		try
		{
			if (m_items != null)
			{
				m_items.Add(new SubscriptionDataProviderItem(m_owner, m_typeCookie, m_feedUrl, pEpisodeItem));
			}
		}
		finally
		{
			Monitor.Exit(m_lock);
		}
	}

	internal unsafe void AsyncRetrieveEpisodeList(SubscriptionSeriesInfo seriesInfo)
	{
		//IL_000a: Expected I, but got I8
		//IL_000e: Expected I, but got I8
		//IL_008f: Expected I, but got I8
		//IL_0093: Expected I, but got I8
		//IL_00a5: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		UpdateQueryStatus(DataProviderQueryStatus.RequestingData);
		ISubscriptionViewer* ptr = null;
		VirtualSubscriptionEpisodeListProxy* ptr2 = null;
		int num = global::_003CModule_003E.ZuneLibraryExports_002ECreateNativeSubscriptionViewer((void**)(&ptr));
		if (num >= 0)
		{
			VirtualSubscriptionEpisodeListProxy* ptr3 = (VirtualSubscriptionEpisodeListProxy*)global::_003CModule_003E.@new(40uL);
			VirtualSubscriptionEpisodeListProxy* ptr4;
			try
			{
				ptr4 = ((ptr3 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002EVirtualSubscriptionEpisodeListProxy_002E_007Bctor_007D(ptr3, seriesInfo, this));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr3);
				throw;
			}
			ptr2 = ptr4;
			num = (((long)(nint)ptr4 == 0) ? (-2147024882) : num);
			if (num >= 0)
			{
				fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_feedUrl)))
				{
					try
					{
						long num2 = *(long*)ptr + 24;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ISubscriptionViewerCallback*, int>)(*(ulong*)num2))((nint)ptr, ptr5, (ISubscriptionViewerCallback*)ptr4);
					}
					catch
					{
						//try-fault
						ptr5 = null;
						throw;
					}
				}
			}
		}
		if (ptr != null)
		{
			ISubscriptionViewer* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			ptr = null;
		}
		if (ptr2 != null)
		{
			VirtualSubscriptionEpisodeListProxy* intPtr2 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
	}

	internal void AsyncOperationCompleted(int hrErrorCode)
	{
		DataProviderQueryStatus dataProviderQueryStatus = DataProviderQueryStatus.Complete;
		dataProviderQueryStatus = ((hrErrorCode < 0) ? DataProviderQueryStatus.Error : dataProviderQueryStatus);
		Sort(null);
		Application.DeferredInvoke(DeferredUpdateQueryStatus, dataProviderQueryStatus);
	}

	internal void Sort(string sortProperty)
	{
		Monitor.Enter(m_lock);
		try
		{
			if (sortProperty != null)
			{
				SetSortProperty(sortProperty);
			}
			if (m_items != null)
			{
				m_items.Sort(CompareItems);
			}
			Application.DeferredInvoke(DeferredResetList, null);
		}
		finally
		{
			Monitor.Exit(m_lock);
		}
	}

	protected override object OnRequestItem(int index)
	{
		return m_items[index];
	}

	private void UpdateQueryStatus(DataProviderQueryStatus status)
	{
		m_owner.Status = status;
	}

	private void DeferredUpdateQueryStatus(object param)
	{
		DataProviderQueryStatus status = (DataProviderQueryStatus)param;
		UpdateQueryStatus(status);
	}

	private void DeferredResetList(object param)
	{
		Clear();
		List<SubscriptionDataProviderItem> items = m_items;
		if (items != null)
		{
			base.Count = items.Count;
		}
	}

	private void SetSortProperty(string sort)
	{
		if (!string.IsNullOrEmpty(sort) && sort.Length > 1)
		{
			m_sortProperty = sort.Substring(1);
			int sortAsc = ((sort[0] == '+') ? 1 : 0);
			m_sortAsc = (byte)sortAsc != 0;
		}
		else
		{
			m_sortAsc = false;
			m_sortProperty = "ReleaseDate";
		}
	}

	private int CompareItems(SubscriptionDataProviderItem x, SubscriptionDataProviderItem y)
	{
		int num = 0;
		object property = x.GetProperty(m_sortProperty);
		object property2 = y.GetProperty(m_sortProperty);
		if (property is IComparable comparable)
		{
			num = comparable.CompareTo(property2);
			if (!m_sortAsc)
			{
				num = -num;
			}
		}
		return num;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				_007EVirtualSubscriptionEpisodeList();
				return;
			}
			finally
			{
				base.Dispose();
			}
		}
		Finalize();
	}

	public new void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
