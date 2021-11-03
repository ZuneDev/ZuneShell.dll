// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceComparerForRental
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class DeviceComparerForRental : IComparer
    {
        public int Compare(object x, object y)
        {
            int num = 0;
            UIDevice uiDevice1 = x as UIDevice;
            UIDevice uiDevice2 = y as UIDevice;
            if (uiDevice1 != null && uiDevice2 != null)
            {
                bool supportsRental1 = uiDevice1.SupportsRental;
                bool supportsRental2 = uiDevice2.SupportsRental;
                num = !supportsRental1 || supportsRental2 ? (!supportsRental2 || supportsRental1 ? uiDevice1.Name.CompareTo(uiDevice2.Name) : 1) : -1;
            }
            return num;
        }
    }
}
