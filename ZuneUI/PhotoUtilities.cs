// Decompiled with JetBrains decompiler
// Type: ZuneUI.PhotoUtilities
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ZuneUI
{
    public static class PhotoUtilities
    {
        public static bool IsPhotoGalleryInstalled => InstalledProductChecker.IsInstalled(ClientConfiguration.Pictures.WinLivePhotoGalleryUpgradeCode, ClientConfiguration.Pictures.WinLivePhotoGalleryMajorVersion, ClientConfiguration.Pictures.WinLivePhotoGalleryMinorVersion);

        public static string PhotoGalleryExecutablePath
        {
            get
            {
                string empty1 = string.Empty;
                try
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), Shell.LoadString(StringId.IDS_PHOTOS_PHOTO_GALLERY_PATH));
                    if (File.Exists(path))
                        return path;
                }
                catch (Exception ex)
                {
                }
                string empty2 = string.Empty;
                try
                {
                    string path = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), Shell.LoadString(StringId.IDS_PHOTOS_PHOTO_GALLERY_PATH));
                    if (File.Exists(path))
                        return path;
                }
                catch (Exception ex)
                {
                }
                return string.Empty;
            }
        }

        public static bool IsMovieMakerInstalled => InstalledProductChecker.IsInstalled(ClientConfiguration.Pictures.WinLiveMovieMakerUpgradeCode, ClientConfiguration.Pictures.WinLiveMovieMakerMajorVersion, ClientConfiguration.Pictures.WinLiveMovieMakerMinorVersion);

        public static string MovieMakerExecutablePath
        {
            get
            {
                string str = string.Empty;
                try
                {
                    string path1 = (string)((Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Live\\Movie Maker") ?? Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows Live\\Movie Maker")) ?? throw new Exception("Could not open registry key")).GetValue("InstallLocation", (object)string.Empty);
                    if (!string.IsNullOrEmpty(path1))
                        str = Path.Combine(path1, "MovieMaker.exe");
                }
                catch (Exception ex)
                {
                }
                return str;
            }
        }

        public static void PlaySlideShow(int folderId, int startIndex) => PhotoUtilities.PlaySlideShow(folderId, string.Empty, startIndex);

        public static void PlaySlideShow(int folderId, string sort, int startIndex)
        {
            SlideShowState slideShowState = new SlideShowState((IModelItemOwner)null);
            SlideshowLand slideshowLand = new SlideshowLand();
            slideshowLand.SlideShowState = slideShowState;
            slideShowState.Sort = sort;
            slideShowState.Index = startIndex >= 0 ? startIndex : 0;
            slideShowState.FolderId = folderId;
            ZuneShell.DefaultInstance.NavigateToPage((ZunePage)slideshowLand);
        }

        public static void ViewInPhotoGallery(string path)
        {
            if (!PhotoUtilities.IsPhotoGalleryAvailable)
                return;
            try
            {
                string str = string.Empty;
                if (!string.IsNullOrEmpty(path))
                    str = !File.Exists(path) ? string.Format(Shell.LoadString(StringId.IDS_PHOTOS_PHOTO_GALLERY_OPEN_FOLDER), (object)path, (object)path) : string.Format(Shell.LoadString(StringId.IDS_PHOTOS_PHOTO_GALLERY_OPEN_FILE), (object)Path.GetDirectoryName(path), (object)path);
                if (string.IsNullOrEmpty(PhotoUtilities.PhotoGalleryExecutablePath))
                    return;
                new Process()
                {
                    StartInfo = {
            FileName = PhotoUtilities.PhotoGalleryExecutablePath,
            WorkingDirectory = Path.GetDirectoryName(PhotoUtilities.PhotoGalleryExecutablePath),
            Arguments = str
          }
                }.Start();
                SQMLog.Log(SQMDataId.LaunchWLPG, 1);
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (Win32Exception ex)
            {
            }
        }

        public static bool IsPhotoGalleryAvailable => !OSVersion.IsXP() && PhotoUtilities.IsPhotoGalleryInstalled;

        public static bool IsMovieMakerAvailable => !OSVersion.IsXP() && PhotoUtilities.IsMovieMakerInstalled;

        public static string WriteMovieMakerMedia(IList mediaFiles)
        {
            string path2 = Guid.NewGuid().ToString() + ".xml";
            string outputFileName = Path.Combine(Path.GetTempPath(), path2);
            using (XmlWriter xmlWriter = XmlWriter.Create(outputFileName, new XmlWriterSettings()
            {
                Indent = true
            }))
            {
                xmlWriter.WriteStartElement("MovieMaker");
                xmlWriter.WriteStartElement("Content");
                foreach (string mediaFile in (IEnumerable)mediaFiles)
                {
                    xmlWriter.WriteStartElement("ContentFile");
                    xmlWriter.WriteAttributeString("Filename", mediaFile);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("AutoEdit");
                xmlWriter.WriteAttributeString("Style", "FadeReveal");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("DeleteOnClose");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            return outputFileName;
        }

        public static void LaunchMovieMaker(IList mediaFiles)
        {
            if (!PhotoUtilities.IsMovieMakerAvailable)
                return;
            string str = PhotoUtilities.WriteMovieMakerMedia(mediaFiles);
            string format = Shell.LoadString(StringId.IDS_PHOTOS_MOVIE_MAKER_ARGS);
            try
            {
                if (string.IsNullOrEmpty(PhotoUtilities.MovieMakerExecutablePath))
                    return;
                new Process()
                {
                    StartInfo = {
            FileName = PhotoUtilities.MovieMakerExecutablePath,
            WorkingDirectory = Path.GetDirectoryName(PhotoUtilities.MovieMakerExecutablePath),
            Arguments = string.Format(format, (object) str)
          }
                }.Start();
                SQMLog.Log(SQMDataId.LaunchMovieMaker, 1);
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (Win32Exception ex)
            {
            }
        }
    }
}
