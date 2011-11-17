using NUnit.Framework;

namespace Web.Tests.Books
{
	[TestFixture]
	public class BookListModelTests
	{
		[Test]
		public void CanCreate()
		{
			var model = new BookListModel();
		}
	}

	public class BookListModel
	{
	}
}