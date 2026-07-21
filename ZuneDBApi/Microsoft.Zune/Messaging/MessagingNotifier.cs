using System;

namespace Microsoft.Zune.Messaging;

// Original wraps a native MessagingSubscriber* callback sink. Not reverse engineered —
// see logs/Microsoft.Zune/Messaging/PlaylistMessageData.md.
public class MessagingNotifier : IDisposable
{
    public event ComposeCompletedHandler OnComposeCompleted;
    public event DeviceItemsPostedHandler OnDeviceMessagesPosted;
    public event DeviceItemsPostedHandler OnDeviceCartItemsPosted;

    public void DeviceCartItemsPosted(int iNewDeviceCartItems)
    {
        OnDeviceCartItemsPosted?.Invoke(iNewDeviceCartItems);
    }

    public void DeviceMessagesPosted(int iNewDeviceMessages)
    {
        OnDeviceMessagesPosted?.Invoke(iNewDeviceMessages);
    }

    public void ComposeCompleted(int iResourceId)
    {
        OnComposeCompleted?.Invoke(iResourceId);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
