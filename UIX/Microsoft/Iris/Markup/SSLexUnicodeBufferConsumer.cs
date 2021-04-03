// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.SSLexUnicodeBufferConsumer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using SSVParseLib;
using System;

namespace Microsoft.Iris.Markup
{
    internal class SSLexUnicodeBufferConsumer : SSLexConsumer
    {
        private unsafe char* _buffer;
        private uint _length;
        private string _prefix;

        public SSLexUnicodeBufferConsumer(IntPtr buffer, uint length)
          : this(buffer, length, "")
        {
        }

        public unsafe SSLexUnicodeBufferConsumer(IntPtr buffer, uint length, string prefix)
        {
            _buffer = (char*)buffer.ToPointer();
            _length = length;
            _prefix = prefix;
        }

        public override bool getNext() => GetCharAt(m_index, out m_current);

        public override unsafe string getSubstring(int start, int length) => start + length <= _prefix.Length ? _prefix.Substring(start, length) : NativeApi.PtrToStringUni(new IntPtr(_buffer + start - _prefix.Length), length);

        public unsafe bool GetCharAt(int position, out char ch)
        {
            if (position < _prefix.Length)
            {
                ch = _prefix[m_index];
                return true;
            }
            int index = position - _prefix.Length;
            if (index < _length)
            {
                ch = _buffer[index];
                return true;
            }
            ch = char.MinValue;
            return false;
        }
    }
}
