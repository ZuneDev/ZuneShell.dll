// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertySetAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Messaging;
using System;

namespace ZuneUI
{
    public abstract class PropertySetAttachment : Attachment
    {
        public PropertySetAttachment(Guid id, string title, string subtitle, string imageUri)
          : base(Guid.Empty, title, subtitle, imageUri)
        {
        }

        public abstract IPropertySetMessageData PropertySet { get; }
    }
}
