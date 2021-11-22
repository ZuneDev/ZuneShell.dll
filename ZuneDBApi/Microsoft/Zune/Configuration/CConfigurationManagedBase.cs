using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class CConfigurationManagedBase : IDisposable
	{
		private string m_basePath;

		private string m_instance;

		private object m_lock;

		private unsafe HKEY* m_hHive;

		private ConfigurationChangeEventHandler m_configurationChangeEventHandler;

		private unsafe NotificationMarshaller* m_pNotificationMarshaller;

		public unsafe string ConfigurationAbsolutePath
		{
			get
			{
				//IL_0003: Expected I, but got I8
				//IL_001f: Expected I, but got I8
				IConfigurationManager* ptr = null;
				int configurationManagerInstance = Module.GetConfigurationManagerInstance(&ptr);
				if (configurationManagerInstance >= 0)
				{
					IConfigurationManager* intPtr = ptr;
					return string.Concat(new string((char*)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*>)(*(ulong*)(*(long*)intPtr + 192)))((nint)intPtr)) + "\\", ConfigurationPath);
				}
				throw new ApplicationException(Module.GetErrorDescription(configurationManagerInstance));
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
			//IL_0046: Expected I, but got I8
			//IL_0057: Expected I, but got I8
			m_basePath = basePath;
			m_instance = instance;
			m_pNotificationMarshaller = null;
			base._002Ector();
			switch (hive)
			{
			default:
				throw new ArgumentException("hive");
			case RegistryHive.LocalMachine:
				m_hHive = (HKEY*)18446744071562067970uL;
				break;
			case RegistryHive.CurrentUser:
				m_hHive = (HKEY*)18446744071562067969uL;
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
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num2 = Module.GetConfigurationManagerInstance(&ptr);
						if (num2 >= 0)
						{
							long num3 = *(long*)ptr + 8;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, int*, int, int>)(*(ulong*)num3))((nint)intPtr, hHive, ptr2, ptr3, ptr4, &num, defaultValue ? 1 : 0);
							if (num2 >= 0)
							{
								return (num != 0) ? true : false;
							}
						}
						throw new ApplicationException(Module.GetErrorDescription(num2));
					}
				}
			}
		}

		public unsafe void SetBoolProperty(string propertyName, [MarshalAs(UnmanagedType.U1)] bool value)
		{
			//IL_0024: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num < 0)
						{
							goto IL_0050;
						}
						long num2 = *(long*)ptr + 16;
						IConfigurationManager* intPtr = ptr;
						HKEY* hHive = m_hHive;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, int, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, value ? 1 : 0);
						if (num < 0)
						{
							goto IL_0050;
						}
						goto end_IL_0023;
						IL_0050:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0023:;
					}
				}
			}
		}

		public unsafe int GetIntProperty(string propertyName, int defaultValue)
		{
			//IL_0027: Expected I, but got I8
			//IL_0050: Expected I, but got I8
			int result = 0;
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num >= 0)
						{
							long num2 = *(long*)ptr + 32;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, int*, int, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, &result, defaultValue);
							if (num >= 0)
							{
								return result;
							}
						}
						throw new ApplicationException(Module.GetErrorDescription(num));
					}
				}
			}
		}

		public unsafe void SetIntProperty(string propertyName, int value)
		{
			//IL_0024: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num < 0)
						{
							goto IL_0050;
						}
						long num2 = *(long*)ptr + 40;
						IConfigurationManager* intPtr = ptr;
						HKEY* hHive = m_hHive;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, int, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, value);
						if (num < 0)
						{
							goto IL_0050;
						}
						goto end_IL_0023;
						IL_0050:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0023:;
					}
				}
			}
		}

		public unsafe long GetInt64Property(string propertyName, long defaultValue)
		{
			//IL_0028: Expected I, but got I8
			//IL_0051: Expected I, but got I8
			long result = 0L;
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num >= 0)
						{
							long num2 = *(long*)ptr + 56;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, long*, long, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, &result, defaultValue);
							if (num >= 0)
							{
								return result;
							}
						}
						throw new ApplicationException(Module.GetErrorDescription(num));
					}
				}
			}
		}

		public unsafe void SetInt64Property(string propertyName, long value)
		{
			//IL_0024: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num < 0)
						{
							goto IL_0050;
						}
						long num2 = *(long*)ptr + 64;
						IConfigurationManager* intPtr = ptr;
						HKEY* hHive = m_hHive;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, long, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, value);
						if (num < 0)
						{
							goto IL_0050;
						}
						goto end_IL_0023;
						IL_0050:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0023:;
					}
				}
			}
		}

		public unsafe double GetDoubleProperty(string propertyName, double defaultValue)
		{
			//IL_002f: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			double result = 0.0;
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num >= 0)
						{
							long num2 = *(long*)ptr + 80;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, double*, double, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, &result, defaultValue);
							if (num >= 0)
							{
								return result;
							}
						}
						throw new ApplicationException(Module.GetErrorDescription(num));
					}
				}
			}
		}

		public unsafe void SetDoubleProperty(string propertyName, double value)
		{
			//IL_0024: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num < 0)
						{
							goto IL_0050;
						}
						long num2 = *(long*)ptr + 88;
						IConfigurationManager* intPtr = ptr;
						HKEY* hHive = m_hHive;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, double, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, value);
						if (num < 0)
						{
							goto IL_0050;
						}
						goto end_IL_0023;
						IL_0050:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0023:;
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
			FILETIME fILETIME;
			*(int*)(&fILETIME) = 0;
            // IL initblk instruction
            Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref fILETIME, 4), 0, 4);
			FILETIME fILETIME2;
			*(long*)(&fILETIME2) = defaultValue.ToFileTimeUtc();
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num >= 0)
						{
							long num2 = *(long*)ptr + 104;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							_003F val = ptr2;
							_003F val2 = ptr3;
							_003F val3 = ptr4;
							FILETIME fILETIME3 = fILETIME2;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, FILETIME*, FILETIME, int>)(*(ulong*)num2))((nint)intPtr, hHive, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, &fILETIME, fILETIME3);
							if (num >= 0)
							{
								return DateTime.FromFileTimeUtc(*(long*)(&fILETIME));
							}
						}
						throw new ApplicationException(Module.GetErrorDescription(num));
					}
				}
			}
		}

		public unsafe void SetDateTimeProperty(string propertyName, DateTime value)
		{
			//IL_002e: Expected I, but got I8
			//IL_0056: Expected I, but got I8
			FILETIME fILETIME;
			*(long*)(&fILETIME) = value.ToFileTimeUtc();
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						if (num < 0)
						{
							goto IL_005b;
						}
						long num2 = *(long*)ptr + 112;
						IConfigurationManager* intPtr = ptr;
						HKEY* hHive = m_hHive;
						_003F val = ptr2;
						_003F val2 = ptr3;
						_003F val3 = ptr4;
						FILETIME fILETIME2 = fILETIME;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, FILETIME, int>)(*(ulong*)num2))((nint)intPtr, hHive, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, fILETIME2);
						if (num < 0)
						{
							goto IL_005b;
						}
						goto end_IL_002d;
						IL_005b:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_002d:;
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
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						fixed (char* defaultValuePtr = defaultValue.ToCharArray())
						{
							ushort* ptr5 = (ushort*)defaultValuePtr;
							IConfigurationManager* ptr = null;
							int num = Module.GetConfigurationManagerInstance(&ptr);
							uint num2 = 0u;
							if (num >= 0)
							{
								long num3 = *(long*)ptr + 128;
								IConfigurationManager* intPtr = ptr;
								HKEY* hHive = m_hHive;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, uint*, ushort*, int>)(*(ulong*)num3))((nint)intPtr, hHive, ptr2, ptr3, ptr4, null, &num2, ptr5);
								if (num >= 0)
								{
									ushort* ptr6 = (ushort*)Module.new_005B_005D(num2 * 2uL);
									num = (((long)(nint)ptr6 == 0) ? (-2147024882) : num);
									if (num >= 0)
									{
										long num4 = *(long*)ptr + 128;
										IConfigurationManager* intPtr2 = ptr;
										HKEY* hHive2 = m_hHive;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, uint*, ushort*, int>)(*(ulong*)num4))((nint)intPtr2, hHive2, ptr2, ptr3, ptr4, ptr6, &num2, ptr5);
										if (num >= 0)
										{
											result = new string((char*)ptr6);
										}
									}
									if (ptr6 != null)
									{
										Module.delete_005B_005D(ptr6);
									}
									if (num >= 0)
									{
										return result;
									}
								}
							}
							throw new ApplicationException(Module.GetErrorDescription(num));
						}
					}
				}
			}
		}

		public unsafe void SetStringProperty(string propertyName, string value)
		{
			//IL_002c: Expected I, but got I8
			//IL_0057: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						fixed (char* valuePtr = value.ToCharArray())
						{
							ushort* ptr5 = (ushort*)valuePtr;
							IConfigurationManager* ptr = null;
							int num = Module.GetConfigurationManagerInstance(&ptr);
							if (num < 0)
							{
								goto IL_005c;
							}
							long num2 = *(long*)ptr + 136;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr2, ptr3, ptr4, ptr5);
							if (num < 0)
							{
								goto IL_005c;
							}
							goto end_IL_002b;
							IL_005c:
							throw new ApplicationException(Module.GetErrorDescription(num));
							end_IL_002b:;
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
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr3 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr4 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr5 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						ushort* ptr2 = null;
						uint num2 = 0u;
						if (num >= 0)
						{
							long num3 = *(long*)ptr + 144;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, uint*, int>)(*(ulong*)num3))((nint)intPtr, hHive, ptr3, ptr4, ptr5, null, &num2);
							if (num >= 0)
							{
								ptr2 = (ushort*)Module.new_005B_005D(num2 * 2uL);
								num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
								if (num >= 0)
								{
									long num4 = *(long*)ptr + 144;
									IConfigurationManager* intPtr2 = ptr;
									HKEY* hHive2 = m_hHive;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, uint*, int>)(*(ulong*)num4))((nint)intPtr2, hHive2, ptr3, ptr4, ptr5, ptr2, &num2);
									if (num >= 0)
									{
										list = new List<string>();
										ushort* ptr6 = ptr2;
										if (*ptr2 != 0)
										{
											do
											{
												string item = new string((char*)ptr6);
												list.Add(item);
												ptr6 = (ushort*)((Module.lstrlenW(ptr6) + 1) * 2L + (nint)ptr6);
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
						if (ptr2 != null)
						{
							Module.delete_005B_005D(ptr2);
						}
						if (num < 0)
						{
							throw new ApplicationException(Module.GetErrorDescription(num));
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
			//IL_00e1: Expected I, but got I8
			//IL_0133: Expected I, but got I8
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr4 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr5 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr6 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						uint num2 = 0u;
						if (num < 0)
						{
							goto IL_0141;
						}
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
						ulong num4 = num2 * 2uL;
						ushort* ptr2 = (ushort*)Module.new_005B_005D(num4);
						if (ptr2 == null)
						{
							num = -2147024882;
						}
						else
						{
                            // IL initblk instruction
                            Unsafe.InitBlockUnaligned(ptr2, 0, num4);
							ushort* ptr3 = ptr2;
							int num5 = 0;
							if (0 < value.Count)
							{
								do
								{
									int length = value[num5].Length;
									fixed (char* valueNum5Ptr = value[num5].ToCharArray())
									{
										ushort* s = (ushort*)valueNum5Ptr;
										try
										{
											if (Module.wmemcpy_s(ptr3, num2, s, (ulong)length) == 0)
											{
												num2 += (uint)(-1 - length);
												ptr3 = (ushort*)(length * 2L + (nint)ptr3 + 2);
												goto IL_00ee;
											}
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
									}
									break;
									IL_00ee:
									num5++;
								}
								while (num5 < value.Count);
							}
							if (num >= 0)
							{
								long num6 = *(long*)ptr + 152;
								IConfigurationManager* intPtr = ptr;
								HKEY* hHive = m_hHive;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num6))((nint)intPtr, hHive, ptr4, ptr5, ptr6, ptr2);
							}
						}
						if (ptr2 != null)
						{
							Module.delete_005B_005D(ptr2);
						}
						if (num < 0)
						{
							goto IL_0141;
						}
						goto end_IL_0024;
						IL_0141:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0024:;
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
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						IConfigurationManager* ptr = null;
						int num = Module.GetConfigurationManagerInstance(&ptr);
						uint num2 = 0u;
						if (num >= 0)
						{
							long num3 = *(long*)ptr + 160;
							IConfigurationManager* intPtr = ptr;
							HKEY* hHive = m_hHive;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, byte*, uint*, int>)(*(ulong*)num3))((nint)intPtr, hHive, ptr2, ptr3, ptr4, null, &num2);
							if (num >= 0)
							{
								byte[] array = new byte[num2];
								fixed (byte* ptr5 = &array[0])
								{
									try
									{
										long num4 = *(long*)ptr + 160;
										IConfigurationManager* intPtr2 = ptr;
										HKEY* hHive2 = m_hHive;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, byte*, uint*, int>)(*(ulong*)num4))((nint)intPtr2, hHive2, ptr2, ptr3, ptr4, ptr5, &num2);
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
						throw new ApplicationException(Module.GetErrorDescription(num));
					}
				}
			}
		}

		public unsafe void SetBinaryProperty(string propertyName, byte[] value)
		{
			//IL_0034: Expected I, but got I8
			//IL_0061: Expected I, but got I8
			//The blocks IL_0031, IL_0040, IL_0066 are reachable both inside and outside the pinned region starting at IL_0030. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					fixed (char* propertyNamePtr = propertyName.ToCharArray())
					{
						ushort* ptr4 = (ushort*)propertyNamePtr;
						/*pinned*/ref byte reference = ref *(byte*)null;
						IConfigurationManager* ptr;
						IConfigurationManager* intPtr;
						HKEY* hHive;
						_003F val;
						_003F val2;
						_003F val3;
						ref byte val4;
						IntPtr intPtr2;
						int num;
						if (value.Length != 0)
						{
							fixed (byte* ptr5 = &value[0])
							{
								ptr = null;
								num = Module.GetConfigurationManagerInstance(&ptr);
								if (num < 0)
								{
									goto IL_0066;
								}
								long num2 = *(long*)ptr + 168;
								intPtr = ptr;
								hHive = m_hHive;
								val = ptr2;
								val2 = ptr3;
								val3 = ptr4;
								val4 = ptr5;
								intPtr2 = (nint)value.LongLength;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, byte*, uint, int>)(*(ulong*)num2))((nint)intPtr, hHive, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, (byte*)val4, (uint)(nint)intPtr2);
								if (num < 0)
								{
									goto IL_0066;
								}
								goto end_IL_0031;
								IL_0066:
								throw new ApplicationException(Module.GetErrorDescription(num));
								end_IL_0031:;
							}
						}
						else
						{
							ptr = null;
							num = Module.GetConfigurationManagerInstance(&ptr);
							if (num < 0)
							{
								goto IL_0066_2;
							}
							long num2 = *(long*)ptr + 168;
							intPtr = ptr;
							hHive = m_hHive;
							val = ptr2;
							val2 = ptr3;
							val3 = ptr4;
							val4 = ref reference;
							intPtr2 = (nint)value.LongLength;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, ushort*, byte*, uint, int>)(*(ulong*)num2))((nint)intPtr, hHive, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, (byte*)Unsafe.AsPointer(ref val4), (uint)(nint)intPtr2);
							if (num < 0)
							{
								goto IL_0066_2;
							}
						}
						goto end_IL_0024;
						IL_0066_2:
						throw new ApplicationException(Module.GetErrorDescription(num));
						end_IL_0024:;
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
				Module._ZuneShipAssert(1004u, 1181u);
				num = -2147418113;
			}
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr4 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr5 = (ushort*)m_instancePtr;
					if (num < 0)
					{
						goto IL_00ab;
					}
					num = Module.GetConfigurationManagerInstance(&ptr);
					if (num < 0)
					{
						goto IL_00ab;
					}
					NotificationMarshaller* ptr2 = (NotificationMarshaller*)Module.@new(24uL);
					NotificationMarshaller* ptr3;
					try
					{
						ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EConfiguration_002ENotificationMarshaller_002E_007Bctor_007D(ptr2, handler));
					}
					catch
					{
						//try-fault
						Module.delete(ptr2);
						throw;
					}
					m_pNotificationMarshaller = ptr3;
					num = (((long)(nint)ptr3 == 0) ? (-2147024882) : num);
					if (num < 0)
					{
						goto IL_00ab;
					}
					long num2 = *(long*)ptr + 176;
					IConfigurationManager* intPtr = ptr;
					HKEY* hHive = m_hHive;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, INotifySubscriber*, int>)(*(ulong*)num2))((nint)intPtr, hHive, ptr4, ptr5, (INotifySubscriber*)ptr3);
					if (num < 0)
					{
						goto IL_00ab;
					}
					goto end_IL_003e;
					IL_00ab:
					throw new ApplicationException(Module.GetErrorDescription(num));
					end_IL_003e:;
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
			fixed (char* m_basePathPtr = m_basePath.ToCharArray())
			{
				ushort* ptr2 = (ushort*)m_basePathPtr;
				fixed (char* m_instancePtr = m_instance.ToCharArray())
				{
					ushort* ptr3 = (ushort*)m_instancePtr;
					if (num < 0)
					{
						goto IL_0069;
					}
					num = Module.GetConfigurationManagerInstance(&ptr);
					if (num < 0)
					{
						goto IL_0069;
					}
					long num2 = *(long*)ptr + 184;
					IConfigurationManager* intPtr = ptr;
					HKEY* hHive = m_hHive;
					_003F val = ptr2;
					_003F val2 = ptr3;
					NotificationMarshaller* pNotificationMarshaller = m_pNotificationMarshaller;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HKEY*, ushort*, ushort*, INotifySubscriber*, int>)(*(ulong*)num2))((nint)intPtr, hHive, (ushort*)(nint)val, (ushort*)(nint)val2, (INotifySubscriber*)pNotificationMarshaller);
					if (num < 0)
					{
						goto IL_0069;
					}
					NotificationMarshaller* pNotificationMarshaller2 = m_pNotificationMarshaller;
					if (pNotificationMarshaller2 != null)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pNotificationMarshaller2 + 16)))((nint)pNotificationMarshaller2);
						m_pNotificationMarshaller = null;
					}
					goto end_IL_0034;
					IL_0069:
					throw new ApplicationException(Module.GetErrorDescription(num));
					end_IL_0034:;
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

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~CConfigurationManagedBase()
		{
			Dispose(false);
		}
	}
}
