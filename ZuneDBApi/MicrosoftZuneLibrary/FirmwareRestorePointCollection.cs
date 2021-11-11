using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class FirmwareRestorePointCollection : IDisposable
	{
		private readonly CComPtrMgd_003CIFirmwareRestorePointCollection_003E m_spRestorePointCollection;

		public unsafe int Count
		{
			get
			{
				//IL_0022: Expected I, but got I8
				int num = 0;
				uint num2 = 0u;
				IFirmwareRestorePointCollection* p = m_spRestorePointCollection.p;
				if (p != null)
				{
					num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, &num2) >= 0) ? ((int)num2) : num);
				}
				return num;
			}
		}

		public unsafe FirmwareRestorePoint GetRestorePoint(int index)
		{
			//IL_002f: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			FirmwareRestorePoint result = null;
			CComPtrNtv_003CIFirmwareRestorePoint_003E cComPtrNtv_003CIFirmwareRestorePoint_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareRestorePoint_003E) = 0L;
			try
			{
				CComPtrMgd_003CIFirmwareRestorePointCollection_003E spRestorePointCollection = m_spRestorePointCollection;
				if (spRestorePointCollection.p != null)
				{
					IFirmwareRestorePointCollection* p = spRestorePointCollection.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IFirmwareRestorePoint**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, (uint)index, (IFirmwareRestorePoint**)(&cComPtrNtv_003CIFirmwareRestorePoint_003E)) >= 0)
					{
						result = new FirmwareRestorePoint((IFirmwareRestorePoint*)(*(ulong*)(&cComPtrNtv_003CIFirmwareRestorePoint_003E)));
					}
				}
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareRestorePoint_003E*, void>)(&_003CModule_003E.CComPtrNtv_003CIFirmwareRestorePoint_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareRestorePoint_003E);
				throw;
			}
			_003CModule_003E.CComPtrNtv_003CIFirmwareRestorePoint_003E_002ERelease(&cComPtrNtv_003CIFirmwareRestorePoint_003E);
			return result;
		}

		internal unsafe FirmwareRestorePointCollection(IFirmwareRestorePointCollection* pRestorePointCollection)
		{
			CComPtrMgd_003CIFirmwareRestorePointCollection_003E spRestorePointCollection = new CComPtrMgd_003CIFirmwareRestorePointCollection_003E();
			try
			{
				m_spRestorePointCollection = spRestorePointCollection;
				base._002Ector();
				if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					_003CModule_003E.WPP_SF_q(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 48uL), 68, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), pRestorePointCollection);
				}
				m_spRestorePointCollection.op_Assign(pRestorePointCollection);
				if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 48uL), 69, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spRestorePointCollection).Dispose();
				throw;
			}
		}

		private unsafe void _007EFirmwareRestorePointCollection()
		{
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 48uL), 70, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_spRestorePointCollection.Release();
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 48uL), 71, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareRestorePointCollection();
				}
				finally
				{
					((IDisposable)m_spRestorePointCollection).Dispose();
				}
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
