using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util
{
	[StructLayout(LayoutKind.Sequential, Size = 112)]
	[NativeCppClass]
	public struct DownloadManagerProxy
	{
		private long _003Calignment_0020member_003E;

		public unsafe static void AddDelegate(DownloadManagerProxy* obj, DownloadManagerUpdateHandler handler)
        {
			throw new NotImplementedException();
        }

		internal unsafe static uint AddRef(DownloadManagerProxy* P_0)
		{
			return (uint)Interlocked.Increment(ref *(int*)((long)(nint)P_0 + 28L));
		}

		internal unsafe static void DownloadBegan(DownloadManagerProxy* P_0, IDownloadTask* pDLTask)
		{
			//IL_0013: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			if (pDLTask == null)
			{
				return;
			}
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)pDLTask + 144)))((nint)pDLTask) == (EDownloadType)0)
			{
				CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
				EnterCriticalSection(ref *ptr);
				if (*(int*)((ulong)(nint)P_0 + 8uL) <= 0)
				{
					ResetCounts(P_0);
				}
				(*(int*)((ulong)(nint)P_0 + 8uL))++;
				UpdatePercentage(P_0, 0f);
				int nQueuePosition = GetQueuePosition(P_0, pDLTask);
				LeaveCriticalSection(ref *ptr);
				NotifyUpdate(P_0, EDownloadManagerUpdateType.TaskAdded, pDLTask, nQueuePosition);
			}
		}

		internal unsafe static void DownloadComplete(DownloadManagerProxy* P_0, IDownloadTask* pDLTask)
		{
			//IL_0013: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			if (pDLTask != null)
			{
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)pDLTask + 144)))((nint)pDLTask) == (EDownloadType)0)
				{
					CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
					EnterCriticalSection(ref *ptr);
					DownloadFinished(P_0, pDLTask, 0);
					UpdatePercentage(P_0, 0f);
					LeaveCriticalSection(ref *ptr);
					NotifyUpdate(P_0, EDownloadManagerUpdateType.TaskCompleted, pDLTask, -1);
				}
			}
		}

		internal unsafe static void DownloadFailed(DownloadManagerProxy* P_0, IDownloadTask* pDLTask, int hrFailure, float fltRemainingPercent)
		{
			//IL_0013: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			if (pDLTask == null)
			{
				return;
			}
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)pDLTask + 144)))((nint)pDLTask) == (EDownloadType)0)
			{
				CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
				EnterCriticalSection(ref *ptr);
				DownloadFinished(P_0, pDLTask, hrFailure);
				UpdatePercentage(P_0, fltRemainingPercent);
				LeaveCriticalSection(ref *ptr);
				if (-2147467260 == hrFailure)
				{
					NotifyUpdate(P_0, EDownloadManagerUpdateType.TaskCancelled, pDLTask, -1);
				}
				else
				{
					NotifyUpdate(P_0, EDownloadManagerUpdateType.TaskFailed, pDLTask, -1);
				}
			}
		}

		internal unsafe static void DownloadFinished(DownloadManagerProxy* P_0, IDownloadTask* pDLTask, int hrFailure)
		{
			//IL_0006: Expected I, but got I8
			CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
			EnterCriticalSection(ref *ptr);
			if (pDLTask != null)
			{
				if (hrFailure < 0)
				{
					if (-2147467260 == hrFailure)
					{
						(*(int*)((ulong)(nint)P_0 + 16uL))++;
					}
					else
					{
						(*(int*)((ulong)(nint)P_0 + 20uL))++;
					}
				}
				else
				{
					(*(int*)((ulong)(nint)P_0 + 12uL))++;
				}
				*(int*)((ulong)(nint)P_0 + 8uL) += -1;
			}
			LeaveCriticalSection(ref *ptr);
		}

		internal unsafe static int GetCredential(DownloadManagerProxy* P_0, IDownloadTask* pDownloadTask, ushort* pwszTargetUrl, EAuthenticationScheme eAuthenticationScheme, int fIsAuthSchemeSafe, ushort* pwszRealm, int iRetryCount, ICredential** ppCredential)
		{
			*(long*)ppCredential = 0L;
			return 1;
		}

		internal unsafe static int GetQueuePosition(DownloadManagerProxy* P_0, IDownloadTask* pDLTask)
		{
			//IL_0016: Expected I, but got I8
			//IL_0016: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_006f: Expected I, but got I8
			//IL_006f: Expected I, but got I8
			//IL_006f: Expected I, but got I8
			int num = -1;
			long num2 = *(long*)((ulong)(nint)P_0 + 56uL);
			long num3 = num2;
			int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)num3 + 88)))((nint)num3);
			int num5 = 0;
			if (0 < num4)
			{
				do
				{
					CComPtrNtv<IDownloadTask> cComPtrNtvIDownloadTask = new();
					try
					{
						num2 = *(long*)((ulong)(nint)P_0 + 56uL);
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IDownloadTask**, int>)(*(ulong*)(*(long*)num2 + 104)))((nint)num2, num5, (IDownloadTask**)(cComPtrNtvIDownloadTask.p)) >= 0)
						{
							long num6 = *(long*)(cComPtrNtvIDownloadTask.p);
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtvIDownloadTask.p)) + 144)))((nint)num6) == (EDownloadType)0)
							{
								long num7 = *(long*)(cComPtrNtvIDownloadTask.p);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, EDownloadTaskState>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtvIDownloadTask.p)) + 200)))((nint)num7, null) != EDownloadTaskState.DLTaskComplete)
								{
									num++;
									if (*(long*)(cComPtrNtvIDownloadTask.p) == (nint)pDLTask)
									{
										goto IL_00a1;
									}
								}
							}
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IDownloadTask*, void>)(&CComPtrNtv<IDownloadTask>.dtor), cComPtrNtvIDownloadTask.p);
						throw;
					}
					cComPtrNtvIDownloadTask.Dispose();
					num5++;
					continue;
				IL_00a1:
					cComPtrNtvIDownloadTask.Dispose();
					break;
				}
				while (num5 < num4);
			}
			return num;
		}

		internal unsafe static int Initialize(DownloadManagerProxy* P_0)
		{
			//IL_0006: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			//IL_002e: Expected I, but got I8
			CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 56uL);
			int num;
			if (*(long*)ptr == 0L)
			{
				num = Module.GetSingleton(Module.GUID_DownloadManagerProxy, (void**)ptr);
				if (num < 0)
				{
					goto done;
				}
			}
			long num2 = *(long*)ptr;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IDownloadTaskProgress*, int>)(*(ulong*)(*(long*)num2 + 152)))((nint)num2, (IDownloadTaskProgress*)P_0);
			goto done;
		done:
			return num;
		}

		internal unsafe static void NotifyUpdate(DownloadManagerProxy* P_0, EDownloadManagerUpdateType type, IDownloadTask* task, int nQueuePosition)
		{
			//IL_0026: Expected I, but got I8
			DownloadTask task2 = null;
			if (task != null)
			{
				task2 = new DownloadTask(task);
			}
			DownloadManagerUpdateArguments args = new(type, task2, nQueuePosition);
			int num = 0;
			if (0 < *(int*)((ulong)(nint)P_0 + 48uL))
			{
				CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 32uL);
				do
				{
					//gcroot<Microsoft::Zune::Util::DownloadManagerUpdateHandler^>..PE$AAVDownloadManagerUpdateHandler @Util@Zune @Microsoft@@(DynamicArray < gcroot < Microsoft::Zune::Util::DownloadManagerUpdateHandler ^> >.[]((DynamicArray < gcroot < Microsoft::Zune::Util::DownloadManagerUpdateHandler ^> > *)ptr, num))(args);
					num++;
				}
				while (num < *(int*)((ulong)(nint)P_0 + 48uL));
			}
		}

		internal unsafe static void PauseQueue(DownloadManagerProxy* P_0)
		{
			//IL_0015: Expected I, but got I8
			//IL_0015: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			long num = *(long*)((ulong)(nint)P_0 + 56uL);
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)num + 128)))((nint)num) >= 0)
			{
				NotifyUpdate(P_0, EDownloadManagerUpdateType.QueuePaused, null, -1);
			}
		}

		internal unsafe static void Progress(DownloadManagerProxy* P_0, IDownloadTask* pDLTask, float percent, float percentDelta)
		{
			//IL_0013: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			if (pDLTask != null)
			{
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)pDLTask + 144)))((nint)pDLTask) == (EDownloadType)0)
				{
					CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
					EnterCriticalSection(ref *ptr);
					UpdatePercentage(P_0, percentDelta);
					LeaveCriticalSection(ref *ptr);
					NotifyUpdate(P_0, EDownloadManagerUpdateType.TaskProgressChanged, pDLTask, -1);
				}
			}
		}

		internal unsafe static int QueryInterface(DownloadManagerProxy* P_0, _GUID* iid, void** ppv)
		{
			//IL_0035: Expected I, but got I8
			//IL_0039: Expected I8, but got I
			//IL_0054: Expected I, but got I8
			//IL_0058: Expected I8, but got I
			if (ppv == null)
			{
				Module._ZuneShipAssert(1001u, 939u);
				return -2147467261;
			}
			*(long*)ppv = 0L;
			if (IsEqualGUID(*iid, Module._GUID_00000000_0000_0000_c000_000000000046))
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)P_0 + 8)))((nint)P_0);
				*(long*)ppv = (nint)P_0;
				return 0;
			}
			if (IsEqualGUID(*iid, Module._GUID_60fcb6b3_8562_4ddf_99f8_b93c08ed5e83))
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)P_0 + 8)))((nint)P_0);
				*(long*)ppv = (nint)P_0;
				return 0;
			}
			return -2147467262;
		}

		internal unsafe static void Redirected(DownloadManagerProxy* P_0, IDownloadTask* pDLTask, int iFile, ushort* pwszNewURL)
		{
		}

		internal unsafe static uint Release(DownloadManagerProxy* P_0)
		{
			uint num = (uint)Interlocked.Decrement(ref *(int*)((long)(nint)P_0 + 28L));
			if (0 == num && P_0 != null)
			{
				new CComPtrNtv<DownloadManagerProxy>(P_0).Dispose();
				//__delDtor(P_0, 1u);
			}
			return num;
		}

		internal unsafe static void RemoveCancelledCount(DownloadManagerProxy* P_0)
		{
			int num = *(int*)((ulong)(nint)P_0 + 16uL);
			if (num > 0)
			{
				*(int*)((ulong)(nint)P_0 + 16uL) = num - 1;
				float num2 = *(float*)((ulong)(nint)P_0 + 104uL);
				if (num2 >= 100f)
				{
					*(float*)((ulong)(nint)P_0 + 104uL) = num2 - 100f;
				}
				UpdatePercentage(P_0, 0f);
			}
		}

		public unsafe static void RemoveDelegate(DownloadManagerProxy* obj, DownloadManagerUpdateHandler handler)
        {
			throw new NotImplementedException();
        }

		internal unsafe static void RemoveFailedCount(DownloadManagerProxy* P_0)
		{
			int num = *(int*)((ulong)(nint)P_0 + 20uL);
			if (num > 0)
			{
				*(int*)((ulong)(nint)P_0 + 20uL) = num - 1;
				float num2 = *(float*)((ulong)(nint)P_0 + 104uL);
				if (num2 >= 100f)
				{
					*(float*)((ulong)(nint)P_0 + 104uL) = num2 - 100f;
				}
				UpdatePercentage(P_0, 0f);
			}
		}

		internal unsafe static void ResetCounts(DownloadManagerProxy* P_0)
		{
			*(int*)((ulong)(nint)P_0 + 8uL) = 0;
			*(int*)((ulong)(nint)P_0 + 12uL) = 0;
			*(int*)((ulong)(nint)P_0 + 16uL) = 0;
			*(int*)((ulong)(nint)P_0 + 20uL) = 0;
			*(float*)((ulong)(nint)P_0 + 24uL) = 0f;
			*(float*)((ulong)(nint)P_0 + 104uL) = 0f;
		}

		internal unsafe static void ResumeQueue(DownloadManagerProxy* P_0)
		{
			//IL_0015: Expected I, but got I8
			//IL_0015: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			long num = *(long*)((ulong)(nint)P_0 + 56uL);
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)num + 136)))((nint)num) >= 0)
			{
				NotifyUpdate(P_0, EDownloadManagerUpdateType.QueueResumed, null, -1);
			}
		}

		internal unsafe static void UpdatePercentage(DownloadManagerProxy* P_0, float fltTaskPercentDelta)
		{
			//IL_0006: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			CRITICAL_SECTION* ptr = (CRITICAL_SECTION*)((ulong)(nint)P_0 + 64uL);
			EnterCriticalSection(ref *ptr);
			float num = *(int*)((ulong)(nint)P_0 + 20uL) + *(int*)((ulong)(nint)P_0 + 16uL) + *(int*)((ulong)(nint)P_0 + 12uL) + *(int*)((ulong)(nint)P_0 + 8uL);
			DownloadManagerProxy* ptr2;
			if (num == 0f)
			{
				ptr2 = (DownloadManagerProxy*)((ulong)(nint)P_0 + 24uL);
				*(float*)ptr2 = 100f;
			}
			else
			{
				float num2 = (*(float*)((ulong)(nint)P_0 + 104uL) = (float)((double)fltTaskPercentDelta + (double)(*(float*)((ulong)(nint)P_0 + 104uL))));
				ptr2 = (DownloadManagerProxy*)((ulong)(nint)P_0 + 24uL);
				*(float*)ptr2 = (float)((double)(float)(1.0 / (double)num) * (double)num2);
			}
			if (*(float*)ptr2 > 100f)
			{
				*(float*)ptr2 = 100f;
			}
			LeaveCriticalSection(ref *ptr);
		}
	}
}
