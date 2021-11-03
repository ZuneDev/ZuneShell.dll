// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.PerfTrace.EtwTraceProvider
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Win32;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.PerfTrace
{
    internal class EtwTraceProvider
    {
        private const ushort _version = 0;
        private EtwProc _etwProc;
        private ulong _registrationHandle;
        private ulong _traceHandle;
        private byte _level;
        private uint _flags;
        private bool _enabled;

        internal EtwTraceProvider(Guid controlGuid, string regPath)
        {
            this._level = 0;
            this._flags = 0U;
            this._enabled = false;
            this._traceHandle = 0UL;
            this._registrationHandle = 0UL;
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regPath);
            int num = 1;
            if (registryKey != null && registryKey.GetValue("EtwEnabled") is int val)
            {
                num = val;
            }
            if (num <= 0)
            {
                this._enabled = false;
            }
            else
            {
                int num1 = (int)this.Register(controlGuid);
            }
        }

        ~EtwTraceProvider()
        {
            UnregisterTraceGuids(this._registrationHandle);
            GC.KeepAlive(_etwProc);
        }

        internal uint Flags => this._flags;

        internal byte Level => this._level;

        internal bool IsEnabled => this._enabled;

        internal unsafe uint ControllerChangeCallback(
          uint requestCode,
          IntPtr context,
          IntPtr bufferSize,
          byte* byteBuffer)
        {
            try
            {
                BaseEvent* baseEventPtr = (BaseEvent*)byteBuffer;
                switch (requestCode)
                {
                    case 4:
                        this._traceHandle = baseEventPtr->HistoricalContext;
                        this._flags = (uint)GetTraceEnableFlags(baseEventPtr->HistoricalContext);
                        this._level = GetTraceEnableLevel(baseEventPtr->HistoricalContext);
                        if (this._flags == 0U && this._level == 0)
                        {
                            this._flags = uint.MaxValue;
                            this._level = 4;
                        }
                        this._enabled = true;
                        break;
                    case 5:
                        this._enabled = false;
                        this._traceHandle = 0UL;
                        this._level = 0;
                        this._flags = 0U;
                        break;
                    default:
                        this._enabled = false;
                        this._traceHandle = 0UL;
                        break;
                }
                return 0;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case NullReferenceException _:
                    case SEHException _:
                        throw;
                    default:
                        return 0;
                }
            }
        }

        private unsafe uint Register(Guid controlGuid)
        {
            TraceGuidRegistration guidReg = new TraceGuidRegistration();
            Guid guid = new Guid(3029687280U, 15089, 18240, 180, 117, 153, 5, 93, 63, 233, 170);
            this._etwProc = new EtwProc(this.ControllerChangeCallback);
            guidReg.Guid = &guid;
            guidReg.RegHandle = null;
            return RegisterTraceGuids(this._etwProc, null, ref controlGuid, 1U, ref guidReg, null, null, out this._registrationHandle);
        }

        internal void TraceEvent(byte level, Guid eventGuid, byte eventType) => this.TraceEvent(level, eventGuid, eventType, null, null);

        internal void TraceEvent(byte level, Guid eventGuid, byte eventType, object data0) => this.TraceEvent(level, eventGuid, eventType, data0, null);

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, null, null, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, null, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, data6, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6,
          object data7)
        {
            int num = (int)this.TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, data6, data7, null);
        }

        internal unsafe uint TraceEvent(
          byte level,
          Guid eventGuid,
          byte evtype,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6,
          object data7,
          object data8)
        {
            char* chPtr1 = stackalloc char[144];
            uint offSet = 0;
            char* ptr = chPtr1;
            int num1 = 0;
            uint num2 = 0;
            int num3 = 0;
            string str1;
            string str2 = str1 = "";
            string str3 = str1;
            string str4 = str1;
            string str5 = str1;
            string str6 = str1;
            string str7 = str1;
            string str8 = str1;
            string str9 = str1;
            string str10 = str1;
            BaseEvent baseEvent;
            baseEvent.ClientContext = 0U;
            baseEvent.Flags = 1179648U;
            baseEvent.Guid = eventGuid;
            baseEvent.EventType = evtype;
            baseEvent.Level = level;
            baseEvent.Version = 0;
            if (data0 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                if ((str10 = this.ProcessOneObject(data0, mofField, ptr, ref offSet)) != null)
                    num1 |= 1;
            }
            if (data1 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str9 = this.ProcessOneObject(data1, mofField, ptr, ref offSet)) != null)
                    num1 |= 2;
            }
            if (data2 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str8 = this.ProcessOneObject(data2, mofField, ptr, ref offSet)) != null)
                    num1 |= 4;
            }
            if (data3 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str7 = this.ProcessOneObject(data3, mofField, ptr, ref offSet)) != null)
                    num1 |= 8;
            }
            if (data4 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str6 = this.ProcessOneObject(data4, mofField, ptr, ref offSet)) != null)
                    num1 |= 16;
            }
            if (data5 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str5 = this.ProcessOneObject(data5, mofField, ptr, ref offSet)) != null)
                    num1 |= 32;
            }
            if (data6 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str4 = this.ProcessOneObject(data6, mofField, ptr, ref offSet)) != null)
                    num1 |= 64;
            }
            if (data7 != null)
            {
                ++num2;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str3 = this.ProcessOneObject(data7, mofField, ptr, ref offSet)) != null)
                    num1 |= 128;
            }
            if (data8 != null)
            {
                uint num4 = num2 + 1U;
                MofField* mofField = &baseEvent.UserData + num3++ * sizeof(MofField);
                ptr = chPtr1 + offSet;
                if ((str2 = this.ProcessOneObject(data8, mofField, ptr, ref offSet)) != null)
                    num1 |= 256;
            }
            if (ptr - chPtr1 > 144L)
                return 1;
            uint num5;
            fixed (char* chPtr2 = str10)
            fixed (char* chPtr3 = str9)
            fixed (char* chPtr4 = str8)
            fixed (char* chPtr5 = str7)
            fixed (char* chPtr6 = str6)
            fixed (char* chPtr7 = str5)
            fixed (char* chPtr8 = str4)
            fixed (char* chPtr9 = str3)
            fixed (char* chPtr10 = str2)
            {
                int index1 = 0;
                if ((num1 & 1) != 0)
                {
                    (&baseEvent.UserData)[index1].DataLength = (uint)((str10.Length + 1) * 2);
                    (&baseEvent.UserData)[index1].DataPointer = chPtr2;
                }
                int index2 = index1 + 1;
                if ((num1 & 2) != 0)
                {
                    (&baseEvent.UserData)[index2].DataLength = (uint)((str9.Length + 1) * 2);
                    (&baseEvent.UserData)[index2].DataPointer = chPtr3;
                }
                int index3 = index2 + 1;
                if ((num1 & 4) != 0)
                {
                    (&baseEvent.UserData)[index3].DataLength = (uint)((str8.Length + 1) * 2);
                    (&baseEvent.UserData)[index3].DataPointer = chPtr4;
                }
                int index4 = index3 + 1;
                if ((num1 & 8) != 0)
                {
                    (&baseEvent.UserData)[index4].DataLength = (uint)((str7.Length + 1) * 2);
                    (&baseEvent.UserData)[index4].DataPointer = chPtr5;
                }
                int index5 = index4 + 1;
                if ((num1 & 16) != 0)
                {
                    (&baseEvent.UserData)[index5].DataLength = (uint)((str6.Length + 1) * 2);
                    (&baseEvent.UserData)[index5].DataPointer = chPtr6;
                }
                int index6 = index5 + 1;
                if ((num1 & 32) != 0)
                {
                    (&baseEvent.UserData)[index6].DataLength = (uint)((str5.Length + 1) * 2);
                    (&baseEvent.UserData)[index6].DataPointer = chPtr7;
                }
                int index7 = index6 + 1;
                if ((num1 & 64) != 0)
                {
                    (&baseEvent.UserData)[index7].DataLength = (uint)((str4.Length + 1) * 2);
                    (&baseEvent.UserData)[index7].DataPointer = chPtr8;
                }
                int index8 = index7 + 1;
                if ((num1 & 128) != 0)
                {
                    (&baseEvent.UserData)[index8].DataLength = (uint)((str3.Length + 1) * 2);
                    (&baseEvent.UserData)[index8].DataPointer = chPtr9;
                }
                int index9 = index8 + 1;
                if ((num1 & 256) != 0)
                {
                    (&baseEvent.UserData)[index9].DataLength = (uint)((str2.Length + 1) * 2);
                    (&baseEvent.UserData)[index9].DataPointer = chPtr10;
                }
                baseEvent.BufferSize = (uint)(48 + num3 * sizeof(MofField));
                num5 = TraceEvent(this._traceHandle, (char*)&baseEvent);
            }
            return num5;
        }

        private unsafe string ProcessOneObject(
          object data,
          MofField* mofField,
          char* ptr,
          ref uint offSet)
        {
            return this.EncodeObject(data, mofField, ptr, ref offSet);
        }

        private unsafe string EncodeObject(
          object data,
          MofField* mofField,
          char* ptr,
          ref uint offSet)
        {
            if (data == null)
            {
                mofField->DataLength = 0U;
                mofField->DataPointer = null;
                return null;
            }
            Type type = data.GetType();
            if (type.IsEnum)
                data = Convert.ChangeType(data, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture);
            switch (data)
            {
                case sbyte num:
                    mofField->DataLength = 1U;
                    sbyte* numPtr1 = (sbyte*)ptr;
                    *numPtr1 = num;
                    mofField->DataPointer = numPtr1;
                    ++offSet;
                    break;
                case byte num:
                    mofField->DataLength = 1U;
                    byte* numPtr2 = (byte*)ptr;
                    *numPtr2 = num;
                    mofField->DataPointer = numPtr2;
                    ++offSet;
                    break;
                case short num:
                    mofField->DataLength = 2U;
                    short* numPtr3 = (short*)ptr;
                    *numPtr3 = num;
                    mofField->DataPointer = numPtr3;
                    offSet += 2U;
                    break;
                case ushort num:
                    mofField->DataLength = 2U;
                    ushort* numPtr4 = (ushort*)ptr;
                    *numPtr4 = num;
                    mofField->DataPointer = numPtr4;
                    offSet += 2U;
                    break;
                case int num:
                    mofField->DataLength = 4U;
                    int* numPtr5 = (int*)ptr;
                    *numPtr5 = num;
                    mofField->DataPointer = numPtr5;
                    offSet += 4U;
                    break;
                case uint num:
                    mofField->DataLength = 4U;
                    uint* numPtr6 = (uint*)ptr;
                    *numPtr6 = num;
                    mofField->DataPointer = numPtr6;
                    offSet += 4U;
                    break;
                case long num:
                    mofField->DataLength = 8U;
                    long* numPtr7 = (long*)ptr;
                    *numPtr7 = num;
                    mofField->DataPointer = numPtr7;
                    offSet += 8U;
                    break;
                case ulong num:
                    mofField->DataLength = 8U;
                    ulong* numPtr8 = (ulong*)ptr;
                    *numPtr8 = num;
                    mofField->DataPointer = numPtr8;
                    offSet += 8U;
                    break;
                case char ch:
                    mofField->DataLength = 2U;
                    char* chPtr = ptr;
                    *chPtr = ch;
                    mofField->DataPointer = chPtr;
                    offSet += 2U;
                    break;
                case float num:
                    mofField->DataLength = 4U;
                    float* numPtr9 = (float*)ptr;
                    *numPtr9 = num;
                    mofField->DataPointer = numPtr9;
                    offSet += 4U;
                    break;
                case double num:
                    mofField->DataLength = 8U;
                    double* numPtr10 = (double*)ptr;
                    *numPtr10 = num;
                    mofField->DataPointer = numPtr10;
                    offSet += 8U;
                    break;
                case bool flag:
                    mofField->DataLength = 1U;
                    bool* flagPtr = (bool*)ptr;
                    *flagPtr = flag;
                    mofField->DataPointer = flagPtr;
                    ++offSet;
                    break;
                case Decimal num:
                    mofField->DataLength = 16U;
                    Decimal* numPtr11 = (Decimal*)ptr;
                    *numPtr11 = num;
                    mofField->DataPointer = numPtr11;
                    offSet += 16U;
                    break;
                default:
                    return data.ToString();
            }
            return null;
        }

        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern int GetTraceEnableFlags(ulong traceHandle);

        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern byte GetTraceEnableLevel(ulong traceHandle);

        [DllImport("advapi32", EntryPoint = "RegisterTraceGuidsW", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint RegisterTraceGuids(
          [In] EtwProc cbFunc,
          [In] void* context,
          [In] ref Guid controlGuid,
          [In] uint guidCount,
          ref TraceGuidRegistration guidReg,
          [In] string mofImagePath,
          [In] string mofResourceName,
          out ulong regHandle);

        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern int UnregisterTraceGuids(ulong regHandle);

        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint TraceEvent(ulong traceHandle, char* header);

        internal sealed class RequestCodes
        {
            internal const uint GetAllData = 0;
            internal const uint GetSingleInstance = 1;
            internal const uint SetSingleInstance = 2;
            internal const uint SetSingleItem = 3;
            internal const uint EnableEvents = 4;
            internal const uint DisableEvents = 5;
            internal const uint EnableCollection = 6;
            internal const uint DisableCollection = 7;
            internal const uint RegInfo = 8;
            internal const uint ExecuteMethod = 9;

            private RequestCodes()
            {
            }
        }

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        internal struct MofField
        {
            [FieldOffset(0)]
            internal unsafe void* DataPointer;
            [FieldOffset(8)]
            internal uint DataLength;
            [FieldOffset(12)]
            internal uint DataType;
        }

        [StructLayout(LayoutKind.Explicit, Size = 304)]
        internal struct BaseEvent
        {
            [FieldOffset(0)]
            internal uint BufferSize;
            [FieldOffset(4)]
            internal byte EventType;
            [FieldOffset(5)]
            internal byte Level;
            [FieldOffset(6)]
            internal ushort Version;
            [FieldOffset(8)]
            internal ulong HistoricalContext;
            [FieldOffset(16)]
            internal long TimeStamp;
            [FieldOffset(24)]
            internal Guid Guid;
            [FieldOffset(40)]
            internal uint ClientContext;
            [FieldOffset(44)]
            internal uint Flags;
            [FieldOffset(48)]
            internal MofField UserData;
        }

        internal unsafe delegate uint EtwProc(
          uint requestCode,
          IntPtr requestContext,
          IntPtr bufferSize,
          byte* buffer);

        internal struct CSTRACE_GUID_REGISTRATION
        {
            internal unsafe Guid* Guid;
            internal uint RegHandle;
        }

        internal struct TraceGuidRegistration
        {
            internal unsafe Guid* Guid;
            internal unsafe void* RegHandle;
        }
    }
}
