// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSYaccTableRow
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal class SSYaccTableRow
    {
        private const int SSYaccTableEntrySize = 8;
        private const int SSYaccTableRowFlagSync = 1;
        private const int SSYaccTableRowFlagError = 2;
        private const int SSYaccTableRowFlagSyncAll = 4;
        private int m_goto;
        private int m_action;
        private bool m_sync;
        private bool m_error;
        private bool m_syncAll;
        private SSYaccTableRowEntry[] m_entries;

        public SSYaccTableRow(int[] q_data, int q_index)
        {
            this.m_action = q_data[q_index];
            this.m_goto = q_data[q_index + 1];
            this.m_error = q_data[q_index + 2] != 0;
            this.m_syncAll = q_data[q_index + 3] != 0;
            this.m_sync = q_data[q_index + 4] != 0;
            this.m_entries = new SSYaccTableRowEntry[this.numEntries()];
            q_index += 5;
            for (int index = 0; index < this.numEntries(); ++index)
            {
                this.m_entries[index] = new SSYaccTableRowEntry(q_data[q_index], q_data[q_index + 1], q_data[q_index + 2], q_data[q_index + 3]);
                q_index += 4;
            }
        }

        public SSYaccTableRowEntry lookupAction(int q_index)
        {
            for (int index = 0; index < this.m_action; ++index)
            {
                if (this.m_entries[index].token() == q_index)
                    return this.m_entries[index];
            }
            return (SSYaccTableRowEntry)null;
        }

        public SSYaccTableRowEntry lookupGoto(int q_index)
        {
            for (int action = this.m_action; action < this.m_action + this.m_goto; ++action)
            {
                if (this.m_entries[action].token() == q_index)
                    return this.m_entries[action];
            }
            return (SSYaccTableRowEntry)null;
        }

        public bool hasError() => this.m_error;

        public int numEntries() => this.m_goto + this.m_action + (this.hasError() ? 1 : 0);
    }
}
