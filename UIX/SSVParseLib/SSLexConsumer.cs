// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLexConsumer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal abstract class SSLexConsumer
    {
        private int m_docOriginLine;
        private int m_docOriginColumn;
        private int m_line;
        private int m_start;
        protected int m_index;
        private int m_offset;
        public char m_current;
        private int m_scanLine;
        private int m_scanOffset;
        public int m_bufferLexemeStart;
        private char m_bof;
        private bool m_endOfData;

        public abstract bool getNext();

        public abstract string getSubstring(int start, int length);

        public bool next()
        {
            if (this.m_endOfData)
                return false;
            if ((this.m_current = this.m_bof) != char.MinValue)
            {
                this.m_bof = char.MinValue;
                return true;
            }
            if (!this.getNext())
            {
                ++this.m_index;
                this.m_endOfData = true;
                return false;
            }
            ++this.m_index;
            if (this.m_current == '\n')
            {
                ++this.m_scanLine;
                this.m_scanOffset = 1;
            }
            else
                ++this.m_scanOffset;
            return true;
        }

        public int line() => this.m_docOriginLine + this.m_line;

        public int offset() => this.m_line != 0 ? this.m_offset : this.m_docOriginColumn + this.m_offset;

        public char getCurrent() => this.m_current;

        public SSLexMark mark() => new SSLexMark(this.m_scanLine, this.m_scanOffset, this.m_index);

        public void flushEndOfLine(ref SSLexMark? q_mark)
        {
            SSLexMark ssLexMark = new SSLexMark(q_mark.Value.m_line - 1, q_mark.Value.m_offset, q_mark.Value.m_index - 1);
            q_mark = new SSLexMark?(ssLexMark);
        }

        public void flushStartOfLine(ref SSLexMark? q_mark)
        {
            ++this.m_line;
            ++this.m_start;
            SSLexMark ssLexMark = new SSLexMark(q_mark.Value.m_line - 1, q_mark.Value.m_offset, q_mark.Value.m_index);
            q_mark = new SSLexMark?(ssLexMark);
            this.m_offset = 1;
        }

        public virtual void flushLexeme(SSLexMark q_mark)
        {
            this.m_start = this.m_index = q_mark.m_index;
            this.m_line += q_mark.m_line;
            this.m_offset = q_mark.m_offset;
            this.m_scanLine = 0;
            this.m_scanOffset = q_mark.m_offset;
            this.m_bufferLexemeStart = this.m_index;
        }

        public void flushLexeme()
        {
            this.m_start = this.m_index;
            this.m_line += this.m_scanLine;
            this.m_offset = this.m_scanOffset;
            this.m_scanLine = 0;
            this.m_bufferLexemeStart = this.m_index;
        }

        public int lexemeLength() => this.m_index - this.m_start;

        public int lexemeLength(SSLexMark q_mark) => q_mark.index() - this.m_start;

        public string lexemeBuffer() => this.getSubstring(this.m_bufferLexemeStart, this.lexemeLength());

        public string lexemeBuffer(SSLexMark q_mark) => this.getSubstring(this.m_bufferLexemeStart, this.lexemeLength(q_mark));

        public void SetDocumentOffset(int line, int column)
        {
            this.m_docOriginLine = line;
            this.m_docOriginColumn = column;
        }
    }
}
