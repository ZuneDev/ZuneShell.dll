// Decompiled with JetBrains decompiler
// Type: ZuneUI.PrivacySettingChoice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System.Collections;

namespace ZuneUI
{
    public class PrivacySettingChoice : Choice
    {
        private string m_detailedDescription;
        private PrivacySettingId m_settingId;
        private PrivacyInfoSettings m_infoSettings;
        private static IList s_allowDenyChoices;
        private static IList s_allowFriendsDenyChoices;

        public PrivacySettingChoice(
          IModelItemOwner owner,
          string detailedDescription,
          string description,
          IList choices,
          PrivacySettingId settingId,
          PrivacyInfoSettings infoSettings,
          string linkDescription,
          string linkUrl)
          : this(owner, detailedDescription, description, choices, settingId, infoSettings)
        {
            this.LinkDescription = linkDescription;
            this.LinkUrl = linkUrl;
        }

        public PrivacySettingChoice(
          IModelItemOwner owner,
          string detailedDescription,
          string description,
          IList choices,
          PrivacySettingId settingId,
          PrivacyInfoSettings infoSettings)
          : base(owner, description, choices)
        {
            this.m_detailedDescription = detailedDescription;
            this.m_settingId = settingId;
            this.m_infoSettings = infoSettings;
        }

        public PrivacySettingId SettingId => this.m_settingId;

        public PrivacyInfoSettings InfoSettings => this.m_infoSettings;

        public string DetailedDescription => this.m_detailedDescription;

        public PrivacySettingValue SettingValue
        {
            get => this.ChosenValue is PrivacySettingValue ? (PrivacySettingValue)this.ChosenValue : PrivacySettingValue.Unknown;
            set
            {
                if (this.Options.Contains((object)value))
                {
                    this.ChosenValue = (object)value;
                }
                else
                {
                    if (value != PrivacySettingValue.Unknown)
                        return;
                    this.Clear();
                }
            }
        }

        public string LinkDescription { get; private set; }

        public string LinkUrl { get; private set; }

        internal static IList AllowDenyChoices
        {
            get
            {
                if (PrivacySettingChoice.s_allowDenyChoices == null)
                {
                    PrivacySettingChoice.s_allowDenyChoices = (IList)new ArrayList(2);
                    PrivacySettingChoice.s_allowDenyChoices.Add((object)PrivacySettingValue.Allow);
                    PrivacySettingChoice.s_allowDenyChoices.Add((object)PrivacySettingValue.Deny);
                }
                return PrivacySettingChoice.s_allowDenyChoices;
            }
        }

        internal static IList AllowFriendDenyChoices
        {
            get
            {
                if (PrivacySettingChoice.s_allowFriendsDenyChoices == null)
                {
                    PrivacySettingChoice.s_allowFriendsDenyChoices = (IList)new ArrayList(3);
                    PrivacySettingChoice.s_allowFriendsDenyChoices.Add((object)PrivacySettingValue.Allow);
                    PrivacySettingChoice.s_allowFriendsDenyChoices.Add((object)PrivacySettingValue.FriendsOnly);
                    PrivacySettingChoice.s_allowFriendsDenyChoices.Add((object)PrivacySettingValue.Deny);
                }
                return PrivacySettingChoice.s_allowFriendsDenyChoices;
            }
        }
    }
}
