using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class MetadataMgrNotifications : IDisposable
	{
		private bool m_disposed = false;

		private bool m_fAdvised = false;

		private ulong m_AdviseCookie;

		internal OnFileAddedHandler m_FileAddedHandler;

		internal OnFileErrorHandler m_FileErrorHandler;

		internal OnFileRemovedHandler m_FileRemovedHandler;

		internal OnBeginScanDirectoryHandler m_BeginScanDirectoryHandler;

		internal OnEndScanDirectoryHandler m_EndScanDirectoryHandler;

		internal OnBeginFileChangeHandler m_BeginFileChangeHandler;

		internal OnEndFileChangeHandler m_EndFileChangeHandler;

		internal OnScanCompletedHandler m_ScanCompletedHandler;

		internal OnMetadataUpdateHandler m_MetadataUpdateHandler;

		internal OnBeginMetadataLifecycleHandler m_BeginMetadataLifecycleHandler;

		internal OnEndMetadataLifecycleHandler m_EndMetadataLifecycleHandler;

		internal OnBeginMigrateHandler m_BeginMigrateHandler;

		internal OnEndMigrateHandler m_EndMigrateHandler;

		internal OnFileDeleteFailed m_FileDeleteFailedHandler;

		internal OnFingerprintingFile m_FingerprintingFileHandler;

		[SpecialName]
		public virtual event OnFingerprintingFile FingerprintingFile
		{
			add
			{
				m_FingerprintingFileHandler = (OnFingerprintingFile)Delegate.Combine(m_FingerprintingFileHandler, value);
			}
			remove
			{
				m_FingerprintingFileHandler = (OnFingerprintingFile)Delegate.Remove(m_FingerprintingFileHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnFileDeleteFailed FileDeleteFailed
		{
			add
			{
				m_FileDeleteFailedHandler = (OnFileDeleteFailed)Delegate.Combine(m_FileDeleteFailedHandler, value);
			}
			remove
			{
				m_FileDeleteFailedHandler = (OnFileDeleteFailed)Delegate.Remove(m_FileDeleteFailedHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnEndMigrateHandler EndMigrate
		{
			add
			{
				m_EndMigrateHandler = (OnEndMigrateHandler)Delegate.Combine(m_EndMigrateHandler, value);
			}
			remove
			{
				m_EndMigrateHandler = (OnEndMigrateHandler)Delegate.Remove(m_EndMigrateHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnBeginMigrateHandler BeginMigrate
		{
			add
			{
				m_BeginMigrateHandler = (OnBeginMigrateHandler)Delegate.Combine(m_BeginMigrateHandler, value);
			}
			remove
			{
				m_BeginMigrateHandler = (OnBeginMigrateHandler)Delegate.Remove(m_BeginMigrateHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnEndMetadataLifecycleHandler EndMetadataLifecycle
		{
			add
			{
				m_EndMetadataLifecycleHandler = (OnEndMetadataLifecycleHandler)Delegate.Combine(m_EndMetadataLifecycleHandler, value);
			}
			remove
			{
				m_EndMetadataLifecycleHandler = (OnEndMetadataLifecycleHandler)Delegate.Remove(m_EndMetadataLifecycleHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnBeginMetadataLifecycleHandler BeginMetadataLifecycle
		{
			add
			{
				m_BeginMetadataLifecycleHandler = (OnBeginMetadataLifecycleHandler)Delegate.Combine(m_BeginMetadataLifecycleHandler, value);
			}
			remove
			{
				m_BeginMetadataLifecycleHandler = (OnBeginMetadataLifecycleHandler)Delegate.Remove(m_BeginMetadataLifecycleHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnMetadataUpdateHandler MetadataUpdate
		{
			add
			{
				m_MetadataUpdateHandler = (OnMetadataUpdateHandler)Delegate.Combine(m_MetadataUpdateHandler, value);
			}
			remove
			{
				m_MetadataUpdateHandler = (OnMetadataUpdateHandler)Delegate.Remove(m_MetadataUpdateHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnScanCompletedHandler ScanCompleted
		{
			add
			{
				m_ScanCompletedHandler = (OnScanCompletedHandler)Delegate.Combine(m_ScanCompletedHandler, value);
			}
			remove
			{
				m_ScanCompletedHandler = (OnScanCompletedHandler)Delegate.Remove(m_ScanCompletedHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnEndFileChangeHandler EndFileChange
		{
			add
			{
				m_EndFileChangeHandler = (OnEndFileChangeHandler)Delegate.Combine(m_EndFileChangeHandler, value);
			}
			remove
			{
				m_EndFileChangeHandler = (OnEndFileChangeHandler)Delegate.Remove(m_EndFileChangeHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnBeginFileChangeHandler BeginFileChange
		{
			add
			{
				m_BeginFileChangeHandler = (OnBeginFileChangeHandler)Delegate.Combine(m_BeginFileChangeHandler, value);
			}
			remove
			{
				m_BeginFileChangeHandler = (OnBeginFileChangeHandler)Delegate.Remove(m_BeginFileChangeHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnEndScanDirectoryHandler EndScanDirectory
		{
			add
			{
				m_EndScanDirectoryHandler = (OnEndScanDirectoryHandler)Delegate.Combine(m_EndScanDirectoryHandler, value);
			}
			remove
			{
				m_EndScanDirectoryHandler = (OnEndScanDirectoryHandler)Delegate.Remove(m_EndScanDirectoryHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnBeginScanDirectoryHandler BeginScanDirectory
		{
			add
			{
				m_BeginScanDirectoryHandler = (OnBeginScanDirectoryHandler)Delegate.Combine(m_BeginScanDirectoryHandler, value);
			}
			remove
			{
				m_BeginScanDirectoryHandler = (OnBeginScanDirectoryHandler)Delegate.Remove(m_BeginScanDirectoryHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnFileRemovedHandler FileRemoved
		{
			add
			{
				m_FileRemovedHandler = (OnFileRemovedHandler)Delegate.Combine(m_FileRemovedHandler, value);
			}
			remove
			{
				m_FileRemovedHandler = (OnFileRemovedHandler)Delegate.Remove(m_FileRemovedHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnFileErrorHandler FileError
		{
			add
			{
				m_FileErrorHandler = (OnFileErrorHandler)Delegate.Combine(m_FileErrorHandler, value);
			}
			remove
			{
				m_FileErrorHandler = (OnFileErrorHandler)Delegate.Remove(m_FileErrorHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnFileAddedHandler FileAdded
		{
			add
			{
				m_FileAddedHandler = (OnFileAddedHandler)Delegate.Combine(m_FileAddedHandler, value);
			}
			remove
			{
				m_FileAddedHandler = (OnFileAddedHandler)Delegate.Remove(m_FileAddedHandler, value);
			}
		}

		public unsafe MetadataMgrNotifications()
		{
			//IL_002d: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			NativeMetadataNotifications* ptr = (NativeMetadataNotifications*)Module.@new(24uL);
			NativeMetadataNotifications* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ENativeMetadataNotifications_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			IMetadataChangeNotify* ptr3;
			if (ptr2 == null || ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr2)))((nint)ptr2, (_GUID*)Unsafe.AsPointer(ref Module._GUID_d67cdf64_5ea9_44ea_bf5c_29a422f4c23f), (void**)(&ptr3)) < 0)
			{
				return;
			}
			fixed (ulong* ptr4 = &m_AdviseCookie)
			{
				try
				{
					if (Module.MetadataChangeAdvise(ptr3, ptr4) >= 0)
					{
						m_fAdvised = true;
					}
					IMetadataChangeNotify* intPtr = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
				catch
				{
					//try-fault
					m_AdviseCookie = null;
					throw;
				}
			}
		}

		private void _007EMetadataMgrNotifications()
		{
			_0021MetadataMgrNotifications();
		}

		private void _0021MetadataMgrNotifications()
		{
			if (!m_disposed)
			{
				if (m_fAdvised)
				{
					Module.MetadataChangeUnAdvise(m_AdviseCookie);
					m_fAdvised = false;
				}
				m_disposed = true;
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021MetadataMgrNotifications();
				return;
			}
			try
			{
				_0021MetadataMgrNotifications();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~MetadataMgrNotifications()
		{
			Dispose(false);
		}
	}
}
