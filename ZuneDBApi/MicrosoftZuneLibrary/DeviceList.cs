using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class DeviceList : IDisposable
	{
		private static DeviceList m_singletonInstance = null;

		private DeviceAddedHandler _003Cbacking_store_003EAdded;

		private readonly CComPtrMgd_003CIEndpointHostManager_003E m_spEndpointHostManager;

		private readonly CComPtrMgd_003CEndpointHostManagerMediator_003E m_spEndpointHostManagerMediator;

		private SortedList m_slDevices;

		private object m_lock;

		private bool m_fInitialized;

		public unsafe bool Initialized
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_l(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 39, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), m_fInitialized ? 1 : 0);
				}
				return m_fInitialized;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_l(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 40, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), value ? 1 : 0);
				}
				m_fInitialized = value;
			}
		}

		public unsafe int Count
		{
			get
			{
				ManagedLock managedLock = null;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 37, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), m_slDevices.Count);
				}
				if (!m_fInitialized && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 38, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				object @lock = m_lock;
				if (@lock != null && m_slDevices != null)
				{
					ManagedLock managedLock2 = new ManagedLock(@lock);
					int count;
					try
					{
						managedLock = managedLock2;
						count = m_slDevices.Count;
					}
					catch
					{
						//try-fault
						((IDisposable)managedLock).Dispose();
						throw;
					}
					((IDisposable)managedLock).Dispose();
					return count;
				}
				return 0;
			}
		}

		public static DeviceList Instance
		{
			get
			{
				if (m_singletonInstance == null)
				{
					m_singletonInstance = new DeviceList();
				}
				return m_singletonInstance;
			}
		}

		[SpecialName]
		public event DeviceAddedHandler Added
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EAdded = (DeviceAddedHandler)Delegate.Combine(_003Cbacking_store_003EAdded, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EAdded = (DeviceAddedHandler)Delegate.Remove(_003Cbacking_store_003EAdded, value);
			}
		}

		private unsafe DeviceList()
		{
			//IL_0093: Expected I, but got I8
			CComPtrMgd_003CIEndpointHostManager_003E spEndpointHostManager = new CComPtrMgd_003CIEndpointHostManager_003E();
			try
			{
				m_spEndpointHostManager = spEndpointHostManager;
				CComPtrMgd_003CEndpointHostManagerMediator_003E spEndpointHostManagerMediator = new CComPtrMgd_003CEndpointHostManagerMediator_003E();
				try
				{
					m_spEndpointHostManagerMediator = spEndpointHostManagerMediator;
					base._002Ector();
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
					{
						Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
					}
					m_fInitialized = false;
					m_lock = new object();
					m_slDevices = new SortedList();
					EndpointHostManagerMediator* ptr = (EndpointHostManagerMediator*)Module.@new(56uL);
					EndpointHostManagerMediator* lp;
					try
					{
						lp = ((ptr == null) ? null : Module.EndpointHostManagerMediator_002E_007Bctor_007D(ptr, this));
					}
					catch
					{
						//try-fault
						Module.delete(ptr);
						throw;
					}
					m_spEndpointHostManagerMediator.op_Assign(lp);
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
					{
						Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
					}
				}
				catch
				{
					//try-fault
					((IDisposable)m_spEndpointHostManagerMediator).Dispose();
					throw;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spEndpointHostManager).Dispose();
				throw;
			}
		}

		private unsafe void _007EDeviceList()
		{
			ManagedLock managedLock = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 12, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
			m_spEndpointHostManager.Release();
			CComPtrMgd_003CEndpointHostManagerMediator_003E spEndpointHostManagerMediator = m_spEndpointHostManagerMediator;
			if ((long)(nint)spEndpointHostManagerMediator.p == 0)
			{
				Module.EndpointHostManagerMediator_002EShutdown(spEndpointHostManagerMediator.p);
			}
			m_spEndpointHostManagerMediator.Release();
			ManagedLock managedLock2 = new ManagedLock(m_lock);
			try
			{
				managedLock = managedLock2;
				foreach (object slDevice in m_slDevices)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)slDevice;
					(dictionaryEntry.Value as IDisposable)?.Dispose();
				}
				m_slDevices = null;
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			m_lock = null;
			m_singletonInstance = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 13, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
		}

		public unsafe int InitializeAndEnumerate()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 14, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
			int num = Module.EndpointHostManagerMediator_002EInitializeAndEnumerate(m_spEndpointHostManagerMediator.p);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 15, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe void SetEndpointHostManager(IEndpointHostManager* pEndpointHostManager)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 16, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
			m_spEndpointHostManager.op_Assign(pEndpointHostManager);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 17, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
		}

		public unsafe Device GetItem(int idx)
		{
			ManagedLock managedLock = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), idx);
			}
			ManagedLock managedLock2 = new ManagedLock(m_lock);
			Device result;
			try
			{
				managedLock = managedLock2;
				if (!m_fInitialized && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				SortedList slDevices = m_slDevices;
				if (slDevices != null && idx < slDevices.Count)
				{
					object byIndex = m_slDevices.GetByIndex(idx);
					if (byIndex != null)
					{
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 20, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
						}
						result = (Device)byIndex;
						goto IL_010f;
					}
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			Device result2;
			try
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 21, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				result2 = null;
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return result2;
			IL_010f:
			((IDisposable)managedLock).Dispose();
			return result;
		}

		public unsafe void HideDevice(Device device)
		{
			string strName = null;
			device.GetFriendlyName(ref strName);
			fixed (char* strNamePtr = strName.ToCharArray())
			{
				ushort* a = (ushort*)strNamePtr;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_S(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 22, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), a);
				}
				if (!m_fInitialized && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 23, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				HideDeviceInternal(device.EndpointId, fHide: true);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 24, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
			}
		}

		public unsafe void UnhideDevice(Device device)
		{
			string strName = null;
			device.GetFriendlyName(ref strName);
			fixed (char* strNamePtr = strName.ToCharArray())
			{
				ushort* a = (ushort*)strNamePtr;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 4u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_S(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 25, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), a);
				}
				if (!m_fInitialized && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				HideDeviceInternal(device.EndpointId, fHide: false);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 27, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
			}
		}

		public unsafe virtual int DeviceArrived(IEndpointHost* pEndpointHost)
		{
			//IL_006f: Expected I, but got I8
			Device device = null;
			if (pEndpointHost == null)
			{
				Module._ZuneShipAssert(1001u, 184u);
				return -2147467261;
			}
			int num = -1;
			device = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 28, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)pEndpointHost + 136)))((nint)pEndpointHost, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &num);
			if (num2 >= 0 && num > 0)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 29, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				num2 = AddDeviceToList(pEndpointHost, ref device);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 30, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), num2);
			}
			return num2;
		}

		public unsafe virtual int DeviceDisconnected(IEndpointHost* pEndpointHost)
		{
			//IL_0098: Expected I, but got I8
			ManagedLock managedLock = null;
			if (pEndpointHost == null)
			{
				Module._ZuneShipAssert(1001u, 284u);
				return -2147467261;
			}
			object @lock = m_lock;
			if (@lock == null)
			{
				return -2147467261;
			}
			ManagedLock managedLock2 = new ManagedLock(@lock);
			int num;
			try
			{
				managedLock = managedLock2;
				int nDeviceId = -1;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 33, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				if (m_slDevices == null)
				{
					num = -2147418113;
				}
				else
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)pEndpointHost + 136)))((nint)pEndpointHost, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &nDeviceId);
					if (num >= 0)
					{
						ResetUpdater(nDeviceId, fFireDisconnect: true);
					}
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 34, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), num);
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return num;
		}

		[SpecialName]
		protected void raise_Added(Device value0)
		{
			_003Cbacking_store_003EAdded?.Invoke(value0);
		}

		public unsafe int GetTranscodedFilesCachePath(ref string strCachePath)
		{
			//IL_0003: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			ushort* ptr = null;
			IEndpointHostManager* p = m_spEndpointHostManager.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 329u);
				return -2147418113;
			}
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, &ptr);
			if (num >= 0 && ptr != null)
			{
				strCachePath = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int SetTranscodedFilesCachePath(string strCachePath)
		{
			//IL_0046: Expected I, but got I8
			fixed (char* strCachePathPtr = strCachePath.ToCharArray())
			{
				ushort* ptr = (ushort*)strCachePathPtr;
				int result;
				if (ptr != null)
				{
					ushort* ptr2 = Module.SysAllocString(ptr);
					IEndpointHostManager* p = m_spEndpointHostManager.p;
					if (p == null)
					{
						Module._ZuneShipAssert(1002u, 353u);
						return -2147418113;
					}
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, ptr2);
					Module.SysFreeString(ptr2);
				}
				else
				{
					result = -2147418113;
				}
				return result;
			}
		}

		public unsafe int SetTranscodedFilesCacheSize(int lCacheSize)
		{
			//IL_0034: Expected I, but got I8
			IEndpointHostManager* p = m_spEndpointHostManager.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 372u);
				return -2147418113;
			}
			IEndpointHostManager* ptr = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr + 104)))((nint)ptr, lCacheSize);
		}

		public unsafe int ClearTranscodeCache()
		{
			//IL_0031: Expected I, but got I8
			IEndpointHostManager* p = m_spEndpointHostManager.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 384u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 112)))((nint)p);
		}

		internal unsafe virtual int AddDeviceToList(IEndpointHost* pEndpointHost, ref Device device)
		{
			//IL_009f: Expected I, but got I8
			ManagedLock managedLock = null;
			if (pEndpointHost == null)
			{
				Module._ZuneShipAssert(1001u, 220u);
				return -2147467261;
			}
			if (m_lock == null)
			{
				return -2147467261;
			}
			int num = -1;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 31, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
			}
			ManagedLock managedLock2 = new ManagedLock(m_lock);
			int num2;
			try
			{
				managedLock = managedLock2;
				if (m_slDevices == null)
				{
					num2 = -2147418113;
				}
				else
				{
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)pEndpointHost + 136)))((nint)pEndpointHost, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &num);
					if (num2 >= 0)
					{
						if (m_slDevices.ContainsKey(num))
						{
							num2 = (device = (Device)m_slDevices[num]).Initialize(pEndpointHost);
						}
						else
						{
							Device device2 = (device = new Device());
							if (device2 != null)
							{
								num2 = device2.Initialize(pEndpointHost);
								if (num2 >= 0)
								{
									Device device3 = device;
									m_slDevices.Add(device3.DeviceID, device3);
									raise_Added(device);
								}
							}
							else
							{
								num2 = -2147024882;
							}
						}
					}
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 32, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), num2);
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return num2;
		}

		internal unsafe int ForgetEndpoint(IEndpointHost* pEndpointHost)
		{
			//IL_00ae: Expected I, but got I8
			//IL_00cd: Expected I, but got I8
			ManagedLock managedLock = null;
			if (m_spEndpointHostManager.p == null)
			{
				Module._ZuneShipAssert(1002u, 397u);
				return -2147418113;
			}
			if (pEndpointHost == null)
			{
				Module._ZuneShipAssert(1001u, 398u);
				return -2147467261;
			}
			object @lock = m_lock;
			if (@lock == null)
			{
				return -2147467261;
			}
			ManagedLock managedLock2 = new ManagedLock(@lock);
			int num2;
			try
			{
				managedLock = managedLock2;
				int num = 0;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 35, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids));
				}
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)pEndpointHost + 136)))((nint)pEndpointHost, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &num);
				if (num2 >= 0)
				{
					IEndpointHostManager* p = m_spEndpointHostManager.p;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IEndpointHost*, int>)(*(ulong*)(*(long*)p + 80)))((nint)p, pEndpointHost);
				}
				ResetUpdater(num, fFireDisconnect: false);
				if (num2 >= 0)
				{
					m_slDevices?.Remove(num);
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 36, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x6a82d7e8_002EWPP_DeviceListAPI_cpp_Traceguids), num2);
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return num2;
		}

		protected unsafe void HideDeviceInternal(string strEndpointId, [MarshalAs(UnmanagedType.U1)] bool fHide)
		{
			//IL_0052: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_0069: Expected I, but got I8
			//IL_0069: Expected I, but got I8
			if (!(strEndpointId != null))
			{
				return;
			}
			CComPtrNtv_003CIEndpointNotification_003E cComPtrNtv_003CIEndpointNotification_003E;
			*(long*)(&cComPtrNtv_003CIEndpointNotification_003E) = 0L;
			try
			{
				fixed (char* strEndpointIdPtr = strEndpointId.ToCharArray())
				{
					ushort* ptr = (ushort*)strEndpointIdPtr;
					try
					{
						CComPtrMgd_003CIEndpointHostManager_003E spEndpointHostManager = m_spEndpointHostManager;
						if (spEndpointHostManager.p != null && Module.IUnknown_002EQueryInterface_003Cstruct_0020IEndpointNotification_003E((IUnknown*)spEndpointHostManager.p, (IEndpointNotification**)(&cComPtrNtv_003CIEndpointNotification_003E)) >= 0)
						{
							if (fHide)
							{
								long num = *(long*)(*(ulong*)(&cComPtrNtv_003CIEndpointNotification_003E)) + 32;
								long num2 = *(long*)(&cComPtrNtv_003CIEndpointNotification_003E);
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)num2, ptr);
							}
							else
							{
								long num3 = *(long*)(*(ulong*)(&cComPtrNtv_003CIEndpointNotification_003E)) + 24;
								long num4 = *(long*)(&cComPtrNtv_003CIEndpointNotification_003E);
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num3))((nint)num4, ptr);
							}
						}
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIEndpointNotification_003E*, void>)(&Module.CComPtrNtv_003CIEndpointNotification_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIEndpointNotification_003E);
				throw;
			}
			Module.CComPtrNtv_003CIEndpointNotification_003E_002ERelease(&cComPtrNtv_003CIEndpointNotification_003E);
		}

		private void ResetUpdater(int nDeviceId, [MarshalAs(UnmanagedType.U1)] bool fFireDisconnect)
		{
			if (!m_slDevices.ContainsKey(nDeviceId))
			{
				return;
			}
			Device device = (Device)m_slDevices[nDeviceId];
			if (device == null)
			{
				return;
			}
			FirmwareUpdater firmwareUpdater = device.FirmwareUpdater;
			if (firmwareUpdater != null)
			{
				bool flag = firmwareUpdater.Restorer.IsDeviceRebooting();
				if (!flag)
				{
					flag = firmwareUpdater.IsDeviceRebooting();
				}
				firmwareUpdater.Reset(flag);
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EDeviceList();
				}
				finally
				{
					try
					{
						((IDisposable)m_spEndpointHostManagerMediator).Dispose();
					}
					finally
					{
						try
						{
							((IDisposable)m_spEndpointHostManager).Dispose();
						}
						finally
						{
						}
					}
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
