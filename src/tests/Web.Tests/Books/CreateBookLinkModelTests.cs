using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;

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