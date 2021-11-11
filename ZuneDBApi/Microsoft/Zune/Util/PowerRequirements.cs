using ZuneUI;

namespace Microsoft.Zune.Util
{
	public class PowerRequirements
	{
		public unsafe static HRESULT CheckOnBatteryPower(out bool fOnBatteryPower)
		{
			//IL_0012: Expected I, but got I8
			fOnBatteryPower = true;
			int num = 0;
			SYSTEM_BATTERY_STATE sYSTEM_BATTERY_STATE;
			int num2 = _003CModule_003E.CallNtPowerInformation((POWER_INFORMATION_LEVEL)5, null, 0u, &sYSTEM_BATTERY_STATE, 32u);
			if (num2 != 0)
			{
				num = num2 | 0x10000000;
				if (num < 0)
				{
					goto IL_002c;
				}
			}
			int num3 = ((fOnBatteryPower = *(byte*)(&sYSTEM_BATTERY_STATE) == 0) ? 1 : 0);
			goto IL_002c;
			IL_002c:
			return num;
		}
	}
}
