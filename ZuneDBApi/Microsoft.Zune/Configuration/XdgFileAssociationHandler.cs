using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using ZuneUI;

namespace Microsoft.Zune.Configuration
{
    /// <summary>
    /// Registers/queries default-application associations for MIME types via
    /// the freedesktop.org <c>xdg-mime</c> command-line utility (part of
    /// xdg-utils, present on virtually every desktop Linux distribution),
    /// mirroring the same "open the settings a real desktop file manager
    /// would use" approach <see cref="WindowsFileAssociationHandler"/> takes
    /// on Windows via the registry.
    /// </summary>
    /// <remarks>
    /// <c>xdg-mime</c> has no "clear the default application" operation, only
    /// "set the default application" — so unlike the Windows handler,
    /// un-checking a file type in <see cref="SetFileAssociationInfo"/> cannot
    /// be honored here; it is silently skipped (see the TODO on
    /// <see cref="SetFileAssociationInfo"/>).
    /// </remarks>
    internal sealed class XdgFileAssociationHandler : IFileAssociationHandler
    {
        // TODO: no .desktop file is shipped by this repo/packaging yet. Once
        // one exists (Exec= pointing at the actual Zune host binary), its
        // filename must match this constant for xdg-open to be able to
        // resolve the association this class records via `xdg-mime default`.
        private const string DesktopFileId = "zuneshell.desktop";

        public bool CanAssociationBeChanged() => FindOnPath("xdg-mime") != null;

        public int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList)
        {
            var list = new List<FileAssociationInfo>(ZuneFileAssociations.All.Count);
            foreach (ZuneFileAssociations.Entry entry in ZuneFileAssociations.All)
            {
                bool isOwned = entry.MimeType != null
                    && string.Equals(QueryDefault(entry.MimeType), DesktopFileId, StringComparison.Ordinal);
                list.Add(new FileAssociationInfo(entry.Extension, entry.ProgId, entry.Description, entry.MediaType, isOwned));
            }
            fileAssociationInfoList = list;
            return HRESULT._S_OK;
        }

        // TODO: extensions with no registered MimeType (see ZuneFileAssociations)
        // and un-checking an extension that xdg-mime cannot un-set are both
        // silently skipped rather than reported, since IFileAssociationHandler
        // has no per-entry error channel — only a single result for the whole
        // batch.
        public int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList)
        {
            foreach (FileAssociationInfo info in fileAssociationInfoList)
            {
                if (!info.IsCurrentlyOwned)
                    continue;

                ZuneFileAssociations.Entry? entry = ZuneFileAssociations.Find(info.Extension);
                if (entry?.MimeType == null)
                    continue;

                RunXdgMime("default", DesktopFileId, entry.MimeType);
            }
            return HRESULT._S_OK;
        }

        private static string? QueryDefault(string mimeType)
        {
            string? output = RunXdgMime("query", "default", mimeType);
            return string.IsNullOrWhiteSpace(output) ? null : output.Trim();
        }

        private static string? RunXdgMime(params string[] args)
        {
            try
            {
                var startInfo = new ProcessStartInfo("xdg-mime")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                };
                foreach (string arg in args)
                    startInfo.ArgumentList.Add(arg);

                using Process? process = Process.Start(startInfo);
                if (process == null)
                    return null;
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
            catch (Win32Exception)
            {
                // xdg-mime isn't installed/on PATH.
                return null;
            }
        }

        private static string? FindOnPath(string fileName)
        {
            string? pathVar = Environment.GetEnvironmentVariable("PATH");
            if (pathVar == null)
                return null;
            foreach (string dir in pathVar.Split(Path.PathSeparator))
            {
                string candidate = Path.Combine(dir, fileName);
                if (File.Exists(candidate))
                    return candidate;
            }
            return null;
        }
    }
}
