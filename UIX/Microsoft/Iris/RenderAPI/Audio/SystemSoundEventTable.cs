// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.RenderAPI.Audio.SystemSoundEventTable
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;

namespace Microsoft.Iris.RenderAPI.Audio
{
    internal class SystemSoundEventTable
    {
        private static readonly string s_RegistryParentKey = "AppEvents\\Schemes\\Apps\\.Default";
        private Map<SystemSoundEvent, SystemSoundEventTable.SystemSound> m_systemSoundDictionary;

        public SystemSoundEventTable()
        {
            this.m_systemSoundDictionary = new Map<SystemSoundEvent, SystemSoundEventTable.SystemSound>();
            this.Add(SystemSoundEvent.Asterisk, "SystemAsterisk");
            this.Add(SystemSoundEvent.CloseProgram, "CloseProgram");
            this.Add(SystemSoundEvent.CriticalBatteryAlarm, "CriticalBatteryAlarm");
            this.Add(SystemSoundEvent.CriticalStop, "SystemHand");
            this.Add(SystemSoundEvent.DefaultBeep, ".Default");
            this.Add(SystemSoundEvent.DeviceConnect, "DeviceConnect");
            this.Add(SystemSoundEvent.DeviceDisconnect, "DeviceDisconnect");
            this.Add(SystemSoundEvent.DeviceFailedToConnect, "DeviceFail");
            this.Add(SystemSoundEvent.Exclamation, "SystemExclamation");
            this.Add(SystemSoundEvent.ExitWindows, "SystemExit");
            this.Add(SystemSoundEvent.LowBatteryAlarm, "LowBatteryAlarm");
            this.Add(SystemSoundEvent.Maximize, "Maximize");
            this.Add(SystemSoundEvent.MenuCommand, "MenuCommand");
            this.Add(SystemSoundEvent.MenuPopup, "MenuPopup");
            this.Add(SystemSoundEvent.Minimize, "Minimize");
            this.Add(SystemSoundEvent.NewFaxNotification, "FaxBeep");
            this.Add(SystemSoundEvent.NewMailNotification, "MailBeep");
            this.Add(SystemSoundEvent.OpenProgram, "Open");
            this.Add(SystemSoundEvent.PrintComplete, "PrintComplete");
            this.Add(SystemSoundEvent.ProgramError, "AppGPFault");
            this.Add(SystemSoundEvent.Question, "SystemQuestion");
            this.Add(SystemSoundEvent.RestoreDown, "RestoreDown");
            this.Add(SystemSoundEvent.RestoreUp, "RestoreUp");
            this.Add(SystemSoundEvent.Select, "CCSelect");
            this.Add(SystemSoundEvent.ShowToolbarBand, "ShowBand");
            this.Add(SystemSoundEvent.StartWindows, "SystemStart");
            this.Add(SystemSoundEvent.SystemNotification, "SystemNotification");
            this.Add(SystemSoundEvent.WindowsLogoff, "WindowsLogoff");
            this.Add(SystemSoundEvent.WindowsLogon, "WindowsLogon");
            this.Refresh();
        }

        public void Refresh()
        {
            RegistryKey registryKey1 = RegistryKey.Open(RegistryKey.HKEY_CURRENT_USER, s_RegistryParentKey);
            if (registryKey1 == null)
                return;
            foreach (SystemSoundEventTable.SystemSound systemSound in this.m_systemSoundDictionary.Values)
            {
                RegistryKey registryKey2 = registryKey1.OpenSubKey(systemSound.RegistrySubKey + "\\.Current");
                if (registryKey2 != null)
                {
                    registryKey2.ReadString(null, out systemSound.FilePath);
                    registryKey2.Close();
                }
            }
            registryKey1.Close();
        }

        public string GetFilePath(SystemSoundEvent systemSoundEvent) => this.m_systemSoundDictionary[systemSoundEvent].FilePath;

        private void Add(SystemSoundEvent systemSoundEvent, string registrySubKey) => this.m_systemSoundDictionary.Add(systemSoundEvent, new SystemSoundEventTable.SystemSound()
        {
            Event = systemSoundEvent,
            RegistrySubKey = registrySubKey
        });

        internal class SystemSound
        {
            public SystemSoundEvent Event;
            public string RegistrySubKey;
            public string FilePath;
        }
    }
}
