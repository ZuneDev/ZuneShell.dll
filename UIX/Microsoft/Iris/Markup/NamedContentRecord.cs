﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.NamedContentRecord
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class NamedContentRecord
    {
        public string Name;
        public uint Offset = uint.MaxValue;

        public NamedContentRecord(string name) => this.Name = name;

        public void SetOffset(uint offset) => this.Offset = offset;
    }
}