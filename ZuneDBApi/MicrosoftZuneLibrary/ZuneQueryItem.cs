using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class ZuneQueryItem : IDisposable
{
	protected ZuneQueryList m_pList;

	protected uint m_index;

	protected bool m_disposed = false;

	public int ID => (int)GetFieldValue(typeof(int), 355u);

	public ZuneQueryItem(ZuneQueryList pList, int index)
	{
		m_index = (uint)index;
		m_pList = pList;
		pList?.AddRef();
	}

	private void _007EZuneQueryItem()
	{
		if (!m_disposed)
		{
			m_disposed = true;
			ZuneQueryList pList = m_pList;
			if (pList != null)
			{
				pList.Release();
				m_pList = null;
			}
		}
	}

	public object GetFieldValue(Type type, uint Atom)
	{
		return m_pList.GetFieldValue(m_index, type, Atom);
	}

	public void SetFieldValue(uint Atom, object Value)
	{
		m_pList.SetFieldValue(m_index, Atom, Value);
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_007EZuneQueryItem();
		}
		else
		{
			Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
