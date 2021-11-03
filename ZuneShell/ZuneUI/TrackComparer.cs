// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections.Generic;

namespace ZuneUI
{
    internal class TrackComparer : IComparer<MetadataEditTrack>
    {
        public int Compare(MetadataEditTrack x, MetadataEditTrack y)
        {
            int result1 = 0;
            int result2 = 0;
            if (x != null && y != null)
            {
                int.TryParse(x.GetProperty(MetadataEditMedia.TrackNumberDescriptor).Value, out result1);
                int.TryParse(y.GetProperty(MetadataEditMedia.TrackNumberDescriptor).Value, out result2);
            }
            return result1 - result2;
        }
    }
}
