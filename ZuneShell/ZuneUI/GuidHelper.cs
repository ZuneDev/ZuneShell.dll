// Decompiled with JetBrains decompiler
// Type: ZuneUI.GuidHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public static class GuidHelper
    {
        public static Guid CreateFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Guid.Empty;
            Guid guid = Guid.Empty;
            try
            {
                guid = new Guid(value);
            }
            catch (Exception ex)
            {
            }
            return guid;
        }

        public static bool IsEmpty(Guid guid) => guid == Guid.Empty;

        public static Guid Empty => Guid.Empty;
    }
}
