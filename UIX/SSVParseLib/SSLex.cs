// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSLex
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;

namespace SSVParseLib
{
    internal class SSLex
    {
        private int m_state;
        private char[] m_currentChar;
        private SSLexTable m_table;
        private SSLexConsumer m_consumer;
        public bool m_hasErrors;

        public SSLex(SSLexTable q_table, SSLexConsumer q_consumer)
        {
            q_table.Reset();
            this.m_table = q_table;
            this.m_consumer = q_consumer;
            this.m_currentChar = new char[1];
        }

        public void Reset(SSLexConsumer consumer)
        {
            this.m_table.Reset();
            this.m_consumer = consumer;
            this.m_currentChar[0] = char.MinValue;
            this.m_state = 0;
            this.m_hasErrors = false;
        }

        public SSLexConsumer consumer() => this.m_consumer;

        public virtual bool error(SSLexLexeme q_lexeme)
        {
            this.m_hasErrors = true;
            string message = string.Format("Syntax Error: Unexpected character encountered: '{0}'", this.m_currentChar[0]);
            ErrorManager.ReportError(q_lexeme.line(), q_lexeme.offset() + q_lexeme.length() - 1, message);
            return true;
        }

        public bool HasErrors => this.m_hasErrors;

        public virtual bool complete(SSLexLexeme q_lexeme) => true;

        public SSLexLexeme next()
        {
            SSLexLexeme ssLexLexeme = null;
            while (true)
            {
                SSLexMark? q_mark;
                SSLexFinalState q_final;
                do
                {
                    this.m_state = 0;
                    bool flag = false;
                    q_mark = new SSLexMark?();
                    q_final = this.m_table.lookupFinal(this.m_state);
                    if (q_final.isFinal())
                        this.m_consumer.mark();
                    while (this.m_consumer.next())
                    {
                        flag = true;
                        this.m_currentChar[0] = this.m_consumer.getCurrent();
                        this.m_state = this.m_table.lookup(this.m_state, this.m_currentChar[0]);
                        if (this.m_state != -1)
                        {
                            SSLexFinalState ssLexFinalState = this.m_table.lookupFinal(this.m_state);
                            if (ssLexFinalState.isFinal())
                            {
                                q_mark = new SSLexMark?(this.m_consumer.mark());
                                q_final = ssLexFinalState;
                            }
                            if (ssLexFinalState.isContextStart())
                            {
                                SSLexMark? nullable = new SSLexMark?(this.m_consumer.mark());
                            }
                        }
                        else
                            break;
                    }
                    if (flag)
                    {
                        if (q_final.isContextEnd() && q_mark.HasValue)
                            this.m_consumer.flushEndOfLine(ref q_mark);
                        if (q_final.isIgnore() && q_mark.HasValue)
                        {
                            this.m_consumer.flushLexeme(q_mark.Value);
                            if (q_final.isPop() && q_final.isPush())
                                this.m_table.gotoSubtable(q_final.pushIndex());
                            else if (q_final.isPop())
                                this.m_table.popSubtable();
                        }
                        else
                            goto label_19;
                    }
                    else
                        goto label_34;
                }
                while (!q_final.isPush());
                this.m_table.pushSubtable(q_final.pushIndex());
                continue;
            label_19:
                if (!q_final.isFinal() || !q_mark.HasValue)
                {
                    ssLexLexeme = new SSLexLexeme(this.m_consumer);
                    if (!this.error(ssLexLexeme))
                    {
                        this.m_consumer.flushLexeme();
                        ssLexLexeme = null;
                    }
                    else
                        break;
                }
                else
                {
                    if (q_final.isPop() && q_final.isPush())
                        this.m_table.gotoSubtable(q_final.pushIndex());
                    else if (q_final.isPop())
                        this.m_table.popSubtable();
                    else if (q_final.isPush())
                        this.m_table.pushSubtable(q_final.pushIndex());
                    if (q_final.isStartOfLine() && this.m_consumer.line() != 0 && this.m_consumer.offset() != 0)
                        this.m_consumer.flushStartOfLine(ref q_mark);
                    ssLexLexeme = new SSLexLexeme(this.m_consumer, q_final, q_mark.Value);
                    if (q_final.isKeyword())
                        this.m_table.findKeyword(ssLexLexeme);
                    this.m_consumer.flushLexeme(q_mark.Value);
                    if (!this.complete(ssLexLexeme))
                        ssLexLexeme = null;
                    else
                        break;
                }
            }
        label_34:
            return ssLexLexeme;
        }
    }
}
