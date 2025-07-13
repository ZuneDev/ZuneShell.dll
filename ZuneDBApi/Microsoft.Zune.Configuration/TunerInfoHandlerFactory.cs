namespace Microsoft.Zune.Configuration;

public class TunerInfoHandlerFactory
{
	public static ITunerInfoHandler CreateTunerInfoHandler()
	{
		return new TunerInfoHandler();
	}
}
