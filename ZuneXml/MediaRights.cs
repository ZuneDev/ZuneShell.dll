// Decompiled with JetBrains decompiler
// Type: ZuneXml.MediaRights
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneXml
{
    internal class MediaRights : XmlDataProviderObject
    {
        internal Right GetOfferRight(ClientTypeEnum clientType, PriceTypeEnum priceType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.Rights != null)
            {
                foreach (Right right in (IEnumerable)this.Rights)
                {
                    if (clientType == right.ClientTypeEnum && priceType == right.PriceTypeEnum && Guid.Empty != right.OfferId)
                        return right;
                }
            }
            return (Right)null;
        }

        internal Right GetOfferRight(
          MediaRightsEnum right,
          ClientTypeEnum clientType,
          PriceTypeEnum priceType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.Rights != null)
            {
                foreach (Right right1 in (IEnumerable)this.Rights)
                {
                    if (right == right1.RightEnum && clientType == right1.ClientTypeEnum && (priceType == right1.PriceTypeEnum && Guid.Empty != right1.OfferId))
                        return right1;
                }
            }
            return (Right)null;
        }

        internal Right GetOfferRight(
          MediaRightsEnum right,
          AudioEncodingEnum encoding,
          PriceTypeEnum priceType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMusic) && this.Rights != null)
            {
                foreach (Right right1 in (IEnumerable)this.Rights)
                {
                    if (right == right1.RightEnum && priceType == right1.PriceTypeEnum && (encoding == right1.AudioEncodingEnum && right1.OfferId != Guid.Empty))
                        return right1;
                }
            }
            return (Right)null;
        }

        internal bool HasOfferRights(
          MediaRightsEnum right,
          ClientTypeEnum clientType,
          PriceTypeEnum priceType)
        {
            return this.GetOfferRight(right, clientType, priceType) != null;
        }

        internal bool HasOfferRights(
          MediaRightsEnum right,
          AudioEncodingEnum encoding,
          PriceTypeEnum priceType,
          out Right offer)
        {
            offer = this.GetOfferRight(right, encoding, priceType);
            return offer != null;
        }

        internal bool HasRights(ClientTypeEnum clientType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.Rights != null)
            {
                foreach (Right right in (IEnumerable)this.Rights)
                {
                    if (clientType == right.ClientTypeEnum)
                        return true;
                }
            }
            return false;
        }

        internal bool HasRights(MediaRightsEnum right, ClientTypeEnum clientType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.Rights != null)
            {
                foreach (Right right1 in (IEnumerable)this.Rights)
                {
                    if (right == right1.RightEnum && clientType == right1.ClientTypeEnum)
                        return true;
                }
            }
            return false;
        }

        internal bool HasRights(MediaRightsEnum right, AudioEncodingEnum encoding)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMusic) && this.Rights != null)
            {
                foreach (Right right1 in (IEnumerable)this.Rights)
                {
                    if (right == right1.RightEnum && encoding == right1.AudioEncodingEnum)
                        return true;
                }
            }
            return false;
        }

        internal Right GetOfferRight(
          MediaRightsEnum right,
          VideoDefinitionEnum definition,
          PriceTypeEnum priceType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.IsAssociatedFeatureEnabled(right))
            {
                if (definition == VideoDefinitionEnum.None)
                {
                    Right right1 = (this.GetOfferRight(right, VideoDefinitionEnum.HD, priceType) ?? this.GetOfferRight(right, VideoDefinitionEnum.SD, priceType)) ?? this.GetOfferRight(right, VideoDefinitionEnum.XD, priceType);
                    if (right1 != null)
                        return right1;
                }
                if (this.Rights != null)
                {
                    foreach (Right right1 in (IEnumerable)this.Rights)
                    {
                        if (right == right1.RightEnum && priceType == right1.PriceTypeEnum && (definition == right1.VideoDefinitionEnum || definition == VideoDefinitionEnum.None) && (right1.VideoResolutionEnum != VideoResolutionEnum.VR_1080P || right1.RightEnum != MediaRightsEnum.Purchase))
                            return right1;
                    }
                }
            }
            return (Right)null;
        }

        internal bool HasRights(
          MediaRightsEnum right,
          VideoDefinitionEnum definition,
          PriceTypeEnum priceType)
        {
            Right offerRight = this.GetOfferRight(right, definition, priceType);
            if (right != MediaRightsEnum.PurchaseStream)
                return offerRight != null;
            return offerRight != null && this.HasRights(MediaRightsEnum.Purchase, definition, VideoDefinitionEnum.XD, priceType);
        }

        internal Right GetOfferRight(
          MediaRightsEnum right,
          VideoDefinitionEnum definition1,
          VideoDefinitionEnum definition2,
          PriceTypeEnum priceType)
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.IsAssociatedFeatureEnabled(right) && this.Rights != null)
            {
                foreach (Right right1 in (IEnumerable)this.Rights)
                {
                    if (right == right1.RightEnum && definition1 == right1.VideoDefinitionEnum && priceType == right1.PriceTypeEnum)
                    {
                        foreach (Right right2 in (IEnumerable)this.Rights)
                        {
                            if (right1 != right2 && right == right2.RightEnum && (definition2 == right2.VideoDefinitionEnum && priceType == right2.PriceTypeEnum) && right1.OfferId == right2.OfferId)
                                return right2;
                        }
                    }
                }
            }
            return (Right)null;
        }

        private bool IsAssociatedFeatureEnabled(MediaRightsEnum right)
        {
            bool flag = true;
            if (right == MediaRightsEnum.RentStream && !FeatureEnablement.IsFeatureEnabled(Features.eMBRRental) || right == MediaRightsEnum.PreviewStream && !FeatureEnablement.IsFeatureEnabled(Features.eMBRPreview) || right == MediaRightsEnum.PurchaseStream && !FeatureEnablement.IsFeatureEnabled(Features.eMBRPurchase))
                flag = false;
            return flag;
        }

        internal bool HasRights(
          MediaRightsEnum right,
          VideoDefinitionEnum definition1,
          VideoDefinitionEnum definition2,
          PriceTypeEnum priceType)
        {
            return this.GetOfferRight(right, definition1, definition2, priceType) != null;
        }

        internal bool HasAnyRights()
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) && this.Rights != null)
            {
                foreach (Right right in (IEnumerable)this.Rights)
                {
                    if (this.IsAssociatedFeatureEnabled(right.RightEnum))
                        return true;
                }
            }
            return false;
        }

        internal string GetCompId()
        {
            string str = (string)null;
            if (this.Rights != null)
            {
                foreach (Right right in (IEnumerable)this.Rights)
                {
                    string providerCode = right.ProviderCode;
                    if (!string.IsNullOrEmpty(providerCode))
                    {
                        int length = providerCode.IndexOf(':');
                        str = length <= 0 ? providerCode : providerCode.Substring(0, length);
                        break;
                    }
                }
            }
            return str;
        }

        internal IList Languages
        {
            get
            {
                List<string> stringList = new List<string>();
                foreach (Right right in (IEnumerable)this.Rights)
                {
                    if (this.IsAssociatedFeatureEnabled(right.RightEnum) && (right.VideoDefinitionEnum == VideoDefinitionEnum.HD || right.VideoDefinitionEnum == VideoDefinitionEnum.SD) && right.PriceTypeEnum == PriceTypeEnum.Points)
                    {
                        string language = right.Language;
                        if (!string.IsNullOrEmpty(language) && !stringList.Contains(language))
                            stringList.Add(language);
                    }
                }
                return (IList)stringList;
            }
        }

        internal static XmlDataProviderObject ConstructMediaRightsObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MediaRights(owner, objectTypeCookie);
        }

        internal MediaRights(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal IList Rights => (IList)base.GetProperty(nameof(Rights));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Languages":
                    return (object)this.Languages;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
