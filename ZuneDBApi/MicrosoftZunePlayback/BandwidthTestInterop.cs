using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

public class BandwidthTestInterop : IDisposable
{
	private BandwidthTestUpdateEventHandler _003Cbacking_store_003EBandwidthTestUpdate;

	private BandwidthTestErrorEventHandler _003Cbacking_store_003EBandwidthTestError;

	private readonly CComPtrMgd_003CIZuneMBRBandwidthTest_003E m_spMBRBandwidthTest;

	private readonly CComPtrMgd_003CMicrosoftZunePlayback_003A_003ACMBRBandwidthTestEventSink_003E m_spMBRBTestSink;

	private bool _ignore;

	private bool _async;

	[SpecialName]
	public virtual event BandwidthTestErrorEventHandler BandwidthTestError
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_003Cbacking_store_003EBandwidthTestError = (BandwidthTestErrorEventHandler)Delegate.Combine(_003Cbacking_store_003EBandwidthTestError, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_003Cbacking_store_003EBandwidthTestError = (BandwidthTestErrorEventHandler)Delegate.Remove(_003Cbacking_store_003EBandwidthTestError, value);
		}
	}

	[SpecialName]
	public virtual event BandwidthTestUpdateEventHandler BandwidthTestUpdate
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_003Cbacking_store_003EBandwidthTestUpdate = (BandwidthTestUpdateEventHandler)Delegate.Combine(_003Cbacking_store_003EBandwidthTestUpdate, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_003Cbacking_store_003EBandwidthTestUpdate = (BandwidthTestUpdateEventHandler)Delegate.Remove(_003Cbacking_store_003EBandwidthTestUpdate, value);
		}
	}

	public BandwidthTestInterop()
	{
		CComPtrMgd_003CIZuneMBRBandwidthTest_003E spMBRBandwidthTest = new CComPtrMgd_003CIZuneMBRBandwidthTest_003E();
		try
		{
			m_spMBRBandwidthTest = spMBRBandwidthTest;
			CComPtrMgd_003CMicrosoftZunePlayback_003A_003ACMBRBandwidthTestEventSink_003E spMBRBTestSink = new CComPtrMgd_003CMicrosoftZunePlayback_003A_003ACMBRBandwidthTestEventSink_003E();
			try
			{
				m_spMBRBTestSink = spMBRBTestSink;
				_ignore = true;
				_async = true;
				base._002Ector();
				return;
			}
			catch
			{
				//try-fault
				((IDisposable)m_spMBRBTestSink).Dispose();
				throw;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)m_spMBRBandwidthTest).Dispose();
			throw;
		}
	}

	private void _007EBandwidthTestInterop()
	{
		Cancel();
	}

	private void _0021BandwidthTestInterop()
	{
		Cancel();
	}

	[SpecialName]
	protected virtual void raise_BandwidthTestUpdate(object value0, BandwidthUpdateArgs value1)
	{
		_003Cbacking_store_003EBandwidthTestUpdate?.Invoke(value0, value1);
	}

	[SpecialName]
	protected virtual void raise_BandwidthTestError(object value0, BandwidthTestErrorArgs value1)
	{
		_003Cbacking_store_003EBandwidthTestError?.Invoke(value0, value1);
	}

	public unsafe void Start(string Uri, int length)
	{
		//IL_000b: Expected I, but got I8
		//IL_0056: Expected I, but got I8
		//IL_00b6: Expected I, but got I8
		_ignore = false;
		IZuneMBRBandwidthTest* p = null;
		int num = global::_003CModule_003E.MBRBandwidthTest_CreateInstance(&p);
		m_spMBRBandwidthTest.Attach(p);
		if (m_spMBRBandwidthTest.p != null && num >= 0)
		{
			CMBRBandwidthTestEventSink* ptr = (CMBRBandwidthTestEventSink*)global::_003CModule_003E.@new(24uL);
			CMBRBandwidthTestEventSink* lp;
			try
			{
				if (ptr != null)
				{
					lp = global::_003CModule_003E.MicrosoftZunePlayback_002ECMBRBandwidthTestEventSink_002E_007Bctor_007D(ptr, this, _async);
				}
				else
				{
					lp = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			m_spMBRBTestSink.op_Assign(lp);
			if (m_spMBRBTestSink.p == null)
			{
				throw new OutOfMemoryException();
			}
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(Uri)))
			{
				IZuneMBRBandwidthTest* p2 = m_spMBRBandwidthTest.p;
				CMBRBandwidthTestEventSink* p3 = m_spMBRBTestSink.p;
				long num2 = *(long*)p2 + 24;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IZuneMBRBandwidthTestEventSink*, uint, int>)(*(ulong*)num2))((nint)p2, ptr2, (IZuneMBRBandwidthTestEventSink*)p3, (uint)length);
				if (num < 0)
				{
					throw new COMException("BandwidthTestInterop call to IZuneMBRBandwidthTest start failed.", num);
				}
				return;
			}
		}
		throw new COMException("BandwidthTestInterop failed to get IZuneMBRBandwidthTest instance.", num);
	}

	public void TestStartSync(string Uri, int length)
	{
		_async = false;
		Start(Uri, length);
	}

	public unsafe void Cancel()
	{
		//IL_0023: Expected I, but got I8
		//IL_003c: Expected I, but got I8
		//IL_005f: Expected I, but got I8
		_ignore = true;
		IZuneMBRBandwidthTest* p = m_spMBRBandwidthTest.p;
		if (p != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 32)))((nint)p);
			m_spMBRBandwidthTest.Release();
			m_spMBRBandwidthTest.op_Assign(null);
		}
		CComPtrMgd_003CMicrosoftZunePlayback_003A_003ACMBRBandwidthTestEventSink_003E spMBRBTestSink = m_spMBRBTestSink;
		if (spMBRBTestSink.p != null)
		{
			spMBRBTestSink.Release();
			m_spMBRBTestSink.op_Assign(null);
		}
	}

	public static void DeferredOnUpdate(object args)
	{
		if (args is object[] array && (nint)array.LongLength == 2)
		{
			BandwidthTestInterop obj = (BandwidthTestInterop)array[0];
			BandwidthUpdateArgs update = (BandwidthUpdateArgs)array[1];
			obj.OnUpdate(update);
		}
	}

	public static void DeferredOnError(object args)
	{
		if (args is object[] array && (nint)array.LongLength == 2)
		{
			BandwidthTestInterop obj = (BandwidthTestInterop)array[0];
			BandwidthTestErrorArgs error = (BandwidthTestErrorArgs)array[1];
			obj.OnError(error);
		}
	}

	public void OnUpdate(BandwidthUpdateArgs update)
	{
		if (!_ignore)
		{
			raise_BandwidthTestUpdate(this, update);
			if (update.currentState == MBRHeuristicState.Test_Completed)
			{
				Cancel();
			}
		}
	}

	public void OnError(BandwidthTestErrorArgs error)
	{
		if (!_ignore)
		{
			raise_BandwidthTestError(this, error);
			Cancel();
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				_007EBandwidthTestInterop();
				return;
			}
			finally
			{
				try
				{
					((IDisposable)m_spMBRBTestSink).Dispose();
				}
				finally
				{
					try
					{
						((IDisposable)m_spMBRBandwidthTest).Dispose();
					}
					finally
					{
					}
				}
			}
		}
		try
		{
			_0021BandwidthTestInterop();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~BandwidthTestInterop()
	{
		Dispose(false);
	}
}
