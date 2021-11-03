// Decompiled with JetBrains decompiler
// Type: ZuneUI.SingletonModelItem`1
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public abstract class SingletonModelItem<T> : ModelItem where T : SingletonModelItem<T>, new()
    {
        private static T _singletonInstance;

        protected SingletonModelItem()
          : this(ZuneShell.DefaultInstance)
        {
        }

        protected SingletonModelItem(IModelItemOwner parent)
          : base(parent)
        {
        }

        public static T Instance
        {
            get
            {
                if (_singletonInstance == null)
                    _singletonInstance = new T();
                return _singletonInstance;
            }
        }
    }
}
