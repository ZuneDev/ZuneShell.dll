using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class CConfigurationManagedBase : IDisposable
{
	private string m_basePath;

	private string m_instance;

	private object m_lock;

	private unsafe HKEY__* m_hHive;

	private ConfigurationChangeEventHandler m_configurationChangeEventHandler;

	private unsafe NotificationMarshaller* m_pNotificationMarshaller;

	public unsafe string ConfigurationAbsolutePath
	{
		get
		{
			//IL_0003: Expected I, but got I8
			//IL_001f: Expected I, but got I8
			IConfigurationManager* ptr = null;
			int configurationManagerInstance = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr);
			if (configurationManagerInstance >= 0)
			{
				IConfigurationManager* intPtr = ptr;
				return string.Concat(new string((char*)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)intPtr + 192)))((nint)intPtr)) + "\\", ConfigurationPath);
			}
			throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(configurationManagerInstance));
		}
	}

	public string ConfigurationPath => (!(m_basePath == null)) ? string.Concat(m_basePath + "\\", m_instance) : m_instance;

	[SpecialName]
	public unsafe event ConfigurationChangeEventHandler OnConfigurationChanged
	{
		add
		{
			try
			{
				Monitor.Enter(m_lock);
				if (null == m_configurationChangeEventHandler)
				{
					Subscribe(OnNativeConfigChangedCallback);
				}
				m_configurationChangeEventHandler = (ConfigurationChangeEventHandler)Delegate.Combine(m_configurationChangeEventHandler, value);
			}
			finally
			{
				Monitor.Exit(m_lock);
			}
		}
		remove
		{
			try
			{
				Monitor.Enter(m_lock);
				if (null == (m_configurationChangeEventHandler = (ConfigurationChangeEventHandler)Delegate.Remove(m_configurationChangeEventHandler, value)))
				{
					Unsubscribe();
				}
			}
			finally
			{
				Monitor.Exit(m_lock);
			}
		}
	}

	public unsafe CConfigurationManagedBase(RegistryHive hive, string basePath, string instance)
	{
		//IL_0016: Expected I, but got I8
		//IL_0057: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		m_basePath = basePath;
		m_instance = instance;
		m_pNotificationMarshaller = null;
		base._002Ector();
		switch (hive)
		{
		default:
			throw new ArgumentException("hive");
		case RegistryHive.LocalMachine:
			m_hHive = (HKEY__*)18446744071562067970uL;
			break;
		case RegistryHive.CurrentUser:
			m_hHive = (HKEY__*)18446744071562067969uL;
			break;
		}
		m_lock = new object();
	}

	private void _007ECConfigurationManagedBase()
	{
		_0021CConfigurationManagedBase();
	}

	private unsafe void _0021CConfigurationManagedBase()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		NotificationMarshaller* pNotificationMarshaller = m_pNotificationMarshaller;
		if (pNotificationMarshaller != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNotificationMarshaller + 16)))((nint)pNotificationMarshaller);
			m_pNotificationMarshaller = null;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool GetBoolProperty(string propertyName, [MarshalAs(UnmanagedType.U1)] bool defaultValue)
	{
		//IL_0028: Expected I, but got I8
		//IL_0051: Expected I, but got I8
		int num = 0;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num2 = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num2 >= 0)
					{
						long num3 = *(long*)ptr4 + 8;
						num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, int*, int, int>)(*(ulong*)num3))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, &num, defaultValue ? 1 : 0);
						if (num2 >= 0)
						{
							return (num != 0) ? true : false;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num2));
				}
			}
		}
	}

	public unsafe void SetBoolProperty(string propertyName, [MarshalAs(UnmanagedType.U1)] bool value)
	{
		//IL_0024: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 16;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, int, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, value ? 1 : 0);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe int GetIntProperty(string propertyName, int defaultValue)
	{
		//IL_0027: Expected I, but got I8
		//IL_0050: Expected I, but got I8
		int result = 0;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 32;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, int*, int, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, &result, defaultValue);
						if (num >= 0)
						{
							return result;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe void SetIntProperty(string propertyName, int value)
	{
		//IL_0024: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 40;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, int, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, value);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe long GetInt64Property(string propertyName, long defaultValue)
	{
		//IL_0028: Expected I, but got I8
		//IL_0051: Expected I, but got I8
		long result = 0L;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 56;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, long*, long, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, &result, defaultValue);
						if (num >= 0)
						{
							return result;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe void SetInt64Property(string propertyName, long value)
	{
		//IL_0024: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 64;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, long, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, value);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe double GetDoubleProperty(string propertyName, double defaultValue)
	{
		//IL_002f: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		double result = 0.0;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 80;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, double*, double, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, &result, defaultValue);
						if (num >= 0)
						{
							return result;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe void SetDoubleProperty(string propertyName, double value)
	{
		//IL_0024: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 88;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, double, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, value);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe DateTime GetDateTimeProperty(string propertyName, DateTime defaultValue)
	{
		//IL_0018: Expected I4, but got I8
		//IL_0047: Expected I, but got I8
		//IL_0072: Expected I, but got I8
		DateTime dateTime = default(DateTime);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _FILETIME fILETIME);
		*(int*)(&fILETIME) = 0;
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref fILETIME, 4), 0, 4);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _FILETIME fILETIME2);
		*(long*)(&fILETIME2) = defaultValue.ToFileTimeUtc();
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 104;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, _FILETIME*, _FILETIME, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, &fILETIME, fILETIME2);
						if (num >= 0)
						{
							return DateTime.FromFileTimeUtc(*(long*)(&fILETIME));
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe void SetDateTimeProperty(string propertyName, DateTime value)
	{
		//IL_002e: Expected I, but got I8
		//IL_0056: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _FILETIME fILETIME);
		*(long*)(&fILETIME) = value.ToFileTimeUtc();
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					if (num >= 0)
					{
						long num2 = *(long*)ptr4 + 112;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, _FILETIME, int>)(*(ulong*)num2))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, fILETIME);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe string GetStringProperty(string propertyName, string defaultValue)
	{
		//IL_002f: Expected I, but got I8
		//IL_0066: Expected I, but got I8
		//IL_0066: Expected I, but got I8
		//IL_00ad: Expected I, but got I8
		string result = null;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(defaultValue)))
					{
						IConfigurationManager* ptr5 = null;
						int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr5);
						uint num2 = 0u;
						if (num >= 0)
						{
							long num3 = *(long*)ptr5 + 128;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, uint*, ushort*, int>)(*(ulong*)num3))((nint)ptr5, m_hHive, ptr, ptr2, ptr3, null, &num2, ptr4);
							if (num >= 0)
							{
								ushort* ptr6 = (ushort*)global::_003CModule_003E.new_005B_005D((ulong)num2 * 2uL);
								num = (((long)(nint)ptr6 == 0) ? (-2147024882) : num);
								if (num >= 0)
								{
									long num4 = *(long*)ptr5 + 128;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, uint*, ushort*, int>)(*(ulong*)num4))((nint)ptr5, m_hHive, ptr, ptr2, ptr3, ptr6, &num2, ptr4);
									if (num >= 0)
									{
										result = new string((char*)ptr6);
									}
								}
								if (ptr6 != null)
								{
									global::_003CModule_003E.delete_005B_005D(ptr6);
								}
								if (num >= 0)
								{
									return result;
								}
							}
						}
						throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
					}
				}
			}
		}
	}

	public unsafe void SetStringProperty(string propertyName, string value)
	{
		//IL_002c: Expected I, but got I8
		//IL_0057: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(value)))
					{
						IConfigurationManager* ptr5 = null;
						int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr5);
						if (num >= 0)
						{
							long num2 = *(long*)ptr5 + 136;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)ptr5, m_hHive, ptr, ptr2, ptr3, ptr4);
							if (num >= 0)
							{
								return;
							}
						}
						throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
					}
				}
			}
		}
	}

	public unsafe IList<string> GetStringListProperty(string propertyName)
	{
		//IL_0028: Expected I, but got I8
		//IL_0033: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_00aa: Expected I, but got I8
		//IL_00dc: Expected I, but got I8
		List<string> list = null;
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					ushort* ptr5 = null;
					uint num2 = 0u;
					if (num >= 0)
					{
						long num3 = *(long*)ptr4 + 144;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, uint*, int>)(*(ulong*)num3))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, null, &num2);
						if (num >= 0)
						{
							ptr5 = (ushort*)global::_003CModule_003E.new_005B_005D((ulong)num2 * 2uL);
							num = (((long)(nint)ptr5 == 0) ? (-2147024882) : num);
							if (num >= 0)
							{
								long num4 = *(long*)ptr4 + 144;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, uint*, int>)(*(ulong*)num4))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, ptr5, &num2);
								if (num >= 0)
								{
									list = new List<string>();
									ushort* ptr6 = ptr5;
									if (*ptr5 != 0)
									{
										do
										{
											string item = new string((char*)ptr6);
											list.Add(item);
											ptr6 = (ushort*)((long)(global::_003CModule_003E.lstrlenW(ptr6) + 1) * 2L + (nint)ptr6);
										}
										while (*ptr6 != 0);
									}
									goto IL_00ef;
								}
							}
						}
					}
					if (num == -2147024894)
					{
						num = 0;
						list = null;
					}
					goto IL_00ef;
					IL_00ef:
					if (ptr5 != null)
					{
						global::_003CModule_003E.delete_005B_005D(ptr5);
					}
					if (num < 0)
					{
						throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
					}
					return list;
				}
			}
		}
	}

	public unsafe void SetStringListProperty(string propertyName, IList<string> value)
	{
		//IL_0026: Expected I, but got I8
		//IL_008f: Expected I4, but got I8
		//IL_0133: Expected I, but got I8
		//IL_00e1: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					uint num2 = 0u;
					if (num >= 0)
					{
						int num3 = 0;
						if (0 < value.Count)
						{
							do
							{
								num2 += (uint)(value[num3].Length + 1);
								num3++;
							}
							while (num3 < value.Count);
						}
						num2++;
						ulong num4 = (ulong)num2 * 2uL;
						ushort* ptr5 = (ushort*)global::_003CModule_003E.new_005B_005D(num4);
						if (ptr5 == null)
						{
							num = -2147024882;
						}
						else
						{
							// IL initblk instruction
							System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ptr5, 0, num4);
							ushort* ptr6 = ptr5;
							int num5 = 0;
							if (0 < value.Count)
							{
								do
								{
									int length = value[num5].Length;
									fixed (ushort* s = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(value[num5])))
									{
										try
										{
											if (global::_003CModule_003E.wmemcpy_s(ptr6, num2, s, (ulong)length) != 0)
											{
												goto end_IL_00bb;
											}
											num2 += (uint)(-1 - length);
											ptr6 = (ushort*)((long)length * 2L + (nint)ptr6 + 2);
											goto end_IL_00bb_2;
											end_IL_00bb:;
										}
										catch
										{
											//try-fault
											s = null;
											throw;
										}
										try
										{
											num = -2147418113;
										}
										catch
										{
											//try-fault
											s = null;
											throw;
										}
										break;
										end_IL_00bb_2:;
									}
									num5++;
								}
								while (num5 < value.Count);
							}
							if (num >= 0)
							{
								long num6 = *(long*)ptr4 + 152;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num6))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, ptr5);
							}
						}
						if (ptr5 != null)
						{
							global::_003CModule_003E.delete_005B_005D(ptr5);
						}
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe byte[] GetBinaryProperty(string propertyName)
	{
		//IL_0025: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		//IL_0090: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					IConfigurationManager* ptr4 = null;
					int num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr4);
					uint num2 = 0u;
					if (num >= 0)
					{
						long num3 = *(long*)ptr4 + 160;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, byte*, uint*, int>)(*(ulong*)num3))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, null, &num2);
						if (num >= 0)
						{
							byte[] array = new byte[num2];
							fixed (byte* ptr5 = &array[0])
							{
								try
								{
									long num4 = *(long*)ptr4 + 160;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, byte*, uint*, int>)(*(ulong*)num4))((nint)ptr4, m_hHive, ptr, ptr2, ptr3, ptr5, &num2);
								}
								catch
								{
									//try-fault
									ptr5 = null;
									throw;
								}
							}
							if (num >= 0)
							{
								return array;
							}
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	public unsafe void SetBinaryProperty(string propertyName, byte[] value)
	{
		//IL_0034: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//The blocks IL_0031, IL_0040, IL_0066 are reachable both inside and outside the pinned region starting at IL_0030. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(propertyName)))
				{
					/*pinned*/ref byte reference = ref *(byte*)null;
					IConfigurationManager* ptr5;
					int num;
					if (value.Length != 0)
					{
						fixed (byte* ptr4 = &value[0])
						{
							ptr5 = null;
							num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr5);
							if (num >= 0)
							{
								long num2 = *(long*)ptr5 + 168;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, byte*, uint, int>)(*(ulong*)num2))((nint)ptr5, m_hHive, ptr, ptr2, ptr3, ptr4, (uint)value.Length);
								if (num >= 0)
								{
									return;
								}
							}
							throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
						}
					}
					ptr5 = null;
					num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr5);
					if (num >= 0)
					{
						long num2 = *(long*)ptr5 + 168;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, ushort*, byte*, uint, int>)(*(ulong*)num2))((nint)ptr5, m_hHive, ptr, ptr2, ptr3, (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference), (uint)value.Length);
						if (num >= 0)
						{
							return;
						}
					}
					throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
				}
			}
		}
	}

	[SpecialName]
	public void raise_OnConfigurationChanged(object sender, ConfigurationChangeEventArgs args)
	{
		ConfigurationChangeEventHandler configurationChangeEventHandler = m_configurationChangeEventHandler;
		if (configurationChangeEventHandler != null)
		{
			configurationChangeEventHandler(sender, args);
		}
	}

	private unsafe void OnNativeConfigChangedCallback(ushort* pwszPropertyName)
	{
		string propertyName = new string((char*)pwszPropertyName);
		raise_OnConfigurationChanged(this, new ConfigurationChangeEventArgs(propertyName));
	}

	private unsafe void Subscribe(NativeConfigurationChangeEventHandler handler)
	{
		//IL_0005: Expected I, but got I8
		//IL_0065: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		int num = 0;
		IConfigurationManager* ptr = null;
		if (m_pNotificationMarshaller != null)
		{
			global::_003CModule_003E._ZuneShipAssert(1004u, 1181u);
			num = -2147418113;
		}
		fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				if (num >= 0)
				{
					num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr);
					if (num >= 0)
					{
						NotificationMarshaller* ptr4 = (NotificationMarshaller*)global::_003CModule_003E.@new(24uL);
						NotificationMarshaller* ptr5;
						try
						{
							ptr5 = ((ptr4 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EConfiguration_002ENotificationMarshaller_002E_007Bctor_007D(ptr4, handler));
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.delete(ptr4);
							throw;
						}
						m_pNotificationMarshaller = ptr5;
						num = (((long)(nint)ptr5 == 0) ? (-2147024882) : num);
						if (num >= 0)
						{
							long num2 = *(long*)ptr + 176;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, INotifySubscriber*, int>)(*(ulong*)num2))((nint)ptr, m_hHive, ptr2, ptr3, (INotifySubscriber*)ptr5);
							if (num >= 0)
							{
								return;
							}
						}
					}
				}
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
			}
		}
	}

	private unsafe void Unsubscribe()
	{
		//IL_0005: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		//IL_008c: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		int num = 0;
		IConfigurationManager* ptr = null;
		num = (((long)(nint)m_pNotificationMarshaller == 0) ? (-2147418113) : num);
		fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_basePath)))
		{
			fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_instance)))
			{
				if (num >= 0)
				{
					num = global::_003CModule_003E.GetConfigurationManagerInstance(&ptr);
					if (num >= 0)
					{
						long num2 = *(long*)ptr + 184;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY__*, ushort*, ushort*, INotifySubscriber*, int>)(*(ulong*)num2))((nint)ptr, m_hHive, ptr2, ptr3, (INotifySubscriber*)m_pNotificationMarshaller);
						if (num >= 0)
						{
							NotificationMarshaller* pNotificationMarshaller = m_pNotificationMarshaller;
							if (pNotificationMarshaller != null)
							{
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNotificationMarshaller + 16)))((nint)pNotificationMarshaller);
								m_pNotificationMarshaller = null;
							}
							return;
						}
					}
				}
				throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(num));
			}
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021CConfigurationManagedBase();
			return;
		}
		try
		{
			_0021CConfigurationManagedBase();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~CConfigurationManagedBase()
	{
		Dispose(false);
	}
}
