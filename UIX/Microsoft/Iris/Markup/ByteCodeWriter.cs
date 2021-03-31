// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.ByteCodeWriter
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Iris.Markup
{
    internal class ByteCodeWriter
    {
        private const int BLOCK_SIZE = 4096;
        private byte[] _scratch = new byte[8];
        private uint _cbFreeInBlock;
        private uint _totalSize;
        private byte[] _currentBlock;
        private ArrayList _blockList = new ArrayList();

        public uint DataSize => this._totalSize;

        public void WriteByte(byte value)
        {
            this._scratch[0] = value;
            this.Write(this._scratch, 1U);
        }

        public void WriteByte(OpCode value)
        {
            this._scratch[0] = (byte)value;
            this.Write(this._scratch, 1U);
        }

        public void WriteBool(bool value)
        {
            this._scratch[0] = value ? (byte)1 : (byte)0;
            this.Write(this._scratch, 1U);
        }

        public void WriteChar(char value)
        {
            this._scratch[0] = (byte)value;
            this._scratch[1] = (byte)((uint)value >> 8);
            this.Write(this._scratch, 2U);
        }

        public void WriteUInt16(ushort value)
        {
            this._scratch[0] = (byte)value;
            this._scratch[1] = (byte)((uint)value >> 8);
            this.Write(this._scratch, 2U);
        }

        public void WriteUInt16(int rawValue)
        {
            ushort num = (ushort)rawValue;
            this._scratch[0] = (byte)num;
            this._scratch[1] = (byte)((uint)num >> 8);
            this.Write(this._scratch, 2U);
        }

        public void WriteUInt16(uint rawValue)
        {
            ushort num = (ushort)rawValue;
            this._scratch[0] = (byte)num;
            this._scratch[1] = (byte)((uint)num >> 8);
            this.Write(this._scratch, 2U);
        }

        public void WriteInt32(int value)
        {
            this._scratch[0] = (byte)value;
            this._scratch[1] = (byte)(value >> 8);
            this._scratch[2] = (byte)(value >> 16);
            this._scratch[3] = (byte)(value >> 24);
            this.Write(this._scratch, 4U);
        }

        public void WriteUInt32(uint value)
        {
            this._scratch[0] = (byte)value;
            this._scratch[1] = (byte)(value >> 8);
            this._scratch[2] = (byte)(value >> 16);
            this._scratch[3] = (byte)(value >> 24);
            this.Write(this._scratch, 4U);
        }

        public void WriteInt64(long value) => this.WriteUInt64((ulong)value);

        public void WriteUInt64(ulong value)
        {
            this._scratch[0] = (byte)value;
            this._scratch[1] = (byte)(value >> 8);
            this._scratch[2] = (byte)(value >> 16);
            this._scratch[3] = (byte)(value >> 24);
            this._scratch[4] = (byte)(value >> 32);
            this._scratch[5] = (byte)(value >> 40);
            this._scratch[6] = (byte)(value >> 48);
            this._scratch[7] = (byte)(value >> 56);
            this.Write(this._scratch, 8U);
        }

        public unsafe void WriteSingle(float value)
        {
            uint num = *(uint*)&value;
            this._scratch[0] = (byte)num;
            this._scratch[1] = (byte)(num >> 8);
            this._scratch[2] = (byte)(num >> 16);
            this._scratch[3] = (byte)(num >> 24);
            this.Write(this._scratch, 4U);
        }

        public unsafe void WriteDouble(double value) => this.WriteInt64(*(long*)&value);

        public void WriteString(string value)
        {
            if (value == null)
            {
                this.WriteUInt16(ushort.MaxValue);
            }
            else
            {
                if (value.Length >= (int)short.MaxValue)
                    throw new ArgumentException("String too long");
                bool flag = false;
                foreach (char ch in value)
                {
                    if (ch > 'ÿ')
                    {
                        flag = true;
                        break;
                    }
                }
                uint length = (uint)value.Length;
                if (!flag)
                    length |= 32768U;
                this.WriteUInt16((ushort)length);
                foreach (char ch in value)
                {
                    if (flag)
                        this.WriteChar(ch);
                    else
                        this.WriteByte((byte)ch);
                }
            }
        }

        public void Write(ByteCodeReader value) => this.Write(value, 0U);

        public unsafe void Write(ByteCodeReader value, uint offset)
        {
            IntPtr intPtr = value.ToIntPtr(out long size);
            if (offset > size)
                throw new ArgumentOutOfRangeException(nameof(offset));
            this.Write(new IntPtr(intPtr.ToInt64() + offset), (uint)(size - offset));
        }

        public unsafe void Write(byte[] buffer, uint count)
        {
            fixed (byte* pbData = buffer)
                this.Write(pbData, count);
        }

        public unsafe void Write(IntPtr buffer, uint count) => this.Write((byte*)buffer.ToPointer(), count);

        private unsafe void Write(byte* pbData, uint cbData)
        {
            while (cbData > 0U)
            {
                if (this._cbFreeInBlock == 0U)
                {
                    this._currentBlock = new byte[4096];
                    this._blockList.Add((object)this._currentBlock);
                    this._cbFreeInBlock = 4096U;
                }
                uint num1 = cbData <= this._cbFreeInBlock ? cbData : this._cbFreeInBlock;
                uint num2 = 4096U - this._cbFreeInBlock;
                Marshal.Copy(new IntPtr((void*)pbData), this._currentBlock, (int)num2, (int)num1);
                pbData += (int)num1;
                cbData -= num1;
                this._cbFreeInBlock -= num1;
                this._totalSize += num1;
            }
        }

        public void Overwrite(uint offset, uint value)
        {
            if (offset + 4U > this._totalSize)
                throw new ArgumentException("Invalid offset");
            this.OverwriteByte(offset, (byte)value);
            this.OverwriteByte(offset + 1U, (byte)(value >> 8));
            this.OverwriteByte(offset + 2U, (byte)(value >> 16));
            this.OverwriteByte(offset + 3U, (byte)(value >> 24));
        }

        private void OverwriteByte(uint offset, byte value) => ((byte[])this._blockList[(int)(offset / 4096U)])[(offset % 4096U)] = value;

        private unsafe byte* ComposeFinalBuffer(out uint totalSize)
        {
            byte* pointer = (byte*)NativeApi.MemAlloc(this._totalSize, false).ToPointer();
            byte* numPtr = pointer;
            for (int index = 0; index < this._blockList.Count - 1; ++index)
            {
                Marshal.Copy((byte[])this._blockList[index], 0, new IntPtr((void*)numPtr), 4096);
                numPtr += 4096;
            }
            uint num = 4096U - this._cbFreeInBlock;
            if (this._currentBlock != null && num != 0U)
                Marshal.Copy(this._currentBlock, 0, new IntPtr((void*)numPtr), (int)num);
            totalSize = this._totalSize;
            this._blockList.Clear();
            this._currentBlock = (byte[])null;
            this._cbFreeInBlock = 0U;
            this._totalSize = 0U;
            return pointer;
        }

        public unsafe ByteCodeReader CreateReader()
        {
            uint totalSize;
            return new ByteCodeReader(new IntPtr((void*)this.ComposeFinalBuffer(out totalSize)), totalSize, true);
        }
    }
}
