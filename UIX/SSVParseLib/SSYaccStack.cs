// Decompiled with JetBrains decompiler
// Type: SSVParseLib.SSYaccStack
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace SSVParseLib
{
    internal class SSYaccStack
    {
        private Vector<SSYaccStackElement> m_list = new Vector<SSYaccStackElement>();

        public SSYaccStack(int q_size, int q_inc)
        {
        }

        public void Clear() => this.m_list.Clear();

        public void push(SSYaccStackElement q_ele) => this.m_list.Add(q_ele);

        public void pop() => this.m_list.RemoveAt(this.m_list.Count - 1);

        public SSYaccStackElement elementAt(int index) => this.m_list[index];

        public int getSize() => this.m_list.Count;

        public SSYaccStackElement peek() => this.elementAt(this.m_list.Count - 1);
    }
}
