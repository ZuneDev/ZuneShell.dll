// Decompiled with JetBrains decompiler
// Type: ZuneUI.LocalizationHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Shell;
using System;
using System.IO;

namespace ZuneUI
{
    public static class LocalizationHelper
    {
        public static string GetLocalizedFolderName(string path)
        {
            try
            {
                string fileName = GetLocalizedFolderPath(path);
                FileInfo fileInfo = new FileInfo(fileName);
                if (!string.IsNullOrEmpty(fileInfo.Name))
                    fileName = fileInfo.Name;
                return fileName;
            }
            catch (Exception ex)
            {
                return path;
            }
        }

        public static string GetLocalizedFolderPath(string path)
        {
            string localizedPath;
            return !string.IsNullOrEmpty(path) && ((HRESULT)ZuneApplication.ZuneLibrary.GetLocalizedPathOfFolder(path, false, out localizedPath)).IsSuccess ? localizedPath : path;
        }
    }
}
