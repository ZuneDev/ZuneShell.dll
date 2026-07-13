using System;
using System.Collections.Generic;

namespace Microsoft.Zune.Configuration
{
    internal class TunerInfoHandler : ITunerInfoHandler, IDisposable
    {
        public event EventHandler? OnChanged;

        private IList<TunerInfo> m_PCsList = new List<TunerInfo>();
        private IList<TunerInfo> m_devicesList = new List<TunerInfo>();
        private IList<TunerInfo> m_appStoreDevicesList = new List<TunerInfo>();
        private DateTime m_nextPCDeregistrationDate;
        private DateTime m_nextSubscriptionDeviceDeregistrationDate;
        private DateTime m_nextAppStoreDeviceDeregistrationDate;

        public virtual bool CanQueryTunerList() { throw new NotImplementedException(); }
        public virtual IList<TunerInfo> GetPCsList() { return m_PCsList; }
        public virtual IList<TunerInfo> GetDevicesList() { return m_devicesList; }
        public virtual IList<TunerInfo> GetAppStoreDevicesList() { return m_appStoreDevicesList; }
        public virtual DateTime GetNextPCDeregistrationDate() { return m_nextPCDeregistrationDate; }
        public virtual DateTime GetNextSubscriptionDeviceDeregistrationDate() { return m_nextSubscriptionDeviceDeregistrationDate; }
        public virtual DateTime GetNextAppStoreDeviceDeregistrationDate() { return m_nextAppStoreDeviceDeregistrationDate; }
        public virtual void RefreshTunerList() { throw new NotImplementedException(); }
        public virtual void DeregisterTuner(TunerInfo info) { throw new NotImplementedException(); }
        protected virtual void raise_OnChanged(object value0, EventArgs value1) { OnChanged?.Invoke(value0, value1); }
        internal void UpdateTunerInfoLists(int cTunerInfo, IntPtr rgTunerInfo, IntPtr pwszNextPCDeregistrationDate, IntPtr pwszNextSubscriptionDeviceDeregistrationDate, IntPtr pwszNextAppStoreDeviceDeregistrationDate) { throw new NotImplementedException(); }
        internal void FinishRemoveTunerInfo(IntPtr pwszTunerId, TunerType tunerType, TunerRegisterType tunerRegisterType) { throw new NotImplementedException(); }
        internal void ReportError(int hrError) { raise_OnChanged(this, new EventArgsHR { HResult = hrError }); }
        protected virtual void Dispose(bool P_0) { }
        public virtual void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
    }
}