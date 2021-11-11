using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicrosoftZuneLibrary
{
	internal class ManagedLock : IDisposable
	{
		private object m_pObject;

		private bool m_locked;

		[SpecialName]
		private ManagedLock op_Assign(ManagedLock P_0)
		{
			return this;
		}

		public ManagedLock(object pObject)
		{
			m_pObject = pObject;
			base._002Ector();
			Monitor.Enter(m_pObject);
			m_locked = true;
		}

		private ManagedLock(ManagedLock P_0)
		{
		}

		private void _007EManagedLock()
		{
			if (m_locked)
			{
				Monitor.Exit(m_pObject);
			}
		}

		public void Lock()
		{
			if (!m_locked)
			{
				Monitor.Enter(m_pObject);
				m_locked = true;
			}
		}

		public void Unlock()
		{
			if (m_locked)
			{
				Monitor.Exit(m_pObject);
				m_locked = false;
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EManagedLock();
			}
			else
			{
				Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
