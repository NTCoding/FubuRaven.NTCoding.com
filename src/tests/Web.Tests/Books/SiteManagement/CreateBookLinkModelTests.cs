using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.LinkModels;

namespace Web.Tests.Books.SiteManagement
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