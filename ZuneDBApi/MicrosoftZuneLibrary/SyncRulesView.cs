using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class SyncRulesView : IDisposable
	{
		private readonly CComPtrMgd<ISyncRulesView> m_spSyncRulesView;

		private readonly CComPtrMgd<SyncRulesViewMediator> m_spSyncRulesViewMediator;

		private SyncRulesViewItemAddedHandler m_itemAddedHandler;

		private SyncRulesViewItemUpdatedHandler m_itemUpdatedHandler;

		private GasGauge m_predictedGasGauge;

		public GasGauge PredictedGasGauge => m_predictedGasGauge;

		public unsafe int Count
		{
			get
			{
				//IL_001c: Expected I, but got I8
				ISyncRulesView* p = m_spSyncRulesView.p;
				if (p != null)
				{
					return (int)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)p + 40)))((nint)p);
				}
				return 0;
			}
		}

		[SpecialName]
		public virtual event SyncRulesViewItemUpdatedHandler ItemUpdatedEvent
		{
			add
			{
				m_itemUpdatedHandler = (SyncRulesViewItemUpdatedHandler)Delegate.Combine(m_itemUpdatedHandler, value);
			}
			remove
			{
				m_itemUpdatedHandler = (SyncRulesViewItemUpdatedHandler)Delegate.Remove(m_itemUpdatedHandler, value);
			}
		}

		[SpecialName]
		public virtual event SyncRulesViewItemAddedHandler ItemAddedEvent
		{
			add
			{
				m_itemAddedHandler = (SyncRulesViewItemAddedHandler)Delegate.Combine(m_itemAddedHandler, value);
			}
			remove
			{
				m_itemAddedHandler = (SyncRulesViewItemAddedHandler)Delegate.Remove(m_itemAddedHandler, value);
			}
		}

		internal unsafe SyncRulesView(ISyncRulesView* pSyncRulesView)
		{
			//IL_002a: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_008e: Expected I, but got I8
			CComPtrMgd<ISyncRulesView> spSyncRulesView = new();
			try
			{
				m_spSyncRulesView = spSyncRulesView;
				CComPtrMgd<SyncRulesViewMediator> spSyncRulesViewMediator = new();
				try
				{
					m_spSyncRulesViewMediator = spSyncRulesViewMediator;
					if (pSyncRulesView != null)
					{
						IGasGauge* ptr = null;
						m_spSyncRulesView.op_Assign(pSyncRulesView);
						ISyncRulesView* p = m_spSyncRulesView.p;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IGasGauge**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, &ptr) >= 0)
						{
							m_predictedGasGauge = new GasGauge(ptr);
						}
						if (0L != (nint)ptr)
						{
							IGasGauge* intPtr = ptr;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
						}
					}
					SyncRulesViewMediator* ptr2 = (SyncRulesViewMediator*)Module.@new(32uL);
					SyncRulesViewMediator* lp;
					try
					{
						lp = ((ptr2 == null) ? null : Module.SyncRulesViewMediator_002E_007Bctor_007D(ptr2, this, pSyncRulesView));
					}
					catch
					{
						//try-fault
						Module.delete(ptr2);
						throw;
					}
					m_spSyncRulesViewMediator.op_Assign(lp);
				}
				catch
				{
					//try-fault
					((IDisposable)m_spSyncRulesViewMediator).Dispose();
					throw;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spSyncRulesView).Dispose();
				throw;
			}
		}

		private unsafe void _007ESyncRulesView()
		{
			//IL_0053: Expected I, but got I8
			GasGauge predictedGasGauge = m_predictedGasGauge;
			if (predictedGasGauge != null)
			{
				((IDisposable)predictedGasGauge).Dispose();
				m_predictedGasGauge = null;
			}
			SyncRulesViewMediator* p = m_spSyncRulesViewMediator.p;
			if (p != null)
			{
				Module.SyncRulesViewMediator_002EShutdown(p);
				m_spSyncRulesViewMediator.Release();
			}
			ISyncRulesView* p2 = m_spSyncRulesView.p;
			if (p2 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void>)(*(ulong*)(*(long*)p2 + 32)))((nint)p2);
				m_spSyncRulesView.Release();
			}
		}

		public unsafe SyncRuleDetails GetItem(int index)
		{
			//IL_0028: Expected I, but got I8
			SyncRuleDetails syncRuleDetails = new SyncRuleDetails();
			ISyncRulesView* p = m_spSyncRulesView.p;
			SSyncRuleDetails sSyncRuleDetails;
			if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, SSyncRuleDetails*, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (uint)index, &sSyncRuleDetails) >= 0)
			{
				syncRuleDetails.syncCategory = sSyncRuleDetails.syncCategory;
				syncRuleDetails.mediaType = sSyncRuleDetails.mediaType;
				syncRuleDetails.allMedia = sSyncRuleDetails.allMedia;
				syncRuleDetails.mediaId = sSyncRuleDetails.mediaId;
				syncRuleDetails.included = sSyncRuleDetails.included;
				syncRuleDetails.complex = sSyncRuleDetails.complex;
				syncRuleDetails.calculated = sSyncRuleDetails.calculated;
				syncRuleDetails.totalItems = sSyncRuleDetails.totalItems;
				syncRuleDetails.totalSize = sSyncRuleDetails.totalSize;
				syncRuleDetails.ignore = sSyncRuleDetails.ignore;
			}
			return syncRuleDetails;
		}

		public unsafe void UpdateItem(int index, [MarshalAs(UnmanagedType.U1)] bool included)
		{
			//IL_001e: Expected I, but got I8
			ISyncRulesView* p = m_spSyncRulesView.p;
			if (p != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, byte, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, (uint)index, included ? ((byte)1) : ((byte)0));
			}
		}

		internal void InvokeItemAdded(uint iItem)
		{
			if (m_itemAddedHandler != null)
			{
				m_itemAddedHandler(this, (int)iItem);
			}
		}

		internal void InvokeItemUpdated(uint iItem)
		{
			if (m_itemUpdatedHandler != null)
			{
				m_itemUpdatedHandler(this, (int)iItem);
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007ESyncRulesView();
				}
				finally
				{
					try
					{
						((IDisposable)m_spSyncRulesViewMediator).Dispose();
					}
					finally
					{
						try
						{
							((IDisposable)m_spSyncRulesView).Dispose();
						}
						finally
						{
						}
					}
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
