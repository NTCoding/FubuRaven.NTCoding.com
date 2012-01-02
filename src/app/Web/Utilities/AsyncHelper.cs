using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Web.Utilities
{
	public static class AsyncHelper
	{
		public static T DoTimedOperation<T>(Func<T> operation, int timeInSeconds, T defaultValue)
		{
			var t = new Task<T>(operation);
			t.Start();

			var st = new Stopwatch();
			st.Start();

			var time = 1000 * timeInSeconds;
			while (st.ElapsedMilliseconds < time)
			{
				if (t.IsCompleted) return t.Result;
			}

			return defaultValue;
		}
	}
}