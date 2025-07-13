using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

public class VideoWindow(int left, int top, int right, int bottom)
{
	private int _left = left;

	private int _top = top;

	private int _right = right;

	private int _bottom = bottom;

	public int Bottom => bottom;

	public int Right => right;

	public int Top => top;

	public int Left => left;

	[return: MarshalAs(UnmanagedType.U1)]
	public bool IsDifferent(VideoWindow challenger)
	{
		int num = ((left != challenger.left && top != challenger.top && right != challenger.right && bottom != challenger.bottom) ? 1 : 0);
		return (byte)num != 0;
	}
}
