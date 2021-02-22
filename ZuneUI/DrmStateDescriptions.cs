// Decompiled with JetBrains decompiler
// Type: ZuneUI.DrmStateDescriptions
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public static class DrmStateDescriptions
    {
        private static string _noLicenseDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_NO_LICENSE);
        private static string _personalDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_PERSONAL);
        private static string _purchasedDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_PURCHASED);
        private static string _rentalDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_RENTAL);
        private static string _subscriptionDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_SUBSCRIPTION);
        private static string _subscriptionExpiredDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_SUBSCRIPTION_EXPIRED);
        private static string _protectedDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_PROTECTED);
        private static string _unknownDescription = Shell.LoadString(StringId.IDS_DRM_HEADER_UNKNOWN);

        public static string GetAudioDescription(int stateId) => DrmStateDescriptions.GetDescription(MediaType.Track, (DrmState)stateId);

        public static string GetVideoDescription(int stateId) => DrmStateDescriptions.GetDescription(MediaType.Video, (DrmState)stateId);

        public static string GetDescription(MediaType mediaType, DrmState state)
        {
            switch (state)
            {
                case DrmState.Unknown:
                    return DrmStateDescriptions._unknownDescription;
                case DrmState.NoLicense:
                    return DrmStateDescriptions._noLicenseDescription;
                case DrmState.Expired:
                    return MediaType.Track == mediaType ? DrmStateDescriptions._subscriptionExpiredDescription : DrmStateDescriptions._rentalDescription;
                case DrmState.DeviceLicense:
                    return DrmStateDescriptions._rentalDescription;
                case DrmState.Expiring:
                    return MediaType.Track == mediaType ? DrmStateDescriptions._subscriptionDescription : DrmStateDescriptions._rentalDescription;
                case DrmState.Protected:
                    return MediaType.Track == mediaType ? DrmStateDescriptions._protectedDescription : DrmStateDescriptions._purchasedDescription;
                case DrmState.Free:
                    return DrmStateDescriptions._personalDescription;
                default:
                    return DrmStateDescriptions._unknownDescription;
            }
        }
    }
}
