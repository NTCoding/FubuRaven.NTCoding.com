using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.InputModels;

namespace Web.Tests.Books.SiteManagement
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