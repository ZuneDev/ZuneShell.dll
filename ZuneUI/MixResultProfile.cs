// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixResultProfile
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class MixResultProfile : MixResult
    {
        protected MixResultProfile()
        {
        }

        public static MixResultProfile CreateInstance(
          DataProviderObject dataProviderObject,
          string reason)
        {
            MixResultProfile mixResultProfile = new MixResultProfile();
            Guid? property = (Guid?)dataProviderObject.GetProperty("UserGuid");
            mixResultProfile.Initialize(MixResultType.Profile, reason, (string)(dataProviderObject.GetProperty("ZuneTag") ?? (object)""), "", property.HasValue ? property.Value.ToString() : "", (string)(dataProviderObject.GetProperty("TileUrl") ?? (object)""), Guid.Empty, dataProviderObject);
            return mixResultProfile;
        }

        internal static int GetItemPriority(DataProviderObject item, int startPriority) => startPriority;
    }
}
