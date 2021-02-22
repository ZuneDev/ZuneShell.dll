// Decompiled with JetBrains decompiler
// Type: ZuneUI.FamilySettings
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System.Collections.Generic;

namespace ZuneUI
{
    public class FamilySettings
    {
        private Dictionary<string, FamilySetting> _settings;
        private int _userId;

        public FamilySettings(int userId)
        {
            this._userId = userId;
            this._settings = (Dictionary<string, FamilySetting>)null;
            this.ReloadSettings();
        }

        public FamilySettings()
        {
            this._userId = -1;
            this._settings = new Dictionary<string, FamilySetting>();
        }

        public void ReloadSettings()
        {
            this.Settings = new Dictionary<string, FamilySetting>();
            int[] rgSettingIds;
            FamilySettingsManager.Instance.GetSettingIdsForUser(this.UserId, out rgSettingIds);
            if (rgSettingIds == null || rgSettingIds.Length <= 0)
                return;
            for (int index = 0; index < rgSettingIds.Length; ++index)
            {
                string szRatingSystem;
                int nRatingLevel;
                bool fBlockUnrated;
                FamilySettingsManager.Instance.GetSetting(rgSettingIds[index], out szRatingSystem, out nRatingLevel, out fBlockUnrated);
                this.Settings[szRatingSystem] = new FamilySetting(rgSettingIds[index], szRatingSystem, nRatingLevel, fBlockUnrated);
            }
        }

        public int UserId
        {
            get => this._userId;
            set => this._userId = value;
        }

        public Dictionary<string, FamilySetting> Settings
        {
            get => this._settings;
            set => this._settings = value;
        }

        public void CommitSettings()
        {
            if (this.UserId == -1)
                return;
            foreach (FamilySetting familySetting in this.Settings.Values)
            {
                if (familySetting.HasChanged)
                {
                    int settingId;
                    FamilySettingsManager.Instance.AddSetting(familySetting.RatingId, this.UserId, familySetting.RatingSystem, familySetting.RatingLevel, familySetting.BlockUnrated, out settingId);
                    if (familySetting.RatingId == -1)
                        familySetting.RatingId = settingId;
                }
            }
        }

        public void SetSetting(string ratingSystem, int ratingLevel, bool blockUnrated)
        {
            if (string.IsNullOrEmpty(ratingSystem))
                return;
            if (this.Settings.ContainsKey(ratingSystem))
            {
                FamilySetting setting = this.Settings[ratingSystem];
                setting.RatingLevel = ratingLevel;
                setting.BlockUnrated = blockUnrated;
            }
            else
                this.Settings.Add(ratingSystem, new FamilySetting(ratingSystem, ratingLevel, blockUnrated));
        }
    }
}
