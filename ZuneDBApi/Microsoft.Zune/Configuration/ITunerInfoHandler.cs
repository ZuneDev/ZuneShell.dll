using System;
using System.Collections.Generic;

namespace Microsoft.Zune.Configuration
{
    public interface ITunerInfoHandler
    {
        event EventHandler OnChanged;

        bool CanQueryTunerList();

        IList<TunerInfo> GetPCsList();

        IList<TunerInfo> GetDevicesList();

        IList<TunerInfo> GetAppStoreDevicesList();

        DateTime GetNextPCDeregistrationDate();

        DateTime GetNextSubscriptionDeviceDeregistrationDate();

        DateTime GetNextAppStoreDeviceDeregistrationDate();

        void RefreshTunerList();

        void DeregisterTuner(TunerInfo info);
    }
}