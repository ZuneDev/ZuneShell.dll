// Decompiled with JetBrains decompiler
// Type: ZuneUI.SplitAudioTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UIXControls;

namespace ZuneUI
{
    public static class SplitAudioTrack
    {
        public static void Split(IList tracks)
        {
            bool flag = false;
            foreach (DataProviderObject track in (IEnumerable)tracks)
            {
                if ((long)(track.GetProperty("FileCount") ?? (object)0) > 1L)
                {
                    HRESULT hresult = SplitAudioTrack.Split((int)(track.GetProperty("LibraryId") ?? (object)-1));
                    flag |= hresult.IsError;
                }
            }
            if (!flag)
                return;
            MessageBox.Show(Shell.LoadString(StringId.IDS_GENERIC_ERROR), Shell.LoadString(StringId.IDS_SHOW_DUPLICATES_FAILED), (EventHandler)null);
        }

        private static HRESULT Split(int libraryId)
        {
            try
            {
                ZuneLibrary.SplitAudioTrack(libraryId);
            }
            catch (COMException ex)
            {
                return (HRESULT)ex.ErrorCode;
            }
            return HRESULT._S_OK;
        }
    }
}
