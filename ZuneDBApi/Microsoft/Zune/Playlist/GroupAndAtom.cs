namespace Microsoft.Zune.Playlist
{
	public struct GroupAndAtom
	{
		private int groupAndAtom;

		public int Group => (ushort)((ulong)groupAndAtom >> 16);

		public int Atom => (ushort)groupAndAtom;

		public GroupAndAtom(int group, int atom)
		{
			groupAndAtom = ((ushort)group << 16) | (ushort)atom;
		}
	}
}
