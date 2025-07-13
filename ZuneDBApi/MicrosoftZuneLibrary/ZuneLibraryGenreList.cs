namespace MicrosoftZuneLibrary;

public class ZuneLibraryGenreList : ZuneQueryList
{
	public unsafe ZuneLibraryGenreList(IDatabaseQueryResults* pResults)
		: base(pResults, "ZuneLibraryGenreList")
	{
	}

	public string GetGenre(int index)
	{
		return (string)GetFieldValue((uint)index, typeof(string), 399u);
	}
}
