// Decompiled with JetBrains decompiler
// Type: ZuneUI.MultiFeatureMenuItemCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class MultiFeatureMenuItemCommand : MenuItemCommand
    {
        private IList _features;
        private int _hidden = -1;

        public IList Features
        {
            get => this._features;
            set
            {
                if (this._features != value)
                    this._hidden = -1;
                this._features = value;
            }
        }

        public override bool ShouldHide()
        {
            if (this._hidden == -1)
            {
                this._hidden = 0;
                foreach (object feature in (IEnumerable)this._features)
                {
                    if (feature is global::Features eFeature && !FeatureEnablement.IsFeatureEnabled(eFeature))
                    {
                        this._hidden = 1;
                        break;
                    }
                }
            }
            return this._hidden == 1 || base.ShouldHide();
        }
    }
}
