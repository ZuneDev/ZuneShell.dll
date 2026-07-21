namespace Microsoft.Zune.Util;

// Original populates s_rgSQMDataPoints with ~230 SQMDataPoint entries (one per
// SQMDataId, each with a hand-authored SQMAction/argCount) — pure static data, not a
// native call, but the actual per-entry action/argCount values aren't visible via
// ILSpy's member listing (only the field's existence is) and transcribing 230 guesses
// would be worse than an honest empty table. Left empty: SQMLog.FindDataPoint's linear
// scan simply falls through to s_sqmDataPointInvalid for every id, which is the same
// externally-observable behavior as "SQM logging is disabled" — consistent with every
// other telemetry/logging call in this codebase being a no-op. See
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class SQMData
{
    public static SQMDataPoint[] s_rgSQMDataPoints = System.Array.Empty<SQMDataPoint>();
    public static SQMDataPoint s_sqmDataPointInvalid = new(SQMDataId.Invalid, SQMAction.Add, 0);
}
