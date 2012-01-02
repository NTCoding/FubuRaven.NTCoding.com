using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Tests.Utilities
{
	public static class CollectionHelper
	{
		public static void CompareOrderOfItems<T>(IEnumerable<T> collection, Action<T, T> compare)
		{
			for (int i = 1; i < collection.Count(); i++)
			{
				var current = collection.ElementAt(i);
				var previous = collection.ElementAt(i - 1);

				compare(current, previous);
			}
		}
	}
}