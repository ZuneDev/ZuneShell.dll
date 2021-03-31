// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.SharedDisposableObject
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Library
{
    internal class SharedDisposableObject : DisposableObject
    {
        private int _usageCount;

        public SharedDisposableObject() => this.DeclareOwner((object)this);

        protected override void OnDispose() => base.OnDispose();

        public virtual void RegisterUsage(object consumer) => ++this._usageCount;

        public virtual void UnregisterUsage(object consumer)
        {
            --this._usageCount;
            if (this._usageCount != 0)
                return;
            this.Dispose((object)this);
        }
    }
}
