using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
    public class CConfigurationManagedBase : IDisposable
    {
        private string m_basePath;
        private string m_instance;
        private object m_lock;

        public string ConfigurationAbsolutePath
        {
            get { return m_basePath != null ? m_basePath + "\\" + m_instance : m_instance; }
        }

        public string ConfigurationPath => m_basePath != null ? m_basePath + "\\" + m_instance : m_instance;

        public event ConfigurationChangeEventHandler OnConfigurationChanged
        {
            add { lock (m_lock) { OnConfigurationChanged += value; } }
            remove { lock (m_lock) { OnConfigurationChanged -= value; } }
        }

        public CConfigurationManagedBase(RegistryHive hive, string basePath, string instance)
        {
            m_basePath = basePath;
            m_instance = instance;
            m_lock = new object();
        }

        ~CConfigurationManagedBase()
        {
            Dispose(false);
        }

        public virtual bool GetBoolProperty(string propertyName, bool defaultValue) { return defaultValue; }
        public virtual void SetBoolProperty(string propertyName, bool value) { }
        public virtual int GetIntProperty(string propertyName, int defaultValue) { return defaultValue; }
        public virtual void SetIntProperty(string propertyName, int value) { }
        public virtual long GetInt64Property(string propertyName, long defaultValue) { return defaultValue; }
        public virtual void SetInt64Property(string propertyName, long value) { }
        public virtual double GetDoubleProperty(string propertyName, double defaultValue) { return defaultValue; }
        public virtual void SetDoubleProperty(string propertyName, double value) { }
        public virtual DateTime GetDateTimeProperty(string propertyName, DateTime defaultValue) { return defaultValue; }
        public virtual void SetDateTimeProperty(string propertyName, DateTime value) { }
        public virtual string GetStringProperty(string propertyName, string defaultValue) { return defaultValue; }
        public virtual void SetStringProperty(string propertyName, string value) { }
        public virtual IList<string> GetStringListProperty(string propertyName) { return null; }
        public virtual void SetStringListProperty(string propertyName, IList<string> value) { }
        public virtual byte[] GetBinaryProperty(string propertyName) { return null; }
        public virtual void SetBinaryProperty(string propertyName, byte[] value) { }

        public virtual void raise_OnConfigurationChanged(object sender, ConfigurationChangeEventArgs args)
        {
            OnConfigurationChanged?.Invoke(sender, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Cleanup managed resources
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