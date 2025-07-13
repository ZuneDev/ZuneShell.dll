using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class MachineConfiguration
{
	private static HMEConfiguration m_HME = new HMEConfiguration(RegistryHive.LocalMachine);

	private static HMESQMConfiguration m_HMESQM = new HMESQMConfiguration(RegistryHive.LocalMachine);

	private static SetupConfiguration m_Setup = new SetupConfiguration(RegistryHive.LocalMachine);

	private static UnitTestConfiguration m_UnitTest = new UnitTestConfiguration(RegistryHive.LocalMachine);

	private static WindowsPhoneConfiguration m_WindowsPhone = new WindowsPhoneConfiguration(RegistryHive.LocalMachine);

	public static WindowsPhoneConfiguration WindowsPhone => m_WindowsPhone;

	public static UnitTestConfiguration UnitTest => m_UnitTest;

	public static SetupConfiguration Setup => m_Setup;

	public static HMESQMConfiguration HMESQM => m_HMESQM;

	public static HMEConfiguration HME => m_HME;
}
