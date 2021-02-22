// Decompiled with JetBrains decompiler
// Type: ZuneUI.MathConstants
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class MathConstants
    {
        public static float PI => 3.141593f;

        public static float HalfPI => 1.570796f;

        public static float DegreeToRadian(float value) => (float)((double)value / 180.0 * Math.PI);

        public static float E => 2.718282f;
    }
}
