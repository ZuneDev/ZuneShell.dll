// Decompiled with JetBrains decompiler
// Type: ZuneUI.DrmHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;

namespace ZuneUI
{
    public static class DrmHelper
    {
        public static bool IsRental(int state, bool fIncludeExpired)
        {
            DrmState drmState = (DrmState)state;
            switch (drmState)
            {
                case DrmState.DeviceLicense:
                case DrmState.Expiring:
                    return true;
                default:
                    return fIncludeExpired && drmState == DrmState.Expired;
            }
        }

        public static bool IsRentalExpired(int state, string filename)
        {
            bool flag = false;
            switch ((DrmState)state)
            {
                case DrmState.Expired:
                    flag = true;
                    break;
                case DrmState.Expiring:
                    DRMInfo fileDrmInfo = ZuneApplication.Service.GetFileDRMInfo(filename);
                    if (fileDrmInfo != null && (fileDrmInfo.NoLicense || fileDrmInfo.LicenseExpired))
                    {
                        flag = true;
                        break;
                    }
                    break;
            }
            return flag;
        }

        public static bool IsRentalExpired(int state, Guid mediaId)
        {
            bool flag = false;
            switch ((DrmState)state)
            {
                case DrmState.Expired:
                    flag = true;
                    break;
                case DrmState.Expiring:
                    DRMInfo mediaDrmInfo = ZuneApplication.Service.GetMediaDRMInfo(mediaId, EContentType.Video);
                    if (mediaDrmInfo != null && (mediaDrmInfo.NoLicense || mediaDrmInfo.LicenseExpired))
                    {
                        flag = true;
                        break;
                    }
                    break;
            }
            return flag;
        }

        public static void ShowDeviceRentalError() => Shell.ShowErrorDialog(HRESULT._NS_E_DRM_DEVICE_RENTAL_LICENSE.Int, StringId.IDS_PLAYBACK_ERROR);

        public static void ShowRentalExpiredError() => Shell.ShowErrorDialog(HRESULT._NS_E_DRM_RENTAL_LICENSE_EXPIRED.Int, StringId.IDS_PLAYBACK_ERROR);
    }
}
