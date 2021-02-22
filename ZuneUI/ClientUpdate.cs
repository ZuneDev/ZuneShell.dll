// Decompiled with JetBrains decompiler
// Type: ZuneUI.ClientUpdate
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class ClientUpdate : ModelItem
    {
        private object mylock = new object();
        private static ClientUpdate singletonInstance;

        public static ClientUpdate Instance
        {
            get
            {
                if (ClientUpdate.singletonInstance == null)
                    ClientUpdate.singletonInstance = new ClientUpdate();
                return ClientUpdate.singletonInstance;
            }
        }

        private ClientUpdate()
        {
        }

        public void InvokeClientUpdate()
        {
            SoftwareUpdates.Instance.InstallUpdates();
            DeviceManagement.SetupDevice = (UIDevice)null;
        }

        public void ClientUpdateSkipped() => DeviceManagement.HideSetupDevice();
    }
}
