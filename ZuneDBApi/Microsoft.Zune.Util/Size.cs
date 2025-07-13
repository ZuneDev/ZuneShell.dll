namespace Microsoft.Zune.Util;

public class Size(int width, int height)
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
			return width;
		}
		set
		{
			width = value;
		}
	}

	public Size(int width, int height)
	{
		m_width = width;
		m_height = height;
		base._002Ector();
	}

	public Size()
	{
		m_width = 0;
		m_height = 0;
		base._002Ector();
	}
}
