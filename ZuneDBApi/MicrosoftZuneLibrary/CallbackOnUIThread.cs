namespace MicrosoftZuneLibrary;

// Original wraps native bimodal (managed/native) callback marshaling infrastructure
// (CallbackOnUIThreadBimodalManaged_DONOTUSE) used to receive native callbacks on the
// UI thread. Not reverse engineered — see logs/MicrosoftZuneLibrary/ZuneLibrary.md.
// ZuneApplication constructs one and never calls any member on it directly (the
// original's own internal CallbackOnUIThreadRequest/DeferredCallback plumbing is
// triggered only from native code), so only the public no-arg constructor is needed.
public class CallbackOnUIThread
{
}
