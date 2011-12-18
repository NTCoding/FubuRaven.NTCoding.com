using System;
using System.Collections.Generic;
using Model;

namespace Web.Tests.Utilities
{
	public static class BookTestingHelper
	{
		public static Book GetBook(byte[] imageData = null, Model.Genre genre = null, BookStatus? status = null, String id = null, int rating =  0)
		{
			var description = "A fantastic read, I'm sure you'll agree";
			genre = genre ?? new Model.Genre("Pissy Wissy");
			var image = imageData ?? new[] { (byte)1 };

			return new
				Book("Super book", new[] {"Me", "You", "Him"}, description, genre, status ?? BookStatus.Reviewed, image) 
				{Id = id ?? "dkdkj", Rating = rating};

		}

		public static IEnumerable<Book> GetSomeReviewedBooks(int amount = 10)
		{
			var rand = new Random();
			for (int i = 0; i < amount; i++)
			{
				yield return GetBook(status: BookStatus.Reviewed, rating: rand.Next(1, 5));
			}
		}
	}
}