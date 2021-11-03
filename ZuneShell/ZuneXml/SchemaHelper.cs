// Decompiled with JetBrains decompiler
// Type: ZuneXml.SchemaHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    internal class SchemaHelper
    {
        internal static MediaRightsEnum ToMediaRights(string value)
        {
            MediaRightsEnum mediaRightsEnum = MediaRightsEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "Preview":
                        mediaRightsEnum = MediaRightsEnum.Preview;
                        break;
                    case "PreviewStream":
                        mediaRightsEnum = MediaRightsEnum.PreviewStream;
                        break;
                    case "Stream":
                        mediaRightsEnum = MediaRightsEnum.SubscriptionStream;
                        break;
                    case "Subscription":
                        mediaRightsEnum = MediaRightsEnum.SubscriptionDownload;
                        break;
                    case "Purchase":
                        mediaRightsEnum = MediaRightsEnum.Purchase;
                        break;
                    case "PurchaseStream":
                        mediaRightsEnum = MediaRightsEnum.PurchaseStream;
                        break;
                    case "SeasonPurchase":
                        mediaRightsEnum = MediaRightsEnum.SeasonPurchase;
                        break;
                    case "SeasonPurchaseStream":
                        mediaRightsEnum = MediaRightsEnum.SeasonPurchaseStream;
                        break;
                    case "AlbumPurchase":
                        mediaRightsEnum = MediaRightsEnum.AlbumPurchase;
                        break;
                    case "SubscriptionFree":
                        mediaRightsEnum = MediaRightsEnum.SubscriptionFreePurchase;
                        break;
                    case "TransferToPortableDevice":
                        mediaRightsEnum = MediaRightsEnum.TransferToPortableDevice;
                        break;
                    case "Rent":
                        mediaRightsEnum = MediaRightsEnum.Rent;
                        break;
                    case "RentStream":
                        mediaRightsEnum = MediaRightsEnum.RentStream;
                        break;
                    case "Trial":
                        mediaRightsEnum = MediaRightsEnum.PurchaseTrial;
                        break;
                    case "Download":
                        mediaRightsEnum = MediaRightsEnum.Download;
                        break;
                    case "Beta":
                        mediaRightsEnum = MediaRightsEnum.PurchaseBeta;
                        break;
                }
            }
            return mediaRightsEnum;
        }

        internal static AudioEncodingEnum ToAudioEncoding(string value)
        {
            AudioEncodingEnum audioEncodingEnum = AudioEncodingEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "MP3":
                        audioEncodingEnum = AudioEncodingEnum.MP3;
                        break;
                    case "WMA":
                        audioEncodingEnum = AudioEncodingEnum.WMA;
                        break;
                }
            }
            return audioEncodingEnum;
        }

        internal static VideoResolutionEnum ToVideoResolution(string value)
        {
            VideoResolutionEnum videoResolutionEnum = VideoResolutionEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "1080p":
                        videoResolutionEnum = VideoResolutionEnum.VR_1080P;
                        break;
                    case "720p":
                        videoResolutionEnum = VideoResolutionEnum.VR_720P;
                        break;
                    case "480p":
                        videoResolutionEnum = VideoResolutionEnum.VR_480P;
                        break;
                    case "240p":
                        videoResolutionEnum = VideoResolutionEnum.VR_240P;
                        break;
                }
            }
            return videoResolutionEnum;
        }

        internal static VideoDefinitionEnum ToVideoDefinition(string value)
        {
            VideoDefinitionEnum videoDefinitionEnum = VideoDefinitionEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "HD":
                        videoDefinitionEnum = VideoDefinitionEnum.HD;
                        break;
                    case "SD":
                        videoDefinitionEnum = VideoDefinitionEnum.SD;
                        break;
                    case "XD":
                        videoDefinitionEnum = VideoDefinitionEnum.XD;
                        break;
                }
            }
            return videoDefinitionEnum;
        }

        internal static ClientTypeEnum ToClientType(string value)
        {
            ClientTypeEnum clientTypeEnum = ClientTypeEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "Zune 3.0":
                        clientTypeEnum = ClientTypeEnum.Zune;
                        break;
                    case "WinMobile 7.0":
                    case "WinMobile 7.1":
                        clientTypeEnum = ClientTypeEnum.WindowsPhone;
                        break;
                }
            }
            return clientTypeEnum;
        }

        internal static DisclosureEnum ToDisclosureEnum(string value)
        {
            DisclosureEnum disclosureEnum = DisclosureEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "Disclose":
                        disclosureEnum = DisclosureEnum.Disclose;
                        break;
                    case "DiscloseANDPrompt":
                        disclosureEnum = DisclosureEnum.DiscloseAndPrompt;
                        break;
                    case "Prompt":
                        disclosureEnum = DisclosureEnum.Prompt;
                        break;
                }
            }
            return disclosureEnum;
        }

        internal static PriceTypeEnum ToPriceType(string value)
        {
            PriceTypeEnum priceTypeEnum = PriceTypeEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value.ToUpper())
                {
                    case "MPT":
                        priceTypeEnum = PriceTypeEnum.Points;
                        break;
                    default:
                        priceTypeEnum = PriceTypeEnum.Currency;
                        break;
                }
            }
            return priceTypeEnum;
        }

        internal static MessageTypeEnum ToMessageType(string value)
        {
            MessageTypeEnum messageTypeEnum = MessageTypeEnum.Invalid;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "album":
                        messageTypeEnum = MessageTypeEnum.Album;
                        break;
                    case "card":
                        messageTypeEnum = MessageTypeEnum.Card;
                        break;
                    case "forums":
                        messageTypeEnum = MessageTypeEnum.Forums;
                        break;
                    case "friendrequest":
                        messageTypeEnum = MessageTypeEnum.FriendRequest;
                        break;
                    case "message":
                        messageTypeEnum = MessageTypeEnum.Message;
                        break;
                    case "musicvideo":
                        messageTypeEnum = MessageTypeEnum.MusicVideo;
                        break;
                    case "notification":
                        messageTypeEnum = MessageTypeEnum.Notification;
                        break;
                    case "photos":
                        messageTypeEnum = MessageTypeEnum.Photos;
                        break;
                    case "playlist":
                        messageTypeEnum = MessageTypeEnum.Playlist;
                        break;
                    case "podcast":
                        messageTypeEnum = MessageTypeEnum.Podcast;
                        break;
                    case "song":
                        messageTypeEnum = MessageTypeEnum.Song;
                        break;
                    case "video":
                        messageTypeEnum = MessageTypeEnum.Video;
                        break;
                    case "movie":
                        messageTypeEnum = MessageTypeEnum.Movie;
                        break;
                    case "movietrailer":
                        messageTypeEnum = MessageTypeEnum.MovieTrailer;
                        break;
                    default:
                        messageTypeEnum = MessageTypeEnum.Invalid;
                        break;
                }
            }
            return messageTypeEnum;
        }

        internal static MediaTypeEnum ToMediaTypeEnum(string value)
        {
            MediaTypeEnum mediaTypeEnum = MediaTypeEnum.None;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "TVSeason":
                        mediaTypeEnum = MediaTypeEnum.TVSeason;
                        break;
                }
            }
            return mediaTypeEnum;
        }
    }
}
