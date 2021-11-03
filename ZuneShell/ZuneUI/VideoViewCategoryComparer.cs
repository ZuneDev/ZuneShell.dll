// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoViewCategoryComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class VideoViewCategoryComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            VideoViewCategory videoViewCategory1 = x as VideoViewCategory;
            VideoViewCategory videoViewCategory2 = y as VideoViewCategory;
            if (videoViewCategory1 != null && videoViewCategory2 != null)
            {
                string view1 = videoViewCategory1.View;
                string view2 = videoViewCategory2.View;
                if (view1 != null && view2 != null)
                    return view1.CompareTo(view2);
            }
            return 1;
        }
    }
}
