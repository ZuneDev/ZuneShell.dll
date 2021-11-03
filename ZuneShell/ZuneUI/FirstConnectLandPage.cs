// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectLandPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class FirstConnectLandPage : SetupLandPage
    {
        public FirstConnectLandPage() => this.UI = "res://ZuneShellResources!SetupLand.uix#FirstConnect";

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            Application.DeferredInvoke(delegate
           {
               if (UIDeviceList.IsSuitableForConnection(SyncControls.Instance.CurrentDeviceOverride))
                   SyncControls.Instance.CurrentDeviceOverride.Enumerate();
               DeviceManagement.HandleSetupQueue();
           }, DeferredInvokePriority.Low);
        }
    }
}
