namespace MicrosoftZunePlayback;

public class VideoWindow(int left, int top, int right, int bottom)
{
    public int Left => left;

    public int Top => top;

    public int Right => right;

    public int Bottom => bottom;

    public bool IsDifferent(VideoWindow challenger)
    {
        return left != challenger.Left || top != challenger.Top || right != challenger.Right || bottom != challenger.Bottom;
    }
}
