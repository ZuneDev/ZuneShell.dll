using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class GasGauge : IDisposable
	{
		private readonly CComPtrMgd<IGasGauge> m_spGasGauge;

		private readonly CComPtrMgd<GasGaugeMediator> m_spGasGaugeMediator;

		private CategorySpaceUsedUpdatedHandler m_categorySpaceUsedUpdatedHandler;

		private ReservedSpaceUpdatedHandler m_reservedSpaceUpdatedHandler;

		private DeviceOverflowHandler m_deviceOverflowHandler;

		public unsafe ulong Capacity
		{
			get
			{
				//IL_001c: Expected I, but got I8
				IGasGauge* p = m_spGasGauge.p;
				if (p != null)
				{
					return (ulong)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)p + 56)))((nint)p);
				}
				return 0uL;
			}
		}

		public unsafe long SpaceAvailable
		{
			get
			{
				//IL_001c: Expected I, but got I8
				IGasGauge* p = m_spGasGauge.p;
				if (p != null)
				{
					return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)p + 48)))((nint)p);
				}
				return 0L;
			}
		}

		public unsafe long SpaceReserved
		{
			get
			{
				//IL_001c: Expected I, but got I8
				IGasGauge* p = m_spGasGauge.p;
				if (p != null)
				{
					return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)p + 40)))((nint)p);
				}
				return 0L;
			}
		}

		[SpecialName]
		public virtual event DeviceOverflowHandler DeviceOverflowEvent
		{
			add
			{
				m_deviceOverflowHandler = (DeviceOverflowHandler)Delegate.Combine(m_deviceOverflowHandler, value);
			}
			remove
			{
				m_deviceOverflowHandler = (DeviceOverflowHandler)Delegate.Remove(m_deviceOverflowHandler, value);
			}
		}

		[SpecialName]
		public virtual event ReservedSpaceUpdatedHandler ReservedSpaceUpdatedEvent
		{
			add
			{
				m_reservedSpaceUpdatedHandler = (ReservedSpaceUpdatedHandler)Delegate.Combine(m_reservedSpaceUpdatedHandler, value);
			}
			remove
			{
				m_reservedSpaceUpdatedHandler = (ReservedSpaceUpdatedHandler)Delegate.Remove(m_reservedSpaceUpdatedHandler, value);
			}
		}

		[SpecialName]
		public virtual event CategorySpaceUsedUpdatedHandler CategorySpaceUsedUpdatedEvent
		{
			add
			{
				m_categorySpaceUsedUpdatedHandler = (CategorySpaceUsedUpdatedHandler)Delegate.Combine(m_categorySpaceUsedUpdatedHandler, value);
			}
			remove
			{
				m_categorySpaceUsedUpdatedHandler = (CategorySpaceUsedUpdatedHandler)Delegate.Remove(m_categorySpaceUsedUpdatedHandler, value);
			}
		}

		internal unsafe GasGauge(IGasGauge* pGasGauge)
		{
			//IL_004a: Expected I, but got I8
			CComPtrMgd<IGasGauge> spGasGauge = new CComPtrMgd<IGasGauge>();
			try
			{
				m_spGasGauge = spGasGauge;
				CComPtrMgd<GasGaugeMediator> spGasGaugeMediator = new CComPtrMgd<GasGaugeMediator>();
				try
				{
					m_spGasGaugeMediator = spGasGaugeMediator;
					if (pGasGauge != null)
					{
						m_spGasGauge.op_Assign(pGasGauge);
					}
					GasGaugeMediator* ptr = (GasGaugeMediator*)Module.@new(32uL);
					GasGaugeMediator* lp;
					try
					{
						lp = ((ptr == null) ? null : Module.GasGaugeMediator_002E_007Bctor_007D(ptr, this, pGasGauge));
					}
					catch
					{
						//try-fault
						Module.delete(ptr);
						throw;
					}
					m_spGasGaugeMediator.op_Assign(lp);
				}
				catch
				{
					//try-fault
					((IDisposable)m_spGasGaugeMediator).Dispose();
					throw;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spGasGauge).Dispose();
				throw;
			}
		}

		internal void CategorySpaceUsedUpdated(ESyncCategory syncCategory, long llNewSchemaSpace, long llNewFreeSpace)
		{
			if (m_categorySpaceUsedUpdatedHandler != null)
			{
				m_categorySpaceUsedUpdatedHandler(this, syncCategory, llNewSchemaSpace, llNewFreeSpace);
			}
		}

		internal void ReservedSpaceUpdated(long llNewReservedSpace, long llNewFreeSpace)
		{
			if (m_reservedSpaceUpdatedHandler != null)
			{
				m_reservedSpaceUpdatedHandler(this, llNewReservedSpace, llNewFreeSpace);
			}
		}

		internal void DeviceOverflow()
		{
			if (m_deviceOverflowHandler != null)
			{
				m_deviceOverflowHandler(this);
			}
		}

		private unsafe void _007EGasGauge()
		{
			GasGaugeMediator* p = m_spGasGaugeMediator.p;
			if (p != null)
			{
				Module.GasGaugeMediator_002EShutdown(p);
			}
			m_spGasGauge.Release();
			m_spGasGaugeMediator.Release();
		}

		public unsafe long GetSpaceUsedByCategory(ESyncCategory syncCategory)
		{
			//IL_001d: Expected I, but got I8
			IGasGauge* p = m_spGasGauge.p;
			if (p != null)
			{
				return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ESyncCategory, long>)(*(ulong*)(*(long*)p + 32)))((nint)p, syncCategory);
			}
			return 0L;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EGasGauge();
				}
				finally
				{
					try
					{
						((IDisposable)m_spGasGaugeMediator).Dispose();
					}
					finally
					{
						try
						{
							((IDisposable)m_spGasGauge).Dispose();
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
