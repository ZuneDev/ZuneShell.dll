namespace Microsoft.Zune.Util;

public class Point
{
    public int Y { get; set; }

    public int X { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point() : this(0, 0)
    {
    }
}