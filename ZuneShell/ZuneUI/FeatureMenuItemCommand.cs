// Decompiled with JetBrains decompiler
// Type: ZuneUI.FeatureMenuItemCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class FeatureMenuItemCommand : MenuItemCommand
    {
        private Features _feature;
        private int _hidden = -1;

        public Features Features
        {
            get => this._feature;
            set
            {
                if (this._feature != value)
                    this._hidden = -1;
                this._feature = value;
            }
        }

        public override bool ShouldHide()
        {
            if (this._hidden == -1)
                this._hidden = !FeatureEnablement.IsFeatureEnabled(this._feature) ? 1 : 0;
            return this._hidden == 1 || base.ShouldHide();
        }
    }
}
