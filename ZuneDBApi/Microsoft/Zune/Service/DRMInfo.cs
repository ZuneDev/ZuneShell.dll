using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class DRMInfo
	{
		private DRMQueryState m_canPlay;

		private DateTime m_expiryDate;

		private bool m_canSync;

		private bool m_canBurn;

		public bool CanBurn
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canBurn;
			}
		}

		public bool CanSync
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canSync;
			}
		}

		public DateTime ExpiryDate => m_expiryDate;

		public bool HasExpiryDate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_expiryDate != DateTime.MaxValue;
			}
		}

		public bool LicenseExpired
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canPlay == (DRMQueryState)2;
			}
		}

		public bool NoLicense
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canPlay == (DRMQueryState)1;
			}
		}

		public bool ValidLicense
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canPlay == (DRMQueryState)0;
			}
		}

		public bool NotProtected
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_canPlay == (DRMQueryState)4;
			}
		}

		public DRMInfo(DRMQueryState eCanPlay, DateTime expiryDate, [MarshalAs(UnmanagedType.U1)] bool canSync, [MarshalAs(UnmanagedType.U1)] bool canBurn)
		{
			m_canPlay = eCanPlay;
			m_expiryDate = expiryDate;
			m_canSync = canSync;
			m_canBurn = canBurn;
		}
	}
}
