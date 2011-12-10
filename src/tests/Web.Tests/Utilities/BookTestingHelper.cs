using Model;

namespace Web.Tests.Utilities
{
	public static class BookTestingHelper
	{
		public static Book GetBook(byte[] imageData = null)
		{
			var description = "A fantastic read, I'm sure you'll agree";
			var genre = new Model.Genre("Pissy Wissy");
			var image = imageData ?? new[] { (byte)1 };

			return new
				Book("Super book", new[] {"Me", "You", "Him"}, description, genre, BookStatus.Reviewed, image) {Id = "dkdkj"};

		}
	}
}