using System;

namespace Microsoft.Zune.Configuration
{
	public class ConfigurationChangeEventArgs : EventArgs
	{
		private string m_propertyName;

		public string PropertyName => m_propertyName;

		public ConfigurationChangeEventArgs(string propertyName)
		{
			m_propertyName = propertyName;
		}
	}
}
