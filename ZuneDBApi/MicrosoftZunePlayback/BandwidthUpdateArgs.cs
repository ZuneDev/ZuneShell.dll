using System;

namespace MicrosoftZunePlayback
{
	public class BandwidthUpdateArgs : EventArgs
	{
		private long _buffering;

		private float _quality;

		private int _id;

		private int _latestBandwidth;

		private int _recentAverageBandwidth;

		private int _totalAverageBandwidth;

		private int _bitrate;

		private int _droppedFrames;

		private int _testLength;

		private int _testPosition;

		private MBRHeuristicState _currentState;

		private int _percentComplete;

		public int PercentComplete => _percentComplete;

		public MBRHeuristicState currentState => _currentState;

		public int TestPosition => _testPosition;

		public int TestLength => _testLength;

		public int DroppedFrames => _droppedFrames;

		public int Bitrate => _bitrate;

		public int TotalAverageBandwidth => _totalAverageBandwidth;

		public int RecentAverageBandwidth => _recentAverageBandwidth;

		public int LatestBandwidth => _latestBandwidth;

		public int Id => _id;

		public float Quality => _quality;

		public long Buffering => _buffering;

		public BandwidthUpdateArgs(long buffering, float quality, int Id, int latestBandwidth, int recentAverageBandwidth, int totalAverageBandwidth, int bitrate, int droppedFrames, int testLength, int testPosition, MBRHeuristicState currentState)
		{
			_buffering = buffering;
			_quality = quality;
			_id = Id;
			_latestBandwidth = latestBandwidth;
			_recentAverageBandwidth = recentAverageBandwidth;
			_totalAverageBandwidth = totalAverageBandwidth;
			_bitrate = bitrate;
			_droppedFrames = droppedFrames;
			_testLength = testLength;
			_testPosition = testPosition;
			_currentState = currentState;
			int testPosition2 = _testPosition;
			if (testPosition2 != 0)
			{
				int testLength2 = _testLength;
				if (testLength2 != 0 && currentState != MBRHeuristicState.Playback)
				{
					_percentComplete = (int)((testPosition2 >= testLength2) ? 100f : ((float)((float)testPosition2 / (double)(float)testLength2 * 100.0)));
					return;
				}
			}
			_percentComplete = 0;
		}
	}
}
