namespace MicrosoftZuneLibrary
{
	public delegate void SyncProgressedHandler(Device device, uint percentComplete, uint percentItemComplete, uint percentTranscodeComplete, string group, string title, ESyncEngineState engineState);
}
