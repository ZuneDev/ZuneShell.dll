// Decompiled with JetBrains decompiler
// Type: ZuneUI.UrlHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZuneUI
{
    public class UrlHelper
    {
        private static string _endPoint = null;
        private static Size[] _acceptableValues = new Size[25]
        {
      new Size(1920, 1080),
      new Size(1280, 720),
      new Size(854, 480),
      new Size(853, 480),
      new Size(480, 480),
      new Size(420, 320),
      new Size(320, 320),
      new Size(258, 258),
      new Size(258, 194),
      new Size(240, 240),
      new Size(234, 320),
      new Size(172, 258),
      new Size(160, 160),
      new Size(160, 120),
      new Size(107, 160),
      new Size(100, 100),
      new Size(72, 72),
      new Size(64, 64),
      new Size(64, 48),
      new Size(60, 60),
      new Size(52, 52),
      new Size(50, 50),
      new Size(44, 44),
      new Size(43, 64),
      new Size(40, 40)
        };
        private static int[] _acceptableWidths = new int[22]
        {
      1920,
      1280,
      854,
      853,
      480,
      420,
      320,
      258,
      240,
      234,
      172,
      160,
      107,
      100,
      72,
      64,
      60,
      52,
      50,
      44,
      43,
      40
        };
        private static int[] _acceptableHeights = new int[18]
        {
      1080,
      720,
      480,
      320,
      258,
      240,
      194,
      160,
      120,
      100,
      72,
      64,
      60,
      52,
      50,
      48,
      44,
      40
        };

        public static string GetReturnUrl() => Service.GetEndPointUri(EServiceEndpointId.SEID_ZuneNet) + Shell.LoadString(StringId.IDS_RETURN_URL);

        public static string MakeUrlWithReturnParams(string urlPath)
        {
            string returnUrl = GetReturnUrl();
            return MakeUrl(urlPath, "ru", returnUrl, "aru", returnUrl);
        }

        public static string MakeUrl(string urlPath) => MakeUrlEx(urlPath, null);

        public static string MakeUrl(string urlPath, string paramName1, string paramValue1) => MakeUrlEx(urlPath, paramName1, paramValue1);

        public static string MakeUrl(
          string urlPath,
          string paramName1,
          string paramValue1,
          string paramName2,
          string paramValue2)
        {
            return MakeUrlEx(urlPath, paramName1, paramValue1, paramName2, paramValue2);
        }

        public static string MakeUrl(
          string urlPath,
          string paramName1,
          string paramValue1,
          string paramName2,
          string paramValue2,
          string paramName3,
          string paramValue3)
        {
            return MakeUrlEx(urlPath, paramName1, paramValue1, paramName2, paramValue2, paramName3, paramValue3);
        }

        public static string MakeUrl(
          string urlPath,
          string paramName1,
          string paramValue1,
          string paramName2,
          string paramValue2,
          string paramName3,
          string paramValue3,
          string paramName4,
          string paramValue4)
        {
            return MakeUrlEx(urlPath, paramName1, paramValue1, paramName2, paramValue2, paramName3, paramValue3, paramName4, paramValue4);
        }

        public static string MakeUrlEx(string urlPath, params string[] args)
        {
            StringBuilder args1 = new StringBuilder(256);
            args1.Append(Uri.EscapeUriString(urlPath));
            if (args != null && args.Length > 0)
            {
                for (int index = 0; index + 1 < args.Length; index += 2)
                    AppendParam(index == 0, args1, args[index], args[index + 1]);
            }
            return args1.ToString();
        }

        public static void AppendParam(
          bool first,
          StringBuilder args,
          string paramName,
          string paramValue)
        {
            if (first)
                args.Append("?");
            else
                args.Append('&');
            args.Append(paramName);
            args.Append('=');
            args.Append(Uri.EscapeDataString(paramValue));
        }

        public static string PrependHttpIfMissing(string url)
        {
            if (url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                return url;
            StringBuilder stringBuilder = new StringBuilder("http://", url.Length + 7);
            stringBuilder.Append(url);
            return stringBuilder.ToString();
        }

        public static bool ValidUri(string url)
        {
            try
            {
                Uri uri = new Uri(url);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static string StripVersion(string url)
        {
            if (Regex.Match(url, "^/v[0-9]+\\.[0-9]+/").Success)
                url = url.Substring(url.IndexOf("/", 1));
            return url;
        }

        public static string StripLocale(string url)
        {
            if (Regex.Match(url, "^/[a-zA-Z]{2}-[a-zA-Z]{2}/").Success)
                url = url.Substring(url.IndexOf("/", 1));
            return url;
        }

        private static void FindMatchingHeight(ref int width, out int height)
        {
            int index = 0;
            while (index < _acceptableWidths.Length - 1 && width <= _acceptableWidths[index + 1])
                ++index;
            width = _acceptableWidths[index];
            height = 0;
            foreach (Size acceptableValue in _acceptableValues)
            {
                if (acceptableValue.Width == width)
                {
                    height = acceptableValue.Height;
                    break;
                }
            }
        }

        private static void FindMatchingWidth(out int width, ref int height)
        {
            int index = 0;
            while (index < _acceptableHeights.Length - 1 && height <= _acceptableHeights[index + 1])
                ++index;
            height = _acceptableHeights[index];
            width = 0;
            foreach (Size acceptableValue in _acceptableValues)
            {
                if (acceptableValue.Height == height)
                {
                    width = acceptableValue.Width;
                    break;
                }
            }
        }

        private static void CalculateImageUriSize(ref int width, ref int height)
        {
            if (width == 0)
            {
                if (height == 0)
                {
                    width = ImageConstants.Default.Width;
                    height = ImageConstants.Default.Height;
                }
                else
                    FindMatchingWidth(out width, ref height);
            }
            else if (height == 0)
            {
                FindMatchingHeight(ref width, out height);
            }
            else
            {
                int index = 0;
                while (index < _acceptableValues.Length - 1 && width <= _acceptableValues[index + 1].Width)
                    ++index;
                width = _acceptableValues[index].Width;
                while (index < _acceptableValues.Length - 1 && width == _acceptableValues[index + 1].Width && height <= _acceptableValues[index + 1].Height)
                    ++index;
                height = _acceptableValues[index].Height;
            }
        }

        public static string MakeCatalogImageUri(Guid imageId) => MakeCatalogImageUri(imageId, 0, 0);

        public static string MakeCatalogImageUri(Guid imageId, int width, int height) => MakeCatalogImageUri(imageId, width, height, ImageIdType.ImageId, ImageRequested.PrimaryImage);

        public static string MakeCatalogImageUri(
          Guid imageId,
          int width,
          int height,
          ImageIdType imageIdType,
          ImageRequested requestedImage)
        {
            return MakeCatalogImageUri(imageId, width, height, imageIdType, requestedImage, false, false);
        }

        public static string MakeCatalogImageUri(
          Guid imageId,
          int width,
          int height,
          ImageIdType imageIdType,
          ImageRequested requestedImage,
          bool forceImageResize,
          bool ignoreZeroLengths)
        {
            if (_endPoint == null)
                _endPoint = Service.GetEndPointUri(EServiceEndpointId.SEID_ImageCatalog);
            string empty = string.Empty;
            string urlPath;
            switch (imageIdType)
            {
                case ImageIdType.MovieId:
                    urlPath = string.Format("{0}/movie/{1}/{2}", _endPoint, imageId.ToString(), requestedImage.ToString());
                    break;
                case ImageIdType.ArtistId:
                    urlPath = string.Format("{0}/music/artist/{1}/{2}", _endPoint, imageId.ToString(), requestedImage.ToString());
                    break;
                case ImageIdType.MovieTrailerId:
                    urlPath = string.Format("{0}/movieTrailer/{1}/{2}", _endPoint, imageId.ToString(), requestedImage.ToString());
                    break;
                case ImageIdType.ParentalRatingId:
                    urlPath = string.Format("{0}/apps/{1}/ratingImage", Service.GetEndPointUri(EServiceEndpointId.SEID_RootCatalog), imageId.ToString());
                    break;
                default:
                    urlPath = string.Format("{0}/image/{1}", _endPoint, imageId.ToString());
                    break;
            }
            List<string> stringList = new List<string>();
            if (forceImageResize)
                stringList.AddRange(new string[2]
                {
          "resize",
          "true"
                });
            int width1 = width;
            int height1 = height;
            if (width1 != ImageConstants.NowPlaying.Width && height1 != ImageConstants.NowPlaying.Height)
                CalculateImageUriSize(ref width1, ref height1);
            if (width > 0 || !ignoreZeroLengths)
                stringList.AddRange(new string[2]
                {
          nameof (width),
          width1.ToString()
                });
            if (height > 0 || !ignoreZeroLengths)
                stringList.AddRange(new string[2]
                {
          nameof (height),
          height1.ToString()
                });
            return MakeUrlEx(urlPath, stringList.ToArray());
        }
    }
}
