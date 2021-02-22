// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class WizardExperience : Experience
    {
        private CategoryPageNode _fue;
        private CategoryPageNode _wirelessSetup;

        public WizardExperience(Frame frameOwner)
          : base(frameOwner)
        {
        }

        public override IList NodesList => (IList)null;

        public CategoryPageNode FUE
        {
            get
            {
                if (this._fue == null)
                    this._fue = new CategoryPageNode(this, StringId.IDS_SETTINGS_PIVOT, new Category[3]
                    {
            SettingCategories.Collection,
            SettingCategories.Filetype,
            SettingCategories.Privacy
                    }, SQMDataId.Invalid, false, false);
                return this._fue;
            }
        }

        public CategoryPageNode WirelessSetup
        {
            get
            {
                if (this._wirelessSetup == null)
                    this._wirelessSetup = new CategoryPageNode(this, StringId.IDS_SET_UP_YOUR_ZUNE, new Category[1]
                    {
            SettingCategories.WirelessSetup
                    }, SQMDataId.Invalid, true, false);
                return this._wirelessSetup;
            }
        }
    }
}
