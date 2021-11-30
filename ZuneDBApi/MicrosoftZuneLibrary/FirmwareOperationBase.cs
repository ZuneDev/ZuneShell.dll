using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;
using Microsoft.Zune.Util;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public abstract class FirmwareOperationBase : IDisposable
	{
		internal readonly CComPtrMgd<IEndpointHost> m_spEndpointHost;

		internal object m_FirmwareLock;

		internal bool m_fFirmwareProcessSupported;

		internal bool m_Canceled;

		internal bool m_DeviceRebooting;

		internal readonly CComPtrMgd<FirmwareUpdateMediator> m_spFirmwareMediator;

		internal FirmwareCompleteHandler m_OnCompleteHandler;

		internal DeferredInvokeHandler m_HandlerComplete;

		[return: MarshalAs(UnmanagedType.U1)]
		public bool EnterContinuousPowerMode()
		{
			return (Module.SetThreadExecutionState(2147483649u) != 0) ? true : false;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public bool LeaveContinuousPowerMode()
		{
			return (Module.SetThreadExecutionState(2147483648u) != 0) ? true : false;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public virtual bool IsValid()
		{
			return true;
		}

		public HRESULT CheckPowerRequirements(out bool fOnBatteryPower)
		{
            return PowerRequirements.CheckOnBatteryPower(out fOnBatteryPower);
		}

		internal FirmwareOperationBase()
		{
			CComPtrMgd<IEndpointHost> spEndpointHost = new CComPtrMgd<IEndpointHost>();
			try
			{
				m_spEndpointHost = spEndpointHost;
				CComPtrMgd<FirmwareUpdateMediator> spFirmwareMediator = new();
				try
				{
					m_spFirmwareMediator = spFirmwareMediator;
					m_Canceled = false;
					m_FirmwareLock = new object();
				}
				catch
				{
					//try-fault
					((IDisposable)m_spFirmwareMediator).Dispose();
					throw;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spEndpointHost).Dispose();
				throw;
			}
		}

		private void _007EFirmwareOperationBase()
		{
			m_spEndpointHost.Release();
			m_spFirmwareMediator.Release();
		}

		[return: MarshalAs(UnmanagedType.U1)]
		internal unsafe bool IsDeviceRebooting()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			return m_DeviceRebooting;
		}

		internal unsafe int Reset([MarshalAs(UnmanagedType.U1)] bool deviceRebooting)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int result = InternalReset();
			m_DeviceRebooting = deviceRebooting;
			return result;
		}

		internal unsafe void SendCompleteNotification(int hr, [MarshalAs(UnmanagedType.U1)] bool disconnectDeviceOnComplete)
		{
			//IL_001e: Expected I, but got I8
			//IL_0038: Expected I, but got I8
			CComPtrNtv<IFirmwareUpdateErrorInfo> cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E = new();
			try
			{
				InternalErrorInfo* ptr = (InternalErrorInfo*)Module.@new(56uL);
				InternalErrorInfo* lp;
				try
				{
					lp = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002EInternalErrorInfo_002E_007Bctor_007D(ptr, hr));
				}
				catch
				{
					//try-fault
					Module.delete(ptr);
					throw;
				}
				cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E.set_p((IFirmwareUpdateErrorInfo*)lp);
				FirmwareProcessCompleteArgs args = new FirmwareProcessCompleteArgs(new FirmwareUpdateErrorInfo((IFirmwareUpdateErrorInfo*)(*(ulong*)(cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E.p))), null, CompletionAction.Complete, disconnectDeviceOnComplete);
				Application.DeferredInvokeOnWorkerThread(ProcessCompleteWorker, ProcessCompleteWorkerComplete, args);
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E.Dispose();
		}

		internal unsafe void OnFirmwareProcessCompleteWorker(object data)
		{
			FirmwareProcessCompleteArgs firmwareProcessCompleteArgs = (FirmwareProcessCompleteArgs)data;
			switch (firmwareProcessCompleteArgs.Action)
			{
			case CompletionAction.Complete:
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
				{
					Module.WPP_SF_d(_a1: firmwareProcessCompleteArgs.ErrorInfo.HrStatus.hr, Logger: *(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), id: 23, TraceGuid: (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				ProcessCompleteWorker(firmwareProcessCompleteArgs);
				break;
			case CompletionAction.Rollback:
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 2u)
				{
					Module.WPP_SF_d(_a1: firmwareProcessCompleteArgs.ErrorInfo.HrStatus.hr, Logger: *(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), id: 22, TraceGuid: (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				DeferredInvokeHandler handlerComplete2 = m_HandlerComplete;
				if (handlerComplete2 != null)
				{
					handlerComplete2(firmwareProcessCompleteArgs);
				}
				ContinueFirmwareProcess(null);
				break;
			}
			case CompletionAction.Reboot:
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
				{
					Module.WPP_SF_d(_a1: firmwareProcessCompleteArgs.ErrorInfo.HrStatus.hr, Logger: *(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), id: 21, TraceGuid: (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				Reset(deviceRebooting: true);
				DeferredInvokeHandler handlerComplete = m_HandlerComplete;
				if (handlerComplete != null)
				{
					handlerComplete(firmwareProcessCompleteArgs);
				}
				break;
			}
			case CompletionAction.Continue:
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 6u)
				{
					Module.WPP_SF_d(_a1: firmwareProcessCompleteArgs.ErrorInfo.HrStatus.hr, Logger: *(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), id: 20, TraceGuid: (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				ContinueFirmwareProcess(null);
				break;
			}
		}

		internal unsafe void ProcessCompleteWorker(object data)
		{
			//IL_006c: Expected I, but got I8
			FirmwareProcessCompleteArgs firmwareProcessCompleteArgs = (FirmwareProcessCompleteArgs)data;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(_a1: firmwareProcessCompleteArgs.ErrorInfo.HrStatus.hr, Logger: *(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), id: 24, TraceGuid: (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			Reset(deviceRebooting: false);
			m_spFirmwareMediator.op_Assign(null);
			if (m_OnCompleteHandler != null)
			{
				HRESULT hrStatus = firmwareProcessCompleteArgs.ErrorInfo.HrStatus;
				m_OnCompleteHandler(hrStatus.hr);
				m_OnCompleteHandler = null;
			}
			if (m_HandlerComplete != null)
			{
				m_HandlerComplete(firmwareProcessCompleteArgs);
				m_HandlerComplete = null;
			}
			if (firmwareProcessCompleteArgs.DisconnectDeviceOnComplete)
			{
				IEndpointHost* p = m_spEndpointHost.p;
				DeviceList.Instance.DeviceDisconnected(p);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 25, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		internal void ProcessCompleteWorkerComplete(object P_0)
		{
		}

		internal unsafe void DeferredCancel()
		{
			//IL_0086: Expected I, but got I8
			//IL_00cd: Expected I, but got I8
			//IL_00e1: Expected I, but got I8
			//IL_00e1: Expected I, but got I8
			//IL_00e1: Expected I, but got I8
			CComPtrNtv<IFirmwareUpdateNotification> cComPtrNtv_003CIFirmwareUpdateNotification_003E = new();
			try
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				m_DeviceRebooting = false;
				int firmwareUpdateNotification = GetFirmwareUpdateNotification((IFirmwareUpdateNotification**)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p));
				if (*(long*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p) != 0L && firmwareUpdateNotification >= 0)
				{
					string WBSTRString = "";
					try
					{
						IEndpointHost* p = m_spEndpointHost.p;
						firmwareUpdateNotification = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyEndpointId, (ushort**)(&wBSTRString));
						if (firmwareUpdateNotification >= 0)
						{
							if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
							{
								Module.WPP_SF_S(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 27, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), (ushort*)(*(ulong*)(&wBSTRString)));
							}
							long num = *(long*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p);
							long num2 = *(long*)(&wBSTRString);
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p)) + 48)))((nint)num, (ushort*)num2);
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
				}
				SendCompleteNotification(-2147467260, disconnectDeviceOnComplete: true);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 28, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
		}

		internal unsafe int GetFirmwareUpdateNotification(IFirmwareUpdateNotification** ppNotification)
		{
			//IL_0152: Expected I, but got I8
			//IL_0152: Expected I, but got I8
			//IL_016b: Expected I, but got I8
			//IL_016b: Expected I, but got I8
			//IL_0174: Expected I, but got I8
			//IL_017c: Expected I8, but got I
			CComPtrNtv<IEndpointHostManager> cComPtrNtv_003CIEndpointHostManager_003E = new();
			CComPtrNtv<IEndpointManager> cComPtrNtv_003CIEndpointManager_003E = new();
			CComPtrNtv<IFirmwareUpdateNotification> cComPtrNtv_003CIFirmwareUpdateNotification_003E = new();
			EEndpointClass eEndpointClass;
			try
			{
				*(long*)(cComPtrNtv_003CIEndpointManager_003E.p) = 0L;
				try
				{
					*(long*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p) = 0L;
					try
					{
						eEndpointClass = EEndpointClass.eEndpointClassInvalid;
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 29, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
						}
						if (ppNotification == null)
						{
							Module._ZuneShipAssert(1001u, 1289u);
							goto IL_0072;
						}
					}
					catch
					{
						//try-fault
						cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
						throw;
					}
					goto end_IL_000a;
					IL_0072:
					cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
					goto IL_0089;
					end_IL_000a:;
				}
				catch
				{
					//try-fault
					cComPtrNtv_003CIEndpointManager_003E.Dispose();
					throw;
				}
				goto end_IL_0005;
				IL_0089:
				cComPtrNtv_003CIEndpointManager_003E.Dispose();
				goto IL_00a0;
				end_IL_0005:;
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
				throw;
			}
			CComPtrMgd<IEndpointHost> spEndpointHost;
			try
			{
				try
				{
					try
					{
						spEndpointHost = m_spEndpointHost;
						if (spEndpointHost.p == null)
						{
							Module._ZuneShipAssert(1002u, 1290u);
							goto IL_00de;
						}
					}
					catch
					{
						//try-fault
						cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
						throw;
					}
					goto end_IL_00ae;
					IL_00de:
					cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
					goto IL_00f5;
					end_IL_00ae:;
				}
				catch
				{
					//try-fault
					cComPtrNtv_003CIEndpointManager_003E.Dispose();
					throw;
				}
				goto end_IL_00ae_2;
				IL_00f5:
				cComPtrNtv_003CIEndpointManager_003E.Dispose();
				goto IL_010c;
				end_IL_00ae_2:;
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
				throw;
			}
			int num;
			try
			{
				try
				{
					try
					{
						num = Module.GetEnumProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cenum_0020EEndpointClass_003E(spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyClassId, &eEndpointClass);
						if (num >= 0)
						{
							num = Module.GetSingleton(Module.GUID_IEndpointHostManager, (void**)(cComPtrNtv_003CIEndpointHostManager_003E.p));
							if (num >= 0)
							{
								long num2 = *(long*)(cComPtrNtv_003CIEndpointHostManager_003E.p);
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IEndpointManager**, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIEndpointHostManager_003E.p)) + 88)))((nint)num2, (IEndpointManager**)(cComPtrNtv_003CIEndpointManager_003E.p));
								if (num >= 0)
								{
									long num3 = *(long*)(cComPtrNtv_003CIEndpointManager_003E.p);
									EEndpointClass num4 = eEndpointClass;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointClass, IFirmwareUpdateNotification**, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIEndpointManager_003E.p)) + 88)))((nint)num3, num4, (IFirmwareUpdateNotification**)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p));
									if (num >= 0)
									{
										IFirmwareUpdateNotification* ptr = (IFirmwareUpdateNotification*)(*(ulong*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p));
										*(long*)(cComPtrNtv_003CIFirmwareUpdateNotification_003E.p) = 0L;
										*(long*)ppNotification = (nint)ptr;
									}
								}
							}
						}
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 30, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
						}
					}
					catch
					{
						//try-fault
						cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
						throw;
					}
					cComPtrNtv_003CIFirmwareUpdateNotification_003E.Dispose();
				}
				catch
				{
					//try-fault
					cComPtrNtv_003CIEndpointManager_003E.Dispose();
					throw;
				}
				cComPtrNtv_003CIEndpointManager_003E.Dispose();
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
			return num;
			IL_00a0:
			cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
			return -2147467261;
			IL_010c:
			cComPtrNtv_003CIEndpointHostManager_003E.Dispose();
			return -2147418113;
		}

		internal abstract int ContinueFirmwareProcess(FirmwareCompleteHandler onCompleteHandler);

		internal abstract void ReleaseNativeObject();

		internal abstract int InternalReset();

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareOperationBase();
				}
				finally
				{
					try
					{
						((IDisposable)m_spFirmwareMediator).Dispose();
					}
					finally
					{
						try
						{
							((IDisposable)m_spEndpointHost).Dispose();
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
