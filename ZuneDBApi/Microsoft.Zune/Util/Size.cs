namespace Microsoft.Zune.Util;

public class Size
{
    public int Height { get; set; }

    public int Width { get; set; }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public Size() : this(0, 0)
    {
    }
}