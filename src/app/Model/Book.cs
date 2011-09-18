using System;
using System.Collections.Generic;

namespace Model
{
	public class Book
	{
		public Book(string title, IEnumerable<string> authors, string description, Genre genre, BookStatus status, byte[] image)
		{
			Title       = title;
			Authors     = authors;
			Description = description;
			Genre       = genre;
			Status      = status;
			Image       = image;
		}

		public String Title { get; private set; }

		public String ID { get; set; }

		public Genre Genre { get; private set; }

		public String Description { get; private set; }

		public BookStatus Status { get; private set; }

		public IEnumerable<String> Authors { get; private set; }

		public byte[] Image { get; private set; }
	}
}