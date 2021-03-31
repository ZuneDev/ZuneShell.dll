// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLexMark
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal struct SSLexMark
    {
        public int m_line;
        public int m_index;
        public int m_offset;

        public SSLexMark(int q_line, int q_offset, int q_index)
        {
            this.m_index = q_index;
            this.m_line = q_line;
            this.m_offset = q_offset;
        }

        public int index() => this.m_index;
    }
}
