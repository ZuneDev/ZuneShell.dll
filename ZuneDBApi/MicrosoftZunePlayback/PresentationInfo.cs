using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback
{
	public class PresentationInfo
	{
		private int _AspectRatioX;

		private int _AspectRatioY;

		private int _NativeX;

		private int _NativeY;

		private bool _NeedOverscan;

		public bool NeedOverscan
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _NeedOverscan;
			}
		}

		public int ContentHeight => _NativeY;

		public int ContentWidth => _NativeX;

		public int ContentAspectHeight => _AspectRatioY;

		public int ContentAspectWidth => _AspectRatioX;

		public PresentationInfo(int aspectRatioX, int aspectRatioY, int nativeX, int nativeY, [MarshalAs(UnmanagedType.U1)] bool needOverscan)
		{
			_AspectRatioX = aspectRatioX;
			_AspectRatioY = aspectRatioY;
			_NativeX = nativeX;
			_NativeY = nativeY;
			_NeedOverscan = needOverscan;
		}
	}
}
