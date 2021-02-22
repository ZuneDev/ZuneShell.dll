// Decompiled with JetBrains decompiler
// Type: ZuneUI.RatingSystem
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Globalization;

namespace ZuneUI
{
    public class RatingSystem : RatingSystemBase
    {
        public RatingSystem(RatingSystemBase details)
          : base(details.Name, details.Title, details.Description, details.BlockText, details.UseImages, details.ShowBlockUnrated, details.DefaultLanguage, details.Strings, details.Ratings)
        {
        }

        private string CurrentLanguage => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper();

        private string GetString(string id)
        {
            if (string.IsNullOrEmpty(id))
                return (string)null;
            return this.Strings.ContainsKey(this.CurrentLanguage) ? (this.Strings[this.CurrentLanguage].ContainsKey(id) ? this.Strings[this.CurrentLanguage][id] : (string)null) : (this.Strings[this.DefaultLanguage].ContainsKey(id) ? this.Strings[this.DefaultLanguage][id] : (string)null);
        }

        public string GetRatingName(int order)
        {
            for (int index = 0; index < this.Ratings.Length; ++index)
            {
                if (this.Ratings[index].Order == order)
                    return this.GetString(this.Ratings[index].TextId);
            }
            return (string)null;
        }

        public string GetRatingDescription(int order)
        {
            for (int index = 0; index < this.Ratings.Length; ++index)
            {
                if (this.Ratings[index].Order == order)
                    return this.GetString(this.Ratings[index].DescriptionId);
            }
            return (string)null;
        }

        public string GetRatingToolTip(int order)
        {
            for (int index = 0; index < this.Ratings.Length; ++index)
            {
                if (this.Ratings[index].Order == order)
                    return this.GetString(this.Ratings[index].ToolTipId);
            }
            return (string)null;
        }

        public new string Title => this.GetString(base.Title);

        public new string Description => this.GetString(base.Description);

        public new string BlockText => this.GetString(base.BlockText);
    }
}
