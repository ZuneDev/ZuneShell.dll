namespace MicrosoftZuneLibrary
{
	public interface IQueryListEvents
	{
		void ListBeginBulkEvents();

		void ListEndBulkEvents();

		void ListInsert(int index);

		void ListRemoveAt(int index);

		void ListModified(int dwItem);

		void ListNotifyCount(uint ulCount);
	}
}
