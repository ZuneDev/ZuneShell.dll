// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.NotifyService
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System.Diagnostics;

namespace Microsoft.Iris.Markup
{
    internal struct NotifyService
    {
        private ListenerRootNode _listenerRoot;
        private static Map s_canonicalizedStrings = new Map(2048);

        public void Fire(string id)
        {
            if (this._listenerRoot == null || this._listenerRoot.Next == null)
                return;
            for (ListenerNodeBase next = this._listenerRoot.Next; next != this._listenerRoot; next = next.Next)
            {
                Listener listener = (Listener)next;
                if (object.ReferenceEquals((object)listener.Watch, (object)id))
                    listener.OnNotify();
            }
        }

        public void FireThreadSafe(string id)
        {
            id = NotifyService.CanonicalizeString(id);
            if (UIDispatcher.IsUIThread)
                this.Fire(id);
            else
                DeferredCall.Post(DispatchPriority.AppEvent, new DeferredHandler(this.FireThreadSafeMarshalHandler), (object)id);
        }

        public void FireThreadSafeMarshalHandler(object arg) => this.Fire((string)arg);

        public bool HasListeners => this._listenerRoot != null && this._listenerRoot.Next != null;

        public void AddListener(Listener listener)
        {
            if (this._listenerRoot == null)
                this._listenerRoot = new ListenerRootNode();
            this._listenerRoot.AddPrevious((ListenerNodeBase)listener);
        }

        public void ClearListeners()
        {
            if (this._listenerRoot == null)
                return;
            while (this._listenerRoot.Next != null)
                this._listenerRoot.Next.Unlink();
            this._listenerRoot.Dispose();
            this._listenerRoot = (ListenerRootNode)null;
        }

        public static string CanonicalizeString(string value) => NotifyService.GetCanonicalizedString(value, true);

        private static string GetCanonicalizedString(string value, bool addIfNotFound)
        {
            object obj = (object)null;
            if (!NotifyService.s_canonicalizedStrings.TryGetValue((object)value, out obj) && addIfNotFound)
            {
                NotifyService.s_canonicalizedStrings[(object)value] = (object)value;
                obj = (object)value;
            }
            return (string)obj;
        }

        [Conditional("DEBUG")]
        public static void AssertIsCanonicalized(string value) => NotifyService.GetCanonicalizedString(value, false);
    }
}
