using System.Collections.Generic;

namespace HibernatingRhinos.Loci.Common.Models
{
	public class Configs
	{
		public class ArrayContainer<T>
		{
			private T[] items;
			public T[] Items
			{
				get { return items ?? (items = new T[] {}); }
				set { items = value; }
			}
		}

		public class ListContainer<T>
		{
			private IList<T> items;
			public IList<T> Items
			{
				get { return items ?? (items = new List<T>()); }
				set { items = value; }
			}
		}
	}
}
