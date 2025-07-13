using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class WinLiveAvailableInformation : IDisposable
{
	private readonly CComPtrMgd_003CIWinLiveAvailableInformation_003E m_spWinLiveAvailableInformation;

	private string m_signinName;

	private IList m_suggestedNames;

	public unsafe IList SuggestedNames
	{
		get
		{
			//IL_002a: Expected I, but got I8
			//IL_0040: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			if (m_suggestedNames == null)
			{
				IWinLiveAvailableInformation* p = m_spWinLiveAvailableInformation.p;
				if (p != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 48)))((nint)p);
					m_suggestedNames = new ArrayList(num);
					int num2 = 0;
					if (0 < num)
					{
						do
						{
							ushort* ptr = null;
							IWinLiveAvailableInformation* p2 = m_spWinLiveAvailableInformation.p;
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int>)(*(ulong*)(*(long*)p2 + 56)))((nint)p2, num2, &ptr) >= 0)
							{
								m_suggestedNames.Add(new string((char*)ptr));
							}
							global::_003CModule_003E.SysFreeString(ptr);
							num2++;
						}
						while (num2 < num);
					}
				}
			}
			return m_suggestedNames;
		}
	}

	public unsafe bool Available
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			//IL_001e: Expected I, but got I8
			bool result = false;
			IWinLiveAvailableInformation* p = m_spWinLiveAvailableInformation.p;
			if (p != null)
			{
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)p + 40)))((nint)p) != 0;
			}
			return result;
		}
	}

	public unsafe string SigninName
	{
		get
		{
			//IL_0020: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			if (m_signinName == null)
			{
				CComPtrMgd_003CIWinLiveAvailableInformation_003E spWinLiveAvailableInformation = m_spWinLiveAvailableInformation;
				if (spWinLiveAvailableInformation.p != null)
				{
					ushort* ptr = null;
					IWinLiveAvailableInformation* p = spWinLiveAvailableInformation.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &ptr) >= 0)
					{
						m_signinName = new string((char*)ptr);
					}
					global::_003CModule_003E.SysFreeString(ptr);
				}
			}
			return m_signinName;
		}
	}

	internal unsafe WinLiveAvailableInformation(IWinLiveAvailableInformation* pWinLiveAvailableInformation)
	{
		CComPtrMgd_003CIWinLiveAvailableInformation_003E spWinLiveAvailableInformation = new CComPtrMgd_003CIWinLiveAvailableInformation_003E();
		try
		{
			m_spWinLiveAvailableInformation = spWinLiveAvailableInformation;
			base._002Ector();
			m_spWinLiveAvailableInformation.op_Assign(pWinLiveAvailableInformation);
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)m_spWinLiveAvailableInformation).Dispose();
			throw;
		}
	}

	public void _007EWinLiveAvailableInformation()
	{
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				return;
			}
			finally
			{
				((IDisposable)m_spWinLiveAvailableInformation).Dispose();
			}
		}
		Finalize();
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
