using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class CallbackOnUIThreadBimodalManaged_DONOTUSE : IDisposable
	{
		private IRequestCallbackOnUIThread _pUI;

		private unsafe CallbackOnUIThreadBimodalUnmanaged_DONOTUSE* _pUnmanaged;

		public unsafe CallbackOnUIThreadBimodalManaged_DONOTUSE(IRequestCallbackOnUIThread pUI)
		{
			//IL_0026: Expected I, but got I8
			_pUI = pUI;
			CallbackOnUIThreadBimodalUnmanaged_DONOTUSE* ptr = (CallbackOnUIThreadBimodalUnmanaged_DONOTUSE*)Module.@new(16uL);
			CallbackOnUIThreadBimodalUnmanaged_DONOTUSE* pUnmanaged;
			try
			{
				pUnmanaged = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ECallbackOnUIThreadBimodalUnmanaged_DONOTUSE_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			_pUnmanaged = pUnmanaged;
		}

		private void _007ECallbackOnUIThreadBimodalManaged_DONOTUSE()
		{
			_0021CallbackOnUIThreadBimodalManaged_DONOTUSE();
		}

		private unsafe void _0021CallbackOnUIThreadBimodalManaged_DONOTUSE()
		{
			CallbackOnUIThreadBimodalUnmanaged_DONOTUSE* pUnmanaged = _pUnmanaged;
			if (pUnmanaged != null)
			{
				Module.MicrosoftZuneLibrary_002ECallbackOnUIThreadBimodalUnmanaged_DONOTUSE_002E__delDtor(pUnmanaged, 1u);
			}
		}

		public unsafe virtual void CallbackOnUIThreadRequest(CallbackPriorityManaged priority, int id, void* pv, INativeDeferredCallback* pNativeDeferredCallback)
		{
			IntPtr pInterface = (IntPtr)pNativeDeferredCallback;
			IntPtr pData = (IntPtr)pv;
			_pUI.CallbackOnUIThreadRequest(priority, id, pData, pInterface);
		}

		public unsafe void Callback(int id, IntPtr pData, IntPtr pInterface)
		{
			Module.MicrosoftZuneLibrary_002ECallbackOnUIThreadBimodalUnmanaged_DONOTUSE_002ECallback(_pUnmanaged, id, (void*)pData, (INativeDeferredCallback*)pInterface.ToPointer());
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021CallbackOnUIThreadBimodalManaged_DONOTUSE();
				return;
			}
			try
			{
				_0021CallbackOnUIThreadBimodalManaged_DONOTUSE();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~CallbackOnUIThreadBimodalManaged_DONOTUSE()
		{
			Dispose(false);
		}
	}
}
