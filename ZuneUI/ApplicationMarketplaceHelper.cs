// Decompiled with JetBrains decompiler
// Type: ZuneUI.ApplicationMarketplaceHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public static class ApplicationMarketplaceHelper
    {
        public static UIDevice FindAppDevice()
        {
            UIDevice uiDevice1 = UIDeviceList.NullDevice;
            if (SyncControls.Instance.CurrentDevice.SupportsStoreApplications)
            {
                uiDevice1 = SyncControls.Instance.CurrentDevice;
            }
            else
            {
                DateTime minValue = DateTime.MinValue;
                foreach (UIDevice uiDevice2 in SingletonModelItem<UIDeviceList>.Instance)
                {
                    if (uiDevice2.SupportsStoreApplications && uiDevice2.LastConnectTime > minValue)
                        uiDevice1 = uiDevice2;
                }
            }
            return uiDevice1;
        }

        public static void ForceAppUpdateOnAll()
        {
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (uiDevice.SupportsPaidApplications && uiDevice.IsConnectedToClient)
                    uiDevice.ForceAppUpdate();
            }
        }

        public static UIDevice FindConnectedPaidAppDevice()
        {
            UIDevice uiDevice1 = UIDeviceList.NullDevice;
            if (SyncControls.Instance.CurrentDevice.SupportsPaidApplications && SyncControls.Instance.CurrentDevice.IsConnectedToClient)
            {
                uiDevice1 = SyncControls.Instance.CurrentDevice;
            }
            else
            {
                DateTime minValue = DateTime.MinValue;
                foreach (UIDevice uiDevice2 in SingletonModelItem<UIDeviceList>.Instance)
                {
                    if (uiDevice2.SupportsPaidApplications && uiDevice2.IsConnectedToClient && uiDevice2.LastConnectTime > minValue)
                        uiDevice1 = uiDevice2;
                }
            }
            return uiDevice1;
        }
    }
}
