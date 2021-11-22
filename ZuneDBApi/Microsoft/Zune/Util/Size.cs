namespace Microsoft.Zune.Util
{
	public class Size
	{
		private int m_width;

		private int m_height;

		public int Height
		{
			get
			{
				return m_height;
			}
			set
			{
				m_height = value;
			}
		}

		public int Width
		{
			get
			{
				return m_width;
			}
			set
			{
				m_width = value;
			}
		}

		public Size(int width, int height)
		{
			m_width = width;
			m_height = height;
		}

		public Size()
		{
			m_width = 0;
			m_height = 0;
		}
	}
}
