using System;

namespace Model
{
	public class Genre
	{
		public Genre(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

			Name = name;
		}

		public String Name { get; private set; }

		public String Id { get; set; }
	}
}