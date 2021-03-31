// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLexLexeme
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal class SSLexLexeme
    {
        private const string c_TextForEOF = "eof";
        private int m_token;
        private int m_length;
        private int m_line;
        private int m_column;
        private int m_start;

        private SSLexLexeme()
        {
            this.m_line = 0;
            this.m_token = 0;
            this.m_length = 0;
            this.m_column = 0;
        }

        public SSLexLexeme(SSLexConsumer q_consumer)
        {
            this.m_token = 0;
            this.m_line = q_consumer.line();
            this.m_column = q_consumer.offset();
            this.m_length = q_consumer.lexemeLength();
            this.m_start = q_consumer.m_bufferLexemeStart;
        }

        public SSLexLexeme(SSLexConsumer q_consumer, SSLexFinalState q_final, SSLexMark q_mark)
        {
            this.m_token = q_final.token();
            this.m_line = q_consumer.line();
            this.m_column = q_consumer.offset();
            this.m_length = q_consumer.lexemeLength(q_mark);
            this.m_start = q_consumer.m_bufferLexemeStart;
        }

        public int line() => this.m_line;

        public int token() => this.m_token;

        public int offset() => this.m_column;

        public int length() => this.m_length;

        public string GetValue(SSLex lex) => this.m_token == -1 ? "eof" : lex.consumer().getSubstring(this.m_start, this.m_length);

        public string GetTrimmedValue(SSLex lex, int trimLeft, int trimRight)
        {
            int start = this.m_start + trimLeft;
            int length = this.m_length - (trimLeft + trimRight);
            return lex.consumer().getSubstring(start, length);
        }

        public static SSLexLexeme CreateEOFLexeme(SSLex lex) => new SSLexLexeme()
        {
            m_token = -1,
            m_line = lex.consumer().line(),
            m_column = lex.consumer().offset(),
            m_length = "eof".Length
        };
    }
}
