// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertySource
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class PropertySource
    {
        public abstract object Get(object media, PropertyDescriptor property);

        public abstract void Set(object media, PropertyDescriptor property, object value);

        public virtual void Commit(object media)
        {
        }

        public virtual bool NeedsCommit => false;
    }
}
