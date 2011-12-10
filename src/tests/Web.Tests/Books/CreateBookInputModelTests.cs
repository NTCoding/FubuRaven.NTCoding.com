using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.InputModels;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateBookInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateBookInputModel();
		}
	}
}