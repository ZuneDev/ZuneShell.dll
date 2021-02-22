// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileImage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class ProfileImage
    {
        private ProfileImageType m_type;
        private string m_typeName;
        private SafeBitmapWithData m_image;
        private string m_source;
        private static Size s_defaultTileSize;
        private static Size s_defaultBackgroundSize;

        public ProfileImage(ProfileImageType type, SafeBitmapWithData image)
        {
            this.m_type = type;
            this.m_image = image;
        }

        public ProfileImage(
          ProfileImageType type,
          SafeBitmapWithData image,
          int srcX,
          int srcY,
          int srcWidth,
          int srcHeight)
        {
            this.m_type = type;
            this.m_image = image.Clone(srcX, srcY, srcWidth, srcHeight, this.Width, this.Height);
        }

        public ProfileImage(ProfileImageType type, string source)
        {
            this.m_type = type;
            this.m_source = source;
        }

        public ProfileImageType Type => this.m_type;

        public string TypeName
        {
            get
            {
                if (this.m_typeName == null)
                    this.m_typeName = this.m_type != ProfileImageType.Background ? (this.m_type != ProfileImageType.Tile ? string.Empty : "usertile") : "background";
                return this.m_typeName;
            }
        }

        public SafeBitmapWithData Image => this.m_image;

        public string ResourceId
        {
            get
            {
                string str = (string)null;
                int startIndex = -1;
                if (!string.IsNullOrEmpty(this.Source))
                    startIndex = this.Source.IndexOf('!') + 1;
                if (startIndex > 0 && this.Source.Length > startIndex)
                    str = this.Source.Substring(startIndex);
                return str;
            }
        }

        public string Source => this.m_source;

        public int Width
        {
            get
            {
                int num = 0;
                if (this.m_type == ProfileImageType.Background)
                    num = ProfileImage.DefaultBackgroundSize.Width;
                else if (this.m_type == ProfileImageType.Tile)
                    num = ProfileImage.DefaultTileSize.Width;
                return num;
            }
        }

        public int Height
        {
            get
            {
                int num = 0;
                if (this.m_type == ProfileImageType.Background)
                    num = ProfileImage.DefaultBackgroundSize.Height;
                else if (this.m_type == ProfileImageType.Tile)
                    num = ProfileImage.DefaultTileSize.Height;
                return num;
            }
        }

        public static Size DefaultTileSize
        {
            get
            {
                if (ProfileImage.s_defaultTileSize == null)
                    ProfileImage.s_defaultTileSize = new Size(64, 64);
                return ProfileImage.s_defaultTileSize;
            }
        }

        public static Size DefaultBackgroundSize
        {
            get
            {
                if (ProfileImage.s_defaultBackgroundSize == null)
                    ProfileImage.s_defaultBackgroundSize = new Size(535, 196);
                return ProfileImage.s_defaultBackgroundSize;
            }
        }

        public static string[] SupportedFileTypes
        {
            get
            {
                string str1 = Shell.LoadString(StringId.IDS_IMAGE_ALL_FILTER_NAME);
                string str2 = Shell.LoadString(StringId.IDS_IMAGE_JPEG_FILTER_NAME);
                string str3 = Shell.LoadString(StringId.IDS_IMAGE_PNG_FILTER_NAME);
                string str4 = Shell.LoadString(StringId.IDS_IMAGE_BITMAP_FILTER_NAME);
                string str5 = Shell.LoadString(StringId.IDS_IMAGE_GIF_FILTER_NAME);
                string str6 = Shell.LoadString(StringId.IDS_IMAGE_TIFF_FILTER_NAME);
                return new string[12]
                {
          str1,
          "*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.bmp;*.dip;*.gif;*.tif;*.tiff",
          str4,
          "*.bmp;*.dib",
          str2,
          "*.jpg;*.jpeg;*.jpe;*.jfif",
          str5,
          "*.gif",
          str6,
          "*.tif;*.tiff",
          str3,
          "*.png"
                };
            }
        }
    }
}
