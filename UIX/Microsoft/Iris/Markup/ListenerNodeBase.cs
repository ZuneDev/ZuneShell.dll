// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.ListenerNodeBase
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Diagnostics;

namespace Microsoft.Iris.Markup
{
    internal class ListenerNodeBase
    {
        protected ListenerNodeBase _next;
        protected ListenerNodeBase _prev;

        protected ListenerNodeBase()
        {
            this._next = (ListenerNodeBase)null;
            this._prev = (ListenerNodeBase)null;
        }

        public virtual void Dispose()
        {
            if (!this.IsLinked)
                return;
            this.Unlink();
        }

        public ListenerNodeBase Next => this._next;

        public bool IsLinked => this._next != null && this._prev != null;

        public void AddPrevious(ListenerNodeBase node)
        {
            if (this._prev == null)
            {
                this._prev = this;
                this._next = this;
            }
            node._prev = this._prev;
            node._next = this;
            this._prev._next = node;
            this._prev = node;
        }

        public void Unlink()
        {
            if (this._prev == this._next)
            {
                this._prev._next = (ListenerNodeBase)null;
                this._prev._prev = (ListenerNodeBase)null;
            }
            else
            {
                this._prev._next = this._next;
                this._next._prev = this._prev;
            }
            this._prev = (ListenerNodeBase)null;
            this._next = (ListenerNodeBase)null;
        }

        [Conditional("DEBUG")]
        private void DEBUG_ValidateList()
        {
        }
    }
}
