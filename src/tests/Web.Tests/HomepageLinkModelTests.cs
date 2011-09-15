using NUnit.Framework;
using Web.Endpoints.HomepageModels;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageLinkModel();
		}
	}
}