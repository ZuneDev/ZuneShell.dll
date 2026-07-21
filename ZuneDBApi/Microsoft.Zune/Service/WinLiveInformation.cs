using System;
using System.Collections;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Service;

// Original wraps a native IWinLiveInformation*. Not reverse engineered — see
// logs/Microsoft.Zune/Service/WinLiveSignup.md.
public class WinLiveInformation : IDisposable
{
    internal WinLiveInformation()
    {
    }

    public IList Domains { get; } = new ArrayList();

    public int HipLength => 0;

    public SafeBitmapWithData HipImage => null;

    public string HipChallenge => null;

    public string PrivacyUrl => null;

    public int TermsOfServiceVersion => 0;

    public string TermsOfServiceUrl => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
