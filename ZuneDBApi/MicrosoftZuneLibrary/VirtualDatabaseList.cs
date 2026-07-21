using System;
using System.Collections;
using System.Reflection;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Real logic transcribed verbatim from the original decompiled body (it's pure C#/Iris
// framework code, no native pointers involved) — only the ETW perf-tracing calls
// (PERFTRACE_COLLECTIONEVENT) were dropped, matching how WPP/ETW tracing is handled
// elsewhere in this codebase (diagnostic-only, not behavior). See
// logs/MicrosoftZuneLibrary/ZuneLibrary.md for the base-class recovery this fixes.
[DefaultMember("Item")]
public class VirtualDatabaseList : VirtualList, IQueryListEvents, IDisposable
{
    protected int m_QueryRN;
    protected ZuneQueryList m_pQueryList;
    protected bool m_fInBulkEvents;
    protected bool m_fDisposed;
    protected ArrayList m_lNotifyData = new();
    protected int m_blockChangesFlags;
    protected bool m_fBlockChanges;
    protected bool m_fEndBulkArrivedDuringBlock;
    protected bool m_fItemsAdded;

    internal VirtualDatabaseList(ZuneQueryList pQueryList, bool enableSlowDataRequests)
        : base(enableSlowDataRequests)
    {
        m_pQueryList = pQueryList;
    }

    public virtual void ListInsert(int index)
    {
        if (m_fInBulkEvents)
        {
            m_lNotifyData.Add(new ListNotifyData { type = 0, pos = index });
        }
        else
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(DeferredInsert), index);
        }
    }

    public virtual void ListRemoveAt(int index)
    {
        if (m_fInBulkEvents)
        {
            m_lNotifyData.Add(new ListNotifyData { type = 1, pos = index });
        }
        else
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(DeferredRemoveAt), index);
        }
    }

    public virtual void ListModified(int index)
    {
        if (m_fInBulkEvents)
        {
            m_lNotifyData.Add(new ListNotifyData { type = 2, pos = index });
        }
        else
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(DeferredModified), index);
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
        Application.DeferredInvoke(new DeferredInvokeHandler(DeferredEndBulkEvents), null);
    }

    public virtual void ListNotifyCount(uint count)
    {
        Application.DeferredInvoke(new DeferredInvokeHandler(DeferredCountChange), (int)count);
    }

    public void SetBlockChangesFlag(int flag, bool block)
    {
        if (block)
            m_blockChangesFlags |= flag;
        else
            m_blockChangesFlags &= ~flag;
        BlockListChanges(m_blockChangesFlags != 0);
    }

    public int GetQueryRN() => m_QueryRN;

    protected virtual void InvalidateItem(int index)
    {
        Modified(index);
    }

    private void DeferredEndBulkEvents(object args)
    {
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
            m_pQueryList.EndBulkEventsComplete(true);
            m_fItemsAdded = false;
            return;
        }
        bool countChanged = false;
        foreach (ListNotifyData entry in m_lNotifyData)
        {
            if (m_fDisposed)
                break;
            switch (entry.type)
            {
                case 2:
                    DeferredModified(entry.pos);
                    break;
                case 1:
                    DeferredRemoveAt(entry.pos);
                    countChanged = true;
                    break;
                case 0:
                    DeferredInsert(entry.pos);
                    countChanged = true;
                    break;
            }
        }
        m_lNotifyData.Clear();
        m_pQueryList.EndBulkEventsComplete(false);
        if (countChanged)
            m_QueryRN++;
    }

    private void DeferredInsert(object args)
    {
        if (!m_fDisposed && Count > (int)args)
            Insert((int)args);
    }

    private void DeferredRemoveAt(object args)
    {
        if (!m_fDisposed && Count > 0 && Count > (int)args)
            RemoveAt((int)args);
    }

    private void DeferredModified(object args)
    {
        if (!m_fDisposed && Count > (int)args && IsItemAvailable((int)args))
            InvalidateItem((int)args);
    }

    private void DeferredCountChange(object args)
    {
        int newCount = (int)args;
        if (m_fDisposed)
            return;
        if (newCount > Count)
            AddRange(newCount - Count);
        else
            Count = newCount;
    }

    private void BlockListChanges(bool blockChanges)
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

    protected override void OnDispose(bool disposing)
    {
        m_fDisposed = true;
        base.OnDispose(disposing);
    }
}
