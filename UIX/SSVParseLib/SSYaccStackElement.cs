// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSYaccStackElement
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal struct SSYaccStackElement
    {
        private int m_state;
        private SSLexLexeme m_lexeme;
        private object m_object;

        public int state() => this.m_state;

        public SSLexLexeme lexeme() => this.m_lexeme;

        public void setState(int q_state) => this.m_state = q_state;

        public void setLexeme(SSLexLexeme q_lexeme) => this.m_lexeme = q_lexeme;

        public object Object
        {
            get => this.m_object;
            set => this.m_object = value;
        }
    }
}
