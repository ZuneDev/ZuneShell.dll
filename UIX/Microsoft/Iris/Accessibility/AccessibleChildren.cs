// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Accessibility.AccessibleChildren
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.Iris.Accessibility
{
    [ComVisible(true)]
    public class AccessibleChildren : IEnumVARIANT
    {
        private AccessibleProxy _proxy;
        private int _current;

        internal AccessibleChildren(AccessibleProxy proxy)
          : this(proxy, -1)
        {
        }

        internal AccessibleChildren(AccessibleProxy proxy, int position)
        {
            this._proxy = proxy;
            this._current = position;
        }

        internal IEnumVARIANT Clone() => new AccessibleChildren(this._proxy, this._current);

        internal int Next(int count, object[] children)
        {
            int index = 0;
            do
            {
                ++this._current;
                if (this._current >= this._proxy.UI.Children.Count)
                {
                    this._current = this._proxy.UI.Children.Count;
                    break;
                }
                UIClass child = (UIClass)this._proxy.UI.Children[this._current];
                children[index] = child.AccessibleProxy;
                ++index;
            }
            while (index < count);
            return index;
        }

        internal void Reset() => this._current = -1;

        internal bool Skip(int count)
        {
            this._current += count;
            if (this._current < this._proxy.UI.Children.Count)
                return true;
            this._current = this._proxy.UI.Children.Count;
            return false;
        }

        IEnumVARIANT IEnumVARIANT.Clone()
        {
            this.VerifyEnumeratorAccess();
            return this.Clone();
        }

        unsafe int IEnumVARIANT.Next(int celt, object[] rgVar, IntPtr pceltFetched)
        {
            this.VerifyEnumeratorAccess();
            int num = this.Next(celt, rgVar);
            *(int*)(void*)pceltFetched = num;
            return num != celt ? 1 : 0;
        }

        int IEnumVARIANT.Reset()
        {
            this.VerifyEnumeratorAccess();
            this.Reset();
            return 0;
        }

        int IEnumVARIANT.Skip(int celt)
        {
            this.VerifyEnumeratorAccess();
            return !this.Skip(celt) ? 1 : 0;
        }

        private void VerifyEnumeratorAccess()
        {
            if (this._proxy.UI != null && UIDispatcher.IsUIThread)
                return;
            Marshal.ThrowExceptionForHR(-2147467259);
        }
    }
}
