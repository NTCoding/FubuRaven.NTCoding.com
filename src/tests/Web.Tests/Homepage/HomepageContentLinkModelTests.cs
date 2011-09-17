using NUnit.Framework;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageContentLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentLinkModel();
		}
	}
}
