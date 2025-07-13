using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

[DefaultMember("Item")]
public class VirtualDatabaseList : VirtualList, IQueryListEvents, IDisposable
{
	protected int m_QueryRN;

	protected ZuneQueryList m_pQueryList;

	protected bool m_fInBulkEvents;

	protected bool m_fDisposed;

	protected ArrayList m_lNotifyData;

	protected int m_blockChangesFlags;

	protected bool m_fBlockChanges;

	protected bool m_fEndBulkArrivedDuringBlock;

	protected bool m_fItemsAdded;

	internal VirtualDatabaseList(ZuneQueryList pQueryList, [MarshalAs(UnmanagedType.U1)] bool enableSlowDataRequests)
		: base(enableSlowDataRequests)
	{
		try
		{
			m_pQueryList = pQueryList;
			m_lNotifyData = new ArrayList();
			m_fDisposed = false;
			m_fItemsAdded = false;
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)this).Dispose();
			throw;
		}
	}

	private void _007EVirtualDatabaseList()
	{
		m_fDisposed = true;
	}

	public virtual void ListInsert(int index)
	{
		if (m_fInBulkEvents)
		{
			ListNotifyData listNotifyData = new ListNotifyData();
			listNotifyData.type = 0;
			listNotifyData.pos = index;
			m_lNotifyData.Add(listNotifyData);
		}
		else
		{
			Application.DeferredInvoke(DeferredInsert, index);
		}
	}

	public virtual void ListRemoveAt(int index)
	{
		if (m_fInBulkEvents)
		{
			ListNotifyData listNotifyData = new ListNotifyData();
			listNotifyData.type = 1;
			listNotifyData.pos = index;
			m_lNotifyData.Add(listNotifyData);
		}
		else
		{
			Application.DeferredInvoke(DeferredRemoveAt, index);
		}
	}

	public virtual void ListModified(int index)
	{
		if (m_fInBulkEvents)
		{
			ListNotifyData listNotifyData = new ListNotifyData();
			listNotifyData.type = 2;
			listNotifyData.pos = index;
			m_lNotifyData.Add(listNotifyData);
		}
		else
		{
			Application.DeferredInvoke(DeferredModified, index);
		}
	}

	public virtual void ListBeginBulkEvents()
	{
		m_fInBulkEvents = true;
	}

	public virtual void ListEndBulkEvents()
	{
		m_fInBulkEvents = false;
		m_fItemsAdded = false;
		Application.DeferredInvoke(DeferredEndBulkEvents, null);
	}

	public virtual void ListNotifyCount(uint count)
	{
		Application.DeferredInvoke(DeferredCountChange, (int)count);
	}

	public void SetBlockChangesFlag(int flag, [MarshalAs(UnmanagedType.U1)] bool block)
	{
		if (block)
		{
			m_blockChangesFlags |= flag;
		}
		else
		{
			m_blockChangesFlags &= ~flag;
		}
		byte blockChanges = (byte)((m_blockChangesFlags != 0) ? 1 : 0);
		BlockListChanges(blockChanges != 0);
	}

	public int GetQueryRN()
	{
		return m_QueryRN;
	}

	protected virtual void InvalidateItem(int index)
	{
		Modified(index);
	}

	private unsafe void DeferredEndBulkEvents(object P_0)
	{
		//IL_0062: Expected I, but got I8
		//IL_00f8: Expected I, but got I8
		if (m_fDisposed)
		{
			m_lNotifyData.Clear();
			return;
		}
		if (m_fBlockChanges)
		{
			m_fEndBulkArrivedDuringBlock = true;
			return;
		}
		if (m_fItemsAdded)
		{
			m_lNotifyData.Clear();
			m_pQueryList.EndBulkEventsComplete(fAbandon: true);
			m_fItemsAdded = false;
			return;
		}
		bool flag = false;
		global::_003CModule_003E.PERFTRACE_COLLECTIONEVENT((_COLLECTION_EVENT)14, null);
		foreach (ListNotifyData lNotifyDatum in m_lNotifyData)
		{
			if (!m_fDisposed)
			{
				switch (lNotifyDatum.type)
				{
				case 2:
					DeferredModified(lNotifyDatum.pos);
					break;
				case 1:
					DeferredRemoveAt(lNotifyDatum.pos);
					flag = true;
					break;
				case 0:
					DeferredInsert(lNotifyDatum.pos);
					flag = true;
					break;
				}
				continue;
			}
			break;
		}
		global::_003CModule_003E.PERFTRACE_COLLECTIONEVENT((_COLLECTION_EVENT)15, null);
		m_lNotifyData.Clear();
		m_pQueryList.EndBulkEventsComplete(fAbandon: false);
		if (flag)
		{
			m_QueryRN++;
		}
	}

	private void DeferredInsert(object args)
	{
		if (!m_fDisposed && Count > (int)args)
		{
			Insert((int)args);
		}
	}

	private void DeferredRemoveAt(object args)
	{
		if (!m_fDisposed && Count > 0 && Count > (int)args)
		{
			RemoveAt((int)args);
		}
	}

	private void DeferredModified(object args)
	{
		if (!m_fDisposed && Count > (int)args && IsItemAvailable((int)args))
		{
			InvalidateItem((int)args);
		}
	}

	private void DeferredCountChange(object args)
	{
		int num = (int)args;
		if (!m_fDisposed)
		{
			if (num > Count)
			{
				AddRange(num - Count);
			}
			else
			{
				base.Count = num;
			}
		}
	}

	private void BlockListChanges([MarshalAs(UnmanagedType.U1)] bool blockChanges)
	{
		if (m_fBlockChanges != blockChanges)
		{
			m_fBlockChanges = blockChanges;
			if (!blockChanges && m_fEndBulkArrivedDuringBlock)
			{
				DeferredEndBulkEvents(null);
				m_fEndBulkArrivedDuringBlock = false;
			}
			m_pQueryList.ClientBusy(blockChanges);
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				_007EVirtualDatabaseList();
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
