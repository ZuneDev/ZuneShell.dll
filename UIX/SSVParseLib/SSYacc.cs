// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSYacc
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;

namespace SSVParseLib
{
    internal class SSYacc
    {
        private const int SSYaccActionShift = 0;
        private const int SSYaccActionError = 1;
        private const int SSYaccActionReduce = 2;
        private const int SSYaccActionAccept = 3;
        private const int SSYaccActionConflict = 4;
        public const int m_eofToken = -1;
        private const int m_errorToken = -2;
        private const int SSYaccLexemeCacheMax = -1;
        private int m_cache;
        private int m_state;
        protected SSLex m_lex;
        private int m_action;
        private int m_leftside;
        private bool m_error;
        private bool m_abort;
        private int m_production;
        private SSYaccStack m_stack;
        private SSYaccTable m_table;
        private int m_productionSize;
        private bool m_endOfInput;
        private SSLexLexeme m_endLexeme;
        private SSLexLexeme m_lookahead;
        private SSLexLexeme m_larLookahead;
        private SSLexSubtable m_lexSubtable;
        private SSYaccStackElement m_element;
        private SSYaccStackElement m_treeRoot;
        private SSYaccCache m_lexemeCache;
        private SourceMarkupLoader m_owner;
        private bool m_hasErrors;

        public string FromTerminal(int position) => this.elementFromProduction(position).lexeme().GetValue(this.m_lex);

        public string FromTerminalTrim(int position, int trimLeft, int trimRight) => this.elementFromProduction(position).lexeme().GetTrimmedValue(this.m_lex, trimLeft, trimRight);

        public object FromProduction(int position) => this.elementFromProduction(position).Object;

        public SSYaccStackElement ReturnObject(object value)
        {
            SSYaccStackElement yaccStackElement = this.stackElement();
            yaccStackElement.Object = value;
            return yaccStackElement;
        }

        public int Line(int position) => this.elementFromProduction(position).lexeme().line();

        public int Column(int position) => this.elementFromProduction(position).lexeme().offset();

        public SSYacc(SSYaccTable q_table, SSLex q_lex)
        {
            this.m_lex = q_lex;
            this.m_table = q_table;
            this.m_stack = new SSYaccStack(5, 5);
            this.m_lexemeCache = new SSYaccCache();
            this.Reset((SourceMarkupLoader)null);
        }

        public void Reset(SourceMarkupLoader owner)
        {
            this.m_cache = 0;
            this.m_abort = false;
            this.m_error = false;
            this.m_endOfInput = false;
            this.m_owner = owner;
            this.m_action = 0;
            this.m_endOfInput = false;
            this.m_hasErrors = false;
            this.m_larLookahead = (SSLexLexeme)null;
            this.m_leftside = 0;
            this.m_lexSubtable = (SSLexSubtable)null;
            this.m_lookahead = (SSLexLexeme)null;
            this.m_production = 0;
            this.m_productionSize = 0;
            this.m_state = 0;
            this.m_endLexeme = (SSLexLexeme)null;
            this.m_element = new SSYaccStackElement();
            this.m_treeRoot = new SSYaccStackElement();
            this.m_stack.Clear();
            this.m_lexemeCache.Clear();
            if (owner == null)
                return;
            this.m_endLexeme = SSLexLexeme.CreateEOFLexeme(this.m_lex);
            this.m_element = this.stackElement();
            this.push();
        }

        public virtual SSYaccStackElement reduce(int q_prod, int q_length) => this.stackElement();

        public virtual SSLexLexeme nextLexeme() => this.m_lex.next();

        public virtual SSYaccStackElement stackElement() => new SSYaccStackElement();

        public virtual SSYaccStackElement shift(SSLexLexeme q_lexeme) => this.stackElement();

        public bool larLookahead(SSLexLexeme q_lex) => false;

        public virtual bool error(int q_state, SSLexLexeme q_look)
        {
            this.m_hasErrors = true;
            if (!this.m_lex.HasErrors)
            {
                string str = q_look.GetValue(this.m_lex);
                int line = q_look.line();
                int column = q_look.offset();
                string message = string.Format("Syntax Error: Unexpected character encountered: '{0}'", (object)str);
                if (str == "eof")
                {
                    message = string.Format("Unexpected end of script (script beginning at line {0}, column {1})", (object)line, (object)column);
                    line = this.m_lex.consumer().line();
                    column = this.m_lex.consumer().offset();
                }
                ErrorManager.ReportError(line, column, message);
            }
            return true;
        }

        public SourceMarkupLoader Owner => this.m_owner;

        public bool HasErrors => this.m_hasErrors;

        public bool larError(int q_state, SSLexLexeme q_look, SSLexLexeme q_larLook) => this.error(q_state, q_look);

        public bool parse()
        {
            if (this.doGetLexeme(true))
                return true;
            while (!this.m_abort)
            {
                switch (this.m_action)
                {
                    case 0:
                        if (this.doShift())
                            return true;
                        continue;
                    case 1:
                        if (this.doError())
                            return true;
                        continue;
                    case 2:
                        if (this.doReduce())
                            return true;
                        continue;
                    case 3:
                        this.m_treeRoot = this.m_element;
                        return this.m_error;
                    case 4:
                        if (this.doConflict())
                            return true;
                        continue;
                    default:
                        return true;
                }
            }
            return true;
        }

        public bool doShift()
        {
            this.m_element = this.shift(this.m_lookahead);
            this.m_element.setLexeme(this.m_lookahead);
            this.m_element.setState(this.m_state);
            this.push();
            return this.doGetLexeme(true);
        }

        public bool doReduce()
        {
            this.m_element = this.reduce(this.m_production, this.m_productionSize);
            this.pop(this.m_productionSize);
            return this.goTo(this.m_leftside);
        }

        public bool doError()
        {
            this.m_error = true;
            return this.error(this.m_state, this.m_lookahead);
        }

        public bool doLarError()
        {
            this.m_error = true;
            return this.larError(this.m_state, this.m_lookahead, this.m_larLookahead);
        }

        public SSLexLexeme getLexemeCache()
        {
            SSLexLexeme ssLexLexeme = (SSLexLexeme)null;
            if (this.m_cache != -1 && this.m_lexemeCache.hasElements())
                ssLexLexeme = (SSLexLexeme)this.m_lexemeCache.Dequeue();
            if (ssLexLexeme == null)
            {
                this.m_cache = -1;
                ssLexLexeme = this.nextLexeme() ?? this.m_endLexeme;
                this.m_lexemeCache.Enqueue((object)ssLexLexeme);
            }
            return ssLexLexeme;
        }

        public bool doConflict()
        {
            this.m_cache = 0;
            int q_state = this.m_lexSubtable.lookup(0, this.m_lookahead.token());
            while ((this.m_larLookahead = this.getLexemeCache()) != null)
            {
                q_state = this.m_lexSubtable.lookup(q_state, this.m_larLookahead.token());
                if (q_state != -1)
                {
                    SSLexFinalState ssLexFinalState = this.m_lexSubtable.lookupFinal(q_state);
                    if (ssLexFinalState.isFinal())
                    {
                        if (ssLexFinalState.isReduce())
                        {
                            this.m_production = ssLexFinalState.token();
                            SSYaccTableProd ssYaccTableProd = this.m_table.lookupProd(this.m_production);
                            this.m_leftside = ssYaccTableProd.leftside();
                            this.m_productionSize = ssYaccTableProd.size();
                            return this.doReduce();
                        }
                        this.m_state = ssLexFinalState.token();
                        return this.doShift();
                    }
                }
                else
                    break;
            }
            return this.doLarError();
        }

        public bool doGetLexeme(bool q_look)
        {
            if ((this.m_lookahead = this.m_lexemeCache.remove()) == null)
                return this.getLexeme(q_look);
            if (this.larLookahead(this.m_lookahead))
                return true;
            if (q_look)
                this.lookupAction(this.m_state, this.m_lookahead.token());
            return false;
        }

        public bool getLexeme(bool q_look)
        {
            if (this.m_endOfInput)
                return true;
            this.m_lookahead = this.nextLexeme();
            if (this.m_lookahead == null)
            {
                this.m_endOfInput = true;
                this.m_lookahead = this.m_endLexeme;
            }
            if (q_look)
                this.lookupAction(this.m_state, this.m_lookahead.token());
            return false;
        }

        public bool goTo(int q_goto)
        {
            if (this.lookupGoto(this.m_state, this.m_leftside))
                return true;
            this.m_element.setState(this.m_state);
            this.push();
            this.lookupAction(this.m_state, this.m_lookahead.token());
            return false;
        }

        public void lookupAction(int q_state, int q_token)
        {
            SSYaccTableRowEntry yaccTableRowEntry = this.m_table.lookupRow(q_state).lookupAction(q_token);
            if (yaccTableRowEntry == null)
            {
                this.m_action = 1;
            }
            else
            {
                switch (this.m_action = yaccTableRowEntry.action())
                {
                    case 0:
                        this.m_state = yaccTableRowEntry.entry();
                        break;
                    case 2:
                        SSYaccTableProd ssYaccTableProd = this.m_table.lookupProd(yaccTableRowEntry.entry());
                        this.m_production = yaccTableRowEntry.entry();
                        this.m_leftside = ssYaccTableProd.leftside();
                        this.m_productionSize = ssYaccTableProd.size();
                        break;
                    case 4:
                        this.m_lexSubtable = this.m_table.larTable(yaccTableRowEntry.entry());
                        break;
                }
            }
        }

        public bool lookupGoto(int q_state, int q_token)
        {
            SSYaccTableRowEntry yaccTableRowEntry = this.m_table.lookupRow(q_state).lookupGoto(q_token);
            if (yaccTableRowEntry == null)
                return true;
            this.m_state = yaccTableRowEntry.entry();
            return false;
        }

        public bool push()
        {
            this.m_stack.push(this.m_element);
            return true;
        }

        public bool pop(int q_pop)
        {
            for (int index = 0; index < q_pop; ++index)
                this.m_stack.pop();
            this.m_state = this.m_stack.peek().state();
            return false;
        }

        public SSYaccStackElement elementFromProduction(int q_index) => this.m_stack.elementAt(this.m_stack.getSize() - this.m_productionSize + q_index);

        public SSYaccStackElement treeRoot() => this.m_treeRoot;
    }
}
