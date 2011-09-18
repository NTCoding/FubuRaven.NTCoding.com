using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.CreateModels;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateBookLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateBookLinkModel();
		}
	}
}