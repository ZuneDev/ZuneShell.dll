// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceSetupHashtable
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class DeviceSetupHashtable : Hashtable
    {
        public override object this[object key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                DeviceManagement.HandleSetupQueue();
            }
        }
    }
}
