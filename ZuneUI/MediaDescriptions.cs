// Decompiled with JetBrains decompiler
// Type: ZuneUI.MediaDescriptions
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections.Generic;

namespace ZuneUI
{
    public static class MediaDescriptions
    {
        private static Dictionary<MediaType, string> _mappings = new Dictionary<MediaType, string>();

        static MediaDescriptions()
        {
            MediaDescriptions._mappings[MediaType.AudioMP4] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_AUDIOMP4);
            MediaDescriptions._mappings[MediaType.AudioMP3] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_AUDIOMP3);
            MediaDescriptions._mappings[MediaType.AudioWMA] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_AUDIOWMA);
            MediaDescriptions._mappings[MediaType.AudioWAV] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_AUDIOWAV);
            MediaDescriptions._mappings[MediaType.ImageJPEG] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_IMAGEJPEG);
            MediaDescriptions._mappings[MediaType.VideoAVI] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOAVI);
            MediaDescriptions._mappings[MediaType.VideoMP4] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOMP4);
            MediaDescriptions._mappings[MediaType.VideoMPG] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOMPG);
            MediaDescriptions._mappings[MediaType.VideoWMV] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOWMV);
            MediaDescriptions._mappings[MediaType.VideoQT] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOQT);
            MediaDescriptions._mappings[MediaType.VideoDVRMS] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEODVRMS);
            MediaDescriptions._mappings[MediaType.VideoMBR] = Shell.LoadString(StringId.IDS_LIBRARY_MEDIATYPE_VIDEOMBR);
        }

        public static string Map(MediaType mediaType)
        {
            string str;
            if (!MediaDescriptions._mappings.TryGetValue(mediaType, out str))
                str = "";
            return str;
        }
    }
}
