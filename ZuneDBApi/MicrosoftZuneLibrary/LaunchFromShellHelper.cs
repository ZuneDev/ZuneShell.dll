using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class LaunchFromShellHelper
	{
		private unsafe delegate void FileFoundCallback(ushort* fileName, EMediaTypes mediaType);

		private string _003Cbacking_store_003ETaskName;

		private DeferredInvokeHandler _completeHandler;

		private string _startParam;

		private string _eventName;

		private bool _startParamIsDataObject;

		private bool _cancelled;

		private List<FileEntry> _files;

		public string TaskName
		{
			get
			{
				return _003Cbacking_store_003ETaskName;
			}
			set
			{
				_003Cbacking_store_003ETaskName = value;
			}
		}

		public List<FileEntry> Files => _files;

		public LaunchFromShellHelper(string taskName, string marshalledDataObject, string eventName)
		{
			Init(taskName, marshalledDataObject, eventName, startParamIsDataObject: true);
		}

		public LaunchFromShellHelper(string taskName, string initialUrl)
		{
			Init(taskName, initialUrl, null, startParamIsDataObject: false);
		}

		public void Go(DeferredInvokeHandler completeHandler)
		{
			_completeHandler = completeHandler;
			Thread thread = new Thread(ThreadProc);
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start();
		}

		public void Cancel()
		{
			_cancelled = true;
		}

		private void Init(string taskName, string startParam, string eventName, [MarshalAs(UnmanagedType.U1)] bool startParamIsDataObject)
		{
			_003Cbacking_store_003ETaskName = taskName.ToLowerInvariant();
			_startParam = startParam;
			_eventName = eventName;
			_startParamIsDataObject = startParamIsDataObject;
			_files = new List<FileEntry>();
		}

		private unsafe void ThreadProc()
		{
			//IL_0005: Expected I, but got I8
			//IL_0053: Expected I, but got I8
			//IL_00be: Expected I, but got I8
			//IL_00d0: Expected I, but got I8
			EventWaitHandle eventWaitHandle = null;
			IDataObjectEnumerator* ptr = null;
			fixed (char* _startParamPtr = _startParam.ToCharArray())
			{
				ushort* ptr2 = (ushort*)_startParamPtr;
				FileFoundCallback fileFoundCallback = FileFound;
				delegate* unmanaged[Cdecl, Cdecl]<ushort*, EMediaTypes, void> delegate_002A = (delegate* unmanaged[Cdecl, Cdecl]<ushort*, EMediaTypes, void>)Marshal.GetFunctionPointerForDelegate(fileFoundCallback).ToPointer();
				int num = Module.ZuneLibraryExports_002ECreateDataObjectEnum(&ptr);
				try
				{
					if (num >= 0)
					{
						long num2 = *(long*)ptr;
						IDataObjectEnumerator* intPtr = ptr;
						_003F val = ptr2;
						bool startParamIsDataObject = _startParamIsDataObject;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int, delegate* unmanaged[Cdecl, Cdecl]<ushort*, EMediaTypes, void>, int>)(*(ulong*)num2))((nint)intPtr, (ushort*)(nint)val, startParamIsDataObject ? 1 : 0, delegate_002A);
					}
					if (_eventName != (string)null)
					{
						try
						{
							eventWaitHandle = EventWaitHandle.OpenExisting(_eventName);
							eventWaitHandle.Set();
						}
						catch (ArgumentException)
						{
						}
						catch (WaitHandleCannotBeOpenedException)
						{
						}
						catch (IOException)
						{
						}
						catch (UnauthorizedAccessException)
						{
						}
						finally
						{
							((IDisposable)eventWaitHandle)?.Dispose();
						}
					}
					if (num >= 0)
					{
						bool flag = true;
						while (!_cancelled && num >= 0 && flag)
						{
							IDataObjectEnumerator* intPtr2 = ptr;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, bool*, int>)(*(ulong*)(*(long*)ptr + 8)))((nint)intPtr2, &flag);
						}
					}
				}
				finally
				{
					if (ptr != null)
					{
						Module.ZuneLibraryExports_002EDestroyDataObjectEnum(ptr);
						ptr = null;
					}
				}
				GC.KeepAlive(fileFoundCallback);
				if (!_cancelled)
				{
					Application.DeferredInvoke(CompletedOnAppThread, null);
				}
			}
		}

		private void CompletedOnAppThread(object unused)
		{
			if (!_cancelled)
			{
				_completeHandler(this);
			}
		}

		private unsafe void FileFound(ushort* fileName, EMediaTypes mediaType)
		{
			_files.Add(new FileEntry(fileName, mediaType));
		}
	}
}
