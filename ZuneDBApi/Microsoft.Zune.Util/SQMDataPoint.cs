namespace Microsoft.Zune.Util;

public class SQMDataPoint
{
	public SQMDataId id;

	public SQMAction action;

	public int argCount;

	public string GetName()
	{
		return ((SQMDataId)(object)id).ToString();
	}

	internal SQMDataPoint(SQMDataId idParam, SQMAction actionParam, int argCountParam)
	{
		id = idParam;
		action = actionParam;
		argCount = argCountParam;
	}
}
