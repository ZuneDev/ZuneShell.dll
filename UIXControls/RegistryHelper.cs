// Decompiled with JetBrains decompiler
// Type: UIXControls.RegistryHelper
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace UIXControls
{
    public class RegistryHelper
    {
        private static string s_settingsRegistryPath;

        public static string SettingsRegistryPath
        {
            get => RegistryHelper.s_settingsRegistryPath;
            set => RegistryHelper.s_settingsRegistryPath = value;
        }

        public static void SaveString(string keyName, string value)
        {
            if (string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath))
                return;
            Registry.SetValue(RegistryHelper.SettingsRegistryPath, keyName, value);
        }

        public static string GetString(string keyName, string defaultValue)
        {
            string str = null;
            if (!string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath))
                str = Registry.GetValue(RegistryHelper.SettingsRegistryPath, keyName, defaultValue) as string;
            return str ?? defaultValue;
        }

        public static void SaveInt(string keyName, int value)
        {
            if (string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath))
                return;
            Registry.SetValue(RegistryHelper.SettingsRegistryPath, keyName, value);
        }

        public static int GetInt(string keyName, int min, int max, int defaultValue) => string.IsNullOrEmpty(keyName) || string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath) || (!(Registry.GetValue(RegistryHelper.SettingsRegistryPath, keyName, defaultValue) is int num) || num < min || num > max) ? defaultValue : num;

        private static void SaveList(string keyName, IList values, RegistryHelper.ToStringer toString)
        {
            if (string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath))
                return;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object obj in values)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(';');
                stringBuilder.Append(toString(obj));
            }
            Registry.SetValue(RegistryHelper.SettingsRegistryPath, keyName, stringBuilder.ToString());
        }

        public static void SaveIntList(string keyName, IList values) => RegistryHelper.SaveList(keyName, values, value => ((int)value).ToString(NumberFormatInfo.InvariantInfo));

        public static void SaveFloatList(string keyName, IList values) => RegistryHelper.SaveList(keyName, values, value => ((float)value).ToString(NumberFormatInfo.InvariantInfo));

        private static IList GetList(
          string keyName,
          int expectedCount,
          RegistryHelper.TryParser tryParse)
        {
            if (string.IsNullOrEmpty(RegistryHelper.SettingsRegistryPath))
                return null;
            string str = Registry.GetValue(RegistryHelper.SettingsRegistryPath, keyName, null) as string;
            if (string.IsNullOrEmpty(str))
                return null;
            string[] strArray = str.Split(';');
            if (strArray.Length != expectedCount)
                return null;
            ArrayList arrayList = new ArrayList(expectedCount);
            for (int index = 0; index < expectedCount; ++index)
            {
                object obj;
                if (!tryParse(strArray[index], out obj))
                    return null;
                arrayList.Add(obj);
            }
            return arrayList;
        }

        public static IList GetIntList(string keyName, int expectedCount) => RegistryHelper.GetList(keyName, expectedCount, (string s, out object value) =>
       {
           int result;
           bool flag = int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
           value = result;
           return flag;
       });

        public static IList GetPositiveIntList(string keyName, int expectedCount)
        {
            IList list = RegistryHelper.GetIntList(keyName, expectedCount);
            if (list != null)
            {
                foreach (int num in list)
                {
                    if (num <= 0)
                    {
                        list = null;
                        break;
                    }
                }
            }
            return list;
        }

        public static IList GetReorderedIntList(string keyName, int expectedCount)
        {
            IList list = RegistryHelper.GetIntList(keyName, expectedCount);
            if (list != null)
            {
                BitArray bitArray = new BitArray(expectedCount);
                foreach (int index in list)
                {
                    if (index < 0 || index >= expectedCount || bitArray[index])
                    {
                        list = null;
                        break;
                    }
                    bitArray[index] = true;
                }
            }
            return list;
        }

        public static IList GetFloatList(string keyName, int expectedCount) => RegistryHelper.GetList(keyName, expectedCount, (string s, out object value) =>
       {
           float result;
           bool flag = float.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);
           value = result;
           return flag;
       });

        public static IList GetPositionList(string keyName, int expectedCount)
        {
            IList list = RegistryHelper.GetFloatList(keyName, expectedCount);
            if (list != null)
            {
                float num1 = 0.0f;
                foreach (float num2 in list)
                {
                    if (num2 < (double)num1 || num2 > 1.0)
                    {
                        list = null;
                        break;
                    }
                    num1 = num2;
                }
            }
            return list;
        }

        private delegate string ToStringer(object value);

        private delegate bool TryParser(string s, out object value);
    }
}
