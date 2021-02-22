// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class MixHelper
    {
        public static bool MixAvailable => InternetConnection.Instance.IsConnected && FeatureEnablement.IsFeatureEnabled(Features.eMixview);
    }
}
