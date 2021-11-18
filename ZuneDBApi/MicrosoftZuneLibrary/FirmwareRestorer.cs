using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class FirmwareRestorer : FirmwareOperationBase
	{
		private readonly CComPtrMgd_003CIFirmwareRestorer_003E m_spFirmwareRestorer;

		private FirmwareUpdater m_Updater;

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool IsRestoreInProgress()
		{
			//IL_00a6: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 76, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = (((long)(nint)m_spFirmwareMediator.p != 0) ? 1 : 0);
			if (!m_Canceled)
			{
				if (Application.IsApplicationThread)
				{
					Module._ZuneShipAssert(1004u, 2313u);
				}
				Monitor.Enter(m_FirmwareLock);
				try
				{
					if (m_fFirmwareProcessSupported && EnsureNativeObject() >= 0)
					{
						IFirmwareRestorer* p = m_spFirmwareRestorer.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &num);
					}
				}
				finally
				{
					Monitor.Exit(m_FirmwareLock);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_ql(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 77, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), m_spFirmwareRestorer.p, num);
			}
			return (byte)((num != 0) ? 1u : 0u) != 0;
		}

		public unsafe HRESULT StartGetRestorePointCollection(DeferredInvokeHandler getCollectionComplete)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 88, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (m_fFirmwareProcessSupported)
			{
				if (getCollectionComplete == null)
				{
					num = -2147418113;
				}
				else if (!ThreadPool.QueueUserWorkItem(StartGetRestorePointCollectionWorker, getCollectionComplete))
				{
					Module._ZuneShipAssert(1004u, 2660u);
					num = -2147467259;
				}
			}
			else
			{
				Module._ZuneShipAssert(1004u, 2667u);
				num = -2147024846;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 89, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe HRESULT StartFirmwareRestore(FirmwareRestorePoint restorePoint, DeferredInvokeHandler restoreBegin, DeferredInvokeHandler restoreProgress, DeferredInvokeHandler restoreComplete)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 78, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_Canceled = false;
			int num = 0;
			if (m_fFirmwareProcessSupported)
			{
				if (restorePoint == null)
				{
					num = -2147418113;
				}
				else
				{
					StartFirmwareRestoreArgs startFirmwareRestoreArgs = new StartFirmwareRestoreArgs(restorePoint, restoreBegin, restoreProgress, restoreComplete);
					if (Application.IsApplicationThread)
					{
						if (!ThreadPool.QueueUserWorkItem(StartFirmwareRestoreWorker, startFirmwareRestoreArgs))
						{
							Module._ZuneShipAssert(1004u, 2375u);
							num = -2147467259;
						}
					}
					else
					{
						StartFirmwareRestoreWorker(startFirmwareRestoreArgs);
						num = startFirmwareRestoreArgs.HrStatus;
					}
				}
			}
			else
			{
				Module._ZuneShipAssert(1004u, 2388u);
				num = -2147024846;
			}
			return num;
		}

		public unsafe HRESULT Cancel()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 92, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (m_fFirmwareProcessSupported)
			{
				m_Canceled = true;
				if (Application.IsApplicationThread)
				{
					if (!ThreadPool.QueueUserWorkItem(CancelWorker, null))
					{
						Module._ZuneShipAssert(1004u, 2735u);
					}
				}
				else
				{
					CancelWorker(null);
				}
			}
			else
			{
				Module._ZuneShipAssert(1004u, 2745u);
				num = -2147024846;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 93, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			return num;
		}

		internal unsafe FirmwareRestorer(IEndpointHost* pEndpointHost, [MarshalAs(UnmanagedType.U1)] bool fFirmwareProcessSupported, FirmwareUpdater updater)
		{
			CComPtrMgd_003CIFirmwareRestorer_003E spFirmwareRestorer = new CComPtrMgd_003CIFirmwareRestorer_003E();
			try
			{
				m_spFirmwareRestorer = spFirmwareRestorer;
				base._002Ector();
				try
				{
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
					{
						Module.WPP_SF_q(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 72, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), pEndpointHost);
					}
					m_spEndpointHost.op_Assign(pEndpointHost);
					m_fFirmwareProcessSupported = fFirmwareProcessSupported;
					m_Updater = updater;
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
					{
						Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 73, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
					}
				}
				catch
				{
					//try-fault
					base.Dispose(true);
					throw;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spFirmwareRestorer).Dispose();
				throw;
			}
		}

		private unsafe void _007EFirmwareRestorer()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 74, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			if (IsRestoreInProgress())
			{
				Module._ZuneShipAssert(1004u, 2290u);
			}
			m_spFirmwareRestorer.Release();
			m_Updater = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 75, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		internal unsafe override int InternalReset()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 85, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_Updater?.InternalReset();
			return 0;
		}

		internal override int ContinueFirmwareProcess(FirmwareCompleteHandler onCompleteHandler)
		{
			int result = 0;
			if (!ThreadPool.QueueUserWorkItem(state: new ContinueFirmwareUpdateArgs(onCompleteHandler), callBack: ContinueFirmwareRestoreWorker))
			{
				Module._ZuneShipAssert(1004u, 2470u);
				result = -2147467259;
				SendCompleteNotification(-2147467259, disconnectDeviceOnComplete: false);
			}
			return result;
		}

		internal unsafe override void ReleaseNativeObject()
		{
			//IL_0055: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 87, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				m_spFirmwareRestorer.op_Assign(null);
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
		}

		private unsafe int EnsureNativeObject()
		{
			//IL_00b3: Expected I, but got I8
			//IL_00bd: Expected I, but got I8
			//IL_00c8: Expected I, but got I8
			//IL_00d6: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 86, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int result = 0;
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2582u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				if (!m_fFirmwareProcessSupported)
				{
					int num = -2147024846;
					return -2147024846;
				}
				if (m_spFirmwareRestorer.p == null)
				{
					CComPtrNtv_003CIFirmwareUpdater_003E cComPtrNtv_003CIFirmwareUpdater_003E;
					*(long*)(&cComPtrNtv_003CIFirmwareUpdater_003E) = 0L;
					try
					{
						result = m_Updater.GetNativeObject((IFirmwareUpdater**)(&cComPtrNtv_003CIFirmwareUpdater_003E));
						if (result >= 0 && *(long*)(&cComPtrNtv_003CIFirmwareUpdater_003E) != 0L)
						{
							CComPtrNtv_003CIFirmwareRestorer_003E cComPtrNtv_003CIFirmwareRestorer_003E;
							*(long*)(&cComPtrNtv_003CIFirmwareRestorer_003E) = 0L;
							try
							{
								IFirmwareUpdater* ptr = (IFirmwareUpdater*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater_003E));
								result = Module.IUnknown_002EQueryInterface_003Cstruct_0020IFirmwareRestorer_003E((IUnknown*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater_003E)), (IFirmwareRestorer**)(&cComPtrNtv_003CIFirmwareRestorer_003E));
								if (result >= 0)
								{
									IFirmwareRestorer* ptr2 = (IFirmwareRestorer*)(*(ulong*)(&cComPtrNtv_003CIFirmwareRestorer_003E));
									m_spFirmwareRestorer.op_Assign((IFirmwareRestorer*)(*(ulong*)(&cComPtrNtv_003CIFirmwareRestorer_003E)));
								}
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareRestorer_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareRestorer_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareRestorer_003E);
								throw;
							}
							Module.CComPtrNtv_003CIFirmwareRestorer_003E_002ERelease(&cComPtrNtv_003CIFirmwareRestorer_003E);
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdater_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdater_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdater_003E);
						throw;
					}
					Module.CComPtrNtv_003CIFirmwareUpdater_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdater_003E);
					return result;
				}
				return result;
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
		}

		private unsafe void ContinueFirmwareRestoreWorker(object data)
		{
			//IL_015e: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 80, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (m_Canceled)
			{
				num = -2147467260;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 81, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			else if ((long)(nint)m_spFirmwareMediator.p == 0)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 2u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 82, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				num = -2147418113;
			}
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2514u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				if (num >= 0)
				{
					num = EnsureNativeObject();
					if (num >= 0)
					{
						ContinueFirmwareUpdateArgs continueFirmwareUpdateArgs = (ContinueFirmwareUpdateArgs)data;
						if (continueFirmwareUpdateArgs.OnCompleteHandler != null)
						{
							m_OnCompleteHandler = continueFirmwareUpdateArgs.OnCompleteHandler;
						}
						IFirmwareRestorer* p = m_spFirmwareRestorer.p;
						FirmwareUpdateMediator* p2 = m_spFirmwareMediator.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareUpdateCallback*, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (IFirmwareUpdateCallback*)p2);
					}
				}
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			if (num < 0)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 2u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 83, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
				}
				SendCompleteNotification(num, disconnectDeviceOnComplete: false);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 84, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
		}

		private unsafe void StartFirmwareRestoreWorker(object data)
		{
			//IL_0014: Expected I, but got I8
			//IL_0053: Expected I, but got I8
			//IL_00da: Expected I, but got I8
			StartFirmwareRestoreArgs startFirmwareRestoreArgs = (StartFirmwareRestoreArgs)data;
			m_spFirmwareMediator.op_Assign(null);
			m_HandlerComplete = startFirmwareRestoreArgs.FirmwareUpdateComplete;
			int num = 0;
			FirmwareUpdateMediator* ptr = (FirmwareUpdateMediator*)Module.@new(64uL);
			FirmwareUpdateMediator* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002EFirmwareUpdateMediator_002E_007Bctor_007D(ptr, startFirmwareRestoreArgs.FirmwareUpdateBegin, startFirmwareRestoreArgs.FirmwareUpdateProgress, base.OnFirmwareProcessCompleteWorker));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				m_spFirmwareMediator.op_Assign(ptr2);
			}
			else
			{
				num = -2147024882;
			}
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2424u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				if (num >= 0)
				{
					num = EnsureNativeObject();
					if (num >= 0)
					{
						IFirmwareRestorer* p = m_spFirmwareRestorer.p;
						FirmwareUpdateMediator* p2 = m_spFirmwareMediator.p;
						IFirmwareRestorePoint* nativeRestorePointPtr = startFirmwareRestoreArgs.RestorePoint.NativeRestorePointPtr;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareRestorePoint*, IFirmwareUpdateCallback*, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, nativeRestorePointPtr, (IFirmwareUpdateCallback*)p2);
					}
				}
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			startFirmwareRestoreArgs.HrStatus = num;
			if (num < 0)
			{
				Reset(deviceRebooting: false);
				SendCompleteNotification(num, disconnectDeviceOnComplete: false);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 79, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
		}

		private unsafe void StartGetRestorePointCollectionWorker(object data)
		{
			//IL_0092: Expected I, but got I8
			//IL_00a5: Expected I, but got I8
			//IL_00ad: Expected I, but got I8
			CComPtrNtv_003CIFirmwareRestorePointCollection_003E cComPtrNtv_003CIFirmwareRestorePointCollection_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareRestorePointCollection_003E) = 0L;
			try
			{
				DeferredInvokeHandler deferredInvokeHandler = (DeferredInvokeHandler)data;
				int num = 0;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 90, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				if (Application.IsApplicationThread)
				{
					Module._ZuneShipAssert(1004u, 2687u);
				}
				Monitor.Enter(m_FirmwareLock);
				try
				{
					num = EnsureNativeObject();
					if (num >= 0)
					{
						IFirmwareRestorer* p = m_spFirmwareRestorer.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareRestorePointCollection**, int>)(*(ulong*)(*(long*)p + 24)))((nint)p, (IFirmwareRestorePointCollection**)(&cComPtrNtv_003CIFirmwareRestorePointCollection_003E));
					}
					if (deferredInvokeHandler != null)
					{
						FirmwareRestorePointCollection args;
						if (num >= 0)
						{
							IFirmwareRestorePointCollection* ptr = (IFirmwareRestorePointCollection*)(*(ulong*)(&cComPtrNtv_003CIFirmwareRestorePointCollection_003E));
							args = new FirmwareRestorePointCollection((IFirmwareRestorePointCollection*)(*(ulong*)(&cComPtrNtv_003CIFirmwareRestorePointCollection_003E)));
						}
						else
						{
							args = null;
						}
						deferredInvokeHandler(args);
					}
				}
				finally
				{
					Monitor.Exit(m_FirmwareLock);
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 91, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareRestorePointCollection_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareRestorePointCollection_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareRestorePointCollection_003E);
				throw;
			}
			Module.CComPtrNtv_003CIFirmwareRestorePointCollection_003E_002ERelease(&cComPtrNtv_003CIFirmwareRestorePointCollection_003E);
		}

		private unsafe void CancelWorker(object data)
		{
			//IL_005c: Expected I, but got I8
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2758u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				CComPtrMgd_003CMicrosoftZuneLibrary_003A_003AFirmwareUpdateMediator_003E spFirmwareMediator = m_spFirmwareMediator;
				if (spFirmwareMediator.p != null)
				{
					Module.MicrosoftZuneLibrary_002EFirmwareUpdateMediator_002EFirmwareProcessCanceled(spFirmwareMediator.p);
				}
				CComPtrMgd_003CIFirmwareRestorer_003E spFirmwareRestorer = m_spFirmwareRestorer;
				if (spFirmwareRestorer.p != null)
				{
					IFirmwareRestorer* p = spFirmwareRestorer.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 56)))((nint)p) < 0)
					{
						Module._ZuneShipAssert(1004u, 2773u);
					}
					Reset(deviceRebooting: false);
				}
				else if (IsDeviceRebooting())
				{
					DeferredCancel();
				}
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 94, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareRestorer();
				}
				finally
				{
					try
					{
						base.Dispose(true);
					}
					finally
					{
						((IDisposable)m_spFirmwareRestorer).Dispose();
					}
				}
			}
			else
			{
				base.Dispose(false);
			}
		}
	}
}
