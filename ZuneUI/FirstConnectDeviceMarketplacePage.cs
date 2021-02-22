// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectDeviceMarketplacePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstConnectDeviceMarketplacePage : FirstConnectPage
    {
        internal FirstConnectDeviceMarketplacePage(Wizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_ENABLE_ZUNE_MARKETPLACE_HEADER);

        public override string UI => "res://ZuneShellResources!FirstConnect.uix#FirstConnectDeviceMarketplacePage";

        internal override bool OnMovingNext()
        {
            DeviceManagement deviceManagement = ZuneShell.DefaultInstance.Management.DeviceManagement;
            if (deviceManagement.EnableMarketplaceChoice.ChosenIndex == 0 || (HRESULT)deviceManagement.MarketplaceCredentials.hr == HRESULT._S_OK)
                return base.OnMovingNext();
            deviceManagement.CredentialValidationRequested = true;
            return false;
        }
    }
}
