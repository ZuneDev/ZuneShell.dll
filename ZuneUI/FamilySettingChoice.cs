// Decompiled with JetBrains decompiler
// Type: ZuneUI.FamilySettingChoice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class FamilySettingChoice : Choice
    {
        private string _title;
        private string _settingId;
        private bool _showBlockUnrated;
        private BooleanChoice _blockUnrated;
        private IList _choices;

        public FamilySettingChoice(
          IModelItemOwner owner,
          string title,
          string description,
          string blockText,
          string settingId,
          bool showBlockUnrated,
          IList choices)
          : base(owner, description, choices)
        {
            this._title = title;
            this._settingId = settingId;
            this._showBlockUnrated = showBlockUnrated;
            this._blockUnrated = new BooleanChoice(owner, blockText);
            this._choices = choices;
            this.Clear();
        }

        public string Title => this._title;

        public string SettingId => this._settingId;

        public bool ShowBlockUnrated => this._showBlockUnrated;

        public BooleanChoice BlockUnrated => this._blockUnrated;

        public IList Choices => this._choices;

        public FamilySettingValue GetSettingValueById(int value)
        {
            foreach (FamilySettingValue choice in (IEnumerable)this.Choices)
            {
                if (choice.Value == value)
                    return choice;
            }
            return (FamilySettingValue)null;
        }

        public FamilySettingValue SettingValue
        {
            get => (FamilySettingValue)this.ChosenValue;
            set
            {
                if (!this.Options.Contains((object)value))
                    return;
                this.ChosenValue = (object)value;
            }
        }
    }
}
