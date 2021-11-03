// Decompiled with JetBrains decompiler
// Type: ZuneUI.QueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class QueryHelper
    {
        public static bool HasCompletedOrFailed(DataProviderQueryStatus status) => status == DataProviderQueryStatus.Complete || status == DataProviderQueryStatus.Error;

        public static bool HasFailed(DataProviderQueryStatus status) => status == DataProviderQueryStatus.Error;

        public static bool HasCompleted(DataProviderQueryStatus status) => status == DataProviderQueryStatus.Complete;

        public static bool IsBusy(DataProviderQueryStatus status) => status == DataProviderQueryStatus.ProcessingData || status == DataProviderQueryStatus.RequestingData;
    }
}
