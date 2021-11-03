// Decompiled with JetBrains decompiler
// Type: ZuneUI.RatingSystemList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZuneUI
{
    public class RatingSystemList : NotifyPropertyChangedImpl
    {
        private static RatingSystemList s_instance;
        private IDictionary<string, RatingSystem> _ratings;

        private RatingSystemList()
        {
        }

        public static RatingSystemList Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new RatingSystemList();
                return s_instance;
            }
        }

        public IList RatingSystems
        {
            get
            {
                string[] array = null;
                if (this._ratings != null)
                {
                    array = new string[this._ratings.Keys.Count];
                    this._ratings.Keys.CopyTo(array, 0);
                }
                return array;
            }
        }

        public RatingSystem GetRatingSystem(string system) => this._ratings[system];

        public bool Loaded => this._ratings != null;

        public void LoadData() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadDataOnWorkerThread));

        private void LoadDataOnWorkerThread(object state) => this.LoadDataOnWorkerThread();

        internal bool LoadDataOnWorkerThread()
        {
            bool flag = true;
            if (this._ratings == null)
            {
                RatingSystemBase[] ratingSystems = Service.Instance.GetRatingSystems();
                if (ratingSystems != null && ratingSystems.Length > 0)
                {
                    Dictionary<string, RatingSystem> dictionary = new Dictionary<string, RatingSystem>(ratingSystems.Length);
                    foreach (RatingSystemBase details in ratingSystems)
                    {
                        if (!string.IsNullOrEmpty(details.Description))
                            dictionary.Add(details.Name, new RatingSystem(details));
                    }
                    if (dictionary == null || dictionary.Count == 0)
                        flag = false;
                    else
                        this._ratings = dictionary;
                }
            }
            if (flag)
                Application.DeferredInvoke(new DeferredInvokeHandler(this.NotifyLoaded), null);
            return flag;
        }

        private void NotifyLoaded(object args)
        {
            this.FirePropertyChanged("RatingSystems");
            this.FirePropertyChanged("Loaded");
        }
    }
}
