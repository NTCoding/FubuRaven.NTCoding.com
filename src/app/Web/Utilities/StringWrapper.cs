using System;

namespace Web.Utilities
{
	public class StringWrapper
	{
		public String Text { get; set; }

		public override bool Equals(object obj)
		{
			var s = obj as String;

			if (s == null) return false;

			return s == this.Text;
		}

		public static bool operator ==(String b, StringWrapper a)
		{
			return a.Text == b;
		}

		public static bool operator !=(string b, StringWrapper a)
		{
			return !(b == a);
		}
	}
}