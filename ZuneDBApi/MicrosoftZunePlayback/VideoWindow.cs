using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback
{
	public class VideoWindow
	{
		private int _left;

		private int _top;

		private int _right;

		private int _bottom;

		public int Bottom => _bottom;

		public int Right => _right;

		public int Top => _top;

		public int Left => _left;

		public VideoWindow(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsDifferent(VideoWindow challenger)
		{
			int num = ((_left != challenger._left && _top != challenger._top && _right != challenger._right && _bottom != challenger._bottom) ? 1 : 0);
			return (byte)num != 0;
		}
	}
}
