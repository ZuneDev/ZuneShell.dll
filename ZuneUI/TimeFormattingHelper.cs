// Decompiled with JetBrains decompiler
// Type: ZuneUI.TimeFormattingHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class TimeFormattingHelper
    {
        public static string FormatSeconds(int seconds)
        {
            if (seconds <= 0)
                return (string)null;
            int num1 = seconds / 3600;
            int num2 = seconds / 60 % 60;
            seconds %= 60;
            if (num1 >= 1 || num2 > 20)
            {
                num2 = num2 / 5 * 5;
                seconds = 0;
            }
            if (num2 >= 1 || seconds > 20)
                seconds = seconds / 5 * 5;
            if (seconds == 1 && num2 == 0 && num1 == 0)
                return Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_ONE_SECOND);
            if (seconds > 1 && num2 == 0 && num1 == 0)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_N_SECONDS), (object)seconds);
            if (seconds == 0 && num2 == 1 && num1 == 0)
                return Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_ONE_MINUTE);
            if (seconds == 0 && num2 > 1 && num1 == 0)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_N_MINUTES), (object)num2);
            if (num2 == 0 && num1 == 1)
                return Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_ONE_HOUR);
            if (num2 == 0 && num1 > 1)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_N_HOURS), (object)num1);
            if (seconds > 1 && num2 == 1 && num1 == 0)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_ONE_MINUTE_N_SECONDS), (object)seconds);
            if (seconds > 1 && num2 > 1 && num1 == 0)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_N_MINUTES_N_SECONDS), (object)num2, (object)seconds);
            if (num2 > 1 && num1 == 1)
                return string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_ONE_HOUR_N_MINUTES), (object)num2);
            return num2 > 1 && num1 > 1 ? string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_TIME_FORMAT_N_HOURS_N_MINUTES), (object)num1, (object)num2) : (string)null;
        }
    }
}
