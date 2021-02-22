// Decompiled with JetBrains decompiler
// Type: ZuneUI.AddTransientMediaTask
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Shell;
using System;
using System.Threading;

namespace ZuneUI
{
    internal class AddTransientMediaTask
    {
        private string _filePath;
        private MediaType _mediaType;
        private int _dbMediaId;
        private bool _fFileAlreadyExists;
        private bool _fAddSuccessful;
        private ManualResetEvent _event;

        private AddTransientMediaTask(string filePath, MediaType mediaType)
        {
            this._filePath = filePath;
            this._mediaType = mediaType;
        }

        private void TaskFunction(object obj)
        {
            this._dbMediaId = -1;
            this._fFileAlreadyExists = false;
            this._fAddSuccessful = false;
            if (!string.IsNullOrEmpty(this._filePath))
                this._fAddSuccessful = ZuneApplication.AddTransientMedia(this._filePath, this._mediaType, out this._dbMediaId, out this._fFileAlreadyExists);
            if (this._event == null)
                return;
            this._event.Set();
        }

        private bool RunSyncTask(
          TimeSpan timeout,
          out int dbMediaId,
          out bool fFileAlreadyExists,
          out bool fTimedout)
        {
            dbMediaId = -1;
            fFileAlreadyExists = false;
            fTimedout = false;
            bool flag = false;
            if (!string.IsNullOrEmpty(this._filePath))
            {
                if (timeout.TotalSeconds > 0.0)
                {
                    this._event = new ManualResetEvent(false);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.TaskFunction));
                    if (this._event.WaitOne(timeout, false))
                    {
                        dbMediaId = this._dbMediaId;
                        fFileAlreadyExists = this._fFileAlreadyExists;
                        flag = this._fAddSuccessful;
                    }
                    else
                        fTimedout = true;
                }
                else
                {
                    this.TaskFunction((object)null);
                    dbMediaId = this._dbMediaId;
                    fFileAlreadyExists = this._fFileAlreadyExists;
                    flag = this._fAddSuccessful;
                }
            }
            return flag;
        }

        public static bool AddTransientMediaWithTimeout(
          string filePath,
          MediaType mediaType,
          TimeSpan timeout,
          out int dbMediaId,
          out bool fFileAlreadyExists,
          out bool fTimedout)
        {
            return new AddTransientMediaTask(filePath, mediaType).RunSyncTask(timeout, out dbMediaId, out fFileAlreadyExists, out fTimedout);
        }
    }
}
