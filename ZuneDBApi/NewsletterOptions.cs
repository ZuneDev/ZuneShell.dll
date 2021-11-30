using Microsoft.Zune.Service;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 12)]
public struct NewsletterOptions
{
	public EmailFormat emailFormat;
	public int allowZuneEmails = 0;
	public int allowPartnerEmails = 0;
}
