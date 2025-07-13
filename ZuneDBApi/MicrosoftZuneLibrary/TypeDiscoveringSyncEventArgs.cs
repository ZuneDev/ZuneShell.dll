using System.Collections.Generic;

namespace MicrosoftZuneLibrary;

public class TypeDiscoveringSyncEventArgs : SyncEventArgs
{
	public List<EMediaTypes> ContainedTypes;
}
