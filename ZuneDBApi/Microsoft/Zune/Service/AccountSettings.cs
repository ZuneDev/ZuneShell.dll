using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class AccountSettings
	{
		private IDictionary<PrivacySettingId, PrivacySettingValue> m_privacySettings;

		private EmailFormat m_emailFormat;

		private bool m_allowZuneEmails;

		private bool m_allowPartnerEmails;

		public IDictionary<PrivacySettingId, PrivacySettingValue> PrivacySettings
		{
			get
			{
				if (m_privacySettings == null)
				{
					m_privacySettings = new Dictionary<PrivacySettingId, PrivacySettingValue>();
				}
				return m_privacySettings;
			}
		}

		public bool AllowPartnerEmails
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_allowPartnerEmails;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				m_allowPartnerEmails = value;
			}
		}

		public bool AllowZuneEmails
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_allowZuneEmails;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				m_allowZuneEmails = value;
			}
		}

		public EmailFormat EmailFormat
		{
			get
			{
				return m_emailFormat;
			}
			set
			{
				m_emailFormat = value;
			}
		}

		internal unsafe AccountSettings(INewsletterSettings* pNewsletterSettings, IPrivacySettings* pPrivacySettings)
		{
			//IL_0021: Expected I, but got I8
			//IL_0066: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			NewsletterOptions newsletterOptions;
			if (pNewsletterSettings == null || pPrivacySettings == null || ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, NewsletterOptions*, int>)(*(ulong*)(*(long*)pNewsletterSettings + 32)))((nint)pNewsletterSettings, &newsletterOptions) < 0)
			{
				return;
			}
			m_emailFormat = *(EmailFormat*)(&newsletterOptions);
			bool flag = (m_allowZuneEmails = ((Unsafe.As<NewsletterOptions, int>(ref Unsafe.AddByteOffset(ref newsletterOptions, 4)) != 0) ? true : false));
			bool flag2 = (m_allowPartnerEmails = ((Unsafe.As<NewsletterOptions, int>(ref Unsafe.AddByteOffset(ref newsletterOptions, 8)) != 0) ? true : false));
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pPrivacySettings + 32)))((nint)pPrivacySettings);
			int num2 = 0;
			if (0 >= num)
			{
				return;
			}
			EPrivacySettingId key;
			EPrivacySettingValue value;
			while (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EPrivacySettingId*, EPrivacySettingValue*, int>)(*(ulong*)(*(long*)pPrivacySettings + 40)))((nint)pPrivacySettings, num2, &key, &value) >= 0)
			{
				PrivacySettings[(PrivacySettingId)key] = (PrivacySettingValue)value;
				num2++;
				if (num2 >= num)
				{
					break;
				}
			}
		}

		public AccountSettings()
		{
		}
	}
}
