// Decompiled with JetBrains decompiler
// Type: ZuneUI.InstalledProductChecker
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ZuneUI
{
    public static class InstalledProductChecker
    {
        public static bool IsInstalled(string upgradeCode, int minVersionMajor, int minVersionMinor)
        {
            Guid empty1 = Guid.Empty;
            Guid guid;
            try
            {
                guid = new Guid(upgradeCode);
            }
            catch
            {
                return false;
            }
            string empty2 = string.Empty;
            int index = 0;
            string productCode;
            do
            {
                productCode = InstalledProductChecker.EnumRelatedProducts(guid.ToString("B"), index);
                if (!string.IsNullOrEmpty(productCode) && InstalledProductChecker.GetProductVersionMajor(productCode) >= minVersionMajor && InstalledProductChecker.GetProductVersionMinor(productCode) >= minVersionMinor)
                    return true;
                ++index;
            }
            while (!string.IsNullOrEmpty(productCode));
            return false;
        }

        public static string GetProductName(string productCode) => InstalledProductChecker.GetProductInfo(productCode, "ProductName");

        public static int GetProductVersionMajor(string productCode)
        {
            int result;
            int.TryParse(InstalledProductChecker.GetProductInfo(productCode, "VersionMajor"), out result);
            return result;
        }

        public static int GetProductVersionMinor(string productCode)
        {
            int result;
            int.TryParse(InstalledProductChecker.GetProductInfo(productCode, "VersionMinor"), out result);
            return result;
        }

        public static string GetProductInfo(string productCode, string property)
        {
            int size = 512;
            StringBuilder valueBuffer = new StringBuilder(size);
            return (HRESULT)InstalledProductChecker.MsiGetProductInfo(productCode, property, valueBuffer, ref size) != HRESULT._S_OK ? string.Empty : valueBuffer.ToString();
        }

        public static string EnumRelatedProducts(string upgradeCode, int index)
        {
            StringBuilder productCodeBuffer = new StringBuilder(39);
            return (HRESULT)InstalledProductChecker.MsiEnumRelatedProducts(upgradeCode, 0, index, productCodeBuffer) != HRESULT._S_OK ? string.Empty : productCodeBuffer.ToString();
        }

        [DllImport("msi.dll")]
        private static extern int MsiGetProductInfo(
          string productCode,
          string property,
          [Out] StringBuilder valueBuffer,
          ref int size);

        [DllImport("msi.dll")]
        private static extern int MsiEnumRelatedProducts(
          string upgradeCode,
          int reserved,
          int index,
          [Out] StringBuilder productCodeBuffer);
    }
}
