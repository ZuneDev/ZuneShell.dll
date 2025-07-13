namespace MicrosoftZuneLibrary;

public class FriendOfArtist
{
	private int _id;

	private string _zuneTag;

	public string ZuneTag
	{
		get
		{
			return _zuneTag;
		}
		set
		{
			_zuneTag = value;
		}
	}

	public int Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}
}
