using Microsoft.Win32;
using ZuneDBApi.Abstractions;

namespace Microsoft.Zune.Configuration
{
    public class CConfigurationManagedBase : IDisposable
    {
        private string m_basePath;
        private string m_instance;
        private object m_lock;
        private readonly IRegistryProvider m_registry;

        public string ConfigurationAbsolutePath
        {
            get { return m_basePath != null ? m_basePath + "\\" + m_instance : m_instance; }
        }

        public string ConfigurationPath => m_basePath != null ? m_basePath + "\\" + m_instance : m_instance;

        public event ConfigurationChangeEventHandler OnConfigurationChanged;

        public CConfigurationManagedBase(RegistryHive hive, string basePath, string instance)
        {
            m_basePath = basePath;
            m_instance = instance;
            m_lock = new object();
            m_registry = RegistryProviderFactory.Create(hive, ConfigurationPath);
        }

        ~CConfigurationManagedBase()
        {
            Dispose(false);
        }

        public virtual bool GetBoolProperty(string propertyName, bool defaultValue)
        {
            lock (m_lock) return m_registry.GetBoolValue(propertyName, defaultValue);
        }

        public virtual void SetBoolProperty(string propertyName, bool value)
        {
            lock (m_lock) m_registry.SetBoolValue(propertyName, value);
        }

        public virtual int GetIntProperty(string propertyName, int defaultValue)
        {
            lock (m_lock) return m_registry.GetIntValue(propertyName, defaultValue);
        }

        public virtual void SetIntProperty(string propertyName, int value)
        {
            lock (m_lock) m_registry.SetIntValue(propertyName, value);
        }

        public virtual long GetInt64Property(string propertyName, long defaultValue)
        {
            lock (m_lock) return m_registry.GetInt64Value(propertyName, defaultValue);
        }

        public virtual void SetInt64Property(string propertyName, long value)
        {
            lock (m_lock) m_registry.SetInt64Value(propertyName, value);
        }

        public virtual double GetDoubleProperty(string propertyName, double defaultValue)
        {
            lock (m_lock) return m_registry.GetDoubleValue(propertyName, defaultValue);
        }

        public virtual void SetDoubleProperty(string propertyName, double value)
        {
            lock (m_lock) m_registry.SetDoubleValue(propertyName, value);
        }

        public virtual DateTime GetDateTimeProperty(string propertyName, DateTime defaultValue)
        {
            lock (m_lock) return m_registry.GetDateTimeValue(propertyName, defaultValue);
        }

        public virtual void SetDateTimeProperty(string propertyName, DateTime value)
        {
            lock (m_lock) m_registry.SetDateTimeValue(propertyName, value);
        }

        public virtual string GetStringProperty(string propertyName, string defaultValue)
        {
            lock (m_lock) return m_registry.GetStringValue(propertyName, defaultValue);
        }

        public virtual void SetStringProperty(string propertyName, string value)
        {
            lock (m_lock) m_registry.SetStringValue(propertyName, value);
        }

        public virtual IList<string> GetStringListProperty(string propertyName)
        {
            lock (m_lock) return m_registry.GetStringListValue(propertyName);
        }

        public virtual void SetStringListProperty(string propertyName, IList<string> value)
        {
            lock (m_lock) m_registry.SetStringListValue(propertyName, value);
        }

        public virtual byte[] GetBinaryProperty(string propertyName)
        {
            lock (m_lock) return m_registry.GetBinaryValue(propertyName);
        }

        public virtual void SetBinaryProperty(string propertyName, byte[] value)
        {
            lock (m_lock) m_registry.SetBinaryValue(propertyName, value);
        }

        public virtual void raise_OnConfigurationChanged(object sender, ConfigurationChangeEventArgs args)
        {
            OnConfigurationChanged?.Invoke(sender, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_registry.Dispose();
            }
            // Cleanup unmanaged resources
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}