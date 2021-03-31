// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Listeners
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup
{
    internal class Listeners : DisposableObject
    {
        protected Vector<Listener> _listenerList;

        protected Listeners(Vector<Listener> listeners) => this._listenerList = listeners;

        protected Listeners(int listenerCount) => this.AddEntries(listenerCount);

        public void AddEntries(int listenerCount)
        {
            if (this._listenerList == null)
                this._listenerList = new Vector<Listener>(listenerCount);
            else
                this._listenerList.Capacity += listenerCount;
            for (int index = 0; index < listenerCount; ++index)
                this._listenerList.Add((Listener)null);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            for (int index = 0; index < this._listenerList.Count; ++index)
                this._listenerList[index]?.Dispose();
            this._listenerList = (Vector<Listener>)null;
        }
    }
}
