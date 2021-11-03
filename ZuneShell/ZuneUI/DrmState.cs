// Decompiled with JetBrains decompiler
// Type: ZuneUI.DrmState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public enum DrmState
    {
        Unknown = 0,
        NoLicense = 10, // 0x0000000A
        Expired = 20, // 0x00000014
        DeviceLicense = 23, // 0x00000017
        Expiring = 26, // 0x0000001A
        Protected = 30, // 0x0000001E
        Free = 40, // 0x00000028
    }
}
