using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class UnitTestConfiguration : CConfigurationManagedBase
{
	public byte[] MyBinaryValue
	{
		get
		{
			return GetBinaryProperty("MyBinaryValue");
		}
		set
		{
			SetBinaryProperty("MyBinaryValue", value);
		}
	}

	public IList<string> MyStringListValue
	{
		get
		{
			return GetStringListProperty("MyStringListValue");
		}
		set
		{
			SetStringListProperty("MyStringListValue", value);
		}
	}

	public string MyStringValue
	{
		get
		{
			return GetStringProperty("MyStringValue", "Test String");
		}
		set
		{
			SetStringProperty("MyStringValue", value);
		}
	}

	public DateTime MyDateValue
	{
		get
		{
			DateTime defaultValue = new DateTime(2007, 3, 18);
			return GetDateTimeProperty("MyDateValue", defaultValue);
		}
		set
		{
			SetDateTimeProperty("MyDateValue", value);
		}
	}

	public double MyDoubleValue
	{
		get
		{
			return GetDoubleProperty("MyDoubleValue", 0.1);
		}
		set
		{
			SetDoubleProperty("MyDoubleValue", value);
		}
	}

	public long MyInt64Value
	{
		get
		{
			return GetInt64Property("MyInt64Value", 1L);
		}
		set
		{
			SetInt64Property("MyInt64Value", value);
		}
	}

	public int MyIntValue
	{
		get
		{
			return GetIntProperty("MyIntValue", 1);
		}
		set
		{
			SetIntProperty("MyIntValue", value);
		}
	}

	public bool MyBoolValue
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("MyBoolValue", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("MyBoolValue", value);
		}
	}

	internal UnitTestConfiguration(RegistryHive hive)
		: base(hive, null, "UnitTest")
	{
	}

	public UnitTestConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
