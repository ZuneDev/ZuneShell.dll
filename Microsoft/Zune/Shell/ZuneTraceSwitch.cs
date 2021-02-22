// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.ZuneTraceSwitch
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Diagnostics;

namespace Microsoft.Zune.Shell
{
    internal class ZuneTraceSwitch : TraceSwitch
    {
        public ZuneTraceSwitch(string displayName, string description)
          : base(displayName, description)
        {
        }

        protected override void OnValueChanged()
        {
            try
            {
                this.SwitchSetting = (int)Enum.Parse(typeof(TraceLevel), this.Value, true);
            }
            catch (ArgumentException ex)
            {
                this.SwitchSetting = 0;
            }
            catch (FormatException ex)
            {
                this.SwitchSetting = 0;
            }
            catch (OverflowException ex)
            {
                this.SwitchSetting = 0;
            }
        }
    }
}
