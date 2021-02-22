// Decompiled with JetBrains decompiler
// Type: ZuneXml.Right
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using ZuneUI;

namespace ZuneXml
{
    internal class Right : XmlDataProviderObject
    {
        private MediaRightsEnum _rightEnum;
        private AudioEncodingEnum _audioEncodingEnum;
        private VideoDefinitionEnum _videoDefinitionEnum;
        private VideoResolutionEnum _videoResolutionEnum;
        private ClientTypeEnum _clientTypeEnum;
        private PriceTypeEnum _priceTypeEnum;

        public bool HasPoints => this.PriceTypeEnum == PriceTypeEnum.Points;

        public bool HasCurrency => this.PriceTypeEnum == PriceTypeEnum.Currency;

        public int PointsPrice => this.HasPoints ? (int)this.Price : 0;

        public double CurrencyPrice => this.HasCurrency ? this.Price : 0.0;

        public bool IsFree
        {
            get
            {
                if (this.HasPoints && this.PointsPrice <= 0)
                    return true;
                return this.HasCurrency && this.CurrencyPrice <= 0.0;
            }
        }

        public string Language
        {
            get
            {
                string str = null;
                if (!string.IsNullOrEmpty(this.AudioLocale))
                {
                    string displayLanguageName1 = LanguageHelper.GetDisplayLanguageName(this.AudioLocale);
                    if (!string.IsNullOrEmpty(displayLanguageName1))
                    {
                        str = displayLanguageName1;
                        if (!string.IsNullOrEmpty(this.SubtitleLocale))
                        {
                            string displayLanguageName2 = LanguageHelper.GetDisplayLanguageName(this.SubtitleLocale);
                            if (!string.IsNullOrEmpty(displayLanguageName2))
                                str = string.Format(Shell.LoadString(StringId.IDS_VIDEO_SUBTITLE), displayLanguageName1, displayLanguageName2);
                        }
                    }
                }
                return str;
            }
        }

        internal MediaRightsEnum RightEnum
        {
            get => this._rightEnum;
            set
            {
                if (this._rightEnum == value)
                    return;
                this._rightEnum = value;
                this.FirePropertyChanged(nameof(RightEnum));
            }
        }

        internal AudioEncodingEnum AudioEncodingEnum
        {
            get => this._audioEncodingEnum;
            set
            {
                if (this._audioEncodingEnum == value)
                    return;
                this._audioEncodingEnum = value;
                this.FirePropertyChanged(nameof(AudioEncodingEnum));
            }
        }

        internal VideoDefinitionEnum VideoDefinitionEnum
        {
            get => this._videoDefinitionEnum;
            set
            {
                if (this._videoDefinitionEnum == value)
                    return;
                this._videoDefinitionEnum = value;
                this.FirePropertyChanged(nameof(VideoDefinitionEnum));
            }
        }

        internal VideoResolutionEnum VideoResolutionEnum
        {
            get => this._videoResolutionEnum;
            set
            {
                if (this._videoResolutionEnum == value)
                    return;
                this._videoResolutionEnum = value;
                this.FirePropertyChanged(nameof(VideoResolutionEnum));
            }
        }

        internal ClientTypeEnum ClientTypeEnum
        {
            get => this._clientTypeEnum;
            set
            {
                if (this._clientTypeEnum == value)
                    return;
                this._clientTypeEnum = value;
                this.FirePropertyChanged(nameof(ClientTypeEnum));
            }
        }

        internal PriceTypeEnum PriceTypeEnum
        {
            get => this._priceTypeEnum;
            set
            {
                if (this._priceTypeEnum == value)
                    return;
                this._priceTypeEnum = value;
                this.FirePropertyChanged(nameof(PriceTypeEnum));
                this.FirePropertyChanged("HasPoints");
                this.FirePropertyChanged("HasCurrency");
            }
        }

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "LicenseRight":
                    this.RightEnum = SchemaHelper.ToMediaRights((string)value);
                    break;
                case "AudioEncoding":
                    this.AudioEncodingEnum = SchemaHelper.ToAudioEncoding((string)value);
                    break;
                case "VideoDefinition":
                    this.VideoDefinitionEnum = SchemaHelper.ToVideoDefinition((string)value);
                    break;
                case "VideoResolution":
                    this.VideoResolutionEnum = SchemaHelper.ToVideoResolution((string)value);
                    break;
                case "ClientType":
                    this.ClientTypeEnum = SchemaHelper.ToClientType((string)value);
                    break;
                case "CurrencyCode":
                    this.PriceTypeEnum = SchemaHelper.ToPriceType((string)value);
                    break;
            }
            base.SetProperty(propertyName, value);
        }

        internal static XmlDataProviderObject ConstructRightObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Right(owner, objectTypeCookie);
        }

        internal Right(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string LicenseType => (string)base.GetProperty(nameof(LicenseType));

        internal string LicenseRight => (string)base.GetProperty(nameof(LicenseRight));

        internal string ProviderName => (string)base.GetProperty(nameof(ProviderName));

        internal string ProviderCode => (string)base.GetProperty(nameof(ProviderCode));

        internal Guid OfferId => (Guid)base.GetProperty(nameof(OfferId));

        internal double Price => (double)base.GetProperty(nameof(Price));

        internal double OriginalPrice => (double)base.GetProperty(nameof(OriginalPrice));

        internal string CurrencyCode => (string)base.GetProperty(nameof(CurrencyCode));

        internal string DisplayPrice => (string)base.GetProperty(nameof(DisplayPrice));

        internal string AudioEncoding => (string)base.GetProperty(nameof(AudioEncoding));

        internal string AudioLocale => (string)base.GetProperty(nameof(AudioLocale));

        internal string SubtitleLocale => (string)base.GetProperty(nameof(SubtitleLocale));

        internal string VideoEncoding => (string)base.GetProperty(nameof(VideoEncoding));

        internal string VideoDefinition => (string)base.GetProperty(nameof(VideoDefinition));

        internal string VideoResolution => (string)base.GetProperty(nameof(VideoResolution));

        internal string ClientType => (string)base.GetProperty(nameof(ClientType));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "HasPoints":
                    return HasPoints;
                case "HasCurrency":
                    return HasCurrency;
                case "PointsPrice":
                    return PointsPrice;
                case "CurrencyPrice":
                    return CurrencyPrice;
                case "Language":
                    return Language;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
