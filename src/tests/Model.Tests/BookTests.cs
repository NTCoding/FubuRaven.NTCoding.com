using NUnit.Framework;

namespace Model.Tests
{
	[TestFixture]
	public class BookTests
	{
		private Book GetValidBook()
		{
			string title = "hello mommy";
			var authors = new[] { "jim bowen" };
			var description = "not very good";
			var genre = new Genre("Books");
			var status = BookStatus.Reviewed;
			var image = new[] { (byte)1 };

			return new Book(title, authors, description, genre, status, image);
		}

		[Test]
		public void CanCreate()
		{
			GetValidBook();
		}

		[Test]
		public void ShouldConstruct()
		{
			string title = "hello mommy";
			var authors = new[] { "jim bowen" };
			var description = "not very good";
			var genre = new Genre("Books");
			var status = BookStatus.Reviewed;
			var image = new[] { (byte)1 };

			var book = new Book(title, authors, description, genre, status, image);

			Assert.AreEqual(title, book.Title);
			Assert.AreEqual(authors, book.Authors);
			Assert.AreEqual(description, book.Description);
			Assert.AreEqual(genre, book.Genre);
			Assert.AreEqual(status, book.Status);
			Assert.AreEqual(image, book.Image);
		}

		[Test]
		public void ShouldHaveAnIDProperty()
		{
			var b = GetValidBook();

			var x = b.Id;
		}

		// TODO Validate inputs cannot be null

		// TODO - validate status can be "invisible"
	}
}