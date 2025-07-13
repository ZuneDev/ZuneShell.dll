using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration;

public interface ITunerInfoHandler
{
	[SpecialName]
	event EventHandler OnChanged;

	[return: MarshalAs(UnmanagedType.U1)]
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
