using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using DataStructs;

namespace MicrosoftZuneLibrary
{
	public class Device : IDisposable
	{
		private bool m_fInitializationCompleted;

		private object m_Lock;

		private readonly CComPtrMgd<IEndpointHost> m_spEndpointHost;

		private readonly CComPtrMgd<ISyncEngine> m_spSyncEngine;

		private readonly CComPtrMgd<IWlanProvider> m_spWlanProvider;

		private readonly CComPtrMgd<DeviceMediator> m_spDeviceMediator;

		private SyncBeganHandler m_SyncBegan;

		private SyncProgressedHandler m_SyncProgressed;

		private SyncCompletedHandler m_SyncCompleted;

		private FriendlyNameChangedHandler m_FriendlyNameChanged;

		private DeviceStatusChangedHandler m_DeviceStatusChanged;

		private FormatCompleteHandler m_FormatComplete;

		private GetWlanProfilesCompleteHandler m_GetWlanProfilesComplete;

		private GetDeviceWlanNetworksCompleteHandler m_GetDeviceWlanNetworksComplete;

		private GetDeviceWlanProfilesCompleteHandler m_GetDeviceWlanProfilesComplete;

		private SetDeviceWlanProfilesCompleteHandler m_SetDeviceWlanProfilesComplete;

		private AssociateWlanDeviceCompleteHandler m_AssociateWlanDeviceComplete;

		private UnassociateWlanDeviceCompleteHandler m_UnassociateWlanDeviceComplete;

		private TestDeviceWlanCompleteHandler m_TestDeviceWlanComplete;

		private SyncRules m_syncRules;

		private FirmwareUpdater m_firmwareUpdater;

		private GasGauge m_predictedGasGauge;

		private GasGauge m_actualGasGauge;

		private bool m_fClientUpdateRequired;

		private bool m_fFirmwareUpdateRequired;

		private int m_hrEnumeration;

		private DeviceAssetSet m_DeviceAssetSet;

		private int m_iDeviceID;

		private uint m_dwModelID;

		private string m_strIconPath;

		private string m_strEndpointId;

		private string m_strCanonicalName;

		private bool m_fFirmwareUpdateSupported;

		private EEndpointStatus m_eLastDeviceStatus;

		public unsafe string MyPhoneDeviceID
		{
			get
			{
				//IL_0014: Expected I, but got I8
				//IL_002c: Expected I, but got I8
				string result = null;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					ushort* ptr = null;
					IEndpointHost* p = spEndpointHost.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyMyPhoneDeviceId, &ptr) >= 0 && ptr != null)
					{
						result = new string((char*)ptr);
						Module.SysFreeString(ptr);
					}
				}
				return result;
			}
		}

		public unsafe bool InStandardMode
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsInStandardMode, &result);
				}
				return result;
			}
		}

		public DeviceAssetSet DeviceAssetSet => m_DeviceAssetSet;

		public unsafe int LastFirmwareUpdateError
		{
			get
			{
				//IL_003a: Expected I, but got I8
				int result = 0;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p == null)
				{
					Module._ZuneShipAssert(1002u, 3517u);
					return -2147418113;
				}
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLastFirmwareUpdateError, &result);
				return result;
			}
		}

		public FirmwareUpdater FirmwareUpdater => m_firmwareUpdater;

		public unsafe string PicturesVideosViewUrl
		{
			get
			{
				//IL_0014: Expected I, but got I8
				//IL_002c: Expected I, but got I8
				string result = null;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					ushort* ptr = null;
					IEndpointHost* p = spEndpointHost.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPicturesVideosViewUrl, &ptr) >= 0 && ptr != null)
					{
						result = new string((char*)ptr);
						Module.SysFreeString(ptr);
					}
				}
				return result;
			}
		}

		public unsafe string PicturesVideosViewText
		{
			get
			{
				//IL_0014: Expected I, but got I8
				//IL_002c: Expected I, but got I8
				string result = null;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					ushort* ptr = null;
					IEndpointHost* p = spEndpointHost.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPicturesVideosViewText, &ptr) >= 0 && ptr != null)
					{
						result = new string((char*)ptr);
						Module.SysFreeString(ptr);
					}
				}
				return result;
			}
		}

		public bool SyncSetupRequired
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				ESyncRelationship relationship = ESyncRelationship.srNone;
				if (GetSyncRelationship(ref relationship) >= 0 && (ESyncRelationship.srNone == relationship || ESyncRelationship.srSyncWithOtherMachine == relationship))
				{
					return true;
				}
				return false;
			}
		}

		public bool ClientUpdateRequired
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fClientUpdateRequired;
			}
		}

		public bool FirmwareUpdateRequired
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fFirmwareUpdateRequired;
			}
		}

		public GasGauge ActualGasGauge => m_actualGasGauge;

		public GasGauge PredictedGasGauge => m_predictedGasGauge;

		public SyncRules Rules => m_syncRules;

		public unsafe DateTime LastConnectTime
		{
			get
			{
				//IL_002d: Expected I, but got I8
				DateTime result = DateTime.Now;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					IEndpointHost* p = spEndpointHost.p;
					FILETIME fILETIME;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, FILETIME*, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLastConnectTime, &fILETIME) >= 0)
					{
						long fileTime = (uint)(&fILETIME + 4) * 4294967296L + (uint)(*(int*)(&fILETIME));
						try
						{
							result = DateTime.FromFileTime(fileTime);
							return result;
						}
						catch (ArgumentOutOfRangeException)
						{
							return result;
						}
					}
				}
				return result;
			}
		}

		public unsafe DateTime LastSyncTime
		{
			get
			{
				//IL_002d: Expected I, but got I8
				DateTime result = DateTime.Now;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					IEndpointHost* p = spEndpointHost.p;
					FILETIME fILETIME;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, FILETIME*, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLastSyncTime, &fILETIME) >= 0)
					{
						long fileTime = (uint)(&fILETIME + 4) * 4294967296L + (uint)(*(int*)(&fILETIME));
						try
						{
							result = DateTime.FromFileTime(fileTime);
							return result;
						}
						catch (ArgumentOutOfRangeException)
						{
							return result;
						}
					}
				}
				return result;
			}
		}

		public unsafe bool IsFormatting
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsFormatting, &result);
				}
				return result;
			}
		}

		public unsafe bool IsConnectedWirelessly
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsConnectedWirelessly, &result);
				}
				return result;
			}
		}

		public unsafe bool IsConnected
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsConnected, &result);
				}
				return result;
			}
		}

		public unsafe bool IsAvailable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsAvailable, &result);
				}
				return result;
			}
		}

		public unsafe EEndpointStatus DeviceStatus
		{
			get
			{
				int result = 1;
				if (m_spEndpointHost.p != null)
				{
					result = (int)m_eLastDeviceStatus;
				}
				return (EEndpointStatus)result;
			}
		}

		public unsafe string OwnerApplicationName
		{
			get
			{
				//IL_0018: Expected I, but got I8
				//IL_0030: Expected I, but got I8
				string result = string.Empty;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != null)
				{
					ushort* ptr = null;
					IEndpointHost* p = spEndpointHost.p;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyOwnerApplicationName, &ptr) >= 0 && ptr != null)
					{
						result = new string((char*)ptr);
						Module.SysFreeString(ptr);
					}
				}
				return result;
			}
		}

		public unsafe bool IsReady
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0024: Expected I, but got I8
				bool flag = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsReady, &flag);
					int num2 = ((flag && num >= 0 && m_hrEnumeration >= 0) ? 1 : 0);
					flag = (byte)num2 != 0;
				}
				return flag;
			}
		}

		public unsafe bool IsSyncSuspended
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001c: Expected I, but got I8
				ISyncEngine* p = m_spSyncEngine.p;
				if (p != null)
				{
					return (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 80)))((nint)p) != 0) ? true : false;
				}
				return false;
			}
		}

		public unsafe bool IsSyncRunning
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0025: Expected I, but got I8
				bool result = false;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertySyncing, &result);
				}
				return result;
			}
		}

		public unsafe ulong DeviceCapacity
		{
			get
			{
				//IL_0023: Expected I, but got I8
				ulong result = 0uL;
				IEndpointHost* p = m_spEndpointHost.p;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ulong*, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyTotalSpace, &result);
				return result;
			}
		}

		public string CanonicalName => m_strCanonicalName;

		public string EndpointId => m_strEndpointId;

		public unsafe uint StatedCapacity
		{
			get
			{
				//IL_0025: Expected I, but got I8
				uint result = 0u;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyStatedCapacity, &result);
				}
				return result;
			}
		}

		public unsafe uint BackgroundID
		{
			get
			{
				//IL_0025: Expected I, but got I8
				uint result = 0u;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyBackgroundId, &result);
				}
				return result;
			}
		}

		public unsafe uint TattooID
		{
			get
			{
				//IL_0025: Expected I, but got I8
				uint result = 0u;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyTattooId, &result);
				}
				return result;
			}
		}

		public unsafe ulong ColorID
		{
			get
			{
				//IL_0026: Expected I, but got I8
				ulong result = 0uL;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ulong*, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyColorId, &result);
				}
				return result;
			}
		}

		public unsafe uint PrimaryColorID
		{
			get
			{
				//IL_0025: Expected I, but got I8
				uint result = 0u;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPrimaryColorId, &result);
				}
				return result;
			}
		}

		public unsafe uint FamilyID
		{
			get
			{
				//IL_0025: Expected I, but got I8
				uint result = 0u;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyFamilyId, &result);
				}
				return result;
			}
		}

		public unsafe int ClassID
		{
			get
			{
				//IL_0025: Expected I, but got I8
				int result = 0;
				IEndpointHost* p = m_spEndpointHost.p;
				if (p != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyClassId, &result);
				}
				return result;
			}
		}

		public int DeviceID => m_iDeviceID;

		[SpecialName]
		public virtual event TestDeviceWlanCompleteHandler TestDeviceWlanCompleteEvent
		{
			add
			{
				m_TestDeviceWlanComplete = (TestDeviceWlanCompleteHandler)Delegate.Combine(m_TestDeviceWlanComplete, value);
			}
			remove
			{
				m_TestDeviceWlanComplete = (TestDeviceWlanCompleteHandler)Delegate.Remove(m_TestDeviceWlanComplete, value);
			}
		}

		[SpecialName]
		public virtual event UnassociateWlanDeviceCompleteHandler UnassociateWlanDeviceCompleteEvent
		{
			add
			{
				m_UnassociateWlanDeviceComplete = (UnassociateWlanDeviceCompleteHandler)Delegate.Combine(m_UnassociateWlanDeviceComplete, value);
			}
			remove
			{
				m_UnassociateWlanDeviceComplete = (UnassociateWlanDeviceCompleteHandler)Delegate.Remove(m_UnassociateWlanDeviceComplete, value);
			}
		}

		[SpecialName]
		public virtual event AssociateWlanDeviceCompleteHandler AssociateWlanDeviceCompleteEvent
		{
			add
			{
				m_AssociateWlanDeviceComplete = (AssociateWlanDeviceCompleteHandler)Delegate.Combine(m_AssociateWlanDeviceComplete, value);
			}
			remove
			{
				m_AssociateWlanDeviceComplete = (AssociateWlanDeviceCompleteHandler)Delegate.Remove(m_AssociateWlanDeviceComplete, value);
			}
		}

		[SpecialName]
		public virtual event SetDeviceWlanProfilesCompleteHandler SetDeviceWlanProfilesCompleteEvent
		{
			add
			{
				m_SetDeviceWlanProfilesComplete = (SetDeviceWlanProfilesCompleteHandler)Delegate.Combine(m_SetDeviceWlanProfilesComplete, value);
			}
			remove
			{
				m_SetDeviceWlanProfilesComplete = (SetDeviceWlanProfilesCompleteHandler)Delegate.Remove(m_SetDeviceWlanProfilesComplete, value);
			}
		}

		[SpecialName]
		public virtual event GetDeviceWlanProfilesCompleteHandler GetDeviceWlanProfilesCompleteEvent
		{
			add
			{
				m_GetDeviceWlanProfilesComplete = (GetDeviceWlanProfilesCompleteHandler)Delegate.Combine(m_GetDeviceWlanProfilesComplete, value);
			}
			remove
			{
				m_GetDeviceWlanProfilesComplete = (GetDeviceWlanProfilesCompleteHandler)Delegate.Remove(m_GetDeviceWlanProfilesComplete, value);
			}
		}

		[SpecialName]
		public virtual event GetDeviceWlanNetworksCompleteHandler GetDeviceWlanNetworksCompleteEvent
		{
			add
			{
				m_GetDeviceWlanNetworksComplete = (GetDeviceWlanNetworksCompleteHandler)Delegate.Combine(m_GetDeviceWlanNetworksComplete, value);
			}
			remove
			{
				m_GetDeviceWlanNetworksComplete = (GetDeviceWlanNetworksCompleteHandler)Delegate.Remove(m_GetDeviceWlanNetworksComplete, value);
			}
		}

		[SpecialName]
		public virtual event GetWlanProfilesCompleteHandler GetWlanProfilesCompleteEvent
		{
			add
			{
				m_GetWlanProfilesComplete = (GetWlanProfilesCompleteHandler)Delegate.Combine(m_GetWlanProfilesComplete, value);
			}
			remove
			{
				m_GetWlanProfilesComplete = (GetWlanProfilesCompleteHandler)Delegate.Remove(m_GetWlanProfilesComplete, value);
			}
		}

		[SpecialName]
		public virtual event FormatCompleteHandler FormatCompleteEvent
		{
			add
			{
				m_FormatComplete = (FormatCompleteHandler)Delegate.Combine(m_FormatComplete, value);
			}
			remove
			{
				m_FormatComplete = (FormatCompleteHandler)Delegate.Remove(m_FormatComplete, value);
			}
		}

		[SpecialName]
		public virtual event DeviceStatusChangedHandler DeviceStatusChangedEvent
		{
			add
			{
				m_DeviceStatusChanged = (DeviceStatusChangedHandler)Delegate.Combine(m_DeviceStatusChanged, value);
			}
			remove
			{
				m_DeviceStatusChanged = (DeviceStatusChangedHandler)Delegate.Remove(m_DeviceStatusChanged, value);
			}
		}

		[SpecialName]
		public virtual event FriendlyNameChangedHandler FriendlyNameChangedEvent
		{
			add
			{
				m_FriendlyNameChanged = (FriendlyNameChangedHandler)Delegate.Combine(m_FriendlyNameChanged, value);
			}
			remove
			{
				m_FriendlyNameChanged = (FriendlyNameChangedHandler)Delegate.Remove(m_FriendlyNameChanged, value);
			}
		}

		[SpecialName]
		public virtual event SyncCompletedHandler SyncCompleted
		{
			add
			{
				m_SyncCompleted = (SyncCompletedHandler)Delegate.Combine(m_SyncCompleted, value);
			}
			remove
			{
				m_SyncCompleted = (SyncCompletedHandler)Delegate.Remove(m_SyncCompleted, value);
			}
		}

		[SpecialName]
		public virtual event SyncProgressedHandler SyncProgressed
		{
			add
			{
				m_SyncProgressed = (SyncProgressedHandler)Delegate.Combine(m_SyncProgressed, value);
			}
			remove
			{
				m_SyncProgressed = (SyncProgressedHandler)Delegate.Remove(m_SyncProgressed, value);
			}
		}

		[SpecialName]
		public virtual event SyncBeganHandler SyncBegan
		{
			add
			{
				m_SyncBegan = (SyncBeganHandler)Delegate.Combine(m_SyncBegan, value);
			}
			remove
			{
				m_SyncBegan = (SyncBeganHandler)Delegate.Remove(m_SyncBegan, value);
			}
		}

		private unsafe void _007EDevice()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 12, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			DeviceMediator* p = m_spDeviceMediator.p;
			if (p != null)
			{
				Module.DeviceMediator_002EShutdown(p);
			}
			m_spEndpointHost.Release();
			m_spSyncEngine.Release();
			m_spDeviceMediator.Release();
			m_spWlanProvider.Release();
			SyncRules syncRules = m_syncRules;
			if (syncRules != null)
			{
				((IDisposable)syncRules).Dispose();
				m_syncRules = null;
			}
			FirmwareUpdater firmwareUpdater = m_firmwareUpdater;
			if (firmwareUpdater != null)
			{
				((IDisposable)firmwareUpdater).Dispose();
				m_firmwareUpdater = null;
			}
			GasGauge actualGasGauge = m_actualGasGauge;
			if (actualGasGauge != null)
			{
				((IDisposable)actualGasGauge).Dispose();
				m_actualGasGauge = null;
			}
			GasGauge predictedGasGauge = m_predictedGasGauge;
			if (predictedGasGauge != null)
			{
				((IDisposable)predictedGasGauge).Dispose();
				m_predictedGasGauge = null;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 13, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
		}

		public unsafe int StartSync()
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 723u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionStartSync);
		}

		public unsafe int StartSyncNextNotify()
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 732u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionStartSyncNextNotify);
		}

		public unsafe int StopSync()
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 741u);
				return -2147418113;
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionCancelSync);
			return 0;
		}

		public unsafe int StartEnumeration()
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 751u);
				return -2147418113;
			}
			IEndpointHost* ptr = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)ptr + 296)))((nint)ptr, EEndpointHostAction.eEndpointHostActionIngestMetadataAsync);
		}

		public unsafe int Format()
		{
			//IL_0039: Expected I, but got I8
			//IL_003e: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 764u);
				return -2147418113;
			}
			DeviceMediator* p2 = m_spDeviceMediator.p;
			DeviceMediator* ptr = (DeviceMediator*)((p2 == null) ? 0 : ((ulong)(nint)p2 + 24uL));
			IEndpointHost* ptr2 = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, IUnknown*, int>)(*(ulong*)(*(long*)ptr2 + 304)))((nint)ptr2, EEndpointHostAction.eEndpointHostActionFormat, (IUnknown*)ptr);
		}

		public unsafe int GetFriendlyName(ref string strName)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 777u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyName, &ptr);
			if (num >= 0 && ptr != null)
			{
				strName = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int SetFriendlyName(string strName)
		{
			//IL_004d: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 798u);
				return -2147418113;
			}
			fixed (char* strNamePtr = strName.ToCharArray())
			{
				ushort* ptr = (ushort*)strNamePtr;
				int result;
				if (ptr != null)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					long num = *(long*)p + 184;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort*, int>)(*(ulong*)num))((nint)p, EEndpointHostProperty.eEndpointHostPropertyName, ptr);
				}
				else
				{
					result = -2147418113;
				}
				return result;
			}
		}

		public unsafe int GetManufacturer(ref string strManufacturer)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 821u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyManufacturer, &ptr);
			if (num >= 0 && ptr != null)
			{
				strManufacturer = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int GetModelName(ref string strModelName)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 842u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyModelName, &ptr);
			if (num >= 0 && ptr != null)
			{
				strModelName = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int GetUserGuid(ref Guid guidUserGuid)
		{
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 863u);
				return -2147418113;
			}
			_GUID gUID_NULL = Module.GUID_NULL;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, _GUID*, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyUserGuid, &gUID_NULL);
			if (num >= 0)
			{
				Guid guid = (guidUserGuid = gUID_NULL);
			}
			return num;
		}

		public unsafe int GetUserId(ref int userId)
		{
			//IL_0038: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 884u);
				return -2147418113;
			}
			int num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyAssociatedUserId, &num);
			userId = ((num2 >= 0) ? num : 0);
			return num2;
		}

		public unsafe int GetZuneTag(ref string strZuneTag)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1029u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyZuneTag, &ptr);
			if (num >= 0 && ptr != null)
			{
				strZuneTag = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int GetLiveId(ref string strLiveId)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1051u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLiveId, &ptr);
			if (num >= 0 && ptr != null)
			{
				strLiveId = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int GetIsTvOutSupported(ref bool fIsTvOutSupported)
		{
			//IL_003d: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1073u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointCapability, bool*, int>)(*(ulong*)(*(long*)p + 80)))((nint)p, EEndpointCapability.eEndpointCapabilityTvOut, &flag);
			if (num >= 0)
			{
				fIsTvOutSupported = flag;
			}
			return num;
		}

		public unsafe int GetIsRestorePointSupported(ref bool fIsRestorePointSupported)
		{
			//IL_003e: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1095u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointCapability, bool*, int>)(*(ulong*)(*(long*)p + 80)))((nint)p, EEndpointCapability.eEndpointCapabilityRestorePoint, &flag);
			if (num >= 0)
			{
				fIsRestorePointSupported = flag;
			}
			return num;
		}

		public unsafe int GetCapability(EEndpointCapability capabilityId, ref bool fHasCapability)
		{
			//IL_003d: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1117u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointCapability, bool*, int>)(*(ulong*)(*(long*)p + 80)))((nint)p, capabilityId, &flag);
			if (num >= 0)
			{
				fHasCapability = flag;
			}
			return num;
		}

		public unsafe int GetGeoId(ref uint dwGeoId)
		{
			//IL_0038: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 909u);
				return -2147418113;
			}
			uint num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyGeoId, &num);
			dwGeoId = ((num2 >= 0) ? num : 0u);
			return num2;
		}

		public unsafe int SetGeoId(uint dwGeoId)
		{
			//IL_0039: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 934u);
				return -2147418113;
			}
			IEndpointHost* ptr = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint, int>)(*(ulong*)(*(long*)ptr + 208)))((nint)ptr, EEndpointHostProperty.eEndpointHostPropertyGeoId, dwGeoId);
		}

		public unsafe int GetTimeZoneBias(ref int lTimeZoneBias)
		{
			//IL_0038: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 949u);
				return -2147418113;
			}
			int num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyTimeZoneBias, &num);
			lTimeZoneBias = ((num2 >= 0) ? num : 0);
			return num2;
		}

		public unsafe int SetTimeZoneBias(int lTimeZoneBias)
		{
			//IL_0039: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 974u);
				return -2147418113;
			}
			IEndpointHost* ptr = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int, int>)(*(ulong*)(*(long*)ptr + 200)))((nint)ptr, EEndpointHostProperty.eEndpointHostPropertyTimeZoneBias, lTimeZoneBias);
		}

		public unsafe int GetWatsonSetting(ref uint dwWatsonSetting)
		{
			//IL_0038: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 989u);
				return -2147418113;
			}
			uint num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyWatsonSetting, &num);
			dwWatsonSetting = ((num2 >= 0) ? num : 0u);
			return num2;
		}

		public unsafe int SetWatsonSetting(uint dwWatsonSetting)
		{
			//IL_0039: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 1014u);
				return -2147418113;
			}
			IEndpointHost* ptr = p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint, int>)(*(ulong*)(*(long*)ptr + 208)))((nint)ptr, EEndpointHostProperty.eEndpointHostPropertyWatsonSetting, dwWatsonSetting);
		}

		public unsafe int SetUserGuidandZuneTag(Guid guidUserGuid, string strZuneTag)
		{
			//IL_004d: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			//IL_00b1: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1139u);
				return -2147418113;
			}
			_GUID gUID = guidUserGuid;
			IEndpointHost* p = m_spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, _GUID*, int>)(*(ulong*)(*(long*)p + 176)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyUserGuid, &gUID);
			if (num >= 0)
			{
				fixed (char* strZuneTagPtr = strZuneTag.ToCharArray())
				{
					ushort* ptr = (ushort*)strZuneTagPtr;
					try
					{
						if (ptr != null)
						{
							IEndpointHost* p2 = m_spEndpointHost.p;
							long num2 = *(long*)p2 + 184;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort*, int>)(*(ulong*)num2))((nint)p2, EEndpointHostProperty.eEndpointHostPropertyZuneTag, ptr);
						}
						else
						{
							num = -2147418113;
						}
						if (num < 0)
						{
							IEndpointHost* p3 = m_spEndpointHost.p;
							_GUID GUID_NULL = Module.GUID_NULL;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, _GUID*, int>)(*(ulong*)(*(long*)p3 + 176)))((nint)p3, EEndpointHostProperty.eEndpointHostPropertyUserGuid, (_GUID*)Unsafe.AsPointer(ref GUID_NULL));
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
			return num;
		}

		public int ClearUserGuidandZuneTag()
		{
			return SetUserGuidandZuneTag(default(Guid), "");
		}

		public unsafe int SetMarketplaceCredentials(SecureString strUsername, SecureString strPassword)
		{
			//IL_005e: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1191u);
				return -2147418113;
			}
			IntPtr hglobal = Marshal.SecureStringToGlobalAllocUnicode(strUsername);
			IntPtr hglobal2 = Marshal.SecureStringToGlobalAllocUnicode(strPassword);
			IEndpointHost* p = m_spEndpointHost.p;
			long num = *(long*)p + 160;
			void* intPtr = hglobal.ToPointer();
			void* intPtr2 = hglobal2.ToPointer();
			int result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort*, ushort*, int>)(*(ulong*)num))((nint)p, EEndpointHostProperty.eEndpointHostPropertyMarketplaceCredentials, (ushort*)intPtr, (ushort*)intPtr2);
			Marshal.FreeHGlobal(hglobal);
			Marshal.FreeHGlobal(hglobal2);
			return result;
		}

		public unsafe int GetOOBECompleted(ref bool oobeCompleted)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1241u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyOOBECompleted, &flag);
			if (num >= 0)
			{
				oobeCompleted = flag;
			}
			return num;
		}

		public unsafe int GetPurchaseEnabled(ref bool purchaseEnabled)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1220u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPurchaseEnabled, &flag);
			purchaseEnabled = flag;
			return result;
		}

		public unsafe int SetPurchaseEnabled([MarshalAs(UnmanagedType.U1)] bool purchaseEnabled)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 1265u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, byte, int>)(*(ulong*)(*(long*)p + 216)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPurchaseEnabled, purchaseEnabled ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetAndResetLastLoginError(ref int hrLogin)
		{
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1279u);
				return -2147418113;
			}
			int num = 0;
			IEndpointHost* p = spEndpointHost.p;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLastLoginError, &num);
			int num3 = (hrLogin = ((num2 >= 0) ? num : 0));
			return num2;
		}

		public unsafe int GetAndResetLastDownloadError(ref int hrDownload)
		{
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1301u);
				return -2147418113;
			}
			int num = 0;
			IEndpointHost* p = spEndpointHost.p;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyLastDownloadError, &num);
			int num3 = (hrDownload = ((num2 >= 0) ? num : 0));
			return num2;
		}

		public unsafe int LoadWlanProvider()
		{
			//IL_0067: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_0082: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			int num = 0;
			if ((long)(nint)m_spWlanProvider.p == 0)
			{
				if (m_spEndpointHost.p == null)
				{
					Module._ZuneShipAssert(1001u, 1325u);
					return -2147467261;
				}
				CComPtrNtv_003CIWlanProvider_003E cComPtrNtv_003CIWlanProvider_003E;
				*(long*)(&cComPtrNtv_003CIWlanProvider_003E) = 0L;
				try
				{
					num = Module.GetInterfaceProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cstruct_0020IWlanProvider_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyWlanProvider, (IWlanProvider**)(&cComPtrNtv_003CIWlanProvider_003E));
					if (num >= 0)
					{
						m_spWlanProvider.op_Assign((IWlanProvider*)(*(ulong*)(&cComPtrNtv_003CIWlanProvider_003E)));
						DeviceMediator* p = m_spDeviceMediator.p;
						DeviceMediator* ptr = (DeviceMediator*)((p == null) ? 0 : ((ulong)(nint)p + 16uL));
						IWlanProvider* p2 = m_spWlanProvider.p;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IZuneWlanCallback*, int>)(*(ulong*)(*(long*)p2 + 24)))((nint)p2, (IZuneWlanCallback*)ptr);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWlanProvider_003E*, void>)(&Module.CComPtrNtv_003CIWlanProvider_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWlanProvider_003E);
					throw;
				}
				Module.CComPtrNtv_003CIWlanProvider_003E_002ERelease(&cComPtrNtv_003CIWlanProvider_003E);
			}
			return num;
		}

		public unsafe int SetWlanProfileList(WlanProfileList profileList)
		{
			//IL_0026: Expected I, but got I8
			//IL_0095: Expected I, but got I8
			//IL_00b0: Expected I, but got I8
			//IL_00d9: Expected I, but got I8
			//IL_0160: Expected I, but got I8
			//IL_0177: Expected I, but got I8
			//IL_0192: Expected I, but got I8
			//IL_01ad: Expected I, but got I8
			//IL_01d3: Expected I, but got I8
			//IL_022b: Expected I, but got I8
			//IL_024d: Expected I, but got I8
			//IL_0274: Expected I, but got I8
			//IL_0274: Expected I, but got I8
			//IL_0296: Expected I, but got I8
			//IL_02ae: Expected I, but got I8
			//IL_02c9: Expected I, but got I8
			//IL_02e4: Expected I, but got I8
			//IL_02f9: Expected I, but got I8
			//IL_031c: Expected I, but got I8
			//IL_03a5: Expected I, but got I8
			//IL_03b4: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1356u);
				return -2147418113;
			}
			IWlanProfileList* ptr = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 29, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanProfileList**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, &ptr);
				if (num >= 0)
				{
					int num2 = 0;
					while (num2 < profileList.Count)
					{
						IWlanProfile* ptr2 = null;
						WlanProfile wlanProfile = profileList[num2];
						fixed (char* SSIDPtr = wlanProfile.SSID.ToCharArray())
						{
							ushort* ptr3 = (ushort*)SSIDPtr;
							try
							{
								ushort* ptr4;
								ushort* ptr5;
								if (ptr3 != null)
								{
									ptr4 = Module.SysAllocString(ptr3);
									ptr5 = null;
									if ((!wlanProfile.Encrypted && null == wlanProfile.Key) || (wlanProfile.Encrypted && null == wlanProfile.EncryptedKey))
									{
										goto IL_035c;
									}
									if (!(null != wlanProfile.Key))
									{
										goto IL_0142;
									}
									fixed (char* keyPtr = wlanProfile.Key.ToCharArray())
									{
										ushort* ptr6 = (ushort*)keyPtr;
										try
										{
											if (ptr6 != null)
											{
												ptr5 = Module.SysAllocString(ptr6);
												goto IL_0142;
											}
										}
										catch
										{
											//try-fault
											ptr6 = null;
											throw;
										}
									}
									goto IL_035d;
								}
								goto end_IL_00c5;
								IL_0142:
								IWlanProvider* p2 = m_spWlanProvider.p;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanProfile**, int>)(*(ulong*)(*(long*)p2 + 32)))((nint)p2, &ptr2);
								if (num >= 0)
								{
									IWlanProfile* intPtr = ptr2;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)ptr2 + 32)))((nint)intPtr, ptr4);
									if (num >= 0)
									{
										IWlanProfile* intPtr2 = ptr2;
										WirelessAuthenticationTypes auth = wlanProfile.Auth;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _DOT11_AUTH_ALGORITHM, int>)(*(ulong*)(*(long*)ptr2 + 48)))((nint)intPtr2, (_DOT11_AUTH_ALGORITHM)auth);
										if (num >= 0)
										{
											IWlanProfile* intPtr3 = ptr2;
											WirelessCiphers cipher = wlanProfile.Cipher;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _DOT11_CIPHER_ALGORITHM, int>)(*(ulong*)(*(long*)ptr2 + 64)))((nint)intPtr3, (_DOT11_CIPHER_ALGORITHM)cipher);
											if (num >= 0)
											{
												int num3 = (wlanProfile.PassPhrase ? 1 : 0);
												IWlanProfile* intPtr4 = ptr2;
												num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr2 + 80)))((nint)intPtr4, num3);
												if (num >= 0)
												{
													if (wlanProfile.Encrypted)
													{
														num = ((256 < (nuint)wlanProfile.EncryptedKey.LongLength) ? (-2147024774) : num);
														if (num < 0)
														{
															goto IL_02fa;
														}
														int num4 = 0;
														_0024ArrayType_0024_0024_0024BY0BAA_0040E _0024ArrayType_0024_0024_0024BY0BAA_0040E;
														if (0 < (nint)wlanProfile.EncryptedKey.LongLength)
														{
															_0024ArrayType_0024_0024_0024BY0BAA_0040E* ptr7 = &_0024ArrayType_0024_0024_0024BY0BAA_0040E;
															do
															{
																*(byte*)ptr7 = wlanProfile.EncryptedKey[num4];
																num4++;
																ptr7 = (_0024ArrayType_0024_0024_0024BY0BAA_0040E*)((ulong)(nint)ptr7 + 1uL);
															}
															while (num4 < (nint)wlanProfile.EncryptedKey.LongLength);
														}
														IWlanProfile* intPtr5 = ptr2;
														IntPtr intPtr6 = (nint)wlanProfile.EncryptedKey.LongLength;
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*, uint, int, int>)(*(ulong*)(*(long*)ptr2 + 96)))((nint)intPtr5, (byte*)(&_0024ArrayType_0024_0024_0024BY0BAA_0040E), (uint)(nint)intPtr6, 1);
													}
													else if (WirelessCiphers.None == wlanProfile.Cipher && 0 == Module.SysStringLen(ptr5))
													{
														IWlanProfile* intPtr7 = ptr2;
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*, uint, int, int>)(*(ulong*)(*(long*)ptr2 + 96)))((nint)intPtr7, null, 0u, 1);
													}
													else
													{
														long num5 = *(long*)ptr2 + 96;
														IWlanProfile* intPtr8 = ptr2;
														ushort* intPtr9 = ptr5;
														uint num6 = Module.SysStringLen(intPtr9);
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*, uint, int, int>)(*(ulong*)num5))((nint)intPtr8, (byte*)intPtr9, (uint)(num6 * 2uL), 0);
													}
													if (num >= 0)
													{
														IWlanProfile* intPtr10 = ptr2;
														uint keyIndex = wlanProfile.KeyIndex;
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)ptr2 + 112)))((nint)intPtr10, keyIndex);
														if (num >= 0)
														{
															IWlanProfile* intPtr11 = ptr2;
															uint signalQuality = wlanProfile.SignalQuality;
															num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)ptr2 + 144)))((nint)intPtr11, signalQuality);
															if (num >= 0)
															{
																IWlanProfile* intPtr12 = ptr2;
																bool connected = wlanProfile.Connected;
																num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr2 + 160)))((nint)intPtr12, connected ? 1 : 0);
																if (num >= 0)
																{
																	IWlanProfileList* intPtr13 = ptr;
																	IWlanProfile* intPtr14 = ptr2;
																	num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanProfile*, int>)(*(ulong*)(*(long*)ptr + 40)))((nint)intPtr13, intPtr14);
																}
															}
														}
													}
												}
											}
										}
									}
								}
								goto IL_02fa;
								IL_02fa:
								Module.SysFreeString(ptr4);
								Module.SysFreeString(ptr5);
								if (0L != (nint)ptr2)
								{
									IWlanProfile* intPtr15 = ptr2;
									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr15 + 16)))((nint)intPtr15);
								}
								goto IL_032a;
								end_IL_00c5:;
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
							try
							{
								Module._ZuneShipAssert(1002u, 1382u);
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
							goto IL_0353;
							IL_035d:
							try
							{
								try
								{
									Module._ZuneShipAssert(1002u, 1398u);
								}
								catch
								{
									//try-fault
									throw;
								}
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
						}
						return -2147418113;
						IL_032a:
						num2++;
						if (num < 0)
						{
							break;
						}
						continue;
						IL_035c:
						return -2147024809;
						IL_0353:
						return -2147418113;
					}
					IWlanProvider* p3 = m_spWlanProvider.p;
					IWlanProfileList* intPtr16 = ptr;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanProfileList*, int>)(*(ulong*)(*(long*)p3 + 48)))((nint)p3, intPtr16);
					IWlanProfileList* intPtr17 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr17 + 16)))((nint)intPtr17);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 30, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int GetWlanProfileList(ref WlanProfileList profileList)
		{
			//IL_0025: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			//IL_00ae: Expected I, but got I8
			//IL_00c5: Expected I, but got I8
			//IL_00d6: Expected I, but got I8
			//IL_00e2: Expected I, but got I8
			//IL_010d: Expected I, but got I8
			//IL_0124: Expected I, but got I8
			//IL_013b: Expected I, but got I8
			//IL_0152: Expected I, but got I8
			//IL_016d: Expected I, but got I8
			//IL_0184: Expected I, but got I8
			//IL_019e: Expected I, but got I8
			//IL_01b8: Expected I, but got I8
			//IL_023f: Expected I, but got I8
			//IL_0294: Expected I, but got I8
			//IL_02af: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1503u);
				return -2147418113;
			}
			IWlanProfileList* ptr = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 31, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanProfileList**, int>)(*(ulong*)(*(long*)p + 56)))((nint)p, &ptr);
				if (num >= 0)
				{
					int num2 = 0;
					IWlanProfileList* intPtr = ptr;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)ptr + 32)))((nint)intPtr, &num2);
					int num3 = 0;
					if (num >= 0)
					{
						while (num3 < num2)
						{
							IWlanProfile* ptr2 = null;
							IWlanProfileList* intPtr2 = ptr;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IWlanProfile**, int>)(*(ulong*)(*(long*)ptr + 48)))((nint)intPtr2, num3, &ptr2);
							if (num >= 0)
							{
								ushort* ptr3 = null;
								_DOT11_AUTH_ALGORITHM auth = (_DOT11_AUTH_ALGORITHM)1;
								_DOT11_CIPHER_ALGORITHM cipher = (_DOT11_CIPHER_ALGORITHM)0;
								int num4 = 0;
								uint num5 = 256u;
								int num6 = 0;
								uint keyIndex = 0u;
								uint signalQuality = 0u;
								int num7 = 0;
								IWlanProfile* intPtr3 = ptr2;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)ptr2 + 24)))((nint)intPtr3, &ptr3);
								if (num >= 0)
								{
									IWlanProfile* intPtr4 = ptr2;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _DOT11_AUTH_ALGORITHM*, int>)(*(ulong*)(*(long*)ptr2 + 40)))((nint)intPtr4, &auth);
									if (num >= 0)
									{
										IWlanProfile* intPtr5 = ptr2;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _DOT11_CIPHER_ALGORITHM*, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr5, &cipher);
										if (num >= 0)
										{
											IWlanProfile* intPtr6 = ptr2;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)ptr2 + 72)))((nint)intPtr6, &num4);
											if (num >= 0)
											{
												IWlanProfile* intPtr7 = ptr2;
												_0024ArrayType_0024_0024_0024BY0BAA_0040E _0024ArrayType_0024_0024_0024BY0BAA_0040E;
												num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*, uint*, int*, int>)(*(ulong*)(*(long*)ptr2 + 88)))((nint)intPtr7, (byte*)(&_0024ArrayType_0024_0024_0024BY0BAA_0040E), &num5, &num6);
												if (num >= 0)
												{
													IWlanProfile* intPtr8 = ptr2;
													num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)ptr2 + 104)))((nint)intPtr8, &keyIndex);
													if (num >= 0)
													{
														IWlanProfile* intPtr9 = ptr2;
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)ptr2 + 136)))((nint)intPtr9, &signalQuality);
														if (num >= 0)
														{
															IWlanProfile* intPtr10 = ptr2;
															num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)ptr2 + 152)))((nint)intPtr10, &num7);
															if (num >= 0)
															{
																WlanProfile wlanProfile = new WlanProfile();
																wlanProfile.SSID = new string((char*)ptr3);
																wlanProfile.Auth = (WirelessAuthenticationTypes)auth;
																wlanProfile.Cipher = (WirelessCiphers)cipher;
																byte b = ((wlanProfile.PassPhrase = ((num4 != 0) ? true : false)) ? ((byte)1) : ((byte)0));
																if (0 == num5)
																{
																	wlanProfile.EncryptedKey = new byte[0];
																}
																else if (num6 != 0)
																{
																	wlanProfile.EncryptedKey = new byte[num5];
																	uint num8 = 0u;
																	if (0 < num5)
																	{
																		_0024ArrayType_0024_0024_0024BY0BAA_0040E* ptr4 = &_0024ArrayType_0024_0024_0024BY0BAA_0040E;
																		do
																		{
																			wlanProfile.EncryptedKey[num8] = *(byte*)ptr4;
																			num8++;
																			ptr4 = (_0024ArrayType_0024_0024_0024BY0BAA_0040E*)((ulong)(nint)ptr4 + 1uL);
																		}
																		while (num8 < num5);
																	}
																}
																else
																{
																	wlanProfile.Key = new string((char*)(&_0024ArrayType_0024_0024_0024BY0BAA_0040E));
																}
																wlanProfile.KeyIndex = keyIndex;
																wlanProfile.SignalQuality = signalQuality;
																byte b2 = ((wlanProfile.Connected = num7 == 1) ? ((byte)1) : ((byte)0));
																profileList.Add(wlanProfile);
															}
														}
													}
												}
											}
										}
									}
								}
								Module.SysFreeString(ptr3);
								if (0L != (nint)ptr2)
								{
									IWlanProfile* intPtr11 = ptr2;
									((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr11 + 16)))((nint)intPtr11);
								}
							}
							num3++;
							if (num < 0)
							{
								break;
							}
						}
					}
				}
			}
			IWlanProfileList* intPtr12 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr12 + 16)))((nint)intPtr12);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 32, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int GetWlanProfiles()
		{
			//IL_0044: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1636u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 64)))((nint)p);
			}
			return num;
		}

		public unsafe int IsWlanFirewallEnabled(ref bool bEnabled)
		{
			//IL_004a: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1664u);
				return -2147418113;
			}
			int num = 0;
			int num2 = LoadWlanProvider();
			if (num2 >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 72)))((nint)p, &num);
			}
			bool flag = (bEnabled = ((num != 0) ? true : false));
			return num2;
		}

		public void GetWlanProfilesComplete(int hr)
		{
			if (m_GetWlanProfilesComplete != null)
			{
				m_GetWlanProfilesComplete(this, hr);
			}
		}

		public unsafe int GetDeviceWlanNetworks()
		{
			//IL_0044: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1686u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 80)))((nint)p);
			}
			return num;
		}

		public void GetDeviceWlanNetworksComplete(int hr)
		{
			if (m_GetDeviceWlanNetworksComplete != null)
			{
				m_GetDeviceWlanNetworksComplete(this, hr);
			}
		}

		public unsafe int GetDeviceWlanProfiles()
		{
			//IL_0044: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1714u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 88)))((nint)p);
			}
			return num;
		}

		public void GetDeviceWlanProfilesComplete(int hr)
		{
			if (m_GetDeviceWlanProfilesComplete != null)
			{
				m_GetDeviceWlanProfilesComplete(this, hr);
			}
		}

		public unsafe int SetDeviceWlanProfiles()
		{
			//IL_0044: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1742u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 96)))((nint)p);
			}
			return num;
		}

		public void SetDeviceWlanProfilesComplete(int hr)
		{
			if (m_SetDeviceWlanProfilesComplete != null)
			{
				m_SetDeviceWlanProfilesComplete(this, hr);
			}
		}

		public unsafe int GetWlanDeviceAuthCipherPairList(ref WlanAuthCipherPairList authCipherPairList)
		{
			//IL_0025: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			//IL_00ab: Expected I, but got I8
			//IL_00c7: Expected I, but got I8
			//IL_0103: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1770u);
				return -2147418113;
			}
			IWlanAuthCipherPairList* ptr = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 33, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWlanAuthCipherPairList**, int>)(*(ulong*)(*(long*)p + 104)))((nint)p, &ptr);
				if (num >= 0)
				{
					int num2 = 0;
					IWlanAuthCipherPairList* intPtr = ptr;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)ptr + 32)))((nint)intPtr, &num2);
					int num3 = 0;
					if (num >= 0)
					{
						while (num3 < num2)
						{
							IWlanAuthCipherPairList* intPtr2 = ptr;
							DOT11_AUTH_CIPHER_PAIR dOT11_AUTH_CIPHER_PAIR;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, DOT11_AUTH_CIPHER_PAIR*, int>)(*(ulong*)(*(long*)ptr + 48)))((nint)intPtr2, num3, &dOT11_AUTH_CIPHER_PAIR);
							if (num >= 0)
							{
								WlanAuthCipherPair wlanAuthCipherPair = new WlanAuthCipherPair();
								wlanAuthCipherPair.Auth = *(WirelessAuthenticationTypes*)(&dOT11_AUTH_CIPHER_PAIR);
								wlanAuthCipherPair.Cipher = Unsafe.As<DOT11_AUTH_CIPHER_PAIR, WirelessCiphers>(ref Unsafe.AddByteOffset(ref dOT11_AUTH_CIPHER_PAIR, 4));
								authCipherPairList.Add(wlanAuthCipherPair);
							}
							num3++;
							if (num < 0)
							{
								break;
							}
						}
					}
					IWlanAuthCipherPairList* intPtr3 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 34, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int GetDisconnectedWlanDeviceUuid(ref string strDeviceUuid)
		{
			//IL_0025: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1818u);
				return -2147418113;
			}
			ushort* ptr = null;
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, &ptr);
				if (num >= 0)
				{
					strDeviceUuid = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
			}
			return num;
		}

		public unsafe int GetAssociatedWlanDeviceUuidList(ref List<string> deviceUuidList)
		{
			//IL_0071: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1845u);
				return -2147418113;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 35, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			int num = LoadWlanProvider();
			uint num2 = 0u;
			do
			{
				ushort* ptr = null;
				if (num >= 0)
				{
					IWlanProvider* p = m_spWlanProvider.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, num2, &ptr);
					if (num >= 0)
					{
						string item = new string((char*)ptr);
						deviceUuidList.Add(item);
						Module.SysFreeString(ptr);
					}
				}
				num2++;
			}
			while (num >= 0);
			if (-2147024894 == num || -2147024637 == num)
			{
				num = 0;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 36, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int AssociateWlanDevice()
		{
			//IL_0047: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1893u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 128)))((nint)p);
			}
			return num;
		}

		public void AssociateWlanDeviceComplete(int hr)
		{
			if (m_AssociateWlanDeviceComplete != null)
			{
				m_AssociateWlanDeviceComplete(this, hr);
			}
		}

		public unsafe int UnassociateWlanDevice()
		{
			//IL_0047: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1921u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 136)))((nint)p);
			}
			return num;
		}

		public void UnassociateWlanDeviceComplete(int hr)
		{
			if (m_UnassociateWlanDeviceComplete != null)
			{
				m_UnassociateWlanDeviceComplete(this, hr);
			}
		}

		public unsafe int UnassociateWlanDeviceUuid(string strDeviceUuid)
		{
			//IL_0058: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1949u);
				return -2147418113;
			}
			int num = 0;
			fixed (char* strDeviceUuidPtr = strDeviceUuid.ToCharArray())
			{
				ushort* ptr = (ushort*)strDeviceUuidPtr;
				if (ptr != null)
				{
					num = LoadWlanProvider();
					if (num >= 0)
					{
						IWlanProvider* p = m_spWlanProvider.p;
						long num2 = *(long*)p + 144;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num2))((nint)p, ptr);
					}
				}
				return num;
			}
		}

		public unsafe int IsWlanDeviceDisabled(ref bool bDisabled)
		{
			//IL_004d: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1973u);
				return -2147418113;
			}
			int num = 1;
			int num2 = LoadWlanProvider();
			if (num2 >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, &num);
			}
			bool flag = (bDisabled = ((num != 0) ? true : false));
			return num2;
		}

		public unsafe int IsWlanDeviceUuidDisabled(string strDeviceUuid, ref bool bDisabled)
		{
			//IL_005f: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 1995u);
				return -2147418113;
			}
			int num = 0;
			int num2 = 1;
			fixed (char* strDeviceUuidPtr = strDeviceUuid.ToCharArray())
			{
				ushort* ptr = (ushort*)strDeviceUuidPtr;
				if (ptr != null)
				{
					num = LoadWlanProvider();
					if (num >= 0)
					{
						IWlanProvider* p = m_spWlanProvider.p;
						long num3 = *(long*)p + 160;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num3))((nint)p, ptr, &num2);
					}
				}
				bool flag = (bDisabled = ((num2 != 0) ? true : false));
				return num;
			}
		}

		public unsafe int TestDeviceWlan()
		{
			//IL_0047: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2022u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 168)))((nint)p);
			}
			return num;
		}

		public unsafe int CancelTestDeviceWlan()
		{
			//IL_0047: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2041u);
				return -2147418113;
			}
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 176)))((nint)p);
			}
			return num;
		}

		public unsafe void TestDeviceWlanComplete(int hr)
		{
			//IL_002b: Expected I, but got I8
			uint result = 7u;
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)p + 184)))((nint)p, &result);
				if (num >= 0)
				{
					goto IL_003a;
				}
			}
			hr = ((hr >= 0) ? num : hr);
			goto IL_003a;
			IL_003a:
			if (m_TestDeviceWlanComplete != null)
			{
				m_TestDeviceWlanComplete(this, (WlanTestResultCode)result, hr);
			}
		}

		public unsafe int GetDeviceWlanConnectedSSID(ref string strSSID)
		{
			//IL_0025: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2089u);
				return -2147418113;
			}
			ushort* ptr = null;
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 192)))((nint)p, &ptr);
				if (num >= 0)
				{
					strSSID = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
			}
			return num;
		}

		public unsafe int GetDeviceWlanMediaSyncSSID(ref string strSSID)
		{
			//IL_0025: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2116u);
				return -2147418113;
			}
			ushort* ptr = null;
			int num = LoadWlanProvider();
			if (num >= 0)
			{
				IWlanProvider* p = m_spWlanProvider.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 200)))((nint)p, &ptr);
				if (num >= 0)
				{
					strSSID = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
			}
			return num;
		}

		public unsafe int SetDeviceWlanMediaSyncSSID(string strSSID)
		{
			//IL_0056: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2143u);
				return -2147418113;
			}
			fixed (char* strSSIDPtr = strSSID.ToCharArray())
			{
				ushort* ptr = (ushort*)strSSIDPtr;
				int num;
				if (ptr != null)
				{
					num = LoadWlanProvider();
					if (num >= 0)
					{
						IWlanProvider* p = m_spWlanProvider.p;
						long num2 = *(long*)p + 208;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num2))((nint)p, ptr);
					}
				}
				else
				{
					num = -2147467261;
				}
				return num;
			}
		}

		public unsafe int GetSyncRelationship(ref ESyncRelationship relationship)
		{
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2172u);
				return -2147418113;
			}
			ESyncRelationship eSyncRelationship = ESyncRelationship.srNone;
			int num = Module.GetEnumProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cenum_0020ESyncRelationship_003E(p, EEndpointHostProperty.eEndpointHostPropertySyncRelationship, &eSyncRelationship);
			if (num >= 0)
			{
				relationship = eSyncRelationship;
			}
			return num;
		}

		public unsafe int SetSyncRelationship(ESyncRelationship relationship)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2196u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int, int>)(*(ulong*)(*(long*)p + 200)))((nint)p, EEndpointHostProperty.eEndpointHostPropertySyncRelationship, (int)relationship);
		}

		public unsafe int GetPromptGuest(ref bool bPromptGuest)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2206u);
				return -2147418113;
			}
			bool flag;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPromptGuest, &flag);
			if (num >= 0)
			{
				bPromptGuest = flag;
			}
			return num;
		}

		public unsafe int SetPromptGuest([MarshalAs(UnmanagedType.U1)] bool bPromptGuest)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2225u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPromptGuest, bPromptGuest ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetPromptLink(ref bool bPromptLink)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2236u);
				return -2147418113;
			}
			bool flag;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPromptLink, &flag);
			if (num >= 0)
			{
				bPromptLink = flag;
			}
			return num;
		}

		public unsafe int SetPromptLink([MarshalAs(UnmanagedType.U1)] bool bPromptLink)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2255u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPromptLink, bPromptLink ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetFirmwareVersion(ref string strFirmwareVersion)
		{
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2266u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyFirmwareVersion, &ptr);
			if (num >= 0 && ptr != null)
			{
				strFirmwareVersion = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int GetSpaceFree(ref ulong ui64SpaceFree)
		{
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2286u);
				return -2147418113;
			}
			ulong num = 0uL;
			IEndpointHost* p = spEndpointHost.p;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ulong*, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyAvailableSpace, &num);
			if (num2 >= 0)
			{
				ui64SpaceFree = num;
			}
			return num2;
		}

		public unsafe int GetSyncOnConnect(ref bool bSyncOnConnect)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2306u);
				return -2147418113;
			}
			bool flag;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingSyncOnConnect, &flag);
			if (num >= 0)
			{
				bSyncOnConnect = flag;
			}
			return num;
		}

		public unsafe int SetSyncOnConnect([MarshalAs(UnmanagedType.U1)] bool bSyncOnConnect)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2326u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingSyncOnConnect, bSyncOnConnect ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetPercentSpaceReserved(ref uint ulPercentage)
		{
			//IL_0038: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2336u);
				return -2147418113;
			}
			uint num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint*, int>)(*(ulong*)(*(long*)p + 144)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPercentSpaceReserved, &num);
			if (num2 >= 0)
			{
				ulPercentage = num;
			}
			return num2;
		}

		public unsafe int SetPercentSpaceReserved(uint ulPercentage)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2356u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, uint, int>)(*(ulong*)(*(long*)p + 208)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyPercentSpaceReserved, ulPercentage);
		}

		public unsafe int DeleteMedia(int[] rgIds, EMediaTypes mediaType, ref ESyncOperationStatus operationStatus)
		{
			//IL_0103: Expected I, but got I8
			if (m_spSyncEngine.p == null)
			{
				Module._ZuneShipAssert(1002u, 2366u);
				return -2147418113;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 37, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			fixed (int* ptr3 = &rgIds[0])
			{
				int num = ((EMediaTypes.eMediaTypeFolder != mediaType) ? m_syncRules.Remove(rgIds, mediaType, fDeviceFolderIds: false) : m_syncRules.Remove(rgIds, EMediaTypes.eMediaTypeFolder, fDeviceFolderIds: true));
				if (num >= 0)
				{
					num = m_syncRules.Exclude(rgIds, mediaType);
				}
				int num2 = rgIds.Length;
				int[] array = new int[num2];
				int num3 = 0;
				if (0 < num2)
				{
					do
					{
						array[num3] = (int)mediaType;
						num3++;
					}
					while (num3 < (nint)rgIds.LongLength);
				}
				fixed (int* ptr = &array[0])
				{
					try
					{
						int* ptr2 = ptr;
						ESyncOperationStatus eSyncOperationStatus = ESyncOperationStatus.osInvalid;
						if (num >= 0)
						{
							ISyncEngine* p = m_spSyncEngine.p;
							IntPtr intPtr = (nint)rgIds.LongLength;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int*, int*, ESyncOperationStatus*, int>)(*(ulong*)(*(long*)p + 200)))((nint)p, (int)(nint)intPtr, ptr3, ptr2, &eSyncOperationStatus);
							if (num >= 0)
							{
								operationStatus = eSyncOperationStatus;
							}
						}
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 38, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
						}
					}
					catch
					{
						//try-fault
						array = null;
						throw;
					}
				}
				return num;
			}
		}

		public unsafe int ReverseSync(int[] rgIds, EMediaTypes mediaType, ref ESyncOperationStatus operationStatus)
		{
			//IL_00c7: Expected I, but got I8
			if (m_spSyncEngine.p == null)
			{
				Module._ZuneShipAssert(1002u, 2461u);
				return -2147418113;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 39, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
			}
			fixed (int* ptr3 = &rgIds[0])
			{
				int num = rgIds.Length;
				int[] array = new int[num];
				int num2 = 0;
				if (0 < num)
				{
					do
					{
						array[num2] = (int)mediaType;
						num2++;
					}
					while (num2 < (nint)rgIds.LongLength);
				}
				int num3;
				fixed (int* ptr = &array[0])
				{
					try
					{
						int* ptr2 = ptr;
						ESyncOperationStatus eSyncOperationStatus = ESyncOperationStatus.osInvalid;
						ISyncEngine* p = m_spSyncEngine.p;
						IntPtr intPtr = (nint)rgIds.LongLength;
						num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int*, int*, ESyncOperationStatus*, int>)(*(ulong*)(*(long*)p + 208)))((nint)p, (int)(nint)intPtr, ptr3, ptr2, &eSyncOperationStatus);
						if (num3 >= 0)
						{
							operationStatus = eSyncOperationStatus;
						}
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 40, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num3);
						}
					}
					catch
					{
						//try-fault
						array = null;
						throw;
					}
				}
				return num3;
			}
		}

		public unsafe int GetDeviceVideoDeleteSet(out int[] rgIds)
		{
			//IL_005f: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			CComPtrNtv_003CIMetadataManager_003E cComPtrNtv_003CIMetadataManager_003E;
			*(long*)(&cComPtrNtv_003CIMetadataManager_003E) = 0L;
			int num;
			try
			{
				CComPtrNtv_003CIDeviceContentProvider_003E cComPtrNtv_003CIDeviceContentProvider_003E;
				*(long*)(&cComPtrNtv_003CIDeviceContentProvider_003E) = 0L;
				try
				{
					IntSet intSet;
					*(int*)(&intSet) = 0;
                    Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 4)) = -1;
                    Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 8)) = -1;
                    Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 12)) = 0;
                    Unsafe.As<IntSet, long>(ref Unsafe.AddByteOffset(ref intSet, 16)) = 0L;
                    Unsafe.As<IntSet, long>(ref Unsafe.AddByteOffset(ref intSet, 24)) = 0L;
					try
					{
						num = Module.GetSingleton(Module.GUID_IMetadataManager, (void**)(&cComPtrNtv_003CIMetadataManager_003E));
						if (num >= 0)
						{
							long num2 = *(long*)(&cComPtrNtv_003CIMetadataManager_003E);
							_GUID guid_IDeviceContentProvider = Module.GUID_IDeviceContentProvider;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIMetadataManager_003E)) + 24)))((nint)num2, (_GUID)guid_IDeviceContentProvider, (void**)(&cComPtrNtv_003CIDeviceContentProvider_003E));
							if (num >= 0)
							{
								long num3 = *(long*)(&cComPtrNtv_003CIDeviceContentProvider_003E);
								int iDeviceID = m_iDeviceID;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IntSet*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDeviceContentProvider_003E)) + 96)))((nint)num3, iDeviceID, &intSet);
								if (num >= 0 && !(Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 8)) == -1))
								{
									rgIds = new int[Module.DataStructs_002EIntSet_002EMemberCount(&intSet)];
									int num4 = Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 4));
									int num5 = 0;
									if (Unsafe.As<IntSet, int>(ref Unsafe.AddByteOffset(ref intSet, 4)) != -1)
									{
										do
										{
											rgIds[num5] = num4;
											num5++;
											num4 = Module.DataStructs_002EIntSet_002EGetNextMember(&intSet, num4);
										}
										while (num4 != -1);
									}
								}
							}
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IntSet*, void>)(&Module.DataStructs_002EIntSet_002E_007Bdtor_007D), &intSet);
						throw;
					}
					Module.DataStructs_002EIntSet_002EFreeData(&intSet);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDeviceContentProvider_003E*, void>)(&Module.CComPtrNtv_003CIDeviceContentProvider_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDeviceContentProvider_003E);
					throw;
				}
				Module.CComPtrNtv_003CIDeviceContentProvider_003E_002ERelease(&cComPtrNtv_003CIDeviceContentProvider_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMetadataManager_003E*, void>)(&Module.CComPtrNtv_003CIMetadataManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMetadataManager_003E);
				throw;
			}
			Module.CComPtrNtv_003CIMetadataManager_003E_002ERelease(&cComPtrNtv_003CIMetadataManager_003E);
			return num;
		}

		public unsafe int GetVideoTranscodeOptimization(ref ETranscodeOptimization transcodeOptimization)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2500u);
				return -2147418113;
			}
			ETranscodeOptimization eTranscodeOptimization = ETranscodeOptimization.toOptimizeForSize;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, int*, int>)(*(ulong*)(*(long*)p + 232)))((nint)p, EEndpointHostSetting.eEndpointHostSettingVideoTranscodeOptimization, (int*)(&eTranscodeOptimization));
			if (num >= 0)
			{
				transcodeOptimization = eTranscodeOptimization;
			}
			return num;
		}

		public unsafe int SetVideoTranscodeOptimization(ETranscodeOptimization transcodeOptimization)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2519u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, int, int>)(*(ulong*)(*(long*)p + 264)))((nint)p, EEndpointHostSetting.eEndpointHostSettingVideoTranscodeOptimization, (int)transcodeOptimization);
		}

		public unsafe int GetPhotoVideoReverseSync(ref bool bReverseSync)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2530u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingReverseSyncUGC, &flag);
			if (num >= 0)
			{
				bReverseSync = flag;
			}
			return num;
		}

		public unsafe int SetPhotoVideoReverseSync([MarshalAs(UnmanagedType.U1)] bool bReverseSync)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2551u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingReverseSyncUGC, bReverseSync ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetDeletePhotoVideoAfterReverseSync(ref bool bDeleteAfterSync)
		{
			//IL_0041: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2563u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingDeleteUGCAfterSync, &flag);
			if (num >= 0)
			{
				bDeleteAfterSync = flag;
			}
			return num;
		}

		public unsafe int SetDeletePhotoVideoAfterReverseSync([MarshalAs(UnmanagedType.U1)] bool bDeleteAfterSync)
		{
			//IL_0037: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2584u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingDeleteUGCAfterSync, bDeleteAfterSync ? ((byte)1) : ((byte)0));
		}

		public unsafe int GetCameraRollDestinationFolder(ref string strDestinationFolder)
		{
			//IL_0027: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2596u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, ushort**, int>)(*(ulong*)(*(long*)p + 224)))((nint)p, EEndpointHostSetting.eEndpointHostCameraRollDestinationFolder, &ptr);
			if (num >= 0 && ptr != null)
			{
				strDestinationFolder = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int SetCameraRollDestinationFolder(string strDestinationFolder)
		{
			//IL_004d: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2619u);
				return -2147418113;
			}
			fixed (char* strDestinationFolderPtr = strDestinationFolder.ToCharArray())
			{
				ushort* ptr = (ushort*)strDestinationFolderPtr;
				int result;
				if (ptr != null)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					long num = *(long*)p + 256;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, ushort*, int>)(*(ulong*)num))((nint)p, EEndpointHostSetting.eEndpointHostCameraRollDestinationFolder, ptr);
				}
				else
				{
					result = -2147418113;
				}
				return result;
			}
		}

		public unsafe int GetSavedDestinationFolder(ref string strDestinationFolder)
		{
			//IL_0027: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2643u);
				return -2147418113;
			}
			ushort* ptr = null;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, ushort**, int>)(*(ulong*)(*(long*)p + 224)))((nint)p, EEndpointHostSetting.eEndpointHostSavedDestinationFolder, &ptr);
			if (num >= 0 && ptr != null)
			{
				strDestinationFolder = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		public unsafe int SetSavedDestinationFolder(string strDestinationFolder)
		{
			//IL_004d: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2666u);
				return -2147418113;
			}
			fixed (char* strDestinationFolderPtr = strDestinationFolder.ToCharArray())
			{
				ushort* ptr = (ushort*)strDestinationFolderPtr;
				int result;
				if (ptr != null)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					long num = *(long*)p + 256;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, ushort*, int>)(*(ulong*)num))((nint)p, EEndpointHostSetting.eEndpointHostSavedDestinationFolder, ptr);
				}
				else
				{
					result = -2147418113;
				}
				return result;
			}
		}

		public unsafe int GetPhotoTranscodeSetting(ref ETranscodePhotoSetting ePhotoSetting)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2690u);
				return -2147418113;
			}
			ETranscodePhotoSetting eTranscodePhotoSetting = ETranscodePhotoSetting.tsPhotoSettingDevicePreferred;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, int*, int>)(*(ulong*)(*(long*)p + 232)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPhotoTranscodeSetting, (int*)(&eTranscodePhotoSetting));
			if (num >= 0)
			{
				ePhotoSetting = eTranscodePhotoSetting;
			}
			return num;
		}

		public unsafe int SetPhotoTranscodeSetting(ETranscodePhotoSetting ePhotoSetting)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2709u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, int, int>)(*(ulong*)(*(long*)p + 264)))((nint)p, EEndpointHostSetting.eEndpointHostSettingPhotoTranscodeSetting, (int)ePhotoSetting);
		}

		public unsafe int GetAudioTranscodeParams(ref int audioThresholdBitRate, ref int audioTargetBitRate)
		{
			//IL_0023: Expected I, but got I8
			//IL_0047: Expected I, but got I8
			uint num = 0u;
			uint num2 = 0u;
			IEndpointHost* p = m_spEndpointHost.p;
			int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, uint*, int>)(*(ulong*)(*(long*)p + 240)))((nint)p, EEndpointHostSetting.eEndpointHostSettingAudioThresholdBitRate, &num);
			if (num3 >= 0)
			{
				p = m_spEndpointHost.p;
				num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, uint*, int>)(*(ulong*)(*(long*)p + 240)))((nint)p, EEndpointHostSetting.eEndpointHostSettingAudioTargetBitRate, &num2);
				if (num3 >= 0)
				{
					audioThresholdBitRate = (int)num;
					audioTargetBitRate = (int)num2;
				}
			}
			return num3;
		}

		public unsafe int SetAudioTranscodeParams(int audioThresholdBitRate, int audioTargetBitRate)
		{
			//IL_001e: Expected I, but got I8
			//IL_0041: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, uint, int>)(*(ulong*)(*(long*)p + 272)))((nint)p, EEndpointHostSetting.eEndpointHostSettingAudioThresholdBitRate, (uint)audioThresholdBitRate);
			if (num >= 0)
			{
				p = m_spEndpointHost.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, uint, int>)(*(ulong*)(*(long*)p + 272)))((nint)p, EEndpointHostSetting.eEndpointHostSettingAudioTargetBitRate, (uint)audioTargetBitRate);
			}
			return num;
		}

		public unsafe int GetLocalizedDevicePath(ref string strPath)
		{
			//IL_0025: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2770u);
				return -2147418113;
			}
			ushort* ptr = null;
			fixed (char* strPathPtr = strPath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)strPathPtr;
				IEndpointHost* p = m_spEndpointHost.p;
				long num = *(long*)p + 288;
				int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, ushort*, ushort**, int>)(*(ulong*)num))((nint)p, EEndpointHostAction.eEndpointHostActionLocalizeDevicePath, ptr2, &ptr);
				if (num2 >= 0 && ptr != null)
				{
					strPath = new string((char*)ptr);
					Module.SysFreeString(ptr);
				}
				return num2;
			}
		}

		public unsafe int ClearCache()
		{
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 2794u);
				return -2147418113;
			}
			DeviceList instance = DeviceList.Instance;
			int result;
			if (instance != null)
			{
				IEndpointHost* p = m_spEndpointHost.p;
				result = instance.ForgetEndpoint(p);
			}
			else
			{
				result = -2147418113;
			}
			return result;
		}

		public unsafe int ClearRules()
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2815u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionClearRules);
		}

		public unsafe int ClearManualModeRules()
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2826u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionClearManualModeRules);
		}

		public unsafe int DeleteAllGuestContent(ref ESyncOperationStatus operationStatus)
		{
			//IL_003f: Expected I, but got I8
			CComPtrMgd<ISyncEngine> spSyncEngine = m_spSyncEngine;
			if (spSyncEngine.p == null)
			{
				Module._ZuneShipAssert(1002u, 2836u);
				return -2147418113;
			}
			ESyncOperationStatus eSyncOperationStatus = ESyncOperationStatus.osInvalid;
			ISyncEngine* p = spSyncEngine.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ESyncOperationStatus*, int>)(*(ulong*)(*(long*)p + 224)))((nint)p, &eSyncOperationStatus);
			if (num >= 0)
			{
				operationStatus = eSyncOperationStatus;
			}
			return num;
		}

		public unsafe int ForceAppUpdate()
		{
			//IL_0035: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 2853u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionForceAppUpdate);
		}

		public int SyncBeginCallback()
		{
			if (m_SyncBegan != null)
			{
				m_SyncBegan(this);
			}
			return 0;
		}

		public unsafe int SyncProgressCallback(uint uiPercentComplete, uint uiPercentItemComplete, uint uiPercentTranscodeComplete, ushort* bstrGroup, ushort* bstrTitle, ESyncEngineState engineState)
		{
			if (m_SyncProgressed != null)
			{
				m_SyncProgressed(this, uiPercentComplete, uiPercentItemComplete, uiPercentTranscodeComplete, new string((char*)bstrGroup), new string((char*)bstrTitle), engineState);
			}
			return 0;
		}

		public int SyncCompleteCallback(int hrResult)
		{
			if (m_SyncCompleted != null)
			{
				ESyncEventReason reason = ((hrResult < 0) ? ESyncEventReason.eSyncEventFailed : ESyncEventReason.eSyncEventSucceeded);
				m_SyncCompleted(this, reason);
			}
			return 0;
		}

		public unsafe void FriendlyNameChanged(ushort* wszFriendlyName)
		{
			if (m_FriendlyNameChanged != null)
			{
				m_FriendlyNameChanged(this, new string((char*)wszFriendlyName));
			}
		}

		public unsafe void EndpointStatusChanged(int hrEnumeration, EEndpointStatus eDeviceStatus)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_Dd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 41, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), (uint)hrEnumeration, m_iDeviceID);
			}
			m_hrEnumeration = hrEnumeration;
			if (eDeviceStatus == EEndpointStatus.eEndpointStatusAvailable && !m_fInitializationCompleted)
			{
				int num = CompleteInitialization();
				if (-1072885173 == num || -1072885172 == num)
				{
					DeviceList.Instance.HideDevice(this);
				}
			}
			m_eLastDeviceStatus = eDeviceStatus;
			if (m_DeviceStatusChanged != null)
			{
				m_DeviceStatusChanged(this, m_hrEnumeration, eDeviceStatus);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 42, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
		}

		public void FormatComplete(int hrResult)
		{
			if (m_FormatComplete != null)
			{
				m_FormatComplete(this, hrResult);
			}
		}

		public unsafe int FileExistsForTranscode(int iMediaId, EMediaTypes eMediaType, ref string strTranscodedFileName)
		{
			//IL_0027: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			CComPtrMgd<ISyncEngine> spSyncEngine = m_spSyncEngine;
			if (spSyncEngine.p == null)
			{
				Module._ZuneShipAssert(1002u, 2986u);
				return -2147418113;
			}
			ushort* ptr = null;
			ISyncEngine* p = spSyncEngine.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, ushort**, int>)(*(ulong*)(*(long*)p + 192)))((nint)p, iMediaId, (int)eMediaType, &ptr);
			if (num >= 0 && ptr != null)
			{
				strTranscodedFileName = new string((char*)ptr);
				Module.SysFreeString(ptr);
			}
			return num;
		}

		internal unsafe Device()
		{
			CComPtrMgd<IEndpointHost> spEndpointHost = new CComPtrMgd<IEndpointHost>();
			try
			{
				m_spEndpointHost = spEndpointHost;
				CComPtrMgd<ISyncEngine> spSyncEngine = new CComPtrMgd<ISyncEngine>();
				try
				{
					m_spSyncEngine = spSyncEngine;
					CComPtrMgd<IWlanProvider> spWlanProvider = new CComPtrMgd<IWlanProvider>();
					try
					{
						m_spWlanProvider = spWlanProvider;
						CComPtrMgd<DeviceMediator> spDeviceMediator = new CComPtrMgd<DeviceMediator>();
						try
						{
							m_spDeviceMediator = spDeviceMediator;
							if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
							{
								Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
							}
							m_Lock = new object();
							m_eLastDeviceStatus = EEndpointStatus.eEndpointStatusUndefined;
							if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
							{
								Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
							}
						}
						catch
						{
							//try-fault
							((IDisposable)m_spDeviceMediator).Dispose();
							throw;
						}
					}
					catch
					{
						//try-fault
						((IDisposable)m_spWlanProvider).Dispose();
						throw;
					}
				}
				catch
				{
					//try-fault
					((IDisposable)m_spSyncEngine).Dispose();
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

		internal unsafe int Initialize(IEndpointHost* pEndpointHost)
		{
			//IL_01a6: Expected I, but got I8
			//IL_01e2: Expected I, but got I8
			//IL_0233: Expected I, but got I8
			//IL_0246: Expected I, but got I8
			//IL_0266: Expected I, but got I8
			//IL_02ab: Expected I, but got I8
			ManagedLock managedLock = null;
			int num = 0;
			if (pEndpointHost == null)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 14, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
				}
				return -2147467261;
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 15, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
			ManagedLock managedLock2 = new ManagedLock(m_Lock);
			try
			{
				managedLock = managedLock2;
				m_fInitializationCompleted = false;
				CComPtrMgd<IEndpointHost> spEndpointHost = m_spEndpointHost;
				if (spEndpointHost.p != pEndpointHost)
				{
					spEndpointHost.op_Assign(pEndpointHost);
				}
				EEndpointStatus eLastDeviceStatus = EEndpointStatus.eEndpointStatusUndefined;
				if (Module.GetEndpointHostEnumProperty_003Cenum_0020EEndpointStatus_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyEndpointStatus, &eLastDeviceStatus) >= 0)
				{
					m_eLastDeviceStatus = eLastDeviceStatus;
				}
				else
				{
					m_eLastDeviceStatus = EEndpointStatus.eEndpointStatusUndefined;
				}
				SyncRules syncRules = m_syncRules;
				if (syncRules != null)
				{
					((IDisposable)syncRules).Dispose();
					m_syncRules = null;
				}
				if ((m_syncRules = new SyncRules(pEndpointHost)) == null)
				{
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 2u)
					{
						Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 16, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), 0);
					}
					num = -2147024882;
				}
				m_spSyncEngine.Release();
				DeviceMediator* p = m_spDeviceMediator.p;
				CComPtrNtv_003CDeviceMediator_003E cComPtrNtv_003CDeviceMediator_003E;
				Module.CComPtrNtv_003CDeviceMediator_003E_002E_007Bctor_007D(&cComPtrNtv_003CDeviceMediator_003E, p);
				try
				{
					if (num >= 0)
					{
						DeviceMediator* ptr = (DeviceMediator*)Module.@new(72uL);
						DeviceMediator* lp;
						try
						{
							if (ptr != null)
							{
								IEndpointHost* p2 = m_spEndpointHost.p;
								lp = Module.DeviceMediator_002E_007Bctor_007D(ptr, this, p2);
							}
							else
							{
								lp = null;
							}
						}
						catch
						{
							//try-fault
							Module.delete(ptr);
							throw;
						}
						m_spDeviceMediator.op_Assign(lp);
						num = (((long)(nint)m_spDeviceMediator.p == 0) ? (-2147024882) : num);
					}
					if (*(long*)(&cComPtrNtv_003CDeviceMediator_003E) != 0L)
					{
						Module.DeviceMediator_002EShutdown((DeviceMediator*)(*(ulong*)(&cComPtrNtv_003CDeviceMediator_003E)));
					}
					Module.CComPtrNtv_003CDeviceMediator_003E_002ERelease(&cComPtrNtv_003CDeviceMediator_003E);
					m_spWlanProvider.Release();
					m_fClientUpdateRequired = false;
					m_fFirmwareUpdateRequired = false;
					m_hrEnumeration = 0;
					if (num >= 0)
					{
						IEndpointHost* p3 = m_spEndpointHost.p;
						int iDeviceID;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p3 + 136)))((nint)p3, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &iDeviceID);
						m_iDeviceID = iDeviceID;
						if (num >= 0)
						{
							ushort* ptr2 = null;
							IEndpointHost* p4 = m_spEndpointHost.p;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p4 + 120)))((nint)p4, EEndpointHostProperty.eEndpointHostPropertyEndpointId, &ptr2);
							if (num >= 0 && ptr2 != null)
							{
								m_strEndpointId = new string((char*)ptr2);
							}
							Module.SysFreeString(ptr2);
							if (num >= 0)
							{
								CComPtrNtv_003CIDeviceAssetProvider_003E cComPtrNtv_003CIDeviceAssetProvider_003E;
								*(long*)(&cComPtrNtv_003CIDeviceAssetProvider_003E) = 0L;
								try
								{
									if (Module.GetEndpointHostInterfaceProperty_003Cstruct_0020IDeviceAssetProvider_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyDeviceAssetProvider, (IDeviceAssetProvider**)(&cComPtrNtv_003CIDeviceAssetProvider_003E)) >= 0)
									{
										CreateDeviceAssetSet((IDeviceAssetProvider*)(*(ulong*)(&cComPtrNtv_003CIDeviceAssetProvider_003E)));
									}
								}
								catch
								{
									//try-fault
									Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDeviceAssetProvider_003E*, void>)(&Module.CComPtrNtv_003CIDeviceAssetProvider_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDeviceAssetProvider_003E);
									throw;
								}
								Module.CComPtrNtv_003CIDeviceAssetProvider_003E_002ERelease(&cComPtrNtv_003CIDeviceAssetProvider_003E);
							}
						}
					}
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
					{
						Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 17, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CDeviceMediator_003E*, void>)(&Module.CComPtrNtv_003CDeviceMediator_003E_002E_007Bdtor_007D), &cComPtrNtv_003CDeviceMediator_003E);
					throw;
				}
				Module.CComPtrNtv_003CDeviceMediator_003E_002ERelease(&cComPtrNtv_003CDeviceMediator_003E);
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

		internal unsafe int CompleteInitialization()
		{
			//IL_0090: Expected I, but got I8
			//IL_01a1: Expected I, but got I8
			//IL_01c1: Expected I, but got I8
			//IL_0318: Expected I, but got I8
			//IL_037f: Expected I, but got I8
			//IL_03b1: Expected I, but got I8
			ManagedLock managedLock = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
			bool flag = false;
			bool flag2 = false;
			ManagedLock managedLock2 = new ManagedLock(m_Lock);
			int num;
			try
			{
				managedLock = managedLock2;
				num = GetCapability(EEndpointCapability.eEndpointCapabilityFirmwareUpdate, ref m_fFirmwareUpdateSupported);
				bool flag3 = false;
				if (num >= 0)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, bool*, int>)(*(ulong*)(*(long*)p + 152)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyIsAvailable, &flag3);
					if (flag3)
					{
						if (num < 0)
						{
							goto IL_0404;
						}
						num = IsFirmwareProcessInProgress(&flag, &flag2);
					}
					if (num >= 0)
					{
						if (flag2)
						{
							num = m_firmwareUpdater.Restorer.ContinueFirmwareProcess(OnFirmwareProcessComplete);
							if (num < 0)
							{
								if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 2u)
								{
									Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
								}
								goto IL_03fc;
							}
						}
						else
						{
							if (!flag)
							{
								ESyncRelationship eSyncRelationship = ESyncRelationship.srNone;
								EEndpointCompatibilityStatus eEndpointCompatibilityStatus = 0;
								if (!flag3)
								{
									goto IL_0341;
								}
								ushort* ptr = null;
								IEndpointHost* p2 = m_spEndpointHost.p;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, ushort**, int>)(*(ulong*)(*(long*)p2 + 120)))((nint)p2, EEndpointHostProperty.eEndpointHostPropertyCanonicalName, &ptr);
								if (num >= 0 && ptr != null)
								{
									m_strCanonicalName = new string((char*)ptr);
								}
								Module.SysFreeString(ptr);
								if (num >= 0)
								{
									Module.GetEndpointHostEnumProperty_003Cenum_0020ESyncRelationship_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertySyncRelationship, &eSyncRelationship);
									if (IsConnectedWirelessly && eSyncRelationship != ESyncRelationship.srSyncWithThisMachine)
									{
										if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
										{
											Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 21, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), (uint)eSyncRelationship);
										}
										num = -1072885172;
									}
									if (num >= 0)
									{
										num = Module.GetEnumProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cenum_0020EEndpointCompatibilityStatus_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyCompatibilityStatus, &eEndpointCompatibilityStatus);
										if (num >= 0)
										{
											if ((EEndpointCompatibilityStatus)3 == eEndpointCompatibilityStatus)
											{
												m_fClientUpdateRequired = true;
											}
											else if ((EEndpointCompatibilityStatus)2 == eEndpointCompatibilityStatus)
											{
												m_fFirmwareUpdateRequired = true;
											}
											if ((m_fClientUpdateRequired || m_fFirmwareUpdateRequired) && IsConnectedWirelessly)
											{
												if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
												{
													Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 22, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
												}
												num = -1072885173;
											}
											if (num >= 0)
											{
												CComPtrNtv_003CISyncEngine_003E cComPtrNtv_003CISyncEngine_003E;
												*(long*)(&cComPtrNtv_003CISyncEngine_003E) = 0L;
												try
												{
													m_spSyncEngine.Release();
													num = Module.GetInterfaceProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cstruct_0020ISyncEngine_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertySyncEngine, (ISyncEngine**)(&cComPtrNtv_003CISyncEngine_003E));
													if (num >= 0)
													{
														ISyncEngine* p3 = (ISyncEngine*)(*(ulong*)(&cComPtrNtv_003CISyncEngine_003E));
														*(long*)(&cComPtrNtv_003CISyncEngine_003E) = 0L;
														m_spSyncEngine.Attach(p3);
													}
												}
												catch
												{
													//try-fault
													Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CISyncEngine_003E*, void>)(&Module.CComPtrNtv_003CISyncEngine_003E_002E_007Bdtor_007D), &cComPtrNtv_003CISyncEngine_003E);
													throw;
												}
												Module.CComPtrNtv_003CISyncEngine_003E_002ERelease(&cComPtrNtv_003CISyncEngine_003E);
												goto IL_0341;
											}
										}
									}
								}
								goto IL_0404;
							}
							num = m_firmwareUpdater.ContinueFirmwareProcess(OnFirmwareProcessComplete);
							if (num < 0)
							{
								if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 2u)
								{
									Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 20, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
								}
								goto IL_03fc;
							}
						}
						goto IL_0400;
					}
				}
				goto IL_0404;
				IL_0406:
				int fInitializationCompleted;
				m_fInitializationCompleted = (byte)fInitializationCompleted != 0;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_dd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 23, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num, m_iDeviceID);
				}
				goto end_IL_0052;
				IL_0341:
				if (num >= 0)
				{
					CComPtrNtv_003CIGasGauge_003E cComPtrNtv_003CIGasGauge_003E;
					*(long*)(&cComPtrNtv_003CIGasGauge_003E) = 0L;
					try
					{
						CComPtrNtv_003CIGasGauge_003E cComPtrNtv_003CIGasGauge_003E2;
						*(long*)(&cComPtrNtv_003CIGasGauge_003E2) = 0L;
						try
						{
							num = Module.GetEndpointHostInterfaceProperty_003Cstruct_0020IGasGauge_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyPredictedGasGauge, (IGasGauge**)(&cComPtrNtv_003CIGasGauge_003E));
							if (num >= 0)
							{
								if (null == m_predictedGasGauge)
								{
									m_predictedGasGauge = new GasGauge((IGasGauge*)(*(ulong*)(&cComPtrNtv_003CIGasGauge_003E)));
								}
								num = Module.GetEndpointHostInterfaceProperty_003Cstruct_0020IGasGauge_003E(m_spEndpointHost.p, EEndpointHostProperty.eEndpointHostPropertyActualGasGauge, (IGasGauge**)(&cComPtrNtv_003CIGasGauge_003E2));
								if (num >= 0)
								{
									if (null == m_actualGasGauge)
									{
										m_actualGasGauge = new GasGauge((IGasGauge*)(*(ulong*)(&cComPtrNtv_003CIGasGauge_003E2)));
									}
									if (m_predictedGasGauge == null || m_actualGasGauge == null)
									{
										num = -2147024882;
									}
								}
							}
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIGasGauge_003E*, void>)(&Module.CComPtrNtv_003CIGasGauge_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIGasGauge_003E2);
							throw;
						}
						Module.CComPtrNtv_003CIGasGauge_003E_002ERelease(&cComPtrNtv_003CIGasGauge_003E2);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIGasGauge_003E*, void>)(&Module.CComPtrNtv_003CIGasGauge_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIGasGauge_003E);
						throw;
					}
					Module.CComPtrNtv_003CIGasGauge_003E_002ERelease(&cComPtrNtv_003CIGasGauge_003E);
					goto IL_03fc;
				}
				goto IL_0404;
				IL_0400:
				fInitializationCompleted = 1;
				goto IL_0406;
				IL_03fc:
				if (num >= 0)
				{
					goto IL_0400;
				}
				goto IL_0404;
				IL_0404:
				fInitializationCompleted = 0;
				goto IL_0406;
				end_IL_0052:;
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

		private unsafe void OnFirmwareProcessComplete(int hr)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 27, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), hr);
			}
			Initialize(m_spEndpointHost.p);
			CompleteInitialization();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 28, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
		}

		private unsafe int IsFirmwareProcessInProgress(bool* pfFirmwareUpdateInProgress, bool* pfFirmwareRestoreInProgress)
		{
			if (pfFirmwareUpdateInProgress == null)
			{
				Module._ZuneShipAssert(1001u, 647u);
				return -2147467261;
			}
			if (pfFirmwareRestoreInProgress == null)
			{
				Module._ZuneShipAssert(1001u, 648u);
				return -2147467261;
			}
			*pfFirmwareUpdateInProgress = false;
			*pfFirmwareRestoreInProgress = false;
			int num = 0;
			try
			{
				Monitor.Enter(m_Lock);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 24, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
					int num2 = 1;
				}
				else
				{
					int num2 = 0;
				}
				FirmwareUpdater firmwareUpdater = m_firmwareUpdater;
				if (firmwareUpdater == null)
				{
					IEndpointHost* p = m_spEndpointHost.p;
					m_firmwareUpdater = new FirmwareUpdater(p, m_fFirmwareUpdateSupported);
				}
				else
				{
					num = firmwareUpdater.Reset(deviceRebooting: false);
				}
				if (m_fFirmwareUpdateSupported && num >= 0)
				{
					int num3 = ((*pfFirmwareUpdateInProgress = ((m_firmwareUpdater.IsUpdateInProgress() != 0) ? true : false)) ? 1 : 0);
					byte b = ((*pfFirmwareRestoreInProgress = m_firmwareUpdater.Restorer.IsRestoreInProgress()) ? ((byte)1) : ((byte)0));
					if (*pfFirmwareUpdateInProgress || b != 0)
					{
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
						{
							Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 25, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), m_iDeviceID);
							int num4 = 1;
							return num;
						}
						int num5 = 0;
						return num;
					}
					return num;
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 2u)
				{
					Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num);
					int num6 = 1;
					return num;
				}
				int num7 = 0;
				return num;
			}
			finally
			{
				Monitor.Exit(m_Lock);
			}
		}

		private unsafe int CreateDeviceAssetSet(IDeviceAssetProvider* pDeviceAssetProvider)
		{
			//IL_0041: Expected I, but got I8
			//IL_0047: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_00cb: Expected I, but got I8
			//IL_00f7: Expected I, but got I8
			//IL_00f7: Expected I, but got I8
			//IL_0140: Expected I, but got I8
			//IL_016c: Expected I, but got I8
			//IL_016c: Expected I, but got I8
			//IL_01d7: Expected I, but got I8
			//IL_0210: Expected I, but got I8
			//IL_021e: Expected I, but got I8
			//IL_0246: Expected I, but got I8
			//IL_0254: Expected I, but got I8
			//IL_0283: Expected I, but got I8
			//IL_02af: Expected I, but got I8
			//IL_02d1: Expected I, but got I8
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 43, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids));
			}
			uint* ptr = null;
			int num = 0;
			ushort** ptr2 = null;
			int num2 = 0;
			int num3 = 0;
			if (-2147024774 != ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 40)))((nint)pDeviceAssetProvider, null, 0, &num2))
			{
				Module._ZuneShipAssert(1004u, 3551u);
			}
			ulong num4 = (ulong)num2;
			ushort** ptr3 = (ushort**)Module.new_005B_005D((num4 > 2305843009213693951L) ? ulong.MaxValue : (num4 * 8));
			int num5;
			if (ptr3 == null)
			{
				num5 = -2147024882;
				Module._ZuneShipAssert(1012u, 3554u);
			}
			else
			{
				ushort** intPtr = ptr3;
				int num6 = num2;
				num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 40)))((nint)pDeviceAssetProvider, intPtr, num6, &num2);
				if (num5 < 0)
				{
					Module._ZuneShipAssertForHr(num5, 3557u);
				}
				else
				{
					if (-2147024774 != ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 48)))((nint)pDeviceAssetProvider, null, 0, &num3))
					{
						Module._ZuneShipAssert(1004u, 3563u);
					}
					ulong num7 = (ulong)num3;
					ptr2 = (ushort**)Module.new_005B_005D((num7 > 2305843009213693951L) ? ulong.MaxValue : (num7 * 8));
					ushort** intPtr2 = ptr2;
					int num8 = num3;
					num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 48)))((nint)pDeviceAssetProvider, intPtr2, num8, &num3);
					if (num5 < 0)
					{
						Module._ZuneShipAssertForHr(num5, 3569u);
					}
					else
					{
						if (-2147024774 != ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 56)))((nint)pDeviceAssetProvider, null, 0, &num))
						{
							Module._ZuneShipAssert(1004u, 3575u);
						}
						ulong num9 = (ulong)num;
						ptr = (uint*)Module.new_005B_005D((num9 > 4611686018427387903L) ? ulong.MaxValue : (num9 * 4));
						if (ptr == null)
						{
							num5 = -2147024882;
							Module._ZuneShipAssert(1012u, 3578u);
						}
						else
						{
							uint* intPtr3 = ptr;
							int num10 = num;
							num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int, int*, int>)(*(ulong*)(*(long*)pDeviceAssetProvider + 56)))((nint)pDeviceAssetProvider, intPtr3, num10, &num);
							if (num5 < 0)
							{
								Module._ZuneShipAssertForHr(num5, 3581u);
							}
							else
							{
								string[] array = new string[num2];
								int num11 = 0;
								if (0 < num2)
								{
									ushort** ptr4 = ptr3;
									do
									{
										if (*(long*)ptr4 != 0L)
										{
											array[num11] = new string((char*)(*(ulong*)ptr4));
										}
										num11++;
										ptr4 = (ushort**)((ulong)(nint)ptr4 + 8uL);
									}
									while (num11 < num2);
								}
								string[] array2 = new string[num3];
								int num12 = 0;
								if (0 < num3)
								{
									ushort** ptr5 = ptr2;
									do
									{
										if (*(long*)ptr5 != 0L)
										{
											array2[num12] = new string((char*)(*(ulong*)ptr5));
										}
										num12++;
										ptr5 = (ushort**)((ulong)(nint)ptr5 + 8uL);
									}
									while (num12 < num3);
								}
								uint[] array3 = new uint[num];
								int num13 = 0;
								if (0 < num)
								{
									uint* ptr6 = ptr;
									do
									{
										array3[num13] = *ptr6;
										num13++;
										ptr6 = (uint*)((ulong)(nint)ptr6 + 4uL);
									}
									while (num13 < num);
								}
								m_DeviceAssetSet = new DeviceAssetSet(array, array2, array3);
							}
						}
					}
				}
				int num14 = 0;
				if (0 < num2)
				{
					do
					{
						Module.WString_002ESysFreeString((ushort**)(num14 * 8L + (nint)ptr3));
						num14++;
					}
					while (num14 < num2);
				}
				if (ptr2 != null)
				{
					int num15 = 0;
					if (0 < num3)
					{
						do
						{
							Module.WString_002ESysFreeString((ushort**)(num15 * 8L + (nint)ptr2));
							num15++;
						}
						while (num15 < num3);
					}
				}
			}
			Module.SafeDeleteArray_003Cunsigned_0020short_0020_002A_003E(&ptr3);
			Module.SafeDeleteArray_003Cunsigned_0020short_0020_002A_003E(&ptr2);
			Module.SafeDeleteArray_003Cunsigned_0020long_003E(&ptr);
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 2u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 44, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xc0985b64_002EWPP_DeviceAPI_cpp_Traceguids), num5);
			}
			return num5;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EDevice();
				}
				finally
				{
					try
					{
						((IDisposable)m_spDeviceMediator).Dispose();
					}
					finally
					{
						try
						{
							((IDisposable)m_spWlanProvider).Dispose();
						}
						finally
						{
							try
							{
								((IDisposable)m_spSyncEngine).Dispose();
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
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
