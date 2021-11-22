using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class UpdateStep : IDisposable
	{
		private readonly CComPtrMgd_003CIFirmwareUpdateCallbackData_003E m_spUpdateStep;

		private int m_cTotalSteps;

		private int m_nStepNumber;

		public int TotalSteps
		{
			get
			{
				EnsureStepInformation();
				return m_cTotalSteps;
			}
		}

		public int StepNumber
		{
			get
			{
				EnsureStepInformation();
				return m_nStepNumber;
			}
		}

		public unsafe int Progress
		{
			get
			{
				//IL_0020: Expected I, but got I8
				uint result = 0u;
				IFirmwareUpdateCallbackData* p = m_spUpdateStep.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 72)))((nint)p, &result);
				}
				return (int)result;
			}
		}

		public unsafe bool HasProgress
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0020: Expected I, but got I8
				int num = 0;
				IFirmwareUpdateCallbackData* p = m_spUpdateStep.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, &num);
					if (num != 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public unsafe bool Cancelable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0020: Expected I, but got I8
				int num = 0;
				IFirmwareUpdateCallbackData* p = m_spUpdateStep.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, &num);
					if (num != 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public unsafe string Name
		{
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0023: Expected I, but got I8
				string result = null;
				ushort* ptr = null;
				IFirmwareUpdateCallbackData* p = m_spUpdateStep.p;
				if (p != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &ptr) >= 0)
				{
					result = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return result;
			}
		}

		internal unsafe UpdateStep(IFirmwareUpdateCallbackData* pUpdateStep)
		{
			CComPtrMgd_003CIFirmwareUpdateCallbackData_003E spUpdateStep = new CComPtrMgd_003CIFirmwareUpdateCallbackData_003E();
			try
			{
				m_spUpdateStep = spUpdateStep;
				m_spUpdateStep.op_Assign(pUpdateStep);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spUpdateStep).Dispose();
				throw;
			}
		}

		private void _007EUpdateStep()
		{
			m_spUpdateStep.Release();
		}

		private unsafe int EnsureStepInformation()
		{
			//IL_0033: Expected I, but got I8
			int num = 0;
			if (m_nStepNumber == 0 && m_cTotalSteps == 0)
			{
				uint nStepNumber = 0u;
				uint cTotalSteps = 0u;
				IFirmwareUpdateCallbackData* p = m_spUpdateStep.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, uint*, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, &nStepNumber, &cTotalSteps);
				if (num >= 0)
				{
					m_nStepNumber = (int)nStepNumber;
					m_cTotalSteps = (int)cTotalSteps;
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
					_007EUpdateStep();
				}
				finally
				{
					((IDisposable)m_spUpdateStep).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
