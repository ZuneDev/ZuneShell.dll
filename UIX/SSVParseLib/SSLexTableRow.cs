// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLexTableRow
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal class SSLexTableRow
    {
        private int m_size;
        private SSLexTableRowEntry[] m_entries;

        public SSLexTableRow(int[] q_row, int q_index)
        {
            this.m_size = q_row[q_index];
            if (this.m_size > 0)
                this.m_entries = new SSLexTableRowEntry[this.m_size];
            ++q_index;
            for (int index = 0; index < this.m_size; ++index)
            {
                this.m_entries[index] = new SSLexTableRowEntry(q_row[q_index], q_row[q_index + 1], q_row[q_index + 2]);
                q_index += 3;
            }
        }

        public int lookup(int q_code)
        {
            for (int index = 0; index < this.m_size; ++index)
            {
                SSLexTableRowEntry entry = this.m_entries[index];
                if (q_code < entry.start())
                    return -1;
                if (q_code <= entry.end())
                    return entry.state();
            }
            return -1;
        }
    }
}
