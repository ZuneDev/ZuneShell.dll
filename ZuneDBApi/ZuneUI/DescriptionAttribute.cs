using System;

namespace ZuneUI
{
	public class DescriptionAttribute : Attribute
	{
		private uint m_stringId;

		public uint StringId => m_stringId;

		public DescriptionAttribute(uint stringId)
		{
			m_stringId = stringId;
		}
	}
}
