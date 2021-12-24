using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Util
{
	public class FeaturesChangedApi : IDisposable
	{
		private static FeaturesChangedApi m_singletonInstance;

		private FeaturesChangedHandler m_featuresChangedHandler;

		private readonly CComPtrMgd<IAsyncCallback> m_spAsyncCallback;

		public static FeaturesChangedApi Instance
		{
			get
			{
				if (m_singletonInstance == null)
				{
					m_singletonInstance = new FeaturesChangedApi();
					m_singletonInstance.Initialize();
				}
				return m_singletonInstance;
			}
		}

		public static bool HasInstance
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((m_singletonInstance != null) ? 1u : 0u) != 0;
			}
		}

		[SpecialName]
		public unsafe event FeaturesChangedHandler OnFeaturesChangedEvent
		{
			add
			{
				//IL_001c: Expected I, but got I8
				//IL_003c: Expected I, but got I8
				//IL_005a: Expected I, but got I8
				m_featuresChangedHandler = (FeaturesChangedHandler)Delegate.Combine(m_featuresChangedHandler, value);
				bool featuresHaveChanged = false;
				IFeatureEnablementManager* ptr = null;
				int singleton = Module.GetSingleton(Module.GUID_IFeatureEnablementManager, (void**)(&ptr));
				if (singleton >= 0)
				{
					IFeatureEnablementManager* intPtr = ptr;
					singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, int>)(*(ulong*)(*(long*)ptr + 88)))((nint)intPtr, &featuresHaveChanged);
					if (singleton >= 0)
					{
						value(featuresHaveChanged);
					}
				}
				if (0L != (nint)ptr)
				{
					IFeatureEnablementManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
			}
			remove
			{
				m_featuresChangedHandler = (FeaturesChangedHandler)Delegate.Remove(m_featuresChangedHandler, value);
			}
		}

		public void FeaturesHaveChanged([MarshalAs(UnmanagedType.U1)] bool fFeaturesHaveChanged)
		{
			Application.DeferredInvoke(FeaturesHaveChangedAsync, fFeaturesHaveChanged);
		}

		public void FeaturesHaveChangedAsync(object args)
		{
			m_featuresChangedHandler((bool)args);
		}

		private FeaturesChangedApi()
		{
			CComPtrMgd<IAsyncCallback> spAsyncCallback = new CComPtrMgd<IAsyncCallback>();
			try
			{
				m_spAsyncCallback = spAsyncCallback;
			}
			catch
			{
				//try-fault
				((IDisposable)m_spAsyncCallback).Dispose();
				throw;
			}
		}

		private unsafe int Initialize()
		{
			//IL_0026: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_006d: Expected I, but got I8
			int num = 0;
			FeatureChangedInteropWrapper* ptr = (FeatureChangedInteropWrapper*)Module.@new(24uL);
			FeatureChangedInteropWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002EFeatureChangedInteropWrapper_002E_007Bctor_007D(ptr, FeaturesHaveChanged));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			num = ((0L != (nint)ptr2) ? (-2147024882) : num);
			CComPtrNtv<IAsyncCallback> cComPtrNtv_003CIAsyncCallback_003E = new();
			try
			{
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr2)))((nint)ptr2, (_GUID*)Unsafe.AsPointer(ref Module._GUID_f5fcfd66_9e9a_436a_8b10_aeb4d6ce2b3d), (void**)(cComPtrNtv_003CIAsyncCallback_003E.p));
					if (num >= 0)
					{
						m_spAsyncCallback.op_Assign((IAsyncCallback*)(*(ulong*)(cComPtrNtv_003CIAsyncCallback_003E.p)));
					}
				}
			}
			finally
			{
				cComPtrNtv_003CIAsyncCallback_003E.Dispose();
			}
			return num;
		}

		public void _007EFeaturesChangedApi()
		{
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				m_spAsyncCallback.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
