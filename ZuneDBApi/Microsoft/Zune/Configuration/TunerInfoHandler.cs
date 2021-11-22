using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
	internal class TunerInfoHandler : ITunerInfoHandler, IDisposable
	{
		private EventHandler _003Cbacking_store_003EOnChanged;

		private IList<TunerInfo> m_PCsList;

		private IList<TunerInfo> m_devicesList;

		private IList<TunerInfo> m_appStoreDevicesList;

		private DateTime m_nextPCDeregistrationDate;

		private DateTime m_nextSubscriptionDeviceDeregistrationDate;

		private DateTime m_nextAppStoreDeviceDeregistrationDate;

		[SpecialName]
		public virtual event EventHandler OnChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EOnChanged = (EventHandler)Delegate.Combine(_003Cbacking_store_003EOnChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EOnChanged = (EventHandler)Delegate.Remove(_003Cbacking_store_003EOnChanged, value);
			}
		}

		public TunerInfoHandler()
		{
			m_PCsList = new List<TunerInfo>();
			m_devicesList = new List<TunerInfo>();
			m_appStoreDevicesList = new List<TunerInfo>();
		}

		private void _007ETunerInfoHandler()
		{
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe virtual bool CanQueryTunerList()
		{
			//IL_0003: Expected I, but got I8
			//IL_0024: Expected I, but got I8
			//IL_0034: Expected I, but got I8
			IService* ptr = null;
			int num = 0;
			if (Module.GetSingleton(Module.GUID_IService, (void**)(&ptr)) >= 0)
			{
				IService* intPtr = ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 200)))((nint)intPtr);
			}
			if (ptr != null)
			{
				IService* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return (byte)((num != 0) ? 1u : 0u) != 0;
		}

		public virtual IList<TunerInfo> GetPCsList()
		{
			return m_PCsList;
		}

		public virtual IList<TunerInfo> GetDevicesList()
		{
			return m_devicesList;
		}

		public virtual IList<TunerInfo> GetAppStoreDevicesList()
		{
			return m_appStoreDevicesList;
		}

		public virtual DateTime GetNextPCDeregistrationDate()
		{
			return m_nextPCDeregistrationDate;
		}

		public virtual DateTime GetNextSubscriptionDeviceDeregistrationDate()
		{
			return m_nextSubscriptionDeviceDeregistrationDate;
		}

		public virtual DateTime GetNextAppStoreDeviceDeregistrationDate()
		{
			return m_nextAppStoreDeviceDeregistrationDate;
		}

		public unsafe virtual void RefreshTunerList()
		{
			//IL_0003: Expected I, but got I8
			//IL_0029: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0066: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			//IL_007b: Expected I, but got I8
			IService* ptr = null;
			int singleton = Module.GetSingleton(Module.GUID_IService, (void**)(&ptr));
			RefreshCallback* ptr2 = (RefreshCallback*)Module.@new(24uL);
			RefreshCallback* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EConfiguration_002ERefreshCallback_002E_007Bctor_007D(ptr2, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr2);
				throw;
			}
			singleton = (((long)(nint)ptr3 == 0) ? (-2147024882) : singleton);
			if (singleton >= 0)
			{
				IService* intPtr = ptr;
				singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IRefreshTunerListCallback*, int>)(*(ulong*)(*(long*)ptr + 552)))((nint)intPtr, (IRefreshTunerListCallback*)ptr3);
			}
			if (ptr3 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr3 + 16)))((nint)ptr3);
			}
			if (ptr != null)
			{
				IService* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				ptr = null;
			}
			if (singleton < 0 && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 156uL)) & (true ? 1u : 0u)) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 153uL) >= 5u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 144uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x2af74624_002EWPP_RegisteredDevicesApi_cpp_Traceguids), (uint)singleton);
			}
		}

		public unsafe virtual void DeregisterTuner(TunerInfo info)
		{
			//IL_0003: Expected I, but got I8
			//IL_002c: Expected I, but got I8
			//IL_0076: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			IService* ptr = null;
			int singleton = Module.GetSingleton(Module.GUID_IService, (void**)(&ptr));
			DeregisterCallback* ptr2 = (DeregisterCallback*)Module.@new(24uL);
			DeregisterCallback* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EConfiguration_002EDeregisterCallback_002E_007Bctor_007D(ptr2, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr2);
				throw;
			}
			singleton = (((long)(nint)ptr3 == 0) ? (-2147024882) : singleton);
			if (singleton >= 0)
			{
				fixed (char* infoTunerIdPtr = info.m_tunerId.ToCharArray())
				{
					ushort* ptr4 = (ushort*)infoTunerIdPtr;
					try
					{
						long num = *(long*)ptr + 560;
						IService* intPtr = ptr;
						_003F val = ptr4;
						TunerType tunerType = info.TunerType;
						TunerRegisterType tunerRegisterType = info.TunerRegisterType;
						singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ETunerType, ETunerRegisterType, IDeregisterTunerCallback*, int>)(*(ulong*)num))((nint)intPtr, (ushort*)(nint)val, (ETunerType)tunerType, (ETunerRegisterType)tunerRegisterType, (IDeregisterTunerCallback*)ptr3);
					}
					catch
					{
						//try-fault
						ptr4 = null;
						throw;
					}
				}
			}
			if (ptr != null)
			{
				IService* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				ptr = null;
			}
			if (singleton < 0 && Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 156uL)) & (true ? 1u : 0u)) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 153uL) >= 5u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 144uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x2af74624_002EWPP_RegisteredDevicesApi_cpp_Traceguids), (uint)singleton);
			}
		}

		[SpecialName]
		protected virtual void raise_OnChanged(object value0, EventArgs value1)
		{
			_003Cbacking_store_003EOnChanged?.Invoke(value0, value1);
		}

		internal unsafe void UpdateTunerInfoLists(int cTunerInfo, global::TunerInfo* rgTunerInfo, ushort* pwszNextPCDeregistrationDate, ushort* pwszNextSubscriptionDeviceDeregistrationDate, ushort* pwszNextAppStoreDeviceDeregistrationDate)
		{
			//IL_0034: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_006f: Expected I, but got I8
			//IL_00cf: Expected I, but got I8
			m_PCsList.Clear();
			m_devicesList.Clear();
			m_appStoreDevicesList.Clear();
			long num = cTunerInfo;
			if (0 < num)
			{
				global::TunerInfo* ptr = (global::TunerInfo*)((ulong)(nint)rgTunerInfo + 16uL);
				ulong num2 = (ulong)num;
				do
				{
					string name = new string((char*)(*(ulong*)((ulong)(nint)ptr - 8uL)));
					string tunerId = new string((char*)(*(ulong*)ptr));
					string tunerVersion = new string((char*)(*(ulong*)((ulong)(nint)ptr + 8uL)));
					string dateCreated = new string((char*)(*(ulong*)((ulong)(nint)ptr + 16uL)));
					string dateLastUsed = new string((char*)(*(ulong*)((ulong)(nint)ptr + 24uL)));
					TunerType tunerType = *(TunerType*)((ulong)(nint)ptr - 16uL);
					TunerRegisterType tunerRegisterType = *(TunerRegisterType*)((ulong)(nint)ptr - 12uL);
					TunerInfo item = new TunerInfo(name, tunerId, tunerVersion, tunerType, tunerRegisterType, dateCreated, dateLastUsed);
					switch (tunerType)
					{
					case TunerType.PC:
						m_PCsList.Add(item);
						break;
					case TunerType.ZuneDevice:
					case TunerType.MobileDevice:
						switch (tunerRegisterType)
						{
						case TunerRegisterType.Subscription:
							m_devicesList.Add(item);
							break;
						case TunerRegisterType.AppStore:
							m_appStoreDevicesList.Add(item);
							break;
						}
						break;
					}
					ptr = (global::TunerInfo*)((ulong)(nint)ptr + 48uL);
					num2--;
				}
				while (num2 != 0);
			}
			if (pwszNextPCDeregistrationDate == null || !DateTime.TryParse(new string((char*)pwszNextPCDeregistrationDate), out m_nextPCDeregistrationDate))
			{
				m_nextPCDeregistrationDate = DateTime.MinValue;
			}
			if (pwszNextSubscriptionDeviceDeregistrationDate == null || !DateTime.TryParse(new string((char*)pwszNextSubscriptionDeviceDeregistrationDate), out m_nextSubscriptionDeviceDeregistrationDate))
			{
				m_nextSubscriptionDeviceDeregistrationDate = DateTime.MinValue;
			}
			if (pwszNextAppStoreDeviceDeregistrationDate == null || !DateTime.TryParse(new string((char*)pwszNextAppStoreDeviceDeregistrationDate), out m_nextAppStoreDeviceDeregistrationDate))
			{
				m_nextAppStoreDeviceDeregistrationDate = DateTime.MinValue;
			}
			raise_OnChanged(this, null);
		}

		internal unsafe void FinishRemoveTunerInfo(ushort* pwszTunerId, TunerType tunerType, TunerRegisterType tunerRegisterType)
		{
			IList<TunerInfo> list = null;
			switch (tunerType)
			{
			case TunerType.PC:
				list = m_PCsList;
				break;
			case TunerType.ZuneDevice:
			case TunerType.MobileDevice:
				switch (tunerRegisterType)
				{
				case TunerRegisterType.Subscription:
					list = m_devicesList;
					break;
				case TunerRegisterType.AppStore:
					list = m_appStoreDevicesList;
					break;
				}
				break;
			}
			int num = 0;
			if (0 >= list.Count)
			{
				return;
			}
			do
			{
				fixed (char* currentTunerIdPtr = list[num].m_tunerId.ToCharArray())
				{
					ushort* ptr = (ushort*)currentTunerIdPtr;
					try
					{
						if (2 == Module.CompareStringW(1033u, 1u, pwszTunerId, -1, ptr, -1))
						{
							goto IL_0076;
						}
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
					goto end_IL_004a;
					IL_0076:
					try
					{
						list.RemoveAt(num);
						raise_OnChanged(this, null);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
					return;
					end_IL_004a:;
				}
				num++;
			}
			while (num < list.Count);
		}

		internal void ReportError(int hrError)
		{
			EventArgsHR eventArgsHR = new EventArgsHR();
			eventArgsHR.HResult = hrError;
			raise_OnChanged(this, eventArgsHR);
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (!P_0)
			{
				//Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
