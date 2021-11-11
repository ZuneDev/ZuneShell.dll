using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ZuneUI;

namespace Microsoft.Zune.Util
{
	public class PinManager : IDisposable
	{
		private unsafe IPinProvider* m_pPinProvider = null;

		private static PinManager sm_PinManager = null;

		private static object sm_lock = new object();

		public unsafe static PinManager Instance
		{
			get
			{
				//IL_0052: Expected I, but got I8
				//IL_0082: Expected I, but got I8
				//IL_0086: Expected I, but got I8
				if (sm_PinManager == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_PinManager == null)
						{
							PinManager pinManager = new PinManager();
							IMetadataManager* ptr;
							int singleton = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
							if (singleton < 0)
							{
								throw new ApplicationException(_003CModule_003E.GetErrorDescription(singleton));
							}
							IMetadataManager* intPtr = ptr;
							__s_GUID gUID_b396c324_6ab3_4e8e_a5cd_aafb3e01bedc = _003CModule_003E._GUID_b396c324_6ab3_4e8e_a5cd_aafb3e01bedc;
							IPinProvider* pPinProvider;
							int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_b396c324_6ab3_4e8e_a5cd_aafb3e01bedc, (void**)(&pPinProvider));
							if (num < 0)
							{
								throw new ApplicationException(_003CModule_003E.GetErrorDescription(num));
							}
							Thread.MemoryBarrier();
							pinManager.m_pPinProvider = pPinProvider;
							if (0L != (nint)ptr)
							{
								IMetadataManager* intPtr2 = ptr;
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
								ptr = null;
							}
							sm_PinManager = pinManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_PinManager;
			}
		}

		private void _007EPinManager()
		{
			_0021PinManager();
		}

		private unsafe void _0021PinManager()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IPinProvider* pPinProvider = m_pPinProvider;
			if (0L != (nint)pPinProvider)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pPinProvider + 16)))((nint)pPinProvider);
				m_pPinProvider = null;
			}
		}

		public unsafe HRESULT AddPin(EPinType ePinType, string szPinServiceRef, string szDescription, EServiceMediaType ePinServiceTypeId, int nUserId, int nOrdinal, out int nPinId)
		{
			//IL_001b: Incompatible stack types: I8 vs Ref
			//IL_0043: Expected I, but got I8
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szPinServiceRef)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szDescription)))
				{
					int num = -1;
					int* ptr = (int*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref EPinType.ePinTypeQuickMix == ePinType ? ref nOrdinal : ref *(int*)null);
					long num2 = *(long*)m_pPinProvider + 24;
					IPinProvider* pPinProvider = m_pPinProvider;
					int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPinType, ushort*, ushort*, EServiceMediaType, int, int*, int*, int>)(*(ulong*)num2))((nint)pPinProvider, ePinType, ptr2, ptr3, ePinServiceTypeId, nUserId, ptr, &num);
					nPinId = num;
					return new HRESULT(hr);
				}
			}
		}

		public unsafe HRESULT AddPin(EPinType ePinType, int nPinMediaId, EMediaTypes ePinTypeId, int nUserId, int nOrdinal, out int pinId)
		{
			//IL_000b: Incompatible stack types: I8 vs Ref
			//IL_002a: Expected I, but got I8
			int num = -1;
			int* ptr = (int*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref EPinType.ePinTypeQuickMix == ePinType ? ref nOrdinal : ref *(int*)null);
			IPinProvider* pPinProvider = m_pPinProvider;
			int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPinType, int, EMediaTypes, int, int*, int*, int>)(*(ulong*)(*(long*)pPinProvider + 32)))((nint)pPinProvider, ePinType, nPinMediaId, ePinTypeId, nUserId, ptr, &num);
			pinId = num;
			return new HRESULT(hr);
		}

		public unsafe HRESULT FindPin(EPinType ePinType, string szPinServiceRef, EServiceMediaType ePinServiceTypeId, int nUserId, int nMaxAge, out int nPinId)
		{
			//IL_002d: Expected I, but got I8
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szPinServiceRef)))
			{
				int num = -1;
				long num2 = *(long*)m_pPinProvider + 40;
				IPinProvider* pPinProvider = m_pPinProvider;
				int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPinType, ushort*, EServiceMediaType, int, int, int*, int>)(*(ulong*)num2))((nint)pPinProvider, ePinType, ptr, ePinServiceTypeId, nUserId, nMaxAge, &num);
				nPinId = num;
				return new HRESULT(hr);
			}
		}

		public unsafe HRESULT FindPin(EPinType ePinType, int nPinMediaId, EMediaTypes ePinTypeId, int nUserId, int nMaxAge, out int nPinId)
		{
			//IL_001f: Expected I, but got I8
			int num = -1;
			IPinProvider* pPinProvider = m_pPinProvider;
			int hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPinType, int, EMediaTypes, int, int, int*, int>)(*(ulong*)(*(long*)pPinProvider + 48)))((nint)pPinProvider, ePinType, nPinMediaId, ePinTypeId, nUserId, nMaxAge, &num);
			nPinId = num;
			return new HRESULT(hr);
		}

		public unsafe HRESULT DeletePin(int nPinId)
		{
			//IL_0017: Expected I, but got I8
			IPinProvider* pPinProvider = m_pPinProvider;
			return new HRESULT(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pPinProvider + 64)))((nint)pPinProvider, nPinId));
		}

		private unsafe PinManager()
		{
		}//IL_0008: Expected I, but got I8


		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021PinManager();
				return;
			}
			try
			{
				_0021PinManager();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~PinManager()
		{
			Dispose(false);
		}
	}
}
