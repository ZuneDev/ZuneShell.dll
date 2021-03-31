// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.ByteCodeReader
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Iris.Markup
{
    internal class ByteCodeReader : DisposableObject
    {
        private static char[] s_scratchCharArray = new char[100];
        private BinaryReader _reader;
        private IntPtr _buffer;
        private byte[] _bytes;
        private bool _inFixedMemory;

        public unsafe ByteCodeReader(IntPtr buffer, long size, bool ownsAllocation)
        {
            byte[] bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, (int)size);
            _bytes = bytes;

            this._reader = new BinaryReader(new MemoryStream(_bytes));
            _buffer = buffer;
        }

        public byte[] Bytes => _bytes;

        public uint Size => (uint)_reader.BaseStream.Length;

        public uint CurrentOffset
        {
            get => (uint)_reader.BaseStream.Position;
            set => _reader.BaseStream.Seek(value <= Size ? value : throw new Exception("Invalid offset"), SeekOrigin.Begin);
        }

        public unsafe IntPtr CurrentAddress => new IntPtr(_buffer.ToInt64() + _reader.BaseStream.Position);

        public bool IsInFixedMemory => this._inFixedMemory;

        public void MarkAsInFixedMemory() => this._inFixedMemory = true;

        public bool ReadBool() => _reader.ReadBoolean();

        public unsafe byte ReadByte() => _reader.ReadByte();

        public char ReadChar() => _reader.ReadChar();

        public unsafe ushort ReadUInt16() => _reader.ReadUInt16();

        public int ReadInt32() => _reader.ReadInt32();

        public unsafe uint ReadUInt32() => _reader.ReadUInt32();

        public static unsafe ushort ReadUInt16(IntPtr pData)
        {
            var reader = new ByteCodeReader(pData, sizeof(ushort), false);
            return reader.ReadUInt16();
        }

        public static unsafe uint ReadUInt32(IntPtr pData)
        {
            var reader = new ByteCodeReader(pData, sizeof(uint), false);
            return reader.ReadUInt32();
        }

        public long ReadInt64() => _reader.ReadInt64();

        public unsafe ulong ReadUInt64() => _reader.ReadUInt64();

        public unsafe float ReadSingle() => _reader.ReadSingle();

        public unsafe double ReadDouble() => _reader.ReadDouble();

        public unsafe string ReadString()
        {
            uint num1 = this.ReadUInt16();
            if (num1 == ushort.MaxValue)
                return null;
            bool flag;
            uint num2;
            if (((int)num1 & 32768) != 0)
            {
                flag = true;
                num1 &= (uint)short.MaxValue;
                num2 = num1;
            }
            else
            {
                flag = false;
                num2 = num1 * 2U;
            }
            if (this.CurrentOffset + num2 > this.Size)
                this.ThrowReadError();
            char[] chArray = num1 >= s_scratchCharArray.Length ? new char[num1] : s_scratchCharArray;
            byte* numPtr1 = (byte*)(_buffer.ToInt32() + (int)CurrentOffset);
            if (flag)
            {
                for (int index = 0; index < num1; ++index)
                    chArray[index] = (char)*numPtr1++;
            }
            else
            {
                for (int index = 0; index < num1; ++index)
                {
                    byte* numPtr2 = numPtr1;
                    byte* numPtr3 = numPtr2 + 1;
                    byte num3 = *numPtr2;
                    byte* numPtr4 = numPtr3;
                    numPtr1 = numPtr4 + 1;
                    byte num4 = *numPtr4;
                    chArray[index] = (char)(num3 | (uint)num4 << 8);
                }
            }
            _reader.BaseStream.Seek(num2, SeekOrigin.Current);
            return new string(chArray, 0, (int)num1);
        }

        public unsafe IntPtr ToIntPtr(out long size)
        {
            size = Size;
            return _buffer;
        }

        public unsafe IntPtr GetAddress(uint offset) => new IntPtr(_buffer.ToInt64() + offset);

        private void ThrowReadError() => throw new Exception("Attempted to read past the end of the buffer.");

        protected override unsafe void OnDispose()
        {
            base.OnDispose();
            _reader.Close();
            _buffer = IntPtr.Zero;
        }
    }
}
