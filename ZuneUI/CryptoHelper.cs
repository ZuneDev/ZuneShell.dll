// Decompiled with JetBrains decompiler
// Type: ZuneUI.CryptoHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ZuneUI
{
    internal class CryptoHelper : IDisposable
    {
        private const uint PROV_RSA_FULL = 1;
        private const uint CRYPT_VERIFYCONTEXT = 4026531840;
        private const uint CRYPT_NEWKEYSET = 8;
        private const uint ALG_CLASS_DATA_ENCRYPT = 24576;
        private const uint ALG_CLASS_HASH = 32768;
        private const uint ALG_TYPE_ANY = 0;
        private const uint ALG_TYPE_STREAM = 2048;
        private const uint ALG_SID_RC4 = 1;
        private const uint ALG_SID_MD5 = 3;
        private const uint CALG_MD5 = 32771;
        private const uint CALG_RC4 = 26625;
        private const uint NTE_BAD_KEYSET = 2148073494;
        private IntPtr m_hCryptoProvider;
        private IntPtr m_hKey;

        public CryptoHelper(string key)
        {
            IntPtr phHash = IntPtr.Zero;
            byte[] bytes = Encoding.Unicode.GetBytes(key);
            try
            {
                if (!CryptoHelper.CryptAcquireContext(out this.m_hCryptoProvider, (string)null, (string)null, 1U, 4026531840U))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (-2146893802 != lastWin32Error)
                        throw new Win32Exception(lastWin32Error);
                    if (!CryptoHelper.CryptAcquireContext(out this.m_hCryptoProvider, (string)null, (string)null, 1U, 4026531848U))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                if (!CryptoHelper.CryptCreateHash(this.m_hCryptoProvider, 32771U, IntPtr.Zero, 0U, out phHash))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if (!CryptoHelper.CryptHashData(phHash, bytes, (uint)bytes.Length, 0U))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if (!CryptoHelper.CryptDeriveKey(this.m_hCryptoProvider, 26625U, phHash, 8388608U, out this.m_hKey))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                if (phHash != IntPtr.Zero)
                    CryptoHelper.CryptDestroyHash(phHash);
            }
        }

        public string Encrypt(string data)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            uint length = (uint)bytes.Length;
            if (!CryptoHelper.CryptEncrypt(this.m_hKey, IntPtr.Zero, true, 0U, bytes, ref length, length))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return Convert.ToBase64String(bytes);
        }

        public string Decrypt(string data)
        {
            byte[] numArray = Convert.FromBase64String(data);
            uint length = (uint)numArray.Length;
            if (!CryptoHelper.CryptDecrypt(this.m_hKey, IntPtr.Zero, true, 0U, numArray, ref length))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return Encoding.Unicode.GetString(numArray);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public void Dispose(bool disposing)
        {
            if (IntPtr.Zero != this.m_hKey)
                CryptoHelper.CryptDestroyKey(this.m_hKey);
            if (!(IntPtr.Zero != this.m_hCryptoProvider))
                return;
            CryptoHelper.CryptReleaseContext(this.m_hCryptoProvider, 0U);
        }

        ~CryptoHelper() => this.Dispose(false);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptAcquireContext(
          out IntPtr phProv,
          string pszContainer,
          string pszProvider,
          uint dwProvType,
          uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptCreateHash(
          IntPtr hProv,
          uint algId,
          IntPtr hKey,
          uint dwFlags,
          out IntPtr phHash);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptHashData(
          IntPtr hHash,
          byte[] pbData,
          uint dwDataLen,
          uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDestroyHash(IntPtr hHash);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDeriveKey(
          IntPtr hProv,
          uint algId,
          IntPtr hBaseData,
          uint dwFlags,
          out IntPtr phKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDestroyKey(IntPtr hHashKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptEncrypt(
          IntPtr hKey,
          IntPtr hHash,
          bool final,
          uint dwFlags,
          byte[] pbData,
          ref uint pdwDataLen,
          uint dwBufLen);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDecrypt(
          IntPtr hKey,
          IntPtr hHash,
          bool final,
          uint dwFlags,
          byte[] pbData,
          ref uint pdwDataLen);
    }
}
