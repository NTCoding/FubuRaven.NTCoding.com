using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.CreateModels;

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