// Decompiled with JetBrains decompiler
// Type: ZuneUI.ArtistHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;

namespace ZuneUI
{
    public static class ArtistHelper
    {
        public static int InCollection(Guid serviceMediaId)
        {
            int dbMediaId = -1;
            bool fHidden = false;
            return !ZuneApplication.Service2.InCompleteCollection(serviceMediaId, EContentType.Artist, out dbMediaId, out fHidden) ? -1 : dbMediaId;
        }
    }
}
