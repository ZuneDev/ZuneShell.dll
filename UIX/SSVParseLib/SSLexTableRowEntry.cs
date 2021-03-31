// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLexTableRowEntry
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal class SSLexTableRowEntry
    {
        private int m_end;
        private int m_start;
        private int m_state;

        public SSLexTableRowEntry(int q_start, int q_end, int q_state)
        {
            this.m_end = q_end;
            this.m_start = q_start;
            this.m_state = q_state;
        }

        public int end() => this.m_end;

        public int start() => this.m_start;

        public int state() => this.m_state;
    }
}
