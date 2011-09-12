using NUnit.Framework;
using Web.Endpoints.SiteManagement;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageContentInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentInputModel();
		}
	}
}