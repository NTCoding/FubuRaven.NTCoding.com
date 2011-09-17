using NUnit.Framework;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests.Homepage
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