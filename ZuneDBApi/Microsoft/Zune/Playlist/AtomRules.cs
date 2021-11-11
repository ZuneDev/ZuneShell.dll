using System.Collections;

namespace Microsoft.Zune.Playlist
{
	public class AtomRules
	{
		internal IList values;

		internal IList operators;

		public IList Operators => operators;

		public IList Values => values;

		internal AtomRules()
		{
			values = new ArrayList();
			operators = new ArrayList();
		}
	}
}
