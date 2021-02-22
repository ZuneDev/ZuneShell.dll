// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Util.SQMLog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Util
{
    public static class SQMLog
    {
        private static SQMStreamPositionType[] c_rgSPTArrayAllStrings = new SQMStreamPositionType[9]
        {
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString,
      SQMStreamPositionType.sptString
        };

        public static void Log(SQMDataId sqmDataId, int nData)
        {
            SQMDataPoint dataPoint = SQMLog.FindDataPoint(sqmDataId);
            if (dataPoint.id == SQMDataId.Invalid)
                return;
            switch (dataPoint.action)
            {
                case SQMAction.Add:
                    SQMLog.SQMAddWrapper(dataPoint.GetName(), nData);
                    break;
                case SQMAction.Inc:
                    SQMLog.SQMAddWrapper(dataPoint.GetName(), 1);
                    break;
                case SQMAction.SetFlag:
                    SQMLog.SQMSetFlagWrapper(dataPoint.GetName(), nData != 0);
                    break;
                case SQMAction.SetBits:
                    SQMLog.SQMSetBitsWrapper(dataPoint.GetName(), (uint)nData);
                    break;
                default:
                    throw new ArgumentException("Datapoint " + dataPoint.GetName() + " is not a simple DWORD-type datapoint.  Use LogToStream for stream-type datapoints.");
            }
            Telemetry.Instance.ReportEvent(dataPoint, nData);
        }

        public static void LogToStream(SQMDataId sqmDataId, params string[] args)
        {
            SQMDataPoint dataPoint = SQMLog.FindDataPoint(sqmDataId);
            if (dataPoint.id == SQMDataId.Invalid)
                return;
            if (dataPoint.action != SQMAction.MixedStream)
                throw new ArgumentException("Datapoint " + dataPoint.GetName() + " is not a MixedStream-type datapoint.  Error in parameters/paramtypes, or in datapoint declaration.");
            if (args.Length > 9)
                throw new ArgumentException("Too many (or too few) number params.  SQM only allows up to 9 params. Datapoint: " + dataPoint.GetName(), "args.Length");
            if (args.Length != dataPoint.argCount)
                throw new ArgumentException("Passed-in arg count doesn't match datapoint declaration: " + dataPoint.GetName(), "args.Length");
            if (args.Length == 1)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0]);
            else if (args.Length == 2)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1]);
            else if (args.Length == 3)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2]);
            else if (args.Length == 4)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3]);
            else if (args.Length == 5)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3], args[4]);
            else if (args.Length == 6)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5]);
            else if (args.Length == 7)
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
            else if (args.Length == 8)
            {
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
            }
            else
            {
                if (args.Length != 9)
                    return;
                SQMLog.SQMAddToStream(dataPoint.GetName(), SQMLog.c_rgSPTArrayAllStrings, (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
            }
        }

        public static void LogToStream(SQMDataId sqmDataId, params uint[] args)
        {
            SQMDataPoint dataPoint = SQMLog.FindDataPoint(sqmDataId);
            if (dataPoint.id == SQMDataId.Invalid)
                return;
            if (dataPoint.action != SQMAction.NumStream)
                throw new ArgumentException("Datapoint " + dataPoint.GetName() + " is not a NumStream-type datapoint.  Error in parameters or in datapoint declaration.");
            if (args.Length < 1 || args.Length > 9)
                throw new ArgumentException("Too many (or too few) number params.  SQM only allows up to 9 params. Datapoint: " + dataPoint.GetName(), "args.Length");
            if (args.Length != dataPoint.argCount)
                throw new ArgumentException("Passed-in arg count doesn't match datapoint declaration: " + dataPoint.GetName(), "args.Length");
            if (args.Length == 1)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0]);
            else if (args.Length == 2)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1]);
            else if (args.Length == 3)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2]);
            else if (args.Length == 4)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3]);
            else if (args.Length == 5)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3], args[4]);
            else if (args.Length == 6)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5]);
            else if (args.Length == 7)
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
            else if (args.Length == 8)
            {
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
            }
            else
            {
                if (args.Length != 9)
                    return;
                SQMLog.SQMAddNumbersToStream(dataPoint.GetName(), (uint)args.Length, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
            }
        }

        private static SQMDataPoint FindDataPoint(SQMDataId sqmDataId)
        {
            foreach (SQMDataPoint rgSqmDataPoint in SQMData.s_rgSQMDataPoints)
            {
                if (rgSqmDataPoint.id == sqmDataId)
                    return rgSqmDataPoint;
            }
            return SQMData.s_sqmDataPointInvalid;
        }

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode)]
        private static extern void SQMAddWrapper(string sqmDataId, int nData);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode)]
        private static extern void SQMSetFlagWrapper(string sqmDataId, bool fSet);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode)]
        private static extern void SQMSetBitsWrapper(string sqmDataId, uint dwBits);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(string sqmDataId, uint countTotal, uint dw1);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4,
          uint dw5);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4,
          uint dw5,
          uint dw6);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4,
          uint dw5,
          uint dw6,
          uint dw7);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4,
          uint dw5,
          uint dw6,
          uint dw7,
          uint dw8);

        [DllImport("ZuneNativeLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddNumbersToStream(
          string sqmDataId,
          uint countTotal,
          uint dw1,
          uint dw2,
          uint dw3,
          uint dw4,
          uint dw5,
          uint dw6,
          uint dw7,
          uint dw8,
          uint dw9);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4,
          string s5);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4,
          string s5,
          string s6);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4,
          string s5,
          string s6,
          string s7);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4,
          string s5,
          string s6,
          string s7,
          string s8);

        [DllImport("ZuneNativeLib", EntryPoint = "SQMAddToStreamWsz", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SQMAddToStream(
          string sqmDataId,
          [MarshalAs(UnmanagedType.LPArray)] SQMStreamPositionType[] sptTypes,
          uint countTotal,
          string s1,
          string s2,
          string s3,
          string s4,
          string s5,
          string s6,
          string s7,
          string s8,
          string s9);
    }
}
