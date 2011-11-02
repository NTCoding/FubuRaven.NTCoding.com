using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Utilities
{
	public static class StringWrapperExtensions
	{
		public static IEnumerable<String> ToStrings(this IEnumerable<StringWrapper> wrappers)
		{
			return wrappers.Select(w => w.Text);
		}

		public static IEnumerable<StringWrapper> ToStringWrappers(this IEnumerable<String> strings)
		{
			return strings.Select(s => new StringWrapper {Text = s});
		}
	}
}