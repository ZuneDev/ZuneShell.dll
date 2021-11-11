using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Messaging
{
	public class MessagingNotifier : IDisposable
	{
		private unsafe MessagingSubscriber* m_pMessagingSubscriber;

		private DeviceItemsPostedHandler OnDeviceCartItemsPostedInternal;

		private DeviceItemsPostedHandler OnDeviceMessagesPostedInternal;

		private ComposeCompletedHandler OnComposeCompletedInternal;

		[SpecialName]
		public unsafe event ComposeCompletedHandler OnComposeCompleted
		{
			add
			{
				if (OnComposeCompletedInternal == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002ESubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)2);
				}
				OnComposeCompletedInternal = (ComposeCompletedHandler)Delegate.Combine(OnComposeCompletedInternal, value);
			}
			remove
			{
				if ((OnComposeCompletedInternal = (ComposeCompletedHandler)Delegate.Remove(OnComposeCompletedInternal, value)) == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002EUnsubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)2);
				}
			}
		}

		[SpecialName]
		public unsafe event DeviceItemsPostedHandler OnDeviceMessagesPosted
		{
			add
			{
				if (OnDeviceMessagesPostedInternal == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002ESubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)1);
				}
				OnDeviceMessagesPostedInternal = (DeviceItemsPostedHandler)Delegate.Combine(OnDeviceMessagesPostedInternal, value);
			}
			remove
			{
				if ((OnDeviceMessagesPostedInternal = (DeviceItemsPostedHandler)Delegate.Remove(OnDeviceMessagesPostedInternal, value)) == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002EUnsubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)1);
				}
			}
		}

		[SpecialName]
		public unsafe event DeviceItemsPostedHandler OnDeviceCartItemsPosted
		{
			add
			{
				if (OnDeviceCartItemsPostedInternal == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002ESubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)3);
				}
				OnDeviceCartItemsPostedInternal = (DeviceItemsPostedHandler)Delegate.Combine(OnDeviceCartItemsPostedInternal, value);
			}
			remove
			{
				if ((OnDeviceCartItemsPostedInternal = (DeviceItemsPostedHandler)Delegate.Remove(OnDeviceCartItemsPostedInternal, value)) == null)
				{
					_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002EUnsubscribe(m_pMessagingSubscriber, (EZuneNetMessagingEventType)3);
				}
			}
		}

		public unsafe MessagingNotifier()
		{
			//IL_001f: Expected I, but got I8
			//IL_003b: Expected I, but got I8
			MessagingSubscriber* ptr = (MessagingSubscriber*)_003CModule_003E.@new(24uL);
			MessagingSubscriber* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : _003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingSubscriber_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				_003CModule_003E.delete(ptr);
				throw;
			}
			m_pMessagingSubscriber = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 8)))((nint)ptr2);
		}

		public void DeviceCartItemsPosted(int iNewDeviceCartItems)
		{
			raise_OnDeviceCartItemsPosted(iNewDeviceCartItems);
		}

		public void DeviceMessagesPosted(int iNewDeviceMessages)
		{
			raise_OnDeviceMessagesPosted(iNewDeviceMessages);
		}

		public void ComposeCompleted(int iResourceId)
		{
			raise_OnComposeCompleted(iResourceId);
		}

		private void _007EMessagingNotifier()
		{
			_0021MessagingNotifier();
		}

		private unsafe void _0021MessagingNotifier()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			MessagingSubscriber* pMessagingSubscriber = m_pMessagingSubscriber;
			if (0L != (nint)pMessagingSubscriber)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pMessagingSubscriber + 16)))((nint)pMessagingSubscriber);
				m_pMessagingSubscriber = null;
			}
		}

		[SpecialName]
		public void raise_OnDeviceCartItemsPosted(int iNewDeviceCartItems)
		{
			if (OnDeviceCartItemsPostedInternal != null)
			{
				OnDeviceCartItemsPostedInternal(iNewDeviceCartItems);
			}
		}

		[SpecialName]
		public void raise_OnDeviceMessagesPosted(int iNewDeviceMessages)
		{
			if (OnDeviceMessagesPostedInternal != null)
			{
				OnDeviceMessagesPostedInternal(iNewDeviceMessages);
			}
		}

		[SpecialName]
		public void raise_OnComposeCompleted(int iResourceId)
		{
			if (OnComposeCompletedInternal != null)
			{
				OnComposeCompletedInternal(iResourceId);
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021MessagingNotifier();
				return;
			}
			try
			{
				_0021MessagingNotifier();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~MessagingNotifier()
		{
			Dispose(false);
		}
	}
}
