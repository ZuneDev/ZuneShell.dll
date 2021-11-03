// Decompiled with JetBrains decompiler
// Type: ZuneUI.WlanSignalStrenghComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class WlanSignalStrenghComparer : IComparer<WlanProfile>
    {
        public int Compare(WlanProfile x, WlanProfile y) => x != null && y != null ? (int)y.SignalQuality - (int)x.SignalQuality : 0;
    }
}
