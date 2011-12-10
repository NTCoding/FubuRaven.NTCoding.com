using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.LinkModels;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class ViewBookLinkModelTest
	{
		[Test]
		public void CanCreate()
		{
			new ViewBookLinkModel();
		}
	}
}