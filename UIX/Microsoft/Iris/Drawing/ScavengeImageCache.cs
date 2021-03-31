﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.ScavengeImageCache
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Queues;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Drawing
{
    internal sealed class ScavengeImageCache : ImageCache
    {
        private static ScavengeImageCache s_theOnlyCache;
        private static readonly DeferredHandler s_dhReschedule = new DeferredHandler(ScavengeImageCache.Reschedule);
        private UISession _session;
        private ScavengeImageCache.ScavengeCallback _callback;

        public static void Initialize(UISession session)
        {
            ScavengeImageCache.s_theOnlyCache = new ScavengeImageCache(session);
            ScavengeImageCache.s_theOnlyCache.NumItemsToKeep = 200;
            ScavengeImageCache.s_theOnlyCache.ItemRetainTime = new TimeSpan(0, 2, 0);
        }

        public static void Uninitialize(UISession session)
        {
            if (ScavengeImageCache.s_theOnlyCache == null)
                return;
            ScavengeImageCache.s_theOnlyCache.Dispose();
        }

        public static ScavengeImageCache Instance => ScavengeImageCache.s_theOnlyCache;

        private ScavengeImageCache(UISession session)
          : base(session.RenderSession, "GraphicImageCache")
          => this._session = session;

        protected override void OnDispose()
        {
            TimeoutManager timeoutManager = this._session.Dispatcher.TimeoutManager;
            ScavengeImageCache.ScavengeCallback callback = this._callback;
            if (callback != null)
                timeoutManager.CancelTimeout((QueueItem)callback);
            this._callback = (ScavengeImageCache.ScavengeCallback)null;
            base.OnDispose();
        }

        protected override void ScheduleScavenge()
        {
            if (!this.CleanupPending)
                DeferredCall.Post(DispatchPriority.Idle, ScavengeImageCache.s_dhReschedule, (object)this);
            base.ScheduleScavenge();
        }

        private static void Reschedule(object arg)
        {
            ScavengeImageCache cache = arg as ScavengeImageCache;
            if (!cache.CleanupPending)
                return;
            TimeoutManager timeoutManager = cache._session.Dispatcher.TimeoutManager;
            ScavengeImageCache.ScavengeCallback callback = cache._callback;
            cache._callback = (ScavengeImageCache.ScavengeCallback)null;
            if (callback != null)
                timeoutManager.CancelTimeout((QueueItem)callback);
            ScavengeImageCache.ScavengeCallback scavengeCallback = new ScavengeImageCache.ScavengeCallback(cache);
            timeoutManager.SetTimeoutRelative((QueueItem)scavengeCallback, TimeSpan.FromSeconds(5.0));
            cache._callback = scavengeCallback;
        }

        private class ScavengeCallback : QueueItem
        {
            private ScavengeImageCache _cache;

            public ScavengeCallback(ScavengeImageCache cache) => this._cache = cache;

            public override void Dispatch()
            {
                if (this._cache._callback != this)
                    return;
                this._cache._callback = (ScavengeImageCache.ScavengeCallback)null;
                this._cache.CullObjects();
            }
        }
    }
}