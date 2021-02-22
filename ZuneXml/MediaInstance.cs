// Decompiled with JetBrains decompiler
// Type: ZuneXml.MediaInstance
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class MediaInstance : XmlDataProviderObject
    {
        private MediaRightsEnum _rightEnum;
        private VideoDefinitionEnum _videoDefinitionEnum;
        private VideoResolutionEnum _videoResolutionEnum;

        public bool HasPurchasedTrial => this._rightEnum == MediaRightsEnum.PurchaseTrial;

        public bool HasPurchasedBeta => this._rightEnum == MediaRightsEnum.PurchaseBeta;

        public bool HasPurchased => this._rightEnum == MediaRightsEnum.Purchase || this._rightEnum == MediaRightsEnum.PurchaseStream || (this._rightEnum == MediaRightsEnum.PurchaseTrial || this._rightEnum == MediaRightsEnum.PurchaseBeta) || this._rightEnum == MediaRightsEnum.SubscriptionFreePurchase || this._rightEnum == MediaRightsEnum.AlbumPurchase;

        internal MediaRightsEnum RightEnum => this._rightEnum;

        internal VideoDefinitionEnum VideoDefinitionEnum => this._videoDefinitionEnum;

        internal VideoResolutionEnum VideoResolutionEnum => this._videoResolutionEnum;

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "LicenseRight":
                    this._rightEnum = SchemaHelper.ToMediaRights((string)value);
                    break;
                case "VideoDefinition":
                    this._videoDefinitionEnum = SchemaHelper.ToVideoDefinition((string)value);
                    break;
                case "VideoResolution":
                    this._videoResolutionEnum = SchemaHelper.ToVideoResolution((string)value);
                    break;
            }
            base.SetProperty(propertyName, value);
        }

        internal static XmlDataProviderObject ConstructMediaInstanceObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MediaInstance(owner, objectTypeCookie);
        }

        internal MediaInstance(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal bool IsDownloadable => (bool)base.GetProperty(nameof(IsDownloadable));

        internal string LicenseRight => (string)base.GetProperty(nameof(LicenseRight));

        internal string VideoDefinition => (string)base.GetProperty(nameof(VideoDefinition));

        internal string VideoResolution => (string)base.GetProperty(nameof(VideoResolution));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "HasPurchased":
                    return (object)this.HasPurchased;
                case "HasPurchasedTrial":
                    return (object)this.HasPurchasedTrial;
                case "HasPurchasedBeta":
                    return (object)this.HasPurchasedBeta;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
