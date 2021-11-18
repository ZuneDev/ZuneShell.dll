using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class FirmwareUpdater : FirmwareOperationBase
	{
		private readonly CComPtrMgd_003CIFirmwareUpdater_003E m_spFirmwareUpdater;

		private FirmwareRestorer m_Restorer;

		public unsafe FirmwareRestorer Restorer
		{
			get
			{
				if (m_Restorer == null)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					m_Restorer = new FirmwareRestorer(p, m_fFirmwareProcessSupported, this);
				}
				return m_Restorer;
			}
		}

		public unsafe UpdateAction IsUpdateInProgress()
		{
			//IL_00aa: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 37, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			EUpdateAction eUpdateAction = (((long)(nint)m_spFirmwareMediator.p != 0) ? ((EUpdateAction)1) : ((EUpdateAction)0));
			EUpdateAction eUpdateAction2 = eUpdateAction;
			if (!m_Canceled)
			{
				if (Application.IsApplicationThread)
				{
					Module._ZuneShipAssert(1004u, 1435u);
				}
				Monitor.Enter(m_FirmwareLock);
				try
				{
					if (m_fFirmwareProcessSupported && EnsureNativeObject() >= 0)
					{
						IFirmwareUpdater* p = m_spFirmwareUpdater.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EUpdateAction*, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, &eUpdateAction2);
					}
				}
				finally
				{
					Monitor.Exit(m_FirmwareLock);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_qd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 38, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), m_spFirmwareUpdater.p, (int)eUpdateAction2);
			}
			return (UpdateAction)eUpdateAction2;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool RequiresSyncBeforeUpdate()
		{
			//IL_0089: Expected I, but got I8
			//IL_0089: Expected I, but got I8
			ManagedLock managedLock = null;
			bool flag = false;
			CComPtrNtv_003CIFirmwareUpdater2_003E cComPtrNtv_003CIFirmwareUpdater2_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E) = 0L;
			try
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 35, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				ManagedLock managedLock2 = new ManagedLock(m_FirmwareLock);
				try
				{
					managedLock = managedLock2;
					int num = EnsureNativeObject();
					if (num >= 0)
					{
						num = m_spFirmwareUpdater.QueryInterface_003CIFirmwareUpdater2_003E((IFirmwareUpdater2**)(&cComPtrNtv_003CIFirmwareUpdater2_003E));
						if (num >= 0)
						{
							int num2 = 0;
							long num3 = *(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater2_003E)) + 72)))((nint)num3, &num2);
							int num4 = ((num >= 0 && num2 != 0) ? 1 : 0);
							flag = (byte)num4 != 0;
						}
					}
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
					{
						Module.WPP_SF_dl(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 36, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num, flag ? 1 : 0);
					}
				}
				catch
				{
					//try-fault
					((IDisposable)managedLock).Dispose();
					throw;
				}
				((IDisposable)managedLock).Dispose();
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdater2_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdater2_003E);
				throw;
			}
			Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdater2_003E);
			return flag;
		}

		public unsafe HRESULT StartCheckForDiskSpace(DeferredInvokeHandler checkForDiskSpaceComplete)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 47, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (Application.IsApplicationThread)
			{
				if (!ThreadPool.QueueUserWorkItem(StartCheckForDiskSpaceWorker, checkForDiskSpaceComplete))
				{
					Module._ZuneShipAssert(1004u, 1753u);
					num = -2147467259;
				}
			}
			else
			{
				StartCheckForDiskSpaceWorker(checkForDiskSpaceComplete);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 48, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe HRESULT StartCheckForUpdates([MarshalAs(UnmanagedType.U1)] bool fForceServerRequest, DeferredInvokeHandler checkForUpdatesComplete)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_l(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 39, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), fForceServerRequest ? 1 : 0);
			}
			int num = 0;
			if (m_fFirmwareProcessSupported)
			{
				if (checkForUpdatesComplete == null)
				{
					Module._ZuneShipAssert(1004u, 1479u);
					num = -2147467261;
				}
				else
				{
					m_Canceled = false;
					StartCheckForUpdatesArgs startCheckForUpdatesArgs = new StartCheckForUpdatesArgs(fForceServerRequest, checkForUpdatesComplete);
					if (Application.IsApplicationThread)
					{
						if (!ThreadPool.QueueUserWorkItem(StartCheckForUpdatesWorker, startCheckForUpdatesArgs))
						{
							Module._ZuneShipAssert(1004u, 1497u);
							num = -2147467259;
						}
					}
					else
					{
						StartCheckForUpdatesWorker(startCheckForUpdatesArgs);
						num = startCheckForUpdatesArgs.HrStatus;
					}
				}
			}
			else
			{
				num = -2147024846;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 40, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe HRESULT StartFirmwareUpdate(UpdatePackageCollection updates, DeferredInvokeHandler firmwareUpdateBegin, DeferredInvokeHandler firmwareUpdateProgress, DeferredInvokeHandler firmwareUpdateComplete, FirmwareUpdateOption updateOption)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 43, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_Canceled = false;
			int num = 0;
			if (updates == null)
			{
				num = -2147418113;
			}
			else
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 6u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 44, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), updates.Count);
				}
				int num2 = 0;
				if (0 < updates.Count)
				{
					do
					{
						FirmwareUpdatePackage firmwareUpdatePackage = updates[num2];
						fixed (char* firmwareUpdatePackage.NamePtr = firmwareUpdatePackage.Name.ToCharArray())
						{
							ushort* a = (ushort*)firmwareUpdatePackage.NamePtr;
							try
							{
								if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 6u)
								{
									Module.WPP_SF_dSl(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 45, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num2, a, firmwareUpdatePackage.Selected ? 1 : 0);
								}
							}
							catch
							{
								//try-fault
								a = null;
								throw;
							}
						}
						num2++;
					}
					while (num2 < updates.Count);
				}
				StartFirmwareUpdateArgs startFirmwareUpdateArgs = new StartFirmwareUpdateArgs(updates, firmwareUpdateBegin, firmwareUpdateProgress, firmwareUpdateComplete, updateOption);
				if (Application.IsApplicationThread)
				{
					if (!ThreadPool.QueueUserWorkItem(StartFirmwareUpdateWorker, startFirmwareUpdateArgs))
					{
						Module._ZuneShipAssert(1004u, 1641u);
						num = -2147467259;
					}
				}
				else
				{
					StartFirmwareUpdateWorker(startFirmwareUpdateArgs);
					num = startFirmwareUpdateArgs.HrStatus;
				}
			}
			return num;
		}

		public unsafe HRESULT Cancel()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 56, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (m_fFirmwareProcessSupported)
			{
				m_Canceled = true;
				if (Application.IsApplicationThread)
				{
					if (!ThreadPool.QueueUserWorkItem(CancelWorker, null))
					{
						Module._ZuneShipAssert(1004u, 1945u);
					}
				}
				else
				{
					CancelWorker(null);
				}
			}
			else
			{
				Module._ZuneShipAssert(1004u, 1955u);
				num = -2147024846;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 57, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			return num;
		}

		internal unsafe FirmwareUpdater(IEndpointHost* pEndpointHost, [MarshalAs(UnmanagedType.U1)] bool fFirmwareProcessSupported)
		{
			CComPtrMgd_003CIFirmwareUpdater_003E spFirmwareUpdater = new CComPtrMgd_003CIFirmwareUpdater_003E();
			try
			{
				m_spFirmwareUpdater = spFirmwareUpdater;
				base._002Ector();
				try
				{
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
					{
						Module.WPP_SF_q(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 31, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), pEndpointHost);
					}
					m_spEndpointHost.op_Assign(pEndpointHost);
					m_fFirmwareProcessSupported = fFirmwareProcessSupported;
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
					{
						Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 32, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
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
				((IDisposable)m_spFirmwareUpdater).Dispose();
				throw;
			}
		}

		private unsafe void _007EFirmwareUpdater()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 33, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			if (IsUpdateInProgress() != 0)
			{
				Module._ZuneShipAssert(1004u, 1355u);
			}
			m_spFirmwareUpdater.Release();
			FirmwareRestorer restorer = m_Restorer;
			if (restorer != null)
			{
				((IDisposable)restorer).Dispose();
				m_Restorer = null;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 34, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		internal unsafe override int InternalReset()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 59, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			m_Restorer?.ReleaseNativeObject();
			ReleaseNativeObject();
			return 0;
		}

		internal override int ContinueFirmwareProcess(FirmwareCompleteHandler onCompleteHandler)
		{
			int result = 0;
			if (!ThreadPool.QueueUserWorkItem(state: new ContinueFirmwareUpdateArgs(onCompleteHandler), callBack: ContinueFirmwareUpdateWorker))
			{
				Module._ZuneShipAssert(1004u, 1838u);
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
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 61, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				m_spFirmwareUpdater.op_Assign(null);
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
		}

		internal unsafe int GetNativeObject(IFirmwareUpdater** ppUpdater)
		{
			int num = 0;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 62, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			if (ppUpdater == null)
			{
				Module._ZuneShipAssert(1001u, 2089u);
				return -2147467261;
			}
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2091u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				num = EnsureNativeObject();
				if (num >= 0)
				{
					num = m_spFirmwareUpdater.CopyTo(ppUpdater);
				}
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 63, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
			return num;
		}

		private unsafe int EnsureNativeObject()
		{
			//IL_00a7: Expected I, but got I8
			//IL_00b5: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 60, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int result = 0;
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 2028u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				if (!m_fFirmwareProcessSupported)
				{
					int num = -2147024846;
					return -2147024846;
				}
				if (m_spFirmwareUpdater.p == null)
				{
					CComPtrNtv_003CIFirmwareUpdater_003E cComPtrNtv_003CIFirmwareUpdater_003E;
					*(long*)(&cComPtrNtv_003CIFirmwareUpdater_003E) = 0L;
					try
					{
						result = Module.GetEndpointHostInterfaceProperty_003Cstruct_0020IFirmwareUpdater_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyFirmwareUpdater, (IFirmwareUpdater**)(&cComPtrNtv_003CIFirmwareUpdater_003E));
						if (result >= 0)
						{
							IFirmwareUpdater* ptr = (IFirmwareUpdater*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater_003E));
							m_spFirmwareUpdater.op_Assign((IFirmwareUpdater*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater_003E)));
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

		private unsafe void ContinueFirmwareUpdateWorker(object data)
		{
			//IL_015e: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 51, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			int num = 0;
			if (m_Canceled)
			{
				num = -2147467260;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 52, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
			}
			else if ((long)(nint)m_spFirmwareMediator.p == 0)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 2u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 53, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
				}
				num = -2147418113;
			}
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 1882u);
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
						IFirmwareUpdater* p = m_spFirmwareUpdater.p;
						FirmwareUpdateMediator* p2 = m_spFirmwareMediator.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareUpdateCallback*, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, (IFirmwareUpdateCallback*)p2);
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
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 54, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
				}
				SendCompleteNotification(num, disconnectDeviceOnComplete: false);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 55, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
		}

		private unsafe void StartCheckForUpdatesWorker(object data)
		{
			//IL_00a6: Expected I, but got I8
			//IL_00e0: Expected I, but got I8
			//IL_00f8: Expected I, but got I8
			//IL_00f8: Expected I, but got I8
			//IL_0132: Expected I, but got I8
			//IL_0153: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 41, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			StartCheckForUpdatesArgs startCheckForUpdatesArgs = (StartCheckForUpdatesArgs)data;
			int num = 0;
			CComPtrNtv_003CIFirmwareUpdateCallback_003E cComPtrNtv_003CIFirmwareUpdateCallback_003E;
			*(long*)(&cComPtrNtv_003CIFirmwareUpdateCallback_003E) = 0L;
			try
			{
				bool requiresSyncBeforeUpdate = false;
				if (Application.IsApplicationThread)
				{
					Module._ZuneShipAssert(1004u, 1533u);
				}
				Monitor.Enter(m_FirmwareLock);
				try
				{
					num = EnsureNativeObject();
					if (num >= 0)
					{
						requiresSyncBeforeUpdate = RequiresSyncBeforeUpdate();
					}
					CheckForUpdatesCallback* ptr = (CheckForUpdatesCallback*)Module.@new(40uL);
					CheckForUpdatesCallback* ptr2;
					try
					{
						ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ECheckForUpdatesCallback_002E_007Bctor_007D(ptr, this, startCheckForUpdatesArgs.CheckForUpdatesComplete, requiresSyncBeforeUpdate));
						CheckForUpdatesCallback* ptr3 = ptr2;
					}
					catch
					{
						//try-fault
						Module.delete(ptr);
						throw;
					}
					CheckForUpdatesCallback* ptr4 = ptr2;
					if (ptr2 != null)
					{
						Module.CComPtrNtv_003CIFirmwareUpdateCallback_003E_002E_003D(&cComPtrNtv_003CIFirmwareUpdateCallback_003E, (IFirmwareUpdateCallback*)ptr2);
					}
					else
					{
						num = -2147024882;
					}
					if (num >= 0)
					{
						IFirmwareUpdater* p = m_spFirmwareUpdater.p;
						IFirmwareUpdateCallback* ptr5 = (IFirmwareUpdateCallback*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdateCallback_003E));
						bool forceServerRequest = startCheckForUpdatesArgs.ForceServerRequest;
						long num2 = *(long*)(&cComPtrNtv_003CIFirmwareUpdateCallback_003E);
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IFirmwareUpdateCallback*, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, forceServerRequest ? 1 : 0, (IFirmwareUpdateCallback*)num2);
					}
				}
				finally
				{
					Monitor.Exit(m_FirmwareLock);
				}
				startCheckForUpdatesArgs.HrStatus = num;
				if (num < 0)
				{
					CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E;
					*(long*)(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E) = 0L;
					try
					{
						InternalErrorInfo* ptr6 = (InternalErrorInfo*)Module.@new(56uL);
						InternalErrorInfo* lp;
						try
						{
							lp = ((ptr6 == null) ? null : Module.MicrosoftZuneLibrary_002EInternalErrorInfo_002E_007Bctor_007D(ptr6, num));
						}
						catch
						{
							//try-fault
							Module.delete(ptr6);
							throw;
						}
						Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002E_003D(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E, (IFirmwareUpdateErrorInfo*)lp);
						startCheckForUpdatesArgs.CheckForUpdatesComplete(new CheckForUpdatesArgs(new FirmwareUpdateErrorInfo((IFirmwareUpdateErrorInfo*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E))), null, RequiresSyncBeforeUpdate()));
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E);
						throw;
					}
					Module.CComPtrNtv_003CIFirmwareUpdateErrorInfo_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateErrorInfo_003E);
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 42, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdateCallback_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdateCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdateCallback_003E);
				throw;
			}
			Module.CComPtrNtv_003CIFirmwareUpdateCallback_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdateCallback_003E);
		}

		private unsafe void StartFirmwareUpdateWorker(object data)
		{
			//IL_0014: Expected I, but got I8
			//IL_0053: Expected I, but got I8
			//IL_00e8: Expected I, but got I8
			//IL_0135: Expected I, but got I8
			//IL_0135: Expected I, but got I8
			StartFirmwareUpdateArgs startFirmwareUpdateArgs = (StartFirmwareUpdateArgs)data;
			m_spFirmwareMediator.op_Assign(null);
			m_HandlerComplete = startFirmwareUpdateArgs.FirmwareUpdateComplete;
			int num = 0;
			FirmwareUpdateMediator* ptr = (FirmwareUpdateMediator*)Module.@new(64uL);
			FirmwareUpdateMediator* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002EFirmwareUpdateMediator_002E_007Bctor_007D(ptr, startFirmwareUpdateArgs.FirmwareUpdateBegin, startFirmwareUpdateArgs.FirmwareUpdateProgress, base.OnFirmwareProcessCompleteWorker));
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
				Module._ZuneShipAssert(1004u, 1684u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				if (num >= 0)
				{
					num = EnsureNativeObject();
					if (num >= 0)
					{
						if (startFirmwareUpdateArgs.UpdateOption == FirmwareUpdateOption.None)
						{
							IFirmwareUpdater* p = m_spFirmwareUpdater.p;
							FirmwareUpdateMediator* p2 = m_spFirmwareMediator.p;
							IFirmwareUpdateCollection* nativeCollectionPtr = startFirmwareUpdateArgs.Updates.NativeCollectionPtr;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareUpdateCollection*, IFirmwareUpdateCallback*, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, nativeCollectionPtr, (IFirmwareUpdateCallback*)p2);
						}
						else
						{
							CComPtrNtv_003CIFirmwareUpdater2_003E cComPtrNtv_003CIFirmwareUpdater2_003E;
							*(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E) = 0L;
							try
							{
								num = m_spFirmwareUpdater.QueryInterface_003CIFirmwareUpdater2_003E((IFirmwareUpdater2**)(&cComPtrNtv_003CIFirmwareUpdater2_003E));
								if (num >= 0)
								{
									FirmwareUpdateMediator* p3 = m_spFirmwareMediator.p;
									long num2 = *(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E);
									IFirmwareUpdateCollection* nativeCollectionPtr2 = startFirmwareUpdateArgs.Updates.NativeCollectionPtr;
									FirmwareUpdateOption updateOption = startFirmwareUpdateArgs.UpdateOption;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IFirmwareUpdateCollection*, IFirmwareUpdateCallback*, EFirmwareUpdateOption, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater2_003E)) + 80)))((nint)num2, nativeCollectionPtr2, (IFirmwareUpdateCallback*)p3, (EFirmwareUpdateOption)updateOption);
								}
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdater2_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdater2_003E);
								throw;
							}
							Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdater2_003E);
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			startFirmwareUpdateArgs.HrStatus = num;
			if (num < 0)
			{
				Reset(deviceRebooting: false);
				SendCompleteNotification(num, disconnectDeviceOnComplete: false);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 46, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
		}

		private unsafe void StartCheckForDiskSpaceWorker(object data)
		{
			//IL_00b7: Expected I, but got I8
			//IL_00b7: Expected I, but got I8
			//IL_00c3: Expected I, but got I8
			//IL_00cb: Expected I, but got I8
			DeferredInvokeHandler deferredInvokeHandler = (DeferredInvokeHandler)data;
			int num = 0;
			ulong additionalSpaceRequiredInBytes = 0uL;
			ulong totalSpaceRequiredInBytes = 0uL;
			string driveLetter = null;
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 1779u);
			}
			Monitor.Enter(m_FirmwareLock);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 49, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
			try
			{
				WBSTRString wBSTRString;
				Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
				try
				{
					CComPtrNtv_003CIFirmwareUpdater2_003E cComPtrNtv_003CIFirmwareUpdater2_003E;
					*(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E) = 0L;
					try
					{
						num = EnsureNativeObject();
						if (num >= 0)
						{
							num = m_spFirmwareUpdater.QueryInterface_003CIFirmwareUpdater2_003E((IFirmwareUpdater2**)(&cComPtrNtv_003CIFirmwareUpdater2_003E));
							if (num >= 0)
							{
								long num2 = *(long*)(&cComPtrNtv_003CIFirmwareUpdater2_003E);
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, ulong*, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIFirmwareUpdater2_003E)) + 88)))((nint)num2, &additionalSpaceRequiredInBytes, &totalSpaceRequiredInBytes, (ushort**)(&wBSTRString));
								if (num >= 0)
								{
									ushort* ptr = (ushort*)(*(ulong*)(&wBSTRString));
									driveLetter = new string((char*)(*(ulong*)(&wBSTRString)));
								}
							}
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIFirmwareUpdater2_003E*, void>)(&Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIFirmwareUpdater2_003E);
						throw;
					}
					Module.CComPtrNtv_003CIFirmwareUpdater2_003E_002ERelease(&cComPtrNtv_003CIFirmwareUpdater2_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
					throw;
				}
				Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
			}
			finally
			{
				Monitor.Exit(m_FirmwareLock);
			}
			CheckDiskSpaceArgs args = new CheckDiskSpaceArgs(num, additionalSpaceRequiredInBytes, totalSpaceRequiredInBytes, driveLetter);
			deferredInvokeHandler(args);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && (uint)(*(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL)) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 50, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids), num);
			}
		}

		private unsafe void CancelWorker(object data)
		{
			//IL_005c: Expected I, but got I8
			if (Application.IsApplicationThread)
			{
				Module._ZuneShipAssert(1004u, 1968u);
			}
			Monitor.Enter(m_FirmwareLock);
			try
			{
				CComPtrMgd_003CMicrosoftZuneLibrary_003A_003AFirmwareUpdateMediator_003E spFirmwareMediator = m_spFirmwareMediator;
				if (spFirmwareMediator.p != null)
				{
					Module.MicrosoftZuneLibrary_002EFirmwareUpdateMediator_002EFirmwareProcessCanceled(spFirmwareMediator.p);
				}
				CComPtrMgd_003CIFirmwareUpdater_003E spFirmwareUpdater = m_spFirmwareUpdater;
				if (spFirmwareUpdater.p != null)
				{
					IFirmwareUpdater* p = spFirmwareUpdater.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 64)))((nint)p) < 0)
					{
						Module._ZuneShipAssert(1004u, 1983u);
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
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 58, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x15529884_002EWPP_FirmwareUpdateAPI_cpp_Traceguids));
			}
		}

		protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EFirmwareUpdater();
				}
				finally
				{
					try
					{
						base.Dispose(true);
					}
					finally
					{
						((IDisposable)m_spFirmwareUpdater).Dispose();
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
