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
		internal readonly CComPtrMgd_003CIEndpointHost_003E m_spEndpointHost;

		internal object m_FirmwareLock;

		internal bool m_fFirmwareProcessSupported;

		internal bool m_Canceled;

		internal bool m_DeviceRebooting;

		internal readonly CComPtrMgd_003CMicrosoftZuneLibrary_003A_003AFirmwareUpdateMediator_003E m_spFirmwareMediator;

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
			bool fOnBatteryPower2 = true;
			HRESULT hRESULT = PowerRequirements.CheckOnBatteryPower(out fOnBatteryPower2);
			HRESULT hRESULT2 = hRESULT;
			if ((byte)((hRESULT2.hr >= 0) ? 1u : 0u) != 0)
			{
				fOnBatteryPower = fOnBatteryPower2;
			}
			return hRESULT;
		}

		internal FirmwareOperationBase()
		{
			CComPtrMgd_003CIEndpointHost_003E spEndpointHost = new CComPtrMgd_003CIEndpointHost_003E();
			try
			{
				m_spEndpointHost = spEndpointHost;
				CComPtrMgd_003CMicrosoftZuneLibrary_003A_003AFirmwareUpdateMediator_003E spFirmwareMediator = new CComPtrMgd_003CMicrosoftZuneLibrary_003A_003AFirmwareUpdateMediator_003E();
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
			CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E) = 0L;
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
				Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002E_003D(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E, (IFirmwareUpdateErrorInfo*)lp);
				FirmwareProcessCompleteArgs args = new FirmwareProcessCompleteArgs(new FirmwareUpdateErrorInfo((IFirmwareUpdateErrorInfo*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E))), null, CompletionAction.Complete, disconnectDeviceOnComplete);
				Application.DeferredInvokeOnWorkerThread(ProcessCompleteWorker, ProcessCompleteWorkerComplete, args);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E);
				throw;
			}
			Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E);
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
			CComPtrNtv_003CIFirmwareUpdateNotification_003E cComPtrNtv_003CIFirmwareUpdateNotification_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E) = 0L;
			try
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				m_DeviceRebooting = false;
				int firmwareUpdateNotification = GetFirmwareUpdateNotification((IFirmwareUpdateNotification**)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E));
				if (*(long*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E) != 0L && firmwareUpdateNotification >= 0)
				{
					WBSTRString wBSTRString;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
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
							long num = *(long*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E);
							long num2 = *(long*)(&wBSTRString);
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E)) + 48)))((nint)num, (ushort*)num2);
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateNotification_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateNotification_003E);
				throw;
			}
			Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateNotification_003E);
		}

		internal unsafe int GetFirmwareUpdateNotification(IFirmwareUpdateNotification** ppNotification)
		{
			//IL_0152: Expected I, but got I8
			//IL_0152: Expected I, but got I8
			//IL_016b: Expected I, but got I8
			//IL_016b: Expected I, but got I8
			//IL_0174: Expected I, but got I8
			//IL_017c: Expected I8, but got I
			CComPtrNtv_003CIEndpointHostManager_003E cComPtrNtv_003CIEndpointHostManager_003E;
			*(long*)(&cComPtrNtv_003CIEndpointHostManager_003E) = 0L;
			CComPtrNtv_003CIEndpointManager_003E cComPtrNtv_003CIEndpointManager_003E;
			CComPtrNtv_003CIFirmwareUpdateNotification_003E cComPtrNtv_003CIFirmwareUpdateNotification_003E;
			EEndpointClass eEndpointClass;
			try
			{
				*(long*)(&cComPtrNtv_003CIEndpointManager_003E) = 0L;
				try
				{
					*(long*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E) = 0L;
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateNotification_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateNotification_003E);
						throw;
					}
					goto end_IL_000a;
					IL_0072:
					Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateNotification_003E);
					goto IL_0089;
					end_IL_000a:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointManager_003E);
					throw;
				}
				goto end_IL_0005;
				IL_0089:
				Module.CComPtrNtv_003CIEndpointManager_003E_002ERelease(&cComPtrNtv_003CIEndpointManager_003E);
				goto IL_00a0;
				end_IL_0005:;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointHostManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointHostManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointHostManager_003E);
				throw;
			}
			CComPtrMgd_003CIEndpointHost_003E spEndpointHost;
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateNotification_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateNotification_003E);
						throw;
					}
					goto end_IL_00ae;
					IL_00de:
					Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateNotification_003E);
					goto IL_00f5;
					end_IL_00ae:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointManager_003E);
					throw;
				}
				goto end_IL_00ae_2;
				IL_00f5:
				Module.CComPtrNtv_003CIEndpointManager_003E_002ERelease(&cComPtrNtv_003CIEndpointManager_003E);
				goto IL_010c;
				end_IL_00ae_2:;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointHostManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointHostManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointHostManager_003E);
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
							num = Module.GetSingleton((_GUID)Module._GUID_0a3d3343_00d9_4c61_9a86_2d778793e05f, (void**)(&cComPtrNtv_003CIEndpointHostManager_003E));
							if (num >= 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIEndpointHostManager_003E);
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IEndpointManager**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIEndpointHostManager_003E)) + 88)))((nint)num2, (IEndpointManager**)(&cComPtrNtv_003CIEndpointManager_003E));
								if (num >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CIEndpointManager_003E);
									EEndpointClass num4 = eEndpointClass;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointClass, IFirmwareUpdateNotification**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIEndpointManager_003E)) + 88)))((nint)num3, num4, (IFirmwareUpdateNotification**)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E));
									if (num >= 0)
									{
										IFirmwareUpdateNotification* ptr = (IFirmwareUpdateNotification*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E));
										*(long*)(&cComPtrNtv_003CIFirmwareUpdateNotification_003E) = 0L;
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateNotification_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateNotification_003E);
						throw;
					}
					Module.CComPtrNtv_003CIFirmwareUpdateNotification_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateNotification_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointManager_003E);
					throw;
				}
				Module.CComPtrNtv_003CIEndpointManager_003E_002ERelease(&cComPtrNtv_003CIEndpointManager_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointHostManager_003E*, void>)(&Module.CComPtrNtv_003CIEndpointHostManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointHostManager_003E);
				throw;
			}
			Module.CComPtrNtv_003CIEndpointHostManager_003E_002ERelease(&cComPtrNtv_003CIEndpointHostManager_003E);
			return num;
			IL_00a0:
			Module.CComPtrNtv_003CIEndpointHostManager_003E_002ERelease(&cComPtrNtv_003CIEndpointHostManager_003E);
			return -2147467261;
			IL_010c:
			Module.CComPtrNtv_003CIEndpointHostManager_003E_002ERelease(&cComPtrNtv_003CIEndpointHostManager_003E);
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
