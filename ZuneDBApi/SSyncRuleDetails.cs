using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 40)]
[NativeCppClass]
internal struct SSyncRuleDetails
{
	public ESyncCategory syncCategory;

	public EMediaTypes mediaType;

	public bool allMedia;

	public int mediaId;

	public bool included;

	public bool complex;

	public bool calculated;

	public bool ignore;

	public long totalItems;

	public long totalSize;
}
